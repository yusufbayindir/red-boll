using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class RedBallBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Boot()
    {
        if (UnityEngine.Object.FindAnyObjectByType<RedBallGame>() != null)
        {
            return;
        }

        var gameObject = new GameObject("Red Ball Game");
        UnityEngine.Object.DontDestroyOnLoad(gameObject);
        gameObject.AddComponent<RedBallGame>();
    }
}

public sealed class RedBallGame : MonoBehaviour
{
    public const int LevelCount = 15;
    private const int MaxHealth = 5;
    private const int HeartRechargeSeconds = 3600;
    private const string SprintKeyArtResourcePath = "Generated/Sprint01/mastery_update_concept";
    private const string SaveUnlockedCountKey = "RedBall.UnlockedCount";
    private const string SaveCompletedMaskKey = "RedBall.CompletedMask";
    private const string SaveBadgeClearMaskKey = "RedBall.Badges.ClearMask";
    private const string SaveBadgeAllCoinsMaskKey = "RedBall.Badges.AllCoinsMask";
    private const string SaveBadgeCleanRunMaskKey = "RedBall.Badges.CleanRunMask";
    private const string SaveHeartsKey = "RedBall.Hearts";
    private const string SaveHeartStampKey = "RedBall.HeartStamp";
    private const float WorldPpu = 80f;
    private const float UiPpu = 100f;

    private readonly Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
    private readonly Dictionary<string, Sprite[]> animations = new Dictionary<string, Sprite[]>();
    private readonly List<GameObject> levelObjects = new List<GameObject>();
    private readonly List<Image> levelButtonImages = new List<Image>();
    private readonly List<Button> levelButtons = new List<Button>();
    private readonly List<Text> levelButtonTexts = new List<Text>();
    private readonly List<Image[]> levelButtonBadgeImages = new List<Image[]>();
    private readonly List<LocalizedTextBinding> localizedTextBindings = new List<LocalizedTextBinding>();
    private readonly List<Text> languageButtonTexts = new List<Text>();

    private Camera mainCamera;
    private CameraFollow cameraFollow;
    private VirtualJoystick joystick;
    private HoldButton jumpButton;
    private GameObject hudRoot;
    private GameObject mainMenuRoot;
    private GameObject levelSelectRoot;
    private Text scoreText;
    private Text levelText;
    private Text messageText;
    private Text menuHeartText;
    private Text menuStatusText;
    private Text levelSelectHeartText;
    private Text levelSelectStatusText;
    private Font uiFont;
    private Transform levelRoot;
    private RedBallPlayer player;
    private AudioSource sfxSource;
    private AudioClip jumpClip;
    private AudioClip bounceClip;
    private AudioClip pickupClip;
    private AudioClip hurtClip;
    private PhysicsMaterial2D playerMaterial;
    private PhysicsMaterial2D groundMaterial;
    private PhysicsMaterial2D slickMaterial;

    private Vector2 playerSpawn;
    private int levelIndex;
    private int score;
    private int health = MaxHealth;
    private int unlockedLevelCount = 1;
    private int completedLevelMask;
    private int badgeClearMask;
    private int badgeAllCoinsMask;
    private int badgeCleanRunMask;
    private int coinsInLevel;
    private int coinsCollectedInLevel;
    private float messageTimer;
    private float statusTimer;
    private float lifeRefreshTimer;
    private float respawnLockUntil;
    private long heartTimestamp;
    private bool levelFinished;
    private bool attemptCleanRun = true;
    private string cleanRunFailReason = string.Empty;
    private ScreenMode screenMode;

    public float KillY { get; private set; } = -12f;
    public Transform PlayerTransform => player != null ? player.transform : null;
    public int LevelObjectCountForSmokeTest => levelObjects.Count;

    private enum ScreenMode
    {
        MainMenu,
        LevelSelect,
        Gameplay
    }

    private sealed class LocalizedTextBinding
    {
        public Text Text;
        public string Key;
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Input.multiTouchEnabled = true;
        Physics2D.gravity = new Vector2(0f, -23f);

        playerMaterial = new PhysicsMaterial2D("Red Ball") { friction = 0.45f, bounciness = 0.03f };
        groundMaterial = new PhysicsMaterial2D("Ground") { friction = 0.65f, bounciness = 0f };
        slickMaterial = new PhysicsMaterial2D("Moving Platform") { friction = 0.9f, bounciness = 0f };

        RedBallLocalization.Initialize();
        uiFont = RedBallLocalization.CreateUiFont();

        LoadSprites();
        SetupCamera();
        SetupAudio();
        SetupUi();
        LoadProgress();
        ShowMainMenu();
    }

