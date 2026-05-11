using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace RedBall.Tests.PlayMode
{
    public sealed class RedBallTraversalSmokePlayModeTests
    {
        private static readonly string[] ProgressKeys =
        {
            "RedBall.UnlockedCount",
            "RedBall.CompletedMask",
            "RedBall.Badges.ClearMask",
            "RedBall.Badges.AllCoinsMask",
            "RedBall.Badges.CleanRunMask",
            "RedBall.Hearts",
            "RedBall.HeartStamp"
        };

        private Type gameType;
        private GameObject gameObject;
        private Component game;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            Time.timeScale = 1f;
            ClearProgressPrefs();
            PlayerPrefs.SetInt("RedBall.UnlockedCount", 15);
            PlayerPrefs.SetInt("RedBall.Hearts", 5);
            PlayerPrefs.Save();

            gameType = RequireType("RedBallGame");
            DestroyExistingGames();
            yield return null;

            CreateGame();
            yield return null;
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            if (gameObject != null)
            {
                UnityEngine.Object.Destroy(gameObject);
            }

            DestroyExistingGames();
            ClearProgressPrefs();
            yield return null;
        }

        [UnityTest]
        public IEnumerator TargetLevelsBuildExpectedTraversalObjects()
        {
            yield return AssertLevelContract(new LevelContract(0, "Level 01", 1, 0, 0, 0));
            yield return AssertLevelContract(new LevelContract(13, "Level 14", 3, 1, 2, 0));
            yield return AssertLevelContract(new LevelContract(14, "Level 15", 3, 1, 0, 3));
        }

        [UnityTest]
        public IEnumerator Level14VerticalLiftMovesDuringPhysicsStep()
        {
            yield return LoadLevel(13);
            var movers = FindComponents("MovingPlatform").OrderBy(component => component.transform.position.x).ToArray();
            Assert.That(movers.Length, Is.GreaterThanOrEqualTo(2), "Level 14 should expose vertical lift components.");

            Transform firstLift = movers[0].transform;
            Vector3 start = firstLift.position;

            yield return WaitFixedFrames(75);

            Vector3 end = firstLift.position;
            Assert.That(Mathf.Abs(end.y - start.y), Is.GreaterThan(0.25f), "First Level 14 lift should travel vertically during PlayMode physics.");
            Assert.That(Mathf.Abs(end.x - start.x), Is.LessThan(0.05f), "First Level 14 lift should remain a vertical lift.");
            Assert.That(GetPlayerTransform().position.y, Is.GreaterThan(GetKillY() + 2f), "Player should remain alive while the lift simulates.");
        }

        [UnityTest]
        public IEnumerator Level15FirstCrumblingTileCollapsesAfterPlayerContact()
        {
            yield return LoadLevel(14);
            Component firstCrumble = FindComponents("RedBallCrumblingTile")
                .OrderBy(component => component.transform.position.x)
                .FirstOrDefault();
            Assert.That(firstCrumble, Is.Not.Null, "Level 15 should contain at least one crumbling tile.");

            Transform playerTransform = GetPlayerTransform();
            var playerBody = playerTransform.GetComponent<Rigidbody2D>();
            Assert.That(playerBody, Is.Not.Null, "Player should expose a Rigidbody2D for PlayMode physics smoke.");

            Collider2D tileCollider = firstCrumble.GetComponent<Collider2D>();
            Assert.That(tileCollider, Is.Not.Null.And.Property("enabled").True, "Crumbling tile should start with an enabled collider.");

            playerBody.linearVelocity = Vector2.zero;
            playerBody.angularVelocity = 0f;
            playerTransform.position = firstCrumble.transform.position + new Vector3(0f, 0.95f, 0f);

            yield return WaitFixedFrames(90);

            Assert.That(tileCollider.enabled, Is.False, "First crumbling tile should collapse after sustained player contact.");
        }

        [UnityTest]
        public IEnumerator GoalTriggerCompletesLevelAndPersistsBadgeMasks()
        {
            yield return LoadLevel(13);
            Transform goal = FindComponents("GoalTrigger").Single().transform;
            Transform playerTransform = GetPlayerTransform();
            var playerBody = playerTransform.GetComponent<Rigidbody2D>();
            Assert.That(playerBody, Is.Not.Null, "Player should expose a Rigidbody2D for goal trigger smoke.");

            playerBody.linearVelocity = Vector2.zero;
            playerBody.angularVelocity = 0f;
            playerTransform.position = goal.position;

            yield return WaitFixedFrames(12);

            int levelBit = 1 << 13;
            Assert.That(PlayerPrefs.GetInt("RedBall.CompletedMask", 0) & levelBit, Is.Not.Zero, "Goal trigger should mark Level 14 complete.");
            Assert.That(PlayerPrefs.GetInt("RedBall.Badges.ClearMask", 0) & levelBit, Is.Not.Zero, "Goal trigger completion should persist the Clear badge.");
            Assert.That(PlayerPrefs.GetInt("RedBall.Badges.CleanRunMask", 0) & levelBit, Is.Not.Zero, "A no-damage smoke completion should persist the Clean Run badge.");
        }

        [UnityTest]
        public IEnumerator BadgeMasksRoundTripThroughPlayerPrefsAndRuntimeLoad()
        {
            UnityEngine.Object.Destroy(gameObject);
            gameObject = null;
            game = null;
            yield return null;

            int mask = (1 << 0) | (1 << 13) | (1 << 14);
            PlayerPrefs.SetInt("RedBall.UnlockedCount", 15);
            PlayerPrefs.SetInt("RedBall.Hearts", 5);
            PlayerPrefs.SetInt("RedBall.Badges.ClearMask", mask);
            PlayerPrefs.SetInt("RedBall.Badges.AllCoinsMask", mask);
            PlayerPrefs.SetInt("RedBall.Badges.CleanRunMask", mask);
            PlayerPrefs.Save();

            CreateGame();
            yield return null;

            Assert.That(GetPrivateInt("badgeClearMask") & mask, Is.EqualTo(mask));
            Assert.That(GetPrivateInt("badgeAllCoinsMask") & mask, Is.EqualTo(mask));
            Assert.That(GetPrivateInt("badgeCleanRunMask") & mask, Is.EqualTo(mask));
        }

        private IEnumerator AssertLevelContract(LevelContract contract)
        {
            string rootName = InvokeString(RequireMethod(gameType, "LoadLevelForSmokeTest", BindingFlags.Public | BindingFlags.Instance), game, contract.LevelIndex);
            yield return null;
            yield return new WaitForFixedUpdate();

            Assert.That(rootName, Does.Contain(contract.ExpectedRootName));
            Assert.That(GetPlayerTransform(), Is.Not.Null, contract.ExpectedRootName + " should spawn a player.");
            Assert.That(FindComponents("GoalTrigger").Length, Is.EqualTo(1), contract.ExpectedRootName + " should expose exactly one goal.");
            Assert.That(FindComponents("Hazard").Length, Is.GreaterThanOrEqualTo(contract.MinimumHazards), contract.ExpectedRootName + " should expose expected hazard triggers.");
            Assert.That(FindComponents("CheckpointTrigger").Length, Is.EqualTo(contract.Checkpoints), contract.ExpectedRootName + " checkpoint contract changed.");
            Assert.That(FindComponents("MovingPlatform").Length, Is.EqualTo(contract.MovingPlatforms), contract.ExpectedRootName + " moving-platform contract changed.");
            Assert.That(FindComponents("RedBallCrumblingTile").Length, Is.EqualTo(contract.CrumblingTiles), contract.ExpectedRootName + " crumbling-tile contract changed.");
            Assert.That(GetPlayerTransform().GetComponent<Rigidbody2D>(), Is.Not.Null, contract.ExpectedRootName + " player should have a Rigidbody2D.");
        }

        private IEnumerator LoadLevel(int levelIndex)
        {
            InvokeString(RequireMethod(gameType, "LoadLevelForSmokeTest", BindingFlags.Public | BindingFlags.Instance), game, levelIndex);
            yield return null;
            yield return new WaitForFixedUpdate();
        }

        private static IEnumerator WaitFixedFrames(int frames)
        {
            for (int i = 0; i < frames; i++)
            {
                yield return new WaitForFixedUpdate();
            }
        }

        private void CreateGame()
        {
            gameObject = new GameObject("RedBall PlayMode Harness");
            game = gameObject.AddComponent(gameType);
            Camera camera = gameObject.GetComponentInChildren<Camera>();
            if (camera != null && camera.GetComponent<AudioListener>() == null)
            {
                camera.gameObject.AddComponent<AudioListener>();
            }
        }

        private void DestroyExistingGames()
        {
            foreach (MonoBehaviour component in UnityEngine.Object.FindObjectsByType<MonoBehaviour>())
            {
                if (component != null && component.GetType() == gameType)
                {
                    UnityEngine.Object.Destroy(component.gameObject);
                }
            }
        }

        private Transform GetPlayerTransform()
        {
            PropertyInfo property = gameType.GetProperty("PlayerTransform", BindingFlags.Public | BindingFlags.Instance);
            Assert.That(property, Is.Not.Null, "RedBallGame.PlayerTransform smoke property is required.");
            return (Transform)property.GetValue(game);
        }

        private float GetKillY()
        {
            PropertyInfo property = gameType.GetProperty("KillY", BindingFlags.Public | BindingFlags.Instance);
            Assert.That(property, Is.Not.Null, "RedBallGame.KillY smoke property is required.");
            return (float)property.GetValue(game);
        }

        private int GetPrivateInt(string fieldName)
        {
            FieldInfo field = gameType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.That(field, Is.Not.Null, fieldName + " field should exist for PlayerPrefs roundtrip verification.");
            return (int)field.GetValue(game);
        }

        private static MonoBehaviour[] FindComponents(string typeName)
        {
            return UnityEngine.Object.FindObjectsByType<MonoBehaviour>()
                .Where(component => component != null && component.GetType().Name == typeName)
                .ToArray();
        }

        private static Type RequireType(string typeName)
        {
            Type type = Type.GetType(typeName + ", Assembly-CSharp");
            Assert.That(type, Is.Not.Null, typeName + " was not found in Assembly-CSharp.");
            return type;
        }

        private static MethodInfo RequireMethod(Type type, string methodName, BindingFlags flags)
        {
            MethodInfo method = type.GetMethod(methodName, flags);
            Assert.That(method, Is.Not.Null, type.Name + "." + methodName + " was not found.");
            return method;
        }

        private static string InvokeString(MethodInfo method, object target, params object[] args)
        {
            return (string)Invoke(method, target, args);
        }

        private static object Invoke(MethodInfo method, object target, params object[] args)
        {
            try
            {
                return method.Invoke(target, args);
            }
            catch (TargetInvocationException exception) when (exception.InnerException != null)
            {
                ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
                throw;
            }
        }

        private static void ClearProgressPrefs()
        {
            foreach (string key in ProgressKeys)
            {
                PlayerPrefs.DeleteKey(key);
            }

            PlayerPrefs.Save();
        }

        private readonly struct LevelContract
        {
            public LevelContract(int levelIndex, string expectedRootName, int minimumHazards, int checkpoints, int movingPlatforms, int crumblingTiles)
            {
                LevelIndex = levelIndex;
                ExpectedRootName = expectedRootName;
                MinimumHazards = minimumHazards;
                Checkpoints = checkpoints;
                MovingPlatforms = movingPlatforms;
                CrumblingTiles = crumblingTiles;
            }

            public int LevelIndex { get; }
            public string ExpectedRootName { get; }
            public int MinimumHazards { get; }
            public int Checkpoints { get; }
            public int MovingPlatforms { get; }
            public int CrumblingTiles { get; }
        }
    }
}
