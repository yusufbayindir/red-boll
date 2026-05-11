# Ads Pipeline Architecture

## Source

Agent: Ads Pipeline Architect  
Task: `TASK-0006`  
Date: 2026-05-11

## Position

Red Ball should ship a rewarded-ad boundary before any SDK import. The first ad product should be opt-in rewarded recovery from menu or level select, not an interruption inside gameplay and not an ad-driven death continue.

The current runtime has heart loss in `DamagePlayer`, lockout in `TryStartLevel`, heart persistence in `SaveHearts`, and level unlock persistence in `MarkLevelCompleted` / `SaveProgress`. Those methods should not learn about SDKs or placement ids. They should only call a local gameplay/economy service such as `IHeartWallet` or an `IRewardedHeartController` facade.

## Recommended Ad Product Order

1. Rewarded heart recovery.
   - Offer from main menu and level select when hearts are below max, strongest when hearts are 0.
   - Grant exactly 1 heart per completed rewarded view.
   - Daily cap: 3 grants per device per UTC day for MVP.
   - Cooldown: 2 minutes between attempts, including skipped/failed attempts, to prevent spam.
   - No prompt during active gameplay or immediately on a death popup.

2. Rewarded post-level bonus, later and only if a real persistent economy exists.
   - The current score is not a durable economy, so doubling score is weak value and risks teaching players that completion rewards are ad-gated.
   - If mastery badges ship first, do not monetize badge progress.

3. Interstitials: do not ship in MVP.
   - If PM later approves them, allow only after natural navigation such as returning to menu after several completed sessions.
   - Never show after damage, failure, restart, locked-level tap, or before first gameplay.

Rejected for MVP:

- Banners during gameplay.
- Rewarded continues from the death moment.
- Ads to unlock levels, checkpoints, clean-run badges, all-coins badges, or movement advantages.

## Unity Boundary

Keep four layers:

1. Gameplay domain
   - Owns hearts, progress, badges, level flow.
   - Knows nothing about ad SDKs.

2. Monetization coordinator
   - Converts player intent into an ad request.
   - Applies placement policy, caps, cooldowns, and idempotent reward grants.

3. Ad service abstraction
   - Owns load/show lifecycle for rewarded ads.
   - Has a fake implementation for Editor/dev builds and a future provider adapter for SDKs.

4. Provider adapter
   - Future SDK-specific implementation behind compile flags or asmdef boundaries.
   - SDK types must not leak into gameplay or UI code.

Suggested folders for future implementation:

- `Assets/Scripts/Economy/IHeartWallet.cs`
- `Assets/Scripts/Ads/IAdService.cs`
- `Assets/Scripts/Ads/RewardedAdCoordinator.cs`
- `Assets/Scripts/Ads/Fake/FakeAdService.cs`
- `Assets/Scripts/Ads/Config/AdConfig.cs`
- `Assets/Scripts/Ads/Providers/<ProviderName>/...` only after PM approval

## Interfaces And Pseudocode

```csharp
public enum AdPlacementId
{
    RewardedHeartRecovery,
    RewardedPostLevelBonus,
    FutureInterstitialReturnToMenu
}

public enum RewardedAdOutcome
{
    Completed,
    Skipped,
    NoFill,
    Failed,
    BlockedByConsent,
    AlreadyShowing,
    DisabledByPolicy
}

public readonly struct RewardedAdResult
{
    public readonly AdPlacementId Placement;
    public readonly RewardedAdOutcome Outcome;
    public readonly string ShowId;
    public readonly string ErrorCode;
}

public interface IAdService
{
    bool IsInitialized { get; }
    void Initialize(AdRuntimeConfig config, IAdConsentState consent);
    bool CanShowRewarded(AdPlacementId placement);
    Task<RewardedAdResult> ShowRewardedAsync(AdPlacementId placement);
}

public interface IAdConsentService
{
    IAdConsentState Current { get; }
    Task<IAdConsentState> EnsureReadyAsync();
    Task ShowPrivacyOptionsAsync();
}

public interface IHeartWallet
{
    int CurrentHearts { get; }
    int MaxHearts { get; }
    bool CanAddHeart { get; }
    bool TryAddHeart(string source, string grantId);
}

public interface IAdPlacementPolicy
{
    PlacementDecision CanOffer(AdPlacementId placement, PlayerContext context);
    void RecordAttempt(AdPlacementId placement, RewardedAdOutcome outcome);
}
```

Coordinator:

```csharp
public sealed class RewardedHeartController
{
    private readonly IAdService ads;
    private readonly IAdConsentService consent;
    private readonly IHeartWallet hearts;
    private readonly IAdPlacementPolicy policy;
    private readonly IRewardGrantLedger ledger;

    public async Task<RewardedHeartUiState> GetButtonState(PlayerContext context)
    {
        var decision = policy.CanOffer(AdPlacementId.RewardedHeartRecovery, context);
        if (!decision.CanOffer || !hearts.CanAddHeart)
            return RewardedHeartUiState.Hidden(decision.Reason);

        if (!ads.CanShowRewarded(AdPlacementId.RewardedHeartRecovery))
            return RewardedHeartUiState.Disabled("Video unavailable");

        return RewardedHeartUiState.Enabled("+1 heart");
    }

    public async Task<RewardedHeartResult> TryShowHeartAdAsync(PlayerContext context)
    {
        var decision = policy.CanOffer(AdPlacementId.RewardedHeartRecovery, context);
        if (!decision.CanOffer || !hearts.CanAddHeart)
            return RewardedHeartResult.NotGranted(decision.Reason);

        var consentState = await consent.EnsureReadyAsync();
        if (!consentState.CanRequestAds)
            return RewardedHeartResult.NotGranted("Ads unavailable");

        var adResult = await ads.ShowRewardedAsync(AdPlacementId.RewardedHeartRecovery);
        policy.RecordAttempt(AdPlacementId.RewardedHeartRecovery, adResult.Outcome);

        if (adResult.Outcome != RewardedAdOutcome.Completed)
            return RewardedHeartResult.NotGranted("No reward granted");

        var grantId = "rewarded-heart:" + adResult.ShowId;
        if (!ledger.TryBeginGrant(grantId))
            return RewardedHeartResult.NotGranted("Reward already claimed");

        bool granted = hearts.TryAddHeart("rewarded_ad", grantId);
        ledger.CompleteGrant(grantId, granted);
        return granted
            ? RewardedHeartResult.Granted("+1 heart")
            : RewardedHeartResult.NotGranted("Heart storage full");
    }
}
```

Runtime integration, later:

```csharp
// RedBallGame or future MenuController should only see this facade:
rewardedHeartButton.onClick.AddListener(() => rewardedHeartController.TryShowHeartAdAsync(CurrentPlayerContext));

// DamagePlayer, SpendHeart, TryStartLevel, MarkLevelCompleted, and SaveProgress
// should remain gameplay/economy methods. Do not place SDK calls there.
```

## Placement Policy

Rewarded heart recovery:

- Surface only on main menu and level select.
- Use clear text: "Watch video: +1 heart".
- Hide when hearts are full.
- Disable with calm status text on no-fill: "Video unavailable".
- Grant only after `Completed`.
- Do not consume a heart, level attempt, score, or badge opportunity if the ad fails.
- Cap at 3 completed rewarded heart grants per day.
- Cooldown all attempts for 2 minutes to avoid repeated failed ad prompts.
- Never auto-open. The player must tap.

Interstitials, future only:

- Default disabled.
- Minimum 5 minutes since last interstitial.
- Minimum 3 completed levels or menu returns since last interstitial.
- Never in the first session.
- Never after death, restart, no-heart lockout, or locked-level tap.
- Never cover mastery feedback or completion badge reveals.

## Fake Service Testing

Fake service modes:

- `AlwaysReadyCompleted`: completes after a configurable delay and grants reward.
- `NoFill`: `CanShowRewarded` false or returns `NoFill`.
- `Skipped`: simulates player closing early.
- `Failed`: simulates provider error.
- `SlowLoad`: starts unavailable, becomes ready after N seconds.
- `AlreadyShowing`: verifies double taps do not create double grants.

Fake service requirements:

- Works in Editor without internet or SDK.
- Exposes deterministic outcomes in inspector/config.
- Emits a unique `ShowId` per show.
- Logs placement, outcome, and grant id.
- Supports automated tests for cap, cooldown, no-fill, skip, and duplicate callback behavior.

## Privacy And Config Notes

- Do not initialize a real SDK before consent/age/privacy state is resolved.
- Decide whether Red Ball is child-directed. If yes, default to non-personalized/limited ads and comply with COPPA-style requirements.
- For GDPR/UK GDPR regions, collect and pass consent before personalized ad requests.
- For California/US state privacy regimes, support opt-out/limited data use where provider requires it.
- On iOS, request App Tracking Transparency only if the ad stack actually needs IDFA; do not ask just because an SDK is present.
- Maintain App Store privacy nutrition labels, Google Play Data Safety, SDK privacy manifests, SKAdNetwork IDs, and provider data disclosures before release.
- Provide a privacy/options entry from settings or menu once real ads are enabled.
- Use provider test mode and test placement ids in development builds.
- Gate provider adapters with build defines such as `RED_BALL_ADS_FAKE` and future `RED_BALL_ADS_PROVIDER_X`.

## QA Checklist

- Editor fake completed ad grants exactly 1 heart and saves it.
- Completed ad does not grant above max hearts.
- No-fill leaves hearts, progress, and UI unchanged except status text.
- Skipped ad grants nothing.
- Failed ad grants nothing and does not block play.
- Double tap / concurrent show attempt grants at most once.
- Daily cap hides or disables the placement after 3 completed grants.
- Cooldown applies after completed, skipped, failed, and no-fill attempts.
- Button is hidden when hearts are full.
- Button is available from menu/level select only, not while player controls are active.
- No ad prompt appears automatically after damage or level failure.
- Airplane mode or provider outage leaves the game playable.
- Consent denied or ads disabled leaves the game playable.
- Test mode cannot leak into release without an explicit config check.
- Provider callbacks after scene/menu changes do not crash or grant twice.
- Removing the SDK provider still allows fake-service tests to run.

## What Not To Monetize

- Level unlocks.
- Mastery badges: clear, all coins, clean run, stomp/bounce objectives, timers, or future route contracts.
- Checkpoints or respawn position.
- Damage immunity, jump strength, movement speed, physics advantages, or easier hazards.
- Required attempts after a no-heart lockout.
- Death/failure pressure screens.
- Completion feedback screens before the player sees what improved.

## Implementation Recommendation

Do not import an ad SDK yet. First split a small economy/menu boundary out of `RedBallGame`, then add the fake ad service and rewarded heart controller behind config. Only after QA can prove the fake lifecycle should PM approve a provider SDK adapter.