    private void Update()
    {
        if (screenMode == ScreenMode.Gameplay && Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }

        for (int i = 0; i < Mathf.Min(LevelCount, 9); i++)
        {
            if (Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha1 + i)))
            {
                TryStartLevel(i);
            }
        }

        if (screenMode != ScreenMode.Gameplay && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)))
        {
            StartContinueLevel();
        }

        if (messageTimer > 0f)
        {
            messageTimer -= Time.deltaTime;
            if (messageTimer <= 0f && messageText != null)
            {
                messageText.text = string.Empty;
            }
        }

        if (statusTimer > 0f)
        {
            statusTimer -= Time.deltaTime;
            if (statusTimer <= 0f)
            {
                ClearStatusText();
            }
        }

        lifeRefreshTimer -= Time.deltaTime;
        if (lifeRefreshTimer <= 0f)
        {
            lifeRefreshTimer = 1f;
            if (ApplyHeartRegen(true))
            {
                RefreshAllUi();
            }
            else
            {
                UpdateLifeTexts();
            }
        }
    }

    public float MoveInput
    {
        get
        {
            if (screenMode != ScreenMode.Gameplay)
            {
                return 0f;
            }

            float keyboard = 0f;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                keyboard -= 1f;
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                keyboard += 1f;
            }

            float stick = joystick != null ? joystick.Value.x : 0f;
            return Mathf.Clamp(keyboard + stick, -1f, 1f);
        }
    }

    public bool ConsumeJumpPressed()
    {
        if (screenMode != ScreenMode.Gameplay)
        {
            return false;
        }

        bool keyboard = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        bool touch = jumpButton != null && jumpButton.ConsumePressed();
        return keyboard || touch;
    }

    public void CollectCoin(GameObject coin)
    {
        score += 10;
        coinsCollectedInLevel += 1;
        RefreshHud();
        PlayPickupSound();
        Destroy(coin);
    }

    public void AddScore(int amount)
    {
        score += amount;
        RefreshHud();
    }

    public void DamagePlayer()
    {
        if (screenMode != ScreenMode.Gameplay || Time.time < respawnLockUntil || player == null)
        {
            return;
        }

        InvalidateCleanRun("damage");
        respawnLockUntil = Time.time + 0.7f;
        SpendHeart();
        RefreshHud();
        PlayHurtSound();

        if (health <= 0)
        {
            ShowMessage(RedBallLocalization.T("message.noHearts"), 1.2f);
            Invoke(nameof(ReturnToMenuAfterNoHearts), 1.1f);
            return;
        }

        ShowMessage(RedBallLocalization.T("message.damage"), 1.2f);
        player.Respawn(playerSpawn);
    }

    public void ActivateCheckpoint(Vector2 spawn)
    {
        if (screenMode != ScreenMode.Gameplay)
        {
            return;
        }

        playerSpawn = spawn;
        ShowMessage(RedBallLocalization.T("message.checkpoint"), 1.1f);
        PlayPickupSound();
    }

    public void Heal(int amount, GameObject pickup)
    {
        int previous = health;
        health = Mathf.Min(MaxHealth, health + amount);
        if (health > previous)
        {
            score += 5;
        }

        RefreshHud();
        PlayPickupSound();
        SaveHearts();
        Destroy(pickup);
    }

    public void CompleteLevel()
    {
        if (levelFinished)
        {
            return;
        }

        levelFinished = true;
        score += Mathf.Max(0, coinsInLevel - coinsCollectedInLevel) * 2;
        MarkLevelCompleted(levelIndex);
        SaveMasteryBadges(levelIndex);
        RefreshHud();
        RefreshLevelButtons();

        string badgeMessage = RedBallLocalization.T("message.badgesPrefix") + " " + GetCompletionBadgeSummary(levelIndex);
        if (levelIndex >= LevelCount - 1)
        {
            ShowMessage(RedBallLocalization.F("message.gameComplete", badgeMessage), 4f);
            Invoke(nameof(ShowLevelSelect), 2f);
            return;
        }

        ShowMessage(RedBallLocalization.F("message.levelComplete", badgeMessage), 1.8f);
        Invoke(nameof(LoadNextLevel), 1.8f);
    }

    public void RestartLevel()
    {
        if (screenMode == ScreenMode.Gameplay && health > 0)
        {
            InvalidateCleanRun("restart");
            LoadLevel(levelIndex);
        }
    }

    public string LoadLevelForSmokeTest(int requestedIndex)
    {
        LoadLevel(requestedIndex);
        return levelRoot != null ? levelRoot.name : string.Empty;
    }

    public static bool HasBadgeInMask(int mask, int index)
    {
        return HasBadge(mask, index);
    }

    private void LoadNextLevel()
    {
        TryStartLevel(levelIndex + 1);
    }

    private void StartContinueLevel()
    {
        TryStartLevel(GetContinueLevelIndex());
    }

    private void TryStartLevel(int requestedIndex)
    {
        ApplyHeartRegen(true);
        int clampedIndex = Mathf.Clamp(requestedIndex, 0, LevelCount - 1);
        if (health <= 0)
        {
            ShowStatus(RedBallLocalization.F("status.noHearts", GetNextHeartText()), 2.2f);
            RefreshAllUi();
            return;
        }

        if (clampedIndex >= unlockedLevelCount)
        {
            ShowStatus(RedBallLocalization.T("status.locked"), 2.2f);
            RefreshLevelButtons();
            return;
        }

        ShowGameplay();
        LoadLevel(clampedIndex);
    }

    private int GetContinueLevelIndex()
    {
        int maxIndex = Mathf.Clamp(unlockedLevelCount - 1, 0, LevelCount - 1);
        for (int i = 0; i <= maxIndex; i++)
        {
            if ((completedLevelMask & (1 << i)) == 0)
            {
                return i;
            }
        }

        return maxIndex;
    }

    private void ReturnToMenuAfterNoHearts()
    {
        ShowMainMenu();
        ShowStatus(RedBallLocalization.T("status.heartsExhausted"), 2.6f);
    }

    private void LoadLevel(int requestedIndex)
    {
        levelIndex = Mathf.Clamp(requestedIndex, 0, LevelCount - 1);
        levelFinished = false;
        attemptCleanRun = true;
        cleanRunFailReason = string.Empty;
        coinsInLevel = 0;
        coinsCollectedInLevel = 0;
        respawnLockUntil = 0f;
        CancelInvoke(nameof(LoadNextLevel));
        ClearLevel();

        string levelName;
        switch (levelIndex)
        {
            case 0:
                levelName = BuildLevelOne();
                break;
            case 1:
                levelName = BuildLevelTwo();
                break;
            case 2:
                levelName = BuildLevelThree();
                break;
            case 3:
                levelName = BuildLevelFour();
                break;
            case 4:
                levelName = BuildLevelFive();
                break;
            case 5:
                levelName = BuildLevelSix();
                break;
            case 6:
                levelName = BuildLevelSeven();
                break;
            case 7:
                levelName = BuildLevelEight();
                break;
            case 8:
                levelName = BuildLevelNine();
                break;
            case 9:
                levelName = BuildLevelTen();
                break;
            case 10:
                levelName = BuildLevelEleven();
                break;
            case 11:
                levelName = BuildLevelTwelve();
                break;
            case 12:
                levelName = BuildLevelThirteen();
                break;
            case 13:
                levelName = BuildLevelFourteen();
                break;
            default:
                levelName = BuildLevelFifteen();
                break;
        }

        CreatePlayer(playerSpawn);
        cameraFollow.Configure(player.transform, GetBoundsMinX(), GetBoundsMaxX(), KillY + 2f, GetBoundsMaxY());
        RefreshHud();
        ShowMessage(RedBallLocalization.LevelName(levelIndex), 1.6f);
    }

    private void LoadProgress()
    {
        completedLevelMask = PlayerPrefs.GetInt(SaveCompletedMaskKey, 0);
        badgeClearMask = PlayerPrefs.GetInt(SaveBadgeClearMaskKey, 0) | completedLevelMask;
        badgeAllCoinsMask = PlayerPrefs.GetInt(SaveBadgeAllCoinsMaskKey, 0);
        badgeCleanRunMask = PlayerPrefs.GetInt(SaveBadgeCleanRunMaskKey, 0);
        unlockedLevelCount = Mathf.Clamp(PlayerPrefs.GetInt(SaveUnlockedCountKey, 1), 1, LevelCount);

        for (int i = 0; i < LevelCount; i++)
        {
            if ((completedLevelMask & (1 << i)) != 0)
            {
                unlockedLevelCount = Mathf.Max(unlockedLevelCount, Mathf.Min(LevelCount, i + 2));
            }
        }

        health = Mathf.Clamp(PlayerPrefs.GetInt(SaveHeartsKey, MaxHealth), 0, MaxHealth);
        heartTimestamp = GetSavedLong(SaveHeartStampKey, GetNowSeconds());
        ApplyHeartRegen(true);
    }

    private void MarkLevelCompleted(int completedIndex)
    {
        completedLevelMask |= 1 << Mathf.Clamp(completedIndex, 0, LevelCount - 1);
        if (completedIndex + 1 < LevelCount)
        {
            unlockedLevelCount = Mathf.Max(unlockedLevelCount, completedIndex + 2);
        }

        SaveProgress();
    }

    private void SaveMasteryBadges(int completedIndex)
    {
        int bit = 1 << Mathf.Clamp(completedIndex, 0, LevelCount - 1);
        badgeClearMask |= bit;
        if (coinsInLevel > 0 && coinsCollectedInLevel >= coinsInLevel)
        {
            badgeAllCoinsMask |= bit;
        }

        if (attemptCleanRun)
        {
            badgeCleanRunMask |= bit;
        }

        SaveBadgeProgress();
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetInt(SaveCompletedMaskKey, completedLevelMask);
        PlayerPrefs.SetInt(SaveUnlockedCountKey, unlockedLevelCount);
        PlayerPrefs.Save();
    }

    private void SaveBadgeProgress()
    {
        PlayerPrefs.SetInt(SaveBadgeClearMaskKey, badgeClearMask);
        PlayerPrefs.SetInt(SaveBadgeAllCoinsMaskKey, badgeAllCoinsMask);
        PlayerPrefs.SetInt(SaveBadgeCleanRunMaskKey, badgeCleanRunMask);
        PlayerPrefs.Save();
    }

    private void InvalidateCleanRun(string reason)
    {
        if (!attemptCleanRun)
        {
            return;
        }

        attemptCleanRun = false;
        cleanRunFailReason = reason;
    }

    private string GetCompletionBadgeSummary(int completedIndex)
    {
        bool clear = HasBadge(badgeClearMask, completedIndex);
        bool allCoins = HasBadge(badgeAllCoinsMask, completedIndex);
        bool cleanRun = HasBadge(badgeCleanRunMask, completedIndex);
        return RedBallUi.GetCompletionBadgeSummary(clear, allCoins, cleanRun, coinsInLevel > 0, cleanRunFailReason);
    }

    private static bool HasBadge(int mask, int index)
    {
        return (mask & (1 << Mathf.Clamp(index, 0, LevelCount - 1))) != 0;
    }

    private void SpendHeart()
    {
        ApplyHeartRegen(true);
        bool wasFull = health >= MaxHealth;
        health = Mathf.Max(0, health - 1);
        if (wasFull)
        {
            heartTimestamp = GetNowSeconds();
        }

        SaveHearts();
    }

    private bool ApplyHeartRegen(bool save)
    {
        long now = GetNowSeconds();
        int previousHealth = health;
        long previousStamp = heartTimestamp;

        if (health >= MaxHealth)
        {
            health = MaxHealth;
            if (previousHealth < MaxHealth)
            {
                heartTimestamp = now;
            }
        }
        else
        {
            long elapsed = Math.Max(0L, now - heartTimestamp);
            int gained = Mathf.FloorToInt(elapsed / (float)HeartRechargeSeconds);
            if (gained > 0)
            {
                health = Mathf.Min(MaxHealth, health + gained);
                heartTimestamp += gained * HeartRechargeSeconds;
                if (health >= MaxHealth)
                {
                    heartTimestamp = now;
                }
            }
        }

        if (save && (previousHealth != health || previousStamp != heartTimestamp))
        {
            SaveHearts();
        }

        return previousHealth != health || previousStamp != heartTimestamp;
    }

    private void SaveHearts()
    {
        PlayerPrefs.SetInt(SaveHeartsKey, health);
        PlayerPrefs.SetString(SaveHeartStampKey, heartTimestamp.ToString());
        PlayerPrefs.Save();
    }

    private static long GetSavedLong(string key, long fallback)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return fallback;
        }

        long value;
        return long.TryParse(PlayerPrefs.GetString(key), out value) ? value : fallback;
    }

    private static long GetNowSeconds()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    private string BuildLevelOne()
    {
        BeginLevel("Level 01 - Yuvarlanma Dersi", -8f, 36f, -8f, 7f);
        playerSpawn = new Vector2(-5.8f, -1.75f);

        AddSkyDressing(-8f, 36f, -3f);

        AddPlatform(-7f, -3f, 12);
        AddPlatform(7f, -3f, 8);
        AddPlatform(18f, -2.75f, 7);
        AddPlatform(28f, -2.55f, 6);
        AddHalfPlatform(3.5f, -1.55f, 3);
        AddHalfPlatform(14f, -1.25f, 3);

        AddCoinLine(-4.5f, -1.55f, 6, 1f);
        AddCoinArc(new Vector2(4.5f, -0.7f), 4, 0.9f, 200f, 340f);
        AddCoinLine(9f, -1.55f, 5, 0.95f);
        AddCoinLine(19.2f, -1.25f, 5, 0.9f);
        AddCoinLine(29.2f, -1.05f, 4, 0.85f);
        AddRouteSparkle(new Vector2(4.6f, -0.15f), 0.62f);
        AddRouteSparkle(new Vector2(30.4f, -0.52f), 0.58f);
        AddWarningSign(new Vector2(22.8f, -1.42f));
        AddTriangleHazard(24f, -2.23f, false);
        AddGoal(new Vector2(32.4f, -1.55f));
        return "Level 1: Yuvarlanma Dersi";
    }

    private string BuildLevelTwo()
    {
        BeginLevel("Level 02 - Ilk Tehlike", -8f, 43f, -8.5f, 8f);
        playerSpawn = new Vector2(-5.5f, -1.95f);

        AddSkyDressing(-8f, 43f, -3.2f);

        AddPlatform(-7f, -3.2f, 11);
        AddPlatform(6f, -3.08f, 6);
        AddPlatform(16f, -2.85f, 7);
        AddPlatform(27f, -2.6f, 8);
        AddPlatform(38f, -2.42f, 4);
        AddHalfPlatform(2f, -1.55f, 3);
        AddHalfPlatform(12f, -0.95f, 3);

        AddCoinLine(-4.4f, -1.75f, 5, 1f);
        AddCoinArc(new Vector2(7.5f, -1.6f), 4, 1f, 205f, 335f);
        AddCoinLine(17.2f, -1.4f, 5, 0.9f);
        AddCoinLine(28.5f, -1.15f, 5, 0.9f);
        AddRouteSparkle(new Vector2(7.5f, -0.98f), 0.58f);
        AddWarningSign(new Vector2(12.2f, -1.6f));
        AddWarningSign(new Vector2(25.5f, -1.4f));
        AddTriangleHazard(13.4f, -2.48f, false);
        AddTriangleHazard(24.2f, -2.3f, true);
        AddEnemy(new Vector2(30.5f, -1.68f), 28.4f, 33.2f, 1.15f, "green_body_square", "face_f");
        AddGoal(new Vector2(40.2f, -1.42f));
        return "Level 2: Ilk Tehlike";
    }

    private string BuildLevelThree()
    {
        BeginLevel("Level 03 - Sekme Dersi", -8f, 47f, -9f, 9f);
        playerSpawn = new Vector2(-5.5f, -2.25f);

        AddSkyDressing(-8f, 47f, -3.45f);

        AddPlatform(-7f, -3.45f, 11);
        AddBouncePad(new Vector2(3.1f, -2.83f), 13.2f);
        AddHalfPlatform(7f, -0.12f, 4);
        AddPlatform(14f, -2.7f, 7);
        AddMovingPlatform(new Vector2(24f, -1.1f), new Vector2(28f, -1.1f), 3, 1.25f);
        AddPlatform(32f, -2.25f, 8);
        AddHalfPlatform(39f, 0f, 3);

        AddCoinLine(-4.5f, -2.05f, 5, 1f);
        AddCoinArc(new Vector2(8.5f, 1.05f), 5, 1f, 195f, 345f);
        AddCoinLine(15.4f, -1.25f, 5, 0.9f);
        AddCoinArc(new Vector2(26f, 0.2f), 4, 0.95f, 205f, 335f);
        AddCoinLine(33.5f, -0.8f, 5, 0.9f);
        AddRouteSparkle(new Vector2(3.1f, -2.24f), 0.7f);
        AddRouteSparkle(new Vector2(26f, 0.82f), 0.62f);
        AddWarningSign(new Vector2(17.4f, -1.38f));
        AddWarningSign(new Vector2(29.1f, -1.72f));
        AddTriangleHazard(18.6f, -2.18f, false);
        AddTriangleHazard(30.5f, -2.68f, true);
        AddEnemy(new Vector2(35.4f, -1.33f), 33.2f, 38.4f, 1.25f, "blue_body_square", "face_g");
        AddGoal(new Vector2(41.1f, 1f));
        return "Level 3: Sekme Dersi";
    }

    private string BuildLevelFour()
    {
        BeginLevel("Level 04 - Ince Adim", -8f, 52f, -9f, 10f);
        playerSpawn = new Vector2(-5.5f, -2.1f);

        AddSkyDressing(-8f, 52f, -3.35f);

        AddPlatform(-7f, -3.35f, 10);
        AddHalfPlatform(5f, -1.75f, 3);
        AddHalfPlatform(10.5f, -0.5f, 3);
        AddPlatform(16f, -2.8f, 6);
        AddHalfPlatform(24.5f, -1.1f, 3);
        AddPlatform(31f, -2.55f, 7);
        AddHalfPlatform(40f, -0.7f, 3);
        AddPlatform(45f, -2.25f, 6);

        AddCoinLine(-4.5f, -1.95f, 5, 1f);
        AddCoinArc(new Vector2(6f, -0.65f), 3, 0.75f, 205f, 335f);
        AddCoinArc(new Vector2(11.5f, 0.55f), 3, 0.75f, 205f, 335f);
        AddCoinLine(17.1f, -1.4f, 4, 0.9f);
        AddCoinArc(new Vector2(25.5f, -0.05f), 3, 0.75f, 205f, 335f);
        AddCoinLine(32.2f, -1.15f, 5, 0.85f);
        AddRouteSparkle(new Vector2(11.5f, 1.08f), 0.54f);
        AddRouteSparkle(new Vector2(25.5f, 0.48f), 0.54f);
        AddWarningSign(new Vector2(13f, -1.62f));
        AddWarningSign(new Vector2(37.2f, -1.08f));
        AddTriangleHazard(14.2f, -2.55f, true);
        AddTriangleHazard(28.8f, -2.5f, false);
        AddTriangleHazard(38.5f, -2.02f, true);
        AddGoal(new Vector2(48.9f, -1.25f));
        return "Level 4: Ince Adim";
    }

    private string BuildLevelFive()
    {
        BeginLevel("Level 05 - Devriye Yolu", -8f, 54f, -9f, 9f);
        playerSpawn = new Vector2(-5.6f, -1.98f);

        AddSkyDressing(-8f, 54f, -3.25f);

        AddPlatform(-7f, -3.25f, 11);
        AddPlatform(6f, -3f, 8);
        AddPlatform(18f, -2.7f, 8);
        AddHalfPlatform(28f, -0.9f, 4);
        AddPlatform(35f, -2.35f, 8);
        AddPlatform(46f, -2.1f, 6);

        AddCoinLine(-4.4f, -1.85f, 5, 1f);
        AddCoinLine(7.4f, -1.6f, 5, 0.9f);
        AddCoinLine(19.5f, -1.28f, 5, 0.9f);
        AddCoinArc(new Vector2(29.8f, 0.12f), 4, 0.9f, 200f, 340f);
        AddCoinLine(36.6f, -0.95f, 5, 0.85f);
        AddRouteSparkle(new Vector2(9.5f, -1.22f), 0.58f);
        AddRouteSparkle(new Vector2(29.8f, 0.72f), 0.62f);
        AddWarningSign(new Vector2(13.8f, -1.58f));
        AddWarningSign(new Vector2(42.8f, -0.92f));
        AddTriangleHazard(15f, -2.48f, false);
        AddTriangleHazard(27.2f, -2.38f, true);
        AddTriangleHazard(44.1f, -1.82f, false);
        AddEnemy(new Vector2(9.5f, -2.08f), 7.1f, 12.2f, 1.15f, "purple_body_square", "face_g");
        AddEnemy(new Vector2(39f, -1.43f), 36.3f, 41.8f, 1.35f, "green_body_square", "face_f");
        AddGoal(new Vector2(49.6f, -1.1f));
        return "Level 5: Devriye Yolu";
    }

    private string BuildLevelSix()
    {
        BeginLevel("Level 06 - Hareketli Kopru", -8f, 58f, -9.5f, 10f);
        playerSpawn = new Vector2(-5.5f, -2.18f);

        AddSkyDressing(-8f, 58f, -3.45f);

        AddPlatform(-7f, -3.45f, 10);
        AddHalfPlatform(5f, -1.55f, 3);
        AddMovingPlatform(new Vector2(11.5f, -1.45f), new Vector2(17f, -1.45f), 3, 1.15f);
        AddPlatform(22f, -2.85f, 7);
        AddMovingPlatform(new Vector2(33f, -0.9f), new Vector2(38.5f, -0.9f), 3, 1.45f);
        AddPlatform(43f, -2.45f, 8);
        AddHalfPlatform(52f, -0.55f, 3);

        AddCoinLine(-4.4f, -2.05f, 5, 1f);
        AddCoinArc(new Vector2(14.8f, -0.2f), 4, 0.95f, 205f, 335f);
        AddCoinLine(23.3f, -1.45f, 5, 0.9f);
        AddCoinArc(new Vector2(35.8f, 0.28f), 4, 0.95f, 205f, 335f);
        AddCoinLine(44.3f, -1.05f, 5, 0.9f);
        AddRouteSparkle(new Vector2(14.8f, 0.43f), 0.62f);
        AddRouteSparkle(new Vector2(35.8f, 0.98f), 0.62f);
        AddWarningSign(new Vector2(19.2f, -1.58f));
        AddWarningSign(new Vector2(31.8f, -1.7f));
        AddTriangleHazard(20.4f, -2.55f, false);
        AddTriangleHazard(30.6f, -2.72f, true);
        AddEnemy(new Vector2(46.5f, -1.53f), 44.2f, 49.4f, 1.45f, "blue_body_square", "face_d");
        AddGoal(new Vector2(53.3f, 0.45f));
        return "Level 6: Hareketli Kopru";
    }

    private string BuildLevelSeven()
    {
        BeginLevel("Level 07 - Sekme Merdiveni", -8f, 60f, -10f, 12f);
        playerSpawn = new Vector2(-5.5f, -2.22f);

        AddSkyDressing(-8f, 60f, -3.5f);

        AddPlatform(-7f, -3.5f, 10);
        AddBouncePad(new Vector2(2.6f, -2.88f), 13.6f);
        AddHalfPlatform(7f, 0.05f, 4);
        AddPlatform(14f, -1.35f, 7);
        AddBouncePad(new Vector2(18.5f, -0.73f), 12.9f);
        AddHalfPlatform(24f, 1.55f, 4);
        AddPlatform(31f, 0.05f, 7);
        AddMovingPlatform(new Vector2(42f, 1.05f), new Vector2(47f, 1.05f), 3, 1.4f);
        AddPlatform(51f, 0.55f, 6);

        AddCoinLine(-4.3f, -2.1f, 5, 1f);
        AddCoinArc(new Vector2(8.6f, 1.2f), 5, 1f, 195f, 345f);
        AddCoinLine(15.4f, 0.08f, 5, 0.9f);
        AddCoinArc(new Vector2(25.5f, 2.7f), 5, 0.9f, 195f, 345f);
        AddCoinLine(32.4f, 1.4f, 5, 0.9f);
        AddCoinArc(new Vector2(44.5f, 2.15f), 4, 0.85f, 205f, 335f);
        AddRouteSparkle(new Vector2(2.6f, -2.22f), 0.68f);
        AddRouteSparkle(new Vector2(18.5f, -0.06f), 0.68f);
        AddRouteSparkle(new Vector2(25.5f, 3.32f), 0.58f);
        AddWarningSign(new Vector2(11.2f, -2f));
        AddWarningSign(new Vector2(38f, -1.78f));
        AddTriangleHazard(12.4f, -2.98f, true);
        AddTriangleHazard(28.4f, -2.82f, false);
        AddTriangleHazard(39.2f, -2.78f, true);
        AddEnemy(new Vector2(34.3f, 0.97f), 32f, 36.7f, 1.4f, "purple_body_square", "face_f");
        AddGoal(new Vector2(54.4f, 1.55f));
        return "Level 7: Sekme Merdiveni";
    }

    private string BuildLevelEight()
    {
        BeginLevel("Level 08 - Ritim Koprusu", -8f, 62f, -10f, 11f);
        playerSpawn = new Vector2(-5.6f, -2.1f);

        AddSkyDressing(-8f, 62f, -3.35f);

        AddPlatform(-7f, -3.35f, 9);
        AddHalfPlatform(5f, -1.65f, 3);
        AddPlatform(10f, -3.05f, 5);
        AddHalfPlatform(18f, -1.25f, 3);
        AddPlatform(23f, -2.8f, 5);
        AddHalfPlatform(31f, -0.9f, 3);
        AddPlatform(36f, -2.55f, 5);
        AddMovingPlatform(new Vector2(45f, -0.85f), new Vector2(50f, -0.85f), 3, 1.65f);
        AddPlatform(54f, -2.25f, 6);

        AddCoinLine(-4.5f, -1.95f, 4, 1f);
        AddCoinArc(new Vector2(6f, -0.55f), 3, 0.8f, 205f, 335f);
        AddCoinLine(10.9f, -1.65f, 4, 0.85f);
        AddCoinArc(new Vector2(19f, -0.15f), 3, 0.8f, 205f, 335f);
        AddCoinLine(23.8f, -1.4f, 4, 0.85f);
        AddCoinArc(new Vector2(32f, 0.2f), 3, 0.8f, 205f, 335f);
        AddCoinArc(new Vector2(47.5f, 0.1f), 4, 0.85f, 205f, 335f);
        AddRouteSparkle(new Vector2(19f, 0.42f), 0.54f);
        AddRouteSparkle(new Vector2(47.5f, 0.68f), 0.6f);
        AddWarningSign(new Vector2(7.2f, -1.88f));
        AddWarningSign(new Vector2(41.4f, -1.28f));
        AddTriangleHazard(8.4f, -2.78f, false);
        AddTriangleHazard(16.1f, -2.72f, true);
        AddTriangleHazard(29.2f, -2.52f, false);
        AddTriangleHazard(42.8f, -2.28f, true);
        AddEnemy(new Vector2(12.4f, -2.13f), 10.7f, 14f, 1.35f, "green_body_square", "face_g");
        AddEnemy(new Vector2(38.3f, -1.63f), 36.6f, 40.1f, 1.55f, "blue_body_square", "face_d");
        AddGoal(new Vector2(57.4f, -1.25f));
        return "Level 8: Ritim Koprusu";
    }

    private string BuildLevelNine()
    {
        BeginLevel("Level 09 - Yuksek Hat", -8f, 64f, -10.5f, 14f);
        playerSpawn = new Vector2(-5.5f, -2.25f);

        AddSkyDressing(-8f, 64f, -3.55f);

        AddPlatform(-7f, -3.55f, 10);
        AddHalfPlatform(5f, -1f, 3);
        AddHalfPlatform(10.5f, 0.8f, 3);
        AddPlatform(16f, -0.45f, 6);
        AddHalfPlatform(25f, 1.7f, 3);
        AddPlatform(31f, 0.45f, 6);
        AddMovingPlatform(new Vector2(40f, 2.4f), new Vector2(46f, 2.4f), 3, 1.35f);
        AddPlatform(51f, 1.35f, 7);

        AddCoinLine(-4.4f, -2.15f, 5, 1f);
        AddCoinArc(new Vector2(6f, 0.05f), 3, 0.75f, 205f, 335f);
        AddCoinArc(new Vector2(11.5f, 1.85f), 3, 0.75f, 205f, 335f);
        AddCoinLine(17.1f, 0.9f, 4, 0.9f);
        AddCoinArc(new Vector2(26f, 2.75f), 3, 0.75f, 205f, 335f);
        AddCoinLine(32.1f, 1.8f, 4, 0.9f);
        AddCoinArc(new Vector2(43f, 3.5f), 4, 0.9f, 205f, 335f);
        AddCoinLine(52.5f, 2.75f, 5, 0.85f);
        AddRouteSparkle(new Vector2(11.5f, 2.35f), 0.54f);
        AddRouteSparkle(new Vector2(43f, 4.12f), 0.64f);
        AddRouteSparkle(new Vector2(54.1f, 3.22f), 0.58f);
        AddWarningSign(new Vector2(21.6f, -1.82f));
        AddWarningSign(new Vector2(39.2f, -1.68f));
        AddTriangleHazard(22.8f, -2.82f, false);
        AddTriangleHazard(38f, -2.72f, true);
        AddEnemy(new Vector2(33.8f, 1.37f), 32f, 36.2f, 1.5f, "purple_body_square", "face_g");
        AddGoal(new Vector2(56f, 2.35f));
        return "Level 9: Yuksek Hat";
    }

    private string BuildLevelTen()
    {
        BeginLevel("Level 10 - Inis Cikisi", -8f, 66f, -11f, 13f);
        playerSpawn = new Vector2(-5.5f, -2.1f);

        AddSkyDressing(-8f, 66f, -3.35f);

        AddPlatform(-7f, -3.35f, 9);
        AddHalfPlatform(4.5f, -0.9f, 3);
        AddPlatform(11f, 0.45f, 6);
        AddMovingPlatform(new Vector2(20f, -1.2f), new Vector2(25f, -1.2f), 3, 1.6f);
        AddPlatform(29f, -2.95f, 7);
        AddHalfPlatform(38f, -0.6f, 3);
        AddPlatform(44f, 0.75f, 6);
        AddMovingPlatform(new Vector2(54f, -1.05f), new Vector2(59f, -1.05f), 3, 1.75f);
        AddPlatform(61f, -2.45f, 5);

        AddCoinLine(-4.4f, -1.95f, 4, 1f);
        AddCoinArc(new Vector2(5.5f, 0.15f), 3, 0.8f, 205f, 335f);
        AddCoinLine(12.2f, 1.75f, 4, 0.9f);
        AddCoinArc(new Vector2(22.5f, 0.05f), 4, 0.9f, 205f, 335f);
        AddCoinLine(30.4f, -1.55f, 5, 0.9f);
        AddCoinArc(new Vector2(39f, 0.55f), 3, 0.75f, 205f, 335f);
        AddCoinLine(45.2f, 2.1f, 4, 0.85f);
        AddRouteSparkle(new Vector2(22.5f, 0.68f), 0.62f);
        AddRouteSparkle(new Vector2(55.8f, -0.4f), 0.6f);
        AddWarningSign(new Vector2(26f, -1.72f));
        AddWarningSign(new Vector2(50.6f, -1.6f));
        AddTriangleHazard(27.2f, -2.72f, true);
        AddTriangleHazard(36.8f, -2.58f, false);
        AddTriangleHazard(51.8f, -2.6f, true);
        AddEnemy(new Vector2(14f, 1.37f), 12f, 16f, 1.55f, "green_body_square", "face_f");
        AddEnemy(new Vector2(47f, 1.67f), 45f, 49f, 1.7f, "blue_body_square", "face_d");
        AddGoal(new Vector2(63.6f, -1.45f));
        return "Level 10: Inis Cikisi";
    }

    private string BuildLevelEleven()
    {
        BeginLevel("Level 11 - Devriye Kapisi", -8f, 68f, -11f, 12f);
        playerSpawn = new Vector2(-5.5f, -2.08f);

        AddSkyDressing(-8f, 68f, -3.32f);

        AddPlatform(-7f, -3.32f, 10);
        AddPlatform(6f, -3.05f, 6);
        AddHalfPlatform(15f, -1.15f, 3);
        AddPlatform(20f, -2.8f, 8);
        AddHalfPlatform(31f, -0.85f, 3);
        AddPlatform(37f, -2.55f, 7);
        AddMovingPlatform(new Vector2(48f, -0.75f), new Vector2(54f, -0.75f), 3, 1.55f);
        AddPlatform(58f, -2.25f, 7);

        AddCoinLine(-4.4f, -1.9f, 5, 1f);
        AddCoinLine(6.8f, -1.62f, 4, 0.85f);
        AddCoinArc(new Vector2(16f, -0.05f), 3, 0.8f, 205f, 335f);
        AddCoinLine(21.2f, -1.4f, 5, 0.9f);
        AddCoinArc(new Vector2(32f, 0.25f), 3, 0.75f, 205f, 335f);
        AddCoinLine(38.2f, -1.15f, 5, 0.85f);
        AddCoinArc(new Vector2(51f, 0.35f), 4, 0.9f, 205f, 335f);
        AddRouteSparkle(new Vector2(16f, 0.5f), 0.54f);
        AddRouteSparkle(new Vector2(51f, 0.98f), 0.62f);
        AddWarningSign(new Vector2(12f, -1.72f));
        AddWarningSign(new Vector2(44.4f, -1.42f));
        AddTriangleHazard(13.1f, -2.72f, false);
        AddTriangleHazard(29.6f, -2.52f, true);
        AddTriangleHazard(45.6f, -2.42f, false);
        AddTriangleHazard(56.2f, -2.18f, true);
        AddEnemy(new Vector2(8.3f, -2.13f), 6.6f, 10.4f, 1.45f, "purple_body_square", "face_g");
        AddEnemy(new Vector2(23.8f, -1.88f), 21f, 26.4f, 1.65f, "green_body_square", "face_f");
        AddEnemy(new Vector2(40f, -1.63f), 38f, 42.6f, 1.8f, "blue_body_square", "face_d");
        AddGoal(new Vector2(62.2f, -1.25f));
        return "Level 11: Devriye Kapisi";
    }

    private string BuildLevelTwelve()
    {
        BeginLevel("Level 12 - Keskin Sekme", -8f, 72f, -12f, 14f);
        playerSpawn = new Vector2(-5.5f, -2.32f);

        AddSkyDressing(-8f, 72f, -3.6f);

        AddPlatform(-7f, -3.6f, 10);
        AddBouncePad(new Vector2(2.8f, -2.98f), 14.1f);
        AddHalfPlatform(7f, 0.35f, 3);
        AddMovingPlatform(new Vector2(13.5f, 1.35f), new Vector2(19.5f, 1.35f), 3, 1.6f);
        AddPlatform(25f, -0.4f, 6);
        AddBouncePad(new Vector2(29.2f, 0.22f), 12.8f);
        AddHalfPlatform(36f, 2.25f, 3);
        AddMovingPlatform(new Vector2(42.5f, 1.15f), new Vector2(49f, 1.15f), 3, 1.9f);
        AddPlatform(54f, -0.75f, 7);
        AddPlatform(64f, -2.15f, 6);

        AddCoinLine(-4.4f, -2.2f, 5, 1f);
        AddCoinArc(new Vector2(8f, 1.4f), 4, 0.9f, 200f, 340f);
        AddCoinArc(new Vector2(18f, 2.5f), 4, 0.9f, 205f, 335f);
        AddCoinLine(26.2f, 0.95f, 4, 0.85f);
        AddCoinArc(new Vector2(37f, 3.2f), 4, 0.9f, 200f, 340f);
        AddCoinArc(new Vector2(47f, 2.25f), 4, 0.9f, 205f, 335f);
        AddCoinLine(55.4f, 0.6f, 5, 0.85f);
        AddRouteSparkle(new Vector2(2.8f, -2.36f), 0.68f);
        AddRouteSparkle(new Vector2(18f, 3.12f), 0.62f);
        AddRouteSparkle(new Vector2(47f, 2.88f), 0.62f);
        AddWarningSign(new Vector2(22f, -1.8f));
        AddWarningSign(new Vector2(51f, -1.4f));
        AddTriangleHazard(12.4f, -2.98f, false);
        AddTriangleHazard(23.2f, -2.78f, true);
        AddTriangleHazard(33.5f, -2.65f, false);
        AddTriangleHazard(52f, -2.4f, true);
        AddTriangleHazard(62.5f, -2.15f, false);
        AddEnemy(new Vector2(57.4f, 0.18f), 55f, 60f, 1.85f, "purple_body_square", "face_f");
        AddGoal(new Vector2(67.2f, -1.15f));
        return "Level 12: Keskin Sekme";
    }

    private string BuildLevelThirteen()
    {
        BeginLevel("Level 13 - Son Kosu", -8f, 78f, -12f, 14f);
        playerSpawn = new Vector2(-5.6f, -2.2f);

        AddSkyDressing(-8f, 78f, -3.45f);

        AddPlatform(-7f, -3.45f, 9);
        AddPlatform(5f, -3.1f, 5);
        AddHalfPlatform(12.5f, -1.05f, 3);
        AddMovingPlatform(new Vector2(19f, -0.85f), new Vector2(25f, -0.85f), 3, 1.7f);
        AddPlatform(30f, -2.75f, 7);
        AddBouncePad(new Vector2(36.2f, -2.13f), 13.4f);
        AddHalfPlatform(42f, 1.2f, 3);
        AddPlatform(48f, -0.3f, 6);
        AddMovingPlatform(new Vector2(58f, 1.25f), new Vector2(64f, 1.25f), 3, 2f);
        AddPlatform(68f, -1.45f, 8);
        AddCheckpoint(new Vector2(43f, 2.15f));

        AddCoinLine(-4.5f, -2.05f, 4, 1f);
        AddCoinLine(5.7f, -1.7f, 4, 0.85f);
        AddCoinArc(new Vector2(14f, 0.05f), 3, 0.8f, 205f, 335f);
        AddCoinArc(new Vector2(24f, 0.25f), 4, 0.9f, 205f, 335f);
        AddCoinLine(32.2f, -1.35f, 5, 0.85f);
        AddCoinArc(new Vector2(43f, 2.25f), 4, 0.9f, 200f, 340f);
        AddCoinLine(49.1f, 1.05f, 4, 0.85f);
        AddCoinArc(new Vector2(61f, 2.35f), 4, 0.9f, 205f, 335f);
        AddCoinLine(69.3f, -0.05f, 5, 0.85f);
        AddRouteSparkle(new Vector2(21.8f, -0.1f), 0.62f);
        AddRouteSparkle(new Vector2(36.2f, -1.5f), 0.68f);
        AddRouteSparkle(new Vector2(43f, 2.82f), 0.68f);
        AddRouteSparkle(new Vector2(61f, 2.98f), 0.62f);
        AddWarningSign(new Vector2(39.2f, -1.55f));
        AddWarningSign(new Vector2(54.8f, -0.65f));
        AddWarningSign(new Vector2(65.2f, -0.55f));
        AddTriangleHazard(11.1f, -2.8f, false);
        AddTriangleHazard(18.4f, -2.72f, true);
        AddTriangleHazard(29.2f, -2.55f, false);
        AddTriangleHazard(40.2f, -2.52f, true);
        AddTriangleHazard(46.2f, -2.38f, false);
        AddTriangleHazard(55.8f, -2.4f, true);
        AddTriangleHazard(66.2f, -2.1f, false);
        AddEnemy(new Vector2(6.8f, -2.18f), 5.5f, 9f, 1.6f, "purple_body_square", "face_g");
        AddEnemy(new Vector2(33.7f, -1.83f), 32f, 36.3f, 1.85f, "green_body_square", "face_f");
        AddEnemy(new Vector2(50.4f, 0.62f), 49f, 53.2f, 1.95f, "blue_body_square", "face_d");
        AddEnemy(new Vector2(71.5f, -0.53f), 69.5f, 74.5f, 2.05f, "purple_body_square", "face_f");
        AddGoal(new Vector2(74.1f, -0.45f));
        return "Level 13: Son Kosu";
    }

    private string BuildLevelFourteen()
    {
        BeginLevel("Level 14 - Asansor Bahcesi", -8f, 78f, -12f, 15f);
        playerSpawn = new Vector2(-5.6f, -2.15f);

        AddSkyDressing(-8f, 78f, -3.45f);

        AddPlatform(-7f, -3.45f, 10);
        AddPlatform(6f, -3.25f, 8);
        AddPlatform(17f, -3.2f, 9);
        AddMovingPlatform(new Vector2(22f, -2.1f), new Vector2(22f, 1.65f), 3, 1.25f);
        AddPlatform(28f, 1.05f, 8);
        AddCheckpoint(new Vector2(31.5f, 2.05f));

        AddPlatform(39f, -2.75f, 8);
        AddMovingPlatform(new Vector2(48f, -1.65f), new Vector2(48f, 2.75f), 3, 1.35f);
        AddPlatform(52f, 1.65f, 8);
        AddPlatform(65f, -2.35f, 9);

        AddHalfPlatform(39.5f, 1.85f, 3);
        AddHalfPlatform(44.5f, 3.55f, 3);
        AddHalfPlatform(51f, 4.65f, 3);
        AddHalfPlatform(58f, 3.65f, 3);

        AddCoinLine(-4.5f, -2.05f, 5, 1f);
        AddCoinLine(7.4f, -1.85f, 5, 0.9f);
        AddCoinArc(new Vector2(22f, 2.65f), 5, 0.9f, 200f, 340f);
        AddCoinLine(29.5f, 2.4f, 5, 0.85f);
        AddCoinLine(40f, 2.95f, 3, 0.75f);
        AddCoinLine(45f, 4.65f, 3, 0.75f);
        AddCoinLine(51.5f, 5.75f, 3, 0.75f);
        AddCoinLine(58.5f, 4.75f, 3, 0.75f);
        AddCoinLine(53.2f, 3.0f, 5, 0.85f);
        AddCoinLine(66.5f, -0.95f, 5, 0.85f);

        AddRouteSparkle(new Vector2(22f, 2.35f), 0.66f);
        AddRouteSparkle(new Vector2(48f, 3.35f), 0.68f);
        AddWarningSign(new Vector2(38.1f, -1.72f));
        AddWarningSign(new Vector2(46.2f, -0.55f));
        AddTriangleHazard(36.8f, -2.48f, false);
        AddTriangleHazard(46.2f, -2.48f, true);
        AddTriangleHazard(62.4f, -2.18f, false);
        AddEnemy(new Vector2(68.6f, -1.43f), 66f, 72.4f, 1.65f, "green_body_square", "face_f");
        AddGoal(new Vector2(73.8f, -1.35f));
        return "Level 14: Asansor Bahcesi";
    }

    private string BuildLevelFifteen()
    {
        BeginLevel("Level 15 - Kirik Taslar", -8f, 82f, -13f, 13f);
        playerSpawn = new Vector2(-5.6f, -2.15f);

        AddSkyDressing(-8f, 82f, -3.45f);

        AddPlatform(-7f, -3.45f, 11);
        AddCrumblingTile(new Vector2(4.5f, -1.7f), 2, 0.65f);
        AddPlatform(7f, -3.25f, 7);
        AddHalfPlatform(16f, -1.5f, 3);
        AddPlatform(21f, -3.1f, 6);
        AddCheckpoint(new Vector2(24f, -2.0f));

        AddPlatform(31f, -2.85f, 4);
        AddCrumblingTile(new Vector2(38f, -1.45f), 2, 0.55f);
        AddPlatform(42f, -2.65f, 5);
        AddCrumblingTile(new Vector2(51f, -0.95f), 3, 0.5f);
        AddPlatform(55f, -2.4f, 7);
        AddHalfPlatform(67f, -0.65f, 3);
        AddPlatform(72f, -2.2f, 7);

        AddCoinLine(-4.5f, -2.05f, 5, 1f);
        AddCoinArc(new Vector2(4.5f, -0.55f), 4, 0.85f, 205f, 335f);
        AddCoinLine(8.4f, -1.85f, 5, 0.85f);
        AddCoinArc(new Vector2(17f, -0.3f), 3, 0.75f, 205f, 335f);
        AddCoinLine(22.2f, -1.7f, 4, 0.85f);
        AddCoinArc(new Vector2(38f, -0.15f), 4, 0.85f, 205f, 335f);
        AddCoinLine(43f, -1.25f, 4, 0.8f);
        AddCoinArc(new Vector2(51f, 0.35f), 5, 0.9f, 205f, 335f);
        AddCoinLine(56.4f, -1f, 5, 0.85f);
        AddCoinArc(new Vector2(68f, 0.35f), 3, 0.75f, 205f, 335f);
        AddCoinLine(73.3f, -0.8f, 5, 0.85f);

        AddRouteSparkle(new Vector2(4.5f, 0.18f), 0.62f);
        AddRouteSparkle(new Vector2(51f, 0.95f), 0.68f);
        AddWarningSign(new Vector2(4.5f, -2.55f));
        AddWarningSign(new Vector2(34.4f, -2.05f));
        AddWarningSign(new Vector2(48.2f, -1.65f));
        AddTriangleHazard(29.2f, -2.58f, false);
        AddTriangleHazard(54.2f, -2.18f, true);
        AddTriangleHazard(70.2f, -2f, false);
        AddEnemy(new Vector2(58.3f, -1.48f), 56.2f, 61.2f, 1.55f, "blue_body_square", "face_d");
        AddGoal(new Vector2(77.1f, -1.2f));
        return "Level 15: Kirik Taslar";
    }

    private void BeginLevel(string name, float minX, float maxX, float minY, float maxY)
    {
        var rootObject = new GameObject(name);
        rootObject.transform.SetParent(transform, false);
        levelRoot = rootObject.transform;
        levelObjects.Add(rootObject);
        BoundsMarker.MinX = minX;
        BoundsMarker.MaxX = maxX;
        BoundsMarker.MaxY = maxY;
        KillY = minY;
    }

    private void ClearLevel()
    {
        for (int i = levelObjects.Count - 1; i >= 0; i--)
        {
            if (levelObjects[i] != null)
            {
                Destroy(levelObjects[i]);
            }
        }

        levelObjects.Clear();
        player = null;
        levelRoot = null;
    }

    private void CreatePlayer(Vector2 spawn)
    {
        var playerObject = new GameObject("Player");
        playerObject.transform.SetParent(levelRoot, false);
        playerObject.transform.position = spawn;
        levelObjects.Add(playerObject);

        var rollVisual = new GameObject("Roll Visual");
        rollVisual.transform.SetParent(playerObject.transform, false);

        var body = new GameObject("Body");
        body.transform.SetParent(rollVisual.transform, false);
        var bodyRenderer = body.AddComponent<SpriteRenderer>();
        bodyRenderer.sprite = sprites["red_body_circle"];
        bodyRenderer.sortingOrder = 20;

        var face = new GameObject("Face");
        face.transform.SetParent(rollVisual.transform, false);
        face.transform.localPosition = new Vector3(0f, 0.05f, 0f);
        var faceRenderer = face.AddComponent<SpriteRenderer>();
        faceRenderer.sprite = sprites["face_a"];
        faceRenderer.sortingOrder = 21;

        AddRollMark(rollVisual.transform, new Vector2(-0.23f, 0.22f), 0.16f, new Color(1f, 0.72f, 0.72f, 0.9f), 22);
        AddRollMark(rollVisual.transform, new Vector2(0.28f, -0.18f), 0.11f, new Color(0.78f, 0.06f, 0.08f, 0.75f), 22);
        AddRollMark(rollVisual.transform, new Vector2(-0.1f, -0.31f), 0.08f, new Color(0.55f, 0f, 0.02f, 0.55f), 22);

        var shadow = new GameObject("Shadow");
        shadow.transform.SetParent(playerObject.transform, false);
        shadow.transform.localPosition = new Vector3(0f, -0.48f, 0f);
        shadow.transform.localScale = new Vector3(0.85f, 0.65f, 1f);
        var shadowRenderer = shadow.AddComponent<SpriteRenderer>();
        shadowRenderer.sprite = sprites["shadow"];
        shadowRenderer.sortingOrder = 8;

        var rigidbody2D = playerObject.AddComponent<Rigidbody2D>();
        rigidbody2D.gravityScale = 1.05f;
        rigidbody2D.mass = 1f;
        rigidbody2D.interpolation = RigidbodyInterpolation2D.Interpolate;
        rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rigidbody2D.freezeRotation = true;

        var collider = playerObject.AddComponent<CircleCollider2D>();
        collider.radius = 0.48f;
        collider.sharedMaterial = playerMaterial;

        player = playerObject.AddComponent<RedBallPlayer>();
        player.Initialize(this, rigidbody2D, rollVisual.transform);
    }

    private void AddRollMark(Transform parent, Vector2 localPosition, float scale, Color color, int order)
    {
        var mark = new GameObject("Roll Mark");
        mark.transform.SetParent(parent, false);
        mark.transform.localPosition = localPosition;
        mark.transform.localScale = Vector3.one * scale;
        var renderer = mark.AddComponent<SpriteRenderer>();
        renderer.sprite = sprites["red_body_circle"];
        renderer.color = color;
        renderer.sortingOrder = order;
    }

    private void AddPlatform(float startX, float y, int length)
    {
        var platform = new GameObject("Platform");
        platform.transform.SetParent(levelRoot, false);
        platform.transform.position = new Vector2(startX + (length - 1) * 0.5f, y);
        levelObjects.Add(platform);

        for (int i = 0; i < length; i++)
        {
            string key = length == 1 ? "tile" : i == 0 ? "tile_left" : i == length - 1 ? "tile_right" : "tile_center";
            AddPlatformTileVisual(platform.transform, key, i - (length - 1) * 0.5f, 0f, 0);
        }

        var collider = platform.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(length, 0.88f);
        collider.offset = new Vector2(0f, -0.01f);
        collider.sharedMaterial = groundMaterial;
    }

    private void AddHalfPlatform(float startX, float y, int length)
    {
        var platform = new GameObject("Jump Platform");
        platform.transform.SetParent(levelRoot, false);
        platform.transform.position = new Vector2(startX + (length - 1) * 0.5f, y);
        levelObjects.Add(platform);

        for (int i = 0; i < length; i++)
        {
            string key = length == 1 ? "tile_half" : i == 0 ? "tile_half_left" : i == length - 1 ? "tile_half_right" : "tile_half_center";
            AddPlatformTileVisual(platform.transform, key, i - (length - 1) * 0.5f, 0f, 1);
        }

        var collider = platform.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(length - 0.12f, 0.22f);
        collider.offset = new Vector2(0f, -0.12f);
        collider.sharedMaterial = slickMaterial;
        collider.usedByEffector = true;

        var effector = platform.AddComponent<PlatformEffector2D>();
        effector.surfaceArc = 150f;
        effector.sideArc = 1f;
        effector.useOneWay = true;
        effector.useSideFriction = false;
    }

    private void AddPlatformTileVisual(Transform parent, string spriteKey, float localX, float localY, int order)
    {
        var child = new GameObject(spriteKey);
        child.transform.SetParent(parent, false);
        child.transform.localPosition = new Vector3(localX, localY, 0f);
        var renderer = child.AddComponent<SpriteRenderer>();
        renderer.sprite = sprites[spriteKey];
        renderer.sortingOrder = order;
    }

    private GameObject AddSolidTile(string spriteKey, Vector2 position, int order)
    {
        var tile = CreateSpriteObject(spriteKey, spriteKey, position, 1f, order);
        var collider = tile.AddComponent<BoxCollider2D>();
        collider.size = sprites[spriteKey].bounds.size;
        collider.sharedMaterial = groundMaterial;
        return tile;
    }

    private void AddMovingPlatform(Vector2 start, Vector2 end, int length, float speed)
    {
        var platform = new GameObject("Moving Platform");
        platform.transform.SetParent(levelRoot, false);
        platform.transform.position = start;
        levelObjects.Add(platform);

        for (int i = 0; i < length; i++)
        {
            string key = length == 1 ? "tile_half" : i == 0 ? "tile_half_left" : i == length - 1 ? "tile_half_right" : "tile_half_center";
            var child = new GameObject(key);
            child.transform.SetParent(platform.transform, false);
            child.transform.localPosition = new Vector3(i - (length - 1) * 0.5f, 0f, 0f);
            var renderer = child.AddComponent<SpriteRenderer>();
            renderer.sprite = sprites[key];
            renderer.sortingOrder = 2;
        }

        var rigidbody2D = platform.AddComponent<Rigidbody2D>();
        rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        rigidbody2D.interpolation = RigidbodyInterpolation2D.Interpolate;

        var collider = platform.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(length, 0.5f);
        collider.sharedMaterial = slickMaterial;
        collider.usedByEffector = true;

        var effector = platform.AddComponent<PlatformEffector2D>();
        effector.surfaceArc = 150f;
        effector.sideArc = 1f;
        effector.useOneWay = true;
        effector.useSideFriction = false;

        var mover = platform.AddComponent<MovingPlatform>();
        mover.Initialize(start, end, speed);

        var overlay = new GameObject("Generated Lift Polish");
        overlay.transform.SetParent(platform.transform, false);
        overlay.transform.localPosition = new Vector3(0f, 0.03f, 0f);
        overlay.transform.localScale = new Vector3(Mathf.Max(0.5f, length / 3.2f), 0.82f, 1f);
        var overlayRenderer = overlay.AddComponent<SpriteRenderer>();
        overlayRenderer.sprite = sprites["lift_platform_polish"];
        overlayRenderer.color = new Color(1f, 1f, 1f, 0.94f);
        overlayRenderer.sortingOrder = 4;
    }

    private void AddCrumblingTile(Vector2 position, int length, float crumbleDelay)
    {
        var platform = new GameObject("Crumbling Tile");
        platform.transform.SetParent(levelRoot, false);
        platform.transform.position = position;
        levelObjects.Add(platform);

        var tileRenderers = new SpriteRenderer[length];
        for (int i = 0; i < length; i++)
        {
            string key = length == 1 ? "tile_half" : i == 0 ? "tile_half_left" : i == length - 1 ? "tile_half_right" : "tile_half_center";
            var child = new GameObject(key);
            child.transform.SetParent(platform.transform, false);
            child.transform.localPosition = new Vector3(i - (length - 1) * 0.5f, 0f, 0f);
            var renderer = child.AddComponent<SpriteRenderer>();
            renderer.sprite = sprites["crumbling_tile_polish"];
            renderer.color = new Color(1f, 1f, 1f, 1f);
            renderer.sortingOrder = 3;
            tileRenderers[i] = renderer;
        }

        var dust = new GameObject("Generated Crumble Dust");
        dust.transform.SetParent(platform.transform, false);
        dust.transform.localPosition = new Vector3(0f, 0.1f, 0f);
        dust.transform.localScale = new Vector3(Mathf.Max(0.7f, length * 0.42f), 0.65f, 1f);
        var dustRenderer = dust.AddComponent<SpriteRenderer>();
        dustRenderer.sprite = sprites["dust_sparkle"];
        dustRenderer.color = new Color(1f, 1f, 1f, 0.5f);
        dustRenderer.sortingOrder = 4;

        var collider = platform.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(length, 0.5f);
        collider.sharedMaterial = slickMaterial;
        collider.usedByEffector = true;

        var effector = platform.AddComponent<PlatformEffector2D>();
        effector.surfaceArc = 150f;
        effector.sideArc = 1f;
        effector.useOneWay = true;
        effector.useSideFriction = false;

        var crumble = platform.AddComponent<RedBallCrumblingTile>();
        crumble.Initialize(tileRenderers, collider, crumbleDelay, 0.18f);
    }

    private void AddBouncePad(Vector2 position, float force)
    {
        var pad = CreateSpriteObject("Bounce Pad", "tile_half", position, 1.15f, 6);
        var collider = pad.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.size = new Vector2(0.95f, 0.32f);
        collider.offset = new Vector2(0f, 0.02f);
        var bounce = pad.AddComponent<BouncePad>();
        bounce.Force = force;
    }

    private void AddTriangleHazard(float x, float y, bool leftFacing)
    {
        string key = leftFacing ? "tile_ramp_left" : "tile_ramp_right";
        var hazard = CreateSpriteObject("Triangle Hazard", key, new Vector2(x, y), 0.92f, 5);
        var collider = hazard.AddComponent<PolygonCollider2D>();
        collider.isTrigger = true;
        if (leftFacing)
        {
            collider.points = new[] { new Vector2(-0.5f, -0.5f), new Vector2(0.5f, -0.5f), new Vector2(-0.5f, 0.5f) };
        }
        else
        {
            collider.points = new[] { new Vector2(-0.5f, -0.5f), new Vector2(0.5f, -0.5f), new Vector2(0.5f, 0.5f) };
        }

        var damage = hazard.AddComponent<Hazard>();
        damage.Game = this;
    }

    private void AddEnemy(Vector2 position, float leftX, float rightX, float speed, string bodyKey, string faceKey)
    {
        var enemy = new GameObject("Patrol Enemy");
        enemy.transform.SetParent(levelRoot, false);
        enemy.transform.position = position;
        levelObjects.Add(enemy);

        var body = enemy.AddComponent<SpriteRenderer>();
        body.sprite = sprites[bodyKey];
        body.sortingOrder = 10;

        var face = new GameObject("Face");
        face.transform.SetParent(enemy.transform, false);
        face.transform.localPosition = new Vector3(0f, 0.03f, 0f);
        var faceRenderer = face.AddComponent<SpriteRenderer>();
        faceRenderer.sprite = sprites[faceKey];
        faceRenderer.sortingOrder = 11;

        var collider = enemy.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.size = new Vector2(0.9f, 0.9f);

        var damageable = enemy.AddComponent<DamageableEnemy>();
        damageable.Initialize(this, 2, 30);

        var patrol = enemy.AddComponent<EnemyPatrol>();
        patrol.Initialize(this, leftX, rightX, speed);
    }

    private void AddGoal(Vector2 position)
    {
        var goal = new GameObject("Goal");
        goal.transform.SetParent(levelRoot, false);
        goal.transform.position = position;
        levelObjects.Add(goal);

        var halo = new GameObject("Generated Sprint04 Goal Halo");
        halo.transform.SetParent(goal.transform, false);
        halo.transform.localScale = Vector3.one * 0.88f;
        var haloRenderer = halo.AddComponent<SpriteRenderer>();
        haloRenderer.sprite = sprites["goal_halo"];
        haloRenderer.color = new Color(1f, 1f, 1f, 0.72f);
        haloRenderer.sortingOrder = 8;

        var body = goal.AddComponent<SpriteRenderer>();
        body.sprite = sprites["green_body_square"];
        body.color = new Color(0.55f, 1f, 0.64f, 0.92f);
        body.sortingOrder = 9;

        var flag = new GameObject("Flag");
        flag.transform.SetParent(goal.transform, false);
        flag.transform.localPosition = new Vector3(0.05f, 0.05f, 0f);
        flag.transform.localScale = Vector3.one * 0.52f;
        var flagRenderer = flag.AddComponent<SpriteRenderer>();
        flagRenderer.sprite = sprites["flag"];
        flagRenderer.sortingOrder = 12;

        var collider = goal.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.size = new Vector2(1.15f, 1.15f);

        var goalTrigger = goal.AddComponent<GoalTrigger>();
        goalTrigger.Game = this;
    }

    private void AddCheckpoint(Vector2 position)
    {
        var checkpoint = new GameObject("Checkpoint");
        checkpoint.transform.SetParent(levelRoot, false);
        checkpoint.transform.position = position;
        levelObjects.Add(checkpoint);

        var body = checkpoint.AddComponent<SpriteRenderer>();
        body.sprite = sprites["yellow_body_circle"];
        body.color = new Color(1f, 0.86f, 0.18f, 0.92f);
        body.sortingOrder = 9;

        var flag = new GameObject("Checkpoint Flag");
        flag.transform.SetParent(checkpoint.transform, false);
        flag.transform.localPosition = new Vector3(0.08f, 0.05f, 0f);
        flag.transform.localScale = Vector3.one * 0.45f;
        var flagRenderer = flag.AddComponent<SpriteRenderer>();
        flagRenderer.sprite = sprites["flag"];
        flagRenderer.color = new Color(1f, 1f, 1f, 0.9f);
        flagRenderer.sortingOrder = 12;

        var sparkle = new GameObject("Generated Checkpoint Sparkle");
        sparkle.transform.SetParent(checkpoint.transform, false);
        sparkle.transform.localPosition = new Vector3(0f, 0f, 0f);
        sparkle.transform.localScale = Vector3.one * 0.82f;
        var sparkleRenderer = sparkle.AddComponent<SpriteRenderer>();
        sparkleRenderer.sprite = sprites["checkpoint_spark"];
        sparkleRenderer.color = new Color(1f, 1f, 1f, 0.82f);
        sparkleRenderer.sortingOrder = 13;

        var collider = checkpoint.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.size = new Vector2(1.15f, 1.15f);

        var trigger = checkpoint.AddComponent<CheckpointTrigger>();
        trigger.Game = this;
        trigger.SpawnPoint = position + new Vector2(0f, 0.25f);
        trigger.BodyRenderer = body;
    }

    private void AddWarningSign(Vector2 position)
    {
        var glow = CreateSpriteObject("Generated Sprint04 Hazard Glow", "hazard_floor_glow", position + new Vector2(0f, -0.78f), 0.54f, 4);
        var glowRenderer = glow.GetComponent<SpriteRenderer>();
        glowRenderer.color = new Color(1f, 1f, 1f, 0.7f);

        var sign = CreateSpriteObject("Warning Sign", "tile_exclamation", position, 0.75f, 14);
        var renderer = sign.GetComponent<SpriteRenderer>();
        renderer.color = new Color(1f, 0.94f, 0.2f, 0.95f);

        var spark = new GameObject("Generated Warning Spark");
        spark.transform.SetParent(sign.transform, false);
        spark.transform.localPosition = new Vector3(0f, 0.06f, 0f);
        spark.transform.localScale = Vector3.one * 0.72f;
        var sparkRenderer = spark.AddComponent<SpriteRenderer>();
        sparkRenderer.sprite = sprites["warning_spark"];
        sparkRenderer.color = new Color(1f, 1f, 1f, 0.7f);
        sparkRenderer.sortingOrder = 15;
    }

    private void AddRouteSparkle(Vector2 position, float scale)
    {
        var arrow = CreateSpriteObject("Generated Sprint04 Route Arrow", "route_arrow", position + new Vector2(0f, -0.46f), scale * 0.82f, 13);
        var arrowRenderer = arrow.GetComponent<SpriteRenderer>();
        arrowRenderer.color = new Color(1f, 1f, 1f, 0.74f);

        var sparkle = CreateSpriteObject("Generated Route Sparkle", "dust_sparkle", position, scale, 14);
        var renderer = sparkle.GetComponent<SpriteRenderer>();
        renderer.color = new Color(1f, 0.92f, 0.52f, 0.52f);
    }

    private void AddCoinLine(float startX, float y, int count, float spacing)
    {
        for (int i = 0; i < count; i++)
        {
            AddCoin(new Vector2(startX + spacing * i, y));
        }
    }

    private void AddCoinArc(Vector2 center, int count, float radius, float startDegrees, float endDegrees)
    {
        if (count <= 1)
        {
            AddCoin(center + Vector2.up * radius);
            return;
        }

        for (int i = 0; i < count; i++)
        {
            float t = i / (float)(count - 1);
            float angle = Mathf.Lerp(startDegrees, endDegrees, t) * Mathf.Deg2Rad;
            AddCoin(center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius);
        }
    }

    private void AddCoin(Vector2 position)
    {
        coinsInLevel += 1;
        var coin = CreateSpriteObject("Coin", "tile_coin", position, 0.9f, 15);
        var collider = coin.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        collider.radius = 0.38f;
        var pickup = coin.AddComponent<CoinPickup>();
        pickup.Game = this;
    }

    private void AddSkyDressing(float minX, float maxX, float groundY)
    {
        float width = Mathf.Max(1f, maxX - minX);
        var parallax = CreateSpriteObject("Generated Sprint04 Parallax Band", "parallax_hills", new Vector2((minX + maxX) * 0.5f, groundY + 1.92f), 1f, -12);
        parallax.transform.localScale = new Vector3(width / 20.48f, 0.82f, 1f);
        var parallaxRenderer = parallax.GetComponent<SpriteRenderer>();
        parallaxRenderer.color = new Color(1f, 1f, 1f, 0.66f);

        for (float x = minX + 2f; x < maxX; x += 7.5f)
        {
            float y = 3.6f + Mathf.Sin(x * 0.47f) * 0.8f;
            AddDecoration("tile_cloud", new Vector2(x, y), 1.1f + Mathf.PingPong(x * 0.07f, 0.35f), -10, 0.48f);
        }

        for (float x = minX + 4f; x < maxX; x += 9f)
        {
            string treeKey = Mathf.RoundToInt(x) % 2 == 0 ? "tile_background_tree_large" : "tile_background_tree_small";
            AddDecoration(treeKey, new Vector2(x, groundY + 1.05f), 1.25f, -8, 0.72f);
        }

        for (float x = minX; x < maxX; x += 4f)
        {
            AddDecoration("tile_background_grass", new Vector2(x, groundY + 0.58f), 1f, -7, 0.75f);
        }
    }

    private void AddDecoration(string spriteKey, Vector2 position, float scale, int order, float alpha)
    {
        var decoration = CreateSpriteObject(spriteKey, spriteKey, position, scale, order);
        var renderer = decoration.GetComponent<SpriteRenderer>();
        renderer.color = new Color(1f, 1f, 1f, alpha);
    }

    private GameObject CreateSpriteObject(string name, string spriteKey, Vector2 position, float scale, int order)
    {
        var gameObject = new GameObject(name);
        gameObject.transform.SetParent(levelRoot, false);
        gameObject.transform.position = position;
        gameObject.transform.localScale = Vector3.one * scale;
        levelObjects.Add(gameObject);

        var renderer = gameObject.AddComponent<SpriteRenderer>();
        renderer.sprite = sprites[spriteKey];
        renderer.sortingOrder = order;
        return gameObject;
    }

    private void SetupCamera()
    {
        var cameraObject = new GameObject("Main Camera");
        cameraObject.transform.SetParent(transform, false);
        cameraObject.tag = "MainCamera";
        mainCamera = cameraObject.AddComponent<Camera>();
        mainCamera.orthographic = true;
        mainCamera.orthographicSize = 5.4f;
        mainCamera.backgroundColor = new Color(0.67f, 0.84f, 0.94f, 1f);
        cameraObject.transform.position = new Vector3(0f, 0f, -10f);
        cameraFollow = cameraObject.AddComponent<CameraFollow>();
    }

    private void SetupAudio()
    {
        var sfxObject = new GameObject("Game SFX");
        sfxObject.transform.SetParent(transform, false);
        sfxSource = sfxObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.volume = 0.92f;

        jumpClip = CreateToneClip("Jump SFX", 360f, 760f, 0.13f, 0.42f, 0.02f);
        bounceClip = CreateToneClip("Bounce SFX", 300f, 680f, 0.11f, 0.36f, 0.04f);
        pickupClip = CreateToneClip("Pickup SFX", 680f, 1220f, 0.12f, 0.32f, 0f);
        hurtClip = CreateToneClip("Hurt SFX", 170f, 110f, 0.18f, 0.38f, 0.22f);
    }

    public void PlayJumpSound()
    {
        PlaySfx(jumpClip, 0.82f);
    }

    public void PlayBounceSound()
    {
        PlaySfx(bounceClip, 0.78f);
    }

    private void PlayPickupSound()
    {
        PlaySfx(pickupClip, 0.7f);
    }

    private void PlayHurtSound()
    {
        PlaySfx(hurtClip, 0.7f);
    }

    private void PlaySfx(AudioClip clip, float volume)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip, volume);
        }
    }

    private AudioClip CreateToneClip(string name, float startFrequency, float endFrequency, float duration, float volume, float noiseMix)
    {
        const int sampleRate = 44100;
        int sampleCount = Mathf.Max(1, Mathf.CeilToInt(sampleRate * duration));
        var samples = new float[sampleCount];
        float phase = 0f;
        uint seed = 2166136261u;

        for (int i = 0; i < sampleCount; i++)
        {
            float normalized = sampleCount <= 1 ? 1f : i / (float)(sampleCount - 1);
            float frequency = Mathf.Lerp(startFrequency, endFrequency, normalized);
            phase += 2f * Mathf.PI * frequency / sampleRate;

            seed = seed * 16777619u + 1013904223u;
            float noise = ((seed >> 8) / 16777215f) * 2f - 1f;
            float tone = Mathf.Sin(phase) + Mathf.Sin(phase * 2.01f) * 0.25f;
            float wave = Mathf.Lerp(tone, noise, noiseMix);
            float envelope = Mathf.Pow(1f - normalized, 1.65f) * Mathf.Sin(Mathf.Clamp01(normalized * 9f) * Mathf.PI * 0.5f);
            samples[i] = Mathf.Clamp(wave * envelope * volume, -1f, 1f);
        }

        var clip = AudioClip.Create(name, sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }

    private void SetupUi()
    {
        localizedTextBindings.Clear();
        languageButtonTexts.Clear();

        if (UnityEngine.Object.FindAnyObjectByType<EventSystem>() == null)
        {
            var eventSystem = new GameObject("EventSystem");
            eventSystem.transform.SetParent(transform, false);
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }

        var canvasObject = new GameObject("Touch UI");
        canvasObject.transform.SetParent(transform, false);
        var canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000;
        canvasObject.AddComponent<GraphicRaycaster>();

        var scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;

        hudRoot = CreateUiRoot(canvasObject.transform, "Gameplay HUD");
        mainMenuRoot = CreateUiRoot(canvasObject.transform, "Main Menu");
        levelSelectRoot = CreateUiRoot(canvasObject.transform, "Level Select");

        CreateUiImage(hudRoot.transform, "HUD Glass Panel", "hud_panel", new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(-88f, -56f), new Vector2(980f, 92f), Color.white);
        scoreText = CreateUiText(hudRoot.transform, "Score", new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(-205f, -56f), new Vector2(620f, 58f), 27, TextAnchor.MiddleLeft);
        levelText = CreateUiText(hudRoot.transform, "Level", new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(304f, -56f), new Vector2(260f, 58f), 27, TextAnchor.MiddleCenter);
        CreateLanguageButton(hudRoot.transform, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-342f, -56f), new Vector2(250f, 62f), 20);
        CreateUiButton(hudRoot.transform, "Menu", "home", new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-174f, -58f), new Vector2(78f, 78f), ShowMainMenu);
        CreateUiButton(hudRoot.transform, "Restart", "return", new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-76f, -58f), new Vector2(78f, 78f), RestartLevel);

        messageText = CreateUiText(hudRoot.transform, "Message", new Vector2(0.5f, 0.74f), new Vector2(0.5f, 0.74f), Vector2.zero, new Vector2(980f, 78f), 30, TextAnchor.MiddleCenter);
        messageText.color = new Color(1f, 0.98f, 0.9f, 0.98f);

        CreateUiImage(hudRoot.transform, "Joystick Plate", "button_secondary", new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(215f, 190f), new Vector2(300f, 210f), new Color(1f, 1f, 1f, 0.34f));
        var joystickBase = CreateUiImage(hudRoot.transform, "Joystick", "joystick", new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(215f, 190f), new Vector2(245f, 245f), new Color(1f, 1f, 1f, 0.36f));
        var joystickHandle = CreateUiImage(joystickBase.transform, "Joystick Handle", "joystickL_top", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(110f, 110f), new Color(1f, 1f, 1f, 0.84f));
        joystick = joystickBase.gameObject.AddComponent<VirtualJoystick>();
        joystick.Initialize(joystickHandle, 96f);

        CreateUiImage(hudRoot.transform, "Jump Plate", "button_primary", new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(-176f, 178f), new Vector2(250f, 188f), new Color(1f, 1f, 1f, 0.5f));
        var jumpImage = CreateUiImage(hudRoot.transform, "Jump Button", "buttonA", new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(-176f, 178f), new Vector2(168f, 168f), new Color(1f, 1f, 1f, 0.64f));
        jumpButton = jumpImage.gameObject.AddComponent<HoldButton>();

        CreateSolidPanel(mainMenuRoot.transform, "Menu Background", new Color(0.055f, 0.12f, 0.15f, 0.99f));
        CreateUiPanel(mainMenuRoot.transform, "Menu Warm Rail", new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(34f, 0f), new Vector2(68f, 1080f), new Color(0.96f, 0.24f, 0.18f, 0.9f));
        CreateLocalizedText(mainMenuRoot.transform, "Mastery Banner Text", "menu.banner", new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -42f), new Vector2(900f, 48f), 24, TextAnchor.MiddleCenter).color = new Color(1f, 0.82f, 0.3f, 1f);

        CreateUiImage(mainMenuRoot.transform, "Menu Premium Card", "panel_glass", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(320f, 8f), new Vector2(850f, 626f), Color.white);
        CreateUiPanel(mainMenuRoot.transform, "Concept Art Frame", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(-505f, 28f), new Vector2(700f, 502f), new Color(0.02f, 0.06f, 0.08f, 0.72f));
        CreateUiRawImage(mainMenuRoot.transform, "Mastery Concept Preview", SprintKeyArtResourcePath, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(-505f, 28f), new Vector2(660f, 440f), Color.white);

        var title = CreateLocalizedText(mainMenuRoot.transform, "Title", "menu.title", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(320f, 252f), new Vector2(730f, 148f), 58, TextAnchor.MiddleCenter);
        title.lineSpacing = 0.82f;
        title.color = new Color(1f, 0.28f, 0.22f, 1f);
        var subtitle = CreateLocalizedText(mainMenuRoot.transform, "Subtitle", "menu.subtitle", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(320f, 146f), new Vector2(720f, 58f), 28, TextAnchor.MiddleCenter);
        subtitle.color = new Color(0.9f, 0.98f, 1f, 0.96f);

        CreateFeatureChip(mainMenuRoot.transform, "Feature Lift", "lift_platform_polish", "menu.feature.lifts", new Vector2(76f, 38f));
        CreateFeatureChip(mainMenuRoot.transform, "Feature Crumble", "crumbling_tile_polish", "menu.feature.crumble", new Vector2(320f, 38f));
        CreateFeatureChip(mainMenuRoot.transform, "Feature Badges", "mastery_badge_clean_run", "menu.feature.badges", new Vector2(564f, 38f));
        CreateMasteryBadgeStrip(mainMenuRoot.transform, new Vector2(320f, -90f), 1.1f);
        menuHeartText = CreateUiText(mainMenuRoot.transform, "Menu Hearts", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(320f, -158f), new Vector2(760f, 46f), 25, TextAnchor.MiddleCenter);
        menuHeartText.color = new Color(0.9f, 0.98f, 1f, 0.96f);
        menuStatusText = CreateUiText(mainMenuRoot.transform, "Menu Status", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(320f, -204f), new Vector2(780f, 42f), 22, TextAnchor.MiddleCenter);
        menuStatusText.color = new Color(1f, 0.82f, 0.3f, 0.98f);
        CreateLocalizedTextButton(mainMenuRoot.transform, "Continue", "button.continue", "button_primary", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(122f, -292f), new Vector2(300f, 80f), StartContinueLevel);
        CreateLocalizedTextButton(mainMenuRoot.transform, "Levels", "button.levels", "button_secondary", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(452f, -292f), new Vector2(300f, 80f), ShowLevelSelect);
        CreateLanguageButton(mainMenuRoot.transform, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-206f, -54f), new Vector2(330f, 68f), 23);

        CreateSolidPanel(levelSelectRoot.transform, "Level Background", new Color(0.055f, 0.12f, 0.15f, 0.99f));
        CreateUiPanel(levelSelectRoot.transform, "Level Select Accent", new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0f, -42f), new Vector2(1920f, 84f), new Color(0.96f, 0.24f, 0.18f, 0.9f));
        var levelTitle = CreateLocalizedText(levelSelectRoot.transform, "Level Select Title", "levelSelect.title", new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -42f), new Vector2(820f, 60f), 40, TextAnchor.MiddleCenter);
        levelTitle.color = Color.white;
        var levelSubtitle = CreateLocalizedText(levelSelectRoot.transform, "Level Select Subtitle", "levelSelect.subtitle", new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -104f), new Vector2(1160f, 42f), 23, TextAnchor.MiddleCenter);
        levelSubtitle.color = new Color(0.9f, 0.98f, 1f, 0.96f);
        levelSelectHeartText = CreateUiText(levelSelectRoot.transform, "Level Hearts", new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -148f), new Vector2(820f, 42f), 25, TextAnchor.MiddleCenter);
        levelSelectHeartText.color = new Color(0.9f, 0.98f, 1f, 0.95f);
        levelSelectStatusText = CreateUiText(levelSelectRoot.transform, "Level Status", new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -190f), new Vector2(960f, 40f), 22, TextAnchor.MiddleCenter);
        levelSelectStatusText.color = new Color(1f, 0.82f, 0.3f, 0.98f);
        CreateLocalizedTextButton(levelSelectRoot.transform, "Back", "button.back", "button_secondary", new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(122f, -64f), new Vector2(190f, 66f), ShowMainMenu);
        CreateLanguageButton(levelSelectRoot.transform, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-206f, -64f), new Vector2(330f, 66f), 22);

        for (int i = 0; i < LevelCount; i++)
        {
            int index = i;
            int column = i % 5;
            int row = i / 5;
            float rowWidth = 5 * 238f;
            float x = -rowWidth * 0.5f + 119f + 238f * column;
            float y = 108f - row * 154f;
            if (i >= 13)
            {
                CreateUiPanel(levelSelectRoot.transform, "Level " + (i + 1) + " Feature Glow", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(x, y), new Vector2(220f, 134f), new Color(1f, 0.68f, 0.12f, 0.26f));
            }

            var buttonImage = CreateNumberButton(levelSelectRoot.transform, (i + 1).ToString(), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(x, y), new Vector2(206f, 124f), () => TryStartLevel(index));
            levelButtonImages.Add(buttonImage);
            levelButtons.Add(buttonImage.GetComponent<Button>());
            levelButtonTexts.Add(buttonImage.GetComponentInChildren<Text>());
            levelButtonBadgeImages.Add(CreateLevelButtonBadgeIcons(buttonImage.transform));
        }

        var liftTag = CreateLocalizedText(levelSelectRoot.transform, "Level 14 New Tag", "level.tag.lift", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(238f, -126f), new Vector2(188f, 34f), 19, TextAnchor.MiddleCenter);
        liftTag.color = new Color(1f, 0.82f, 0.3f, 1f);
        var crumbleTag = CreateLocalizedText(levelSelectRoot.transform, "Level 15 New Tag", "level.tag.crumble", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(476f, -126f), new Vector2(188f, 34f), 19, TextAnchor.MiddleCenter);
        crumbleTag.color = new Color(1f, 0.82f, 0.3f, 1f);

        CreateUiImage(levelSelectRoot.transform, "Mastery Legend Panel", "feature_chip", new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 82f), new Vector2(1320f, 92f), Color.white);
        var masteryLegend = CreateLocalizedText(levelSelectRoot.transform, "Mastery Legend Text", "levelSelect.legend", new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 82f), new Vector2(1240f, 70f), 23, TextAnchor.MiddleCenter);
        masteryLegend.lineSpacing = 0.88f;
        masteryLegend.color = new Color(0.07f, 0.12f, 0.15f, 1f);
    }

    private Text CreateLocalizedText(Transform parent, string name, string key, Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 size, int fontSize, TextAnchor alignment)
    {
        Text text = CreateUiText(parent, name, anchorMin, anchorMax, position, size, fontSize, alignment);
        BindLocalizedText(text, key);
        return text;
    }

    private void BindLocalizedText(Text text, string key)
    {
        localizedTextBindings.Add(new LocalizedTextBinding { Text = text, Key = key });
        text.text = RedBallLocalization.T(key);
    }

    private void CreateFeatureChip(Transform parent, string name, string iconKey, string textKey, Vector2 center)
    {
        CreateUiImage(parent, name + " Panel", "feature_chip", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), center, new Vector2(226f, 66f), Color.white);
        var icon = CreateUiImage(parent, name + " Icon", iconKey, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), center + new Vector2(-76f, 0f), new Vector2(42f, 42f), Color.white);
        icon.raycastTarget = false;
        var text = CreateLocalizedText(parent, name + " Text", textKey, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), center + new Vector2(28f, 0f), new Vector2(148f, 46f), 19, TextAnchor.MiddleCenter);
        text.color = new Color(0.08f, 0.12f, 0.14f, 1f);
    }

    private Button CreateLanguageButton(Transform parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 size, int fontSize)
    {
        Button button = CreateTextButton(parent, "Language", GetLanguageButtonLabel(), "language_chip", anchorMin, anchorMax, position, size, CycleLanguageAndRefresh);
        Text text = button.GetComponentInChildren<Text>();
        if (text != null)
        {
            text.fontSize = fontSize;
            text.color = new Color(0.08f, 0.12f, 0.14f, 1f);
            languageButtonTexts.Add(text);
        }

        return button;
    }

    private void CycleLanguageAndRefresh()
    {
        RedBallLocalization.CycleLanguage();
        RefreshLocalizedUi();
        ShowStatus(GetLanguageButtonLabel(), 1.3f);
    }

    private string GetLanguageButtonLabel()
    {
        return RedBallLocalization.F("button.language", RedBallLocalization.CurrentLanguageLabel());
    }

    private void RefreshLocalizedUi()
    {
        for (int i = 0; i < localizedTextBindings.Count; i++)
        {
            LocalizedTextBinding binding = localizedTextBindings[i];
            if (binding != null && binding.Text != null)
            {
                binding.Text.text = RedBallLocalization.T(binding.Key);
            }
        }

        for (int i = 0; i < languageButtonTexts.Count; i++)
        {
            if (languageButtonTexts[i] != null)
            {
                languageButtonTexts[i].text = GetLanguageButtonLabel();
            }
        }

        RefreshAllUi();
    }

    private GameObject CreateUiRoot(Transform parent, string name)
    {
        var gameObject = new GameObject(name);
        gameObject.transform.SetParent(parent, false);
        var rect = gameObject.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = Vector2.zero;
        return gameObject;
    }

    private void CreateMasteryBadgeStrip(Transform parent, Vector2 center, float scale)
    {
        string[] spriteKeys = { "mastery_badge_clear", "mastery_badge_all_coins", "mastery_badge_clean_run" };
        string[] labels = { "CLEAR", "COINS", "CLEAN" };
        for (int i = 0; i < spriteKeys.Length; i++)
        {
            float x = center.x + (i - 1) * 126f * scale;
            var icon = CreateUiImage(parent, "Mastery Badge " + labels[i], spriteKeys[i], new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(x, center.y), new Vector2(64f * scale, 64f * scale), Color.white);
            icon.raycastTarget = false;
        }
    }

    private Image[] CreateLevelButtonBadgeIcons(Transform parent)
    {
        string[] spriteKeys = { "mastery_badge_clear", "mastery_badge_all_coins", "mastery_badge_clean_run" };
        var icons = new Image[spriteKeys.Length];
        for (int i = 0; i < spriteKeys.Length; i++)
        {
            var icon = CreateUiImage(parent, "Badge Icon " + i, spriteKeys[i], new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2((i - 1) * 38f, 18f), new Vector2(28f, 28f), Color.white);
            icon.raycastTarget = false;
            icons[i] = icon;
        }

        return icons;
    }

    private static void RefreshLevelBadgeIcons(Image[] icons, bool unlocked, bool clearBadge, bool allCoinsBadge, bool cleanRunBadge)
    {
        if (icons == null || icons.Length < 3)
        {
            return;
        }

        bool[] earned = { clearBadge, allCoinsBadge, cleanRunBadge };
        for (int i = 0; i < icons.Length; i++)
        {
            if (icons[i] == null)
            {
                continue;
            }

            icons[i].enabled = unlocked;
            icons[i].color = earned[i]
                ? Color.white
                : new Color(0.12f, 0.14f, 0.16f, 0.42f);
        }
    }

    private Image CreateSolidPanel(Transform parent, string name, Color color)
    {
        var gameObject = new GameObject(name);
        gameObject.transform.SetParent(parent, false);
        var rect = gameObject.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = Vector2.zero;
        var image = gameObject.AddComponent<Image>();
        image.color = color;
        image.raycastTarget = false;
        return image;
    }

    private Image CreateUiImage(Transform parent, string name, string spriteKey, Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 size, Color color)
    {
        var gameObject = new GameObject(name);
        gameObject.transform.SetParent(parent, false);
        var rect = gameObject.AddComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        var image = gameObject.AddComponent<Image>();
        image.sprite = sprites[spriteKey];
        image.color = color;
        image.preserveAspect = true;
        return image;
    }

    private RawImage CreateUiRawImage(Transform parent, string name, string resourcesPath, Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 size, Color color)
    {
        var texture = Resources.Load<Texture2D>(resourcesPath);
        if (texture == null)
        {
            Debug.LogWarning("Missing UI texture resource: " + resourcesPath);
            return null;
        }

        var gameObject = new GameObject(name);
        gameObject.transform.SetParent(parent, false);
        var rect = gameObject.AddComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        var image = gameObject.AddComponent<RawImage>();
        image.texture = texture;
        image.color = color;
        image.raycastTarget = false;
        return image;
    }

    private Image CreateUiPanel(Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 size, Color color)
    {
        var gameObject = new GameObject(name);
        gameObject.transform.SetParent(parent, false);
        var rect = gameObject.AddComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        var image = gameObject.AddComponent<Image>();
        image.color = color;
        image.raycastTarget = false;
        return image;
    }

    private Text CreateUiText(Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 size, int fontSize, TextAnchor alignment)
    {
        var gameObject = new GameObject(name);
        gameObject.transform.SetParent(parent, false);
        var rect = gameObject.AddComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        var text = gameObject.AddComponent<Text>();
        text.font = uiFont;
        text.fontSize = fontSize;
        text.resizeTextForBestFit = true;
        text.resizeTextMinSize = Mathf.Max(12, Mathf.RoundToInt(fontSize * 0.58f));
        text.resizeTextMaxSize = fontSize;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Truncate;
        text.alignment = alignment;
        text.color = Color.white;
        text.raycastTarget = false;
        return text;
    }

    private void CreateUiButton(Transform parent, string name, string spriteKey, Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 size, UnityEngine.Events.UnityAction action)
    {
        var image = CreateUiImage(parent, name, spriteKey, anchorMin, anchorMax, position, size, new Color(1f, 1f, 1f, 0.58f));
        var button = image.gameObject.AddComponent<Button>();
        button.targetGraphic = image;
        button.onClick.AddListener(action);
        ConfigureButtonColors(button, image.color);
    }

    private Button CreateTextButton(Transform parent, string name, string label, Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 size, UnityEngine.Events.UnityAction action)
    {
        return CreateTextButton(parent, name, label, "button_secondary", anchorMin, anchorMax, position, size, action);
    }

    private Button CreateLocalizedTextButton(Transform parent, string name, string key, string spriteKey, Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 size, UnityEngine.Events.UnityAction action)
    {
        Button button = CreateTextButton(parent, name, RedBallLocalization.T(key), spriteKey, anchorMin, anchorMax, position, size, action);
        Text text = button.GetComponentInChildren<Text>();
        if (text != null)
        {
            BindLocalizedText(text, key);
        }

        return button;
    }

    private Button CreateTextButton(Transform parent, string name, string label, string spriteKey, Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 size, UnityEngine.Events.UnityAction action)
    {
        var gameObject = new GameObject(name + " Button");
        gameObject.transform.SetParent(parent, false);
        var rect = gameObject.AddComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        var image = gameObject.AddComponent<Image>();
        image.sprite = sprites.ContainsKey(spriteKey) ? sprites[spriteKey] : null;
        image.color = Color.white;
        var button = gameObject.AddComponent<Button>();
        button.targetGraphic = image;
        button.onClick.AddListener(action);
        ConfigureButtonColors(button, image.color);

        var text = CreateUiText(gameObject.transform, "Text", new Vector2(0f, 0f), new Vector2(1f, 1f), Vector2.zero, new Vector2(-28f, -10f), 30, TextAnchor.MiddleCenter);
        text.text = label;
        text.color = Color.white;
        return button;
    }

    private Image CreateNumberButton(Transform parent, string label, Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 size, UnityEngine.Events.UnityAction action)
    {
        var gameObject = new GameObject("Level " + label + " Button");
        gameObject.transform.SetParent(parent, false);
        var rect = gameObject.AddComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        var image = gameObject.AddComponent<Image>();
        image.sprite = sprites.ContainsKey("level_card") ? sprites["level_card"] : null;
        image.color = new Color(1f, 1f, 1f, 0.92f);
        var button = gameObject.AddComponent<Button>();
        button.targetGraphic = image;
        button.onClick.AddListener(action);
        ConfigureButtonColors(button, Color.white);

        var text = CreateUiText(gameObject.transform, "Text", new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0f, 16f), new Vector2(-20f, -44f), 24, TextAnchor.MiddleCenter);
        text.text = label;
        text.lineSpacing = 0.8f;
        text.color = Color.white;
        return image;
    }

    private static void ConfigureButtonColors(Button button, Color baseColor)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = baseColor;
        colors.highlightedColor = new Color(1f, 0.95f, 0.82f, baseColor.a);
        colors.pressedColor = new Color(0.86f, 0.92f, 1f, baseColor.a);
        colors.selectedColor = colors.highlightedColor;
        colors.disabledColor = new Color(0.34f, 0.38f, 0.42f, 0.52f);
        colors.colorMultiplier = 1f;
        colors.fadeDuration = 0.08f;
        button.colors = colors;
    }

    private void RefreshHud()
    {
        if (scoreText != null)
        {
            scoreText.text = RedBallLocalization.T("hud.health") + " " + health + "/" + MaxHealth
                + "   " + RedBallLocalization.T("hud.score") + " " + score
                + "   " + RedBallLocalization.T("hud.coin") + " " + coinsCollectedInLevel + "/" + coinsInLevel;
        }

        if (levelText != null)
        {
            levelText.text = RedBallLocalization.T("hud.level") + " " + (levelIndex + 1) + " / " + LevelCount;
        }
    }

    private void RefreshLevelButtons()
    {
        int continueLevelIndex = GetContinueLevelIndex();
        for (int i = 0; i < levelButtonImages.Count; i++)
        {
            bool unlocked = i < unlockedLevelCount;
            bool completed = (completedLevelMask & (1 << i)) != 0;
            bool current = screenMode == ScreenMode.Gameplay && i == levelIndex;
            bool continueTarget = screenMode == ScreenMode.LevelSelect && i == continueLevelIndex;
            RedBallLevelButtonState state = RedBallUi.GetLevelButtonState(unlocked, completed, current, continueTarget);
            levelButtonImages[i].color = RedBallUi.GetLevelButtonColor(state);
            if (i == 13 && !completed)
            {
                levelButtonImages[i].color = unlocked
                    ? new Color(0.82f, 0.46f, 0.12f, 0.9f)
                    : new Color(0.46f, 0.27f, 0.1f, 0.74f);
            }
            else if (i == 14 && !completed)
            {
                levelButtonImages[i].color = unlocked
                    ? new Color(0.72f, 0.28f, 0.2f, 0.9f)
                    : new Color(0.44f, 0.18f, 0.16f, 0.74f);
            }

            if (i < levelButtons.Count && levelButtons[i] != null)
            {
                levelButtons[i].interactable = unlocked;
            }

            if (i < levelButtonTexts.Count && levelButtonTexts[i] != null)
            {
                levelButtonTexts[i].text = RedBallUi.GetLevelButtonLabel(
                    i + 1,
                    state,
                    HasBadge(badgeClearMask, i),
                    HasBadge(badgeAllCoinsMask, i),
                    HasBadge(badgeCleanRunMask, i));
                levelButtonTexts[i].color = RedBallUi.GetLevelButtonTextColor(state);
            }

            if (i < levelButtonBadgeImages.Count)
            {
                RefreshLevelBadgeIcons(levelButtonBadgeImages[i], unlocked, HasBadge(badgeClearMask, i), HasBadge(badgeAllCoinsMask, i), HasBadge(badgeCleanRunMask, i));
            }
        }
    }

    private void RefreshAllUi()
    {
        RefreshHud();
        RefreshLevelButtons();
        UpdateLifeTexts();
    }

    private void ShowMessage(string message, float seconds)
    {
        if (messageText == null)
        {
            return;
        }

        messageText.text = message;
        messageTimer = seconds;
    }

    private void ShowMainMenu()
    {
        CancelInvoke(nameof(LoadNextLevel));
        CancelInvoke(nameof(ReturnToMenuAfterNoHearts));
        ClearLevel();
        screenMode = ScreenMode.MainMenu;
        SetScreenRoots(false, true, false);
        ResetTouchControls();
        RefreshAllUi();
        ClearStatusText();
    }

    private void ShowLevelSelect()
    {
        CancelInvoke(nameof(LoadNextLevel));
        ClearLevel();
        screenMode = ScreenMode.LevelSelect;
        SetScreenRoots(false, false, true);
        ResetTouchControls();
        RefreshAllUi();
        ClearStatusText();
    }

    private void ShowGameplay()
    {
        screenMode = ScreenMode.Gameplay;
        SetScreenRoots(true, false, false);
        ClearStatusText();
    }

    private void SetScreenRoots(bool showHud, bool showMenu, bool showLevelSelect)
    {
        if (hudRoot != null)
        {
            hudRoot.SetActive(showHud);
        }

        if (mainMenuRoot != null)
        {
            mainMenuRoot.SetActive(showMenu);
        }

        if (levelSelectRoot != null)
        {
            levelSelectRoot.SetActive(showLevelSelect);
        }
    }

    private void ResetTouchControls()
    {
        if (joystick != null)
        {
            joystick.ResetInput();
        }

        if (jumpButton != null)
        {
            jumpButton.ResetState();
        }
    }

    private void UpdateLifeTexts()
    {
        string text = RedBallLocalization.T("hud.health") + " " + health + "/" + MaxHealth;
        if (health < MaxHealth)
        {
            text += "   " + RedBallLocalization.T("life.next") + ": " + GetNextHeartText();
        }
        else
        {
            text += "   " + RedBallLocalization.T("life.full");
        }

        if (menuHeartText != null)
        {
            menuHeartText.text = text;
        }

        if (levelSelectHeartText != null)
        {
            levelSelectHeartText.text = text;
        }
    }

    private string GetNextHeartText()
    {
        if (health >= MaxHealth)
        {
            return RedBallLocalization.T("life.full");
        }

        long elapsed = Math.Max(0L, GetNowSeconds() - heartTimestamp);
        long remaining = Math.Max(1L, HeartRechargeSeconds - elapsed);
        int minutes = Mathf.CeilToInt(remaining / 60f);
        if (minutes >= 60)
        {
            return RedBallLocalization.T("time.hourShort");
        }

        return RedBallLocalization.F("time.minShort", minutes);
    }

    private void ShowStatus(string message, float seconds)
    {
        if (screenMode == ScreenMode.LevelSelect && levelSelectStatusText != null)
        {
            levelSelectStatusText.text = message;
        }
        else if (menuStatusText != null)
        {
            menuStatusText.text = message;
        }

        statusTimer = seconds;
    }

    private void ClearStatusText()
    {
        if (menuStatusText != null)
        {
            menuStatusText.text = string.Empty;
        }

        if (levelSelectStatusText != null)
        {
            levelSelectStatusText.text = string.Empty;
        }
    }

    private void LoadSprites()
    {
        AddWorldSprite("red_body_circle");
        AddWorldSprite("face_a");
        AddWorldSprite("face_d");
        AddWorldSprite("face_f");
        AddWorldSprite("face_g");
        AddWorldSprite("tile");
        AddWorldSprite("tile_left");
        AddWorldSprite("tile_center");
        AddWorldSprite("tile_right");
        AddWorldSprite("tile_half");
        AddWorldSprite("tile_half_left");
        AddWorldSprite("tile_half_center");
        AddWorldSprite("tile_half_right");
        AddWorldSprite("tile_grey");
        AddWorldSprite("tile_coin");
        AddWorldSprite("tile_exclamation");
        AddWorldSprite("tile_ramp_left");
        AddWorldSprite("tile_ramp_right");
        AddWorldSprite("tile_background_grass");
        AddWorldSprite("tile_background_tree_large");
        AddWorldSprite("tile_background_tree_small");
        AddWorldSprite("tile_cloud");
        AddWorldSprite("shadow");
        AddWorldSprite("purple_body_square");
        AddWorldSprite("green_body_square");
        AddWorldSprite("yellow_body_circle");
        AddWorldSprite("blue_body_square");

        AddUiSprite("joystick");
        AddUiSprite("joystickL_top");
        AddUiSprite("joystickL_side");
        AddUiSprite("buttonA");
        AddUiSprite("return");
        AddUiSprite("pause");
        AddUiSprite("home");
        AddUiSprite("flag");
        AddUiSprite("coin");

        AddGeneratedSprite("mastery_badge_clear", "mastery_badge_clear", UiPpu, "flag");
        AddGeneratedSprite("mastery_badge_all_coins", "mastery_badge_all_coins", UiPpu, "coin");
        AddGeneratedSprite("mastery_badge_clean_run", "mastery_badge_clean_run", UiPpu, "buttonA");
        AddGeneratedSprite("lift_platform_polish", "lift_platform_polish", 100f, "tile_half_center");
        AddGeneratedSprite("crumbling_tile_polish", "crumbling_tile_polish", 128f, "tile_half");
        AddGeneratedSprite("warning_spark", "warning_spark", 128f, "tile_exclamation");
        AddGeneratedSprite("checkpoint_spark", "checkpoint_spark", 128f, "flag");
        AddGeneratedSprite("dust_sparkle", "dust_sparkle", 128f, "shadow");

        AddGeneratedSprite("panel_glass", "Sprint03/panel_glass", UiPpu, "tile_grey");
        AddGeneratedSprite("button_primary", "Sprint03/button_primary", UiPpu, "buttonA");
        AddGeneratedSprite("button_secondary", "Sprint03/button_secondary", UiPpu, "pause");
        AddGeneratedSprite("language_chip", "Sprint03/language_chip", UiPpu, "tile_grey");
        AddGeneratedSprite("hud_panel", "Sprint03/hud_panel", UiPpu, "tile_grey");
        AddGeneratedSprite("level_card", "Sprint03/level_card", UiPpu, "tile_grey");
        AddGeneratedSprite("feature_chip", "Sprint03/feature_chip", UiPpu, "tile_grey");
        AddGeneratedSprite("parallax_hills", "Sprint04/parallax_hills", 100f, "tile_background_grass");
        AddGeneratedSprite("route_arrow", "Sprint04/route_arrow", 128f, "dust_sparkle");
        AddGeneratedSprite("goal_halo", "Sprint04/goal_halo", 128f, "flag");
        AddGeneratedSprite("hazard_floor_glow", "Sprint04/hazard_floor_glow", 128f, "warning_spark");
    }

    private void AddWorldSprite(string key)
    {
        sprites[key] = LoadSprite("RedBall/" + key, WorldPpu);
    }

    private void AddUiSprite(string key)
    {
        sprites[key] = LoadSprite("UI/" + key, UiPpu);
    }

    private void AddGeneratedSprite(string key, string fileName, float ppu, string fallbackKey)
    {
        string resourcePath = fileName.Contains("/") ? "Generated/" + fileName : "Generated/Sprint02/" + fileName;
        Sprite generated = TryLoadSprite(resourcePath, ppu);
        if (generated != null)
        {
            sprites[key] = generated;
            return;
        }

        if (sprites.ContainsKey(fallbackKey))
        {
            sprites[key] = sprites[fallbackKey];
            return;
        }

        sprites[key] = LoadSprite("RedBall/" + fallbackKey, ppu);
    }

    private void AddImportedSprite(string key, string path, float ppu)
    {
        sprites[key] = LoadSprite(path, ppu);
    }

    private void AddAtlasSprite(string key, string path, Rect rect, float ppu)
    {
        Texture2D texture = LoadTexture(path);
        if (rect.xMax > texture.width || rect.yMax > texture.height)
        {
            rect = new Rect(0f, 0f, texture.width, texture.height);
        }

        sprites[key] = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), ppu, 0, SpriteMeshType.FullRect);
    }

    private void AddSheetAnimation(string key, string path, int frameWidth, int frameHeight, int count, float ppu)
    {
        Texture2D texture = LoadTexture(path);
        if (texture.width < frameWidth || texture.height < frameHeight)
        {
            animations[key] = new[]
            {
                Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), ppu, 0, SpriteMeshType.FullRect)
            };
            return;
        }

        int columns = Mathf.Max(1, texture.width / frameWidth);
        var frames = new Sprite[count];
        for (int i = 0; i < count; i++)
        {
            int column = i % columns;
            int row = i / columns;
            float x = column * frameWidth;
            float y = texture.height - (row + 1) * frameHeight;
            frames[i] = Sprite.Create(texture, new Rect(x, y, frameWidth, frameHeight), new Vector2(0.5f, 0.5f), ppu, 0, SpriteMeshType.FullRect);
        }

        animations[key] = frames;
    }

    private Sprite LoadSprite(string resourcesPath, float ppu)
    {
        var texture = LoadTexture(resourcesPath);
        return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), ppu, 0, SpriteMeshType.FullRect);
    }

    private Sprite TryLoadSprite(string resourcesPath, float ppu)
    {
        var texture = Resources.Load<Texture2D>(resourcesPath);
        if (texture == null)
        {
            Debug.LogWarning("Missing generated sprite resource, using fallback: " + resourcesPath);
            return null;
        }

        texture.filterMode = FilterMode.Bilinear;
        texture.wrapMode = TextureWrapMode.Clamp;
        return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), ppu, 0, SpriteMeshType.FullRect);
    }

    private Texture2D LoadTexture(string resourcesPath)
    {
        var texture = Resources.Load<Texture2D>(resourcesPath);
        if (texture == null)
        {
            Debug.LogWarning("Missing sprite resource: " + resourcesPath);
            texture = CreateFallbackTexture();
        }

        texture.filterMode = FilterMode.Bilinear;
        texture.wrapMode = TextureWrapMode.Clamp;
        return texture;
    }

    private static Texture2D CreateFallbackTexture()
    {
        var texture = new Texture2D(2, 2);
        texture.SetPixels(new[] { Color.magenta, Color.magenta, Color.magenta, Color.magenta });
        texture.Apply();
        return texture;
    }

    private static float GetBoundsMinX()
    {
        return BoundsMarker.MinX;
    }

    private static float GetBoundsMaxX()
    {
        return BoundsMarker.MaxX;
    }

    private static float GetBoundsMaxY()
    {
        return BoundsMarker.MaxY;
    }

    private static class BoundsMarker
    {
        public static float MinX;
        public static float MaxX;
        public static float MaxY;
    }
}

public sealed class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform baseRect;
    private RectTransform handleRect;
    private float radius = 60f;

    public Vector2 Value { get; private set; }

    public void Initialize(Image handle, float dragRadius)
    {
        baseRect = transform as RectTransform;
        handleRect = handle.rectTransform;
        radius = dragRadius;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (baseRect == null || handleRect == null)
        {
            return;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);
        Vector2 clamped = Vector2.ClampMagnitude(localPoint, radius);
        Value = clamped / radius;
        handleRect.anchoredPosition = clamped;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ResetInput();
    }

    public void ResetInput()
    {
        Value = Vector2.zero;
        if (handleRect != null)
        {
            handleRect.anchoredPosition = Vector2.zero;
        }
    }
}

public sealed class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool pressedThisFrame;

    public bool IsHeld { get; private set; }

    public bool ConsumePressed()
    {
        if (!pressedThisFrame)
        {
            return false;
        }

        pressedThisFrame = false;
        return true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsHeld = true;
        pressedThisFrame = true;
        transform.localScale = Vector3.one * 1.06f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ResetState();
    }

    public void ResetState()
    {
        IsHeld = false;
        pressedThisFrame = false;
        transform.localScale = Vector3.one;
    }
}

public sealed class CameraFollow : MonoBehaviour
{
    private Transform target;
    private Camera cameraComponent;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private Vector3 velocity;

    private void Awake()
    {
        cameraComponent = GetComponent<Camera>();
    }

    public void Configure(Transform newTarget, float boundsMinX, float boundsMaxX, float boundsMinY, float boundsMaxY)
    {
        target = newTarget;
        minX = boundsMinX;
        maxX = boundsMaxX;
        minY = boundsMinY;
        maxY = boundsMaxY;
        velocity = Vector3.zero;
    }

    private void LateUpdate()
    {
        if (target == null || cameraComponent == null)
        {
            return;
        }

        float halfHeight = cameraComponent.orthographicSize;
        float halfWidth = halfHeight * cameraComponent.aspect;
        Vector3 desired = target.position + new Vector3(1.7f, 1.15f, -10f);

        if (maxX - minX > halfWidth * 2f)
        {
            desired.x = Mathf.Clamp(desired.x, minX + halfWidth, maxX - halfWidth);
        }

        if (maxY - minY > halfHeight * 2f)
        {
            desired.y = Mathf.Clamp(desired.y, minY + halfHeight, maxY - halfHeight);
        }

        desired.z = -10f;
        transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, 0.18f);
    }
}

public sealed class MovingPlatform : MonoBehaviour
{
    private Rigidbody2D body;
    private Vector2 start;
    private Vector2 end;
    private float speed;
    private float journeyLength;
    private float timer;

    public void Initialize(Vector2 startPoint, Vector2 endPoint, float platformSpeed)
    {
        start = startPoint;
        end = endPoint;
        speed = platformSpeed;
        journeyLength = Mathf.Max(0.01f, Vector2.Distance(start, end));
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime * speed / journeyLength;
        float eased = Mathf.SmoothStep(0f, 1f, Mathf.PingPong(timer, 1f));
        Vector2 position = Vector2.Lerp(start, end, eased);
        if (body != null)
        {
            body.MovePosition(position);
        }
        else
        {
            transform.position = position;
        }
    }
}

public sealed class CoinPickup : MonoBehaviour
{
    public RedBallGame Game;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<RedBallPlayer>() != null)
        {
            Game.CollectCoin(gameObject);
        }
    }
}

public sealed class DamageableEnemy : MonoBehaviour
{
    private RedBallGame game;
    private int health = 1;
    private int scoreValue = 25;
    private bool dead;

    public bool IsDead => dead;

    public void Initialize(RedBallGame owner, int hitPoints, int score)
    {
        game = owner;
        health = Mathf.Max(1, hitPoints);
        scoreValue = score;
    }

    public void Hit(int damage)
    {
        if (dead)
        {
            return;
        }

        health -= Mathf.Max(1, damage);
        transform.position += Vector3.up * 0.08f;
        if (health > 0)
        {
            return;
        }

        dead = true;
        if (game != null)
        {
            game.AddScore(scoreValue);
        }

        Destroy(gameObject);
    }
}

public sealed class Hazard : MonoBehaviour
{
    public RedBallGame Game;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<RedBallPlayer>() != null)
        {
            Game.DamagePlayer();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<RedBallPlayer>() != null)
        {
            Game.DamagePlayer();
        }
    }
}

public sealed class BouncePad : MonoBehaviour
{
    public float Force = 12f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<RedBallPlayer>();
        if (player != null)
        {
            player.Bounce(Force);
        }
    }
}

public sealed class GoalTrigger : MonoBehaviour
{
    public RedBallGame Game;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<RedBallPlayer>() != null)
        {
            Game.CompleteLevel();
        }
    }
}

public sealed class EnemyPatrol : MonoBehaviour
{
    private RedBallGame game;
    private float leftX;
    private float rightX;
    private float speed;
    private int direction = 1;

    public void Initialize(RedBallGame owner, float minX, float maxX, float patrolSpeed)
    {
        game = owner;
        leftX = Mathf.Min(minX, maxX);
        rightX = Mathf.Max(minX, maxX);
        speed = patrolSpeed;
    }

    private void Update()
    {
        Vector3 position = transform.position;
        position.x += direction * speed * Time.deltaTime;
        if (position.x > rightX)
        {
            position.x = rightX;
            direction = -1;
        }
        else if (position.x < leftX)
        {
            position.x = leftX;
            direction = 1;
        }

        transform.position = position;
        transform.localScale = new Vector3(direction, 1f, 1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ResolvePlayerTouch(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        ResolvePlayerTouch(other);
    }

    private void ResolvePlayerTouch(Collider2D other)
    {
        var player = other.GetComponent<RedBallPlayer>();
        if (player == null || game == null)
        {
            return;
        }

        if (player.transform.position.y > transform.position.y + 0.28f && player.IsFalling)
        {
            player.Bounce(10f);
            game.AddScore(25);
            Destroy(gameObject);
            return;
        }

        game.DamagePlayer();
    }
}
