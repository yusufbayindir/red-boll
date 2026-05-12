using UnityEngine;

public enum RedBallLevelButtonState
{
    Locked,
    Available,
    Completed,
    ContinueTarget,
    Current
}

public static class RedBallUi
{
    public static RedBallLevelButtonState GetLevelButtonState(bool unlocked, bool completed, bool current, bool continueTarget)
    {
        if (!unlocked)
        {
            return RedBallLevelButtonState.Locked;
        }

        if (current)
        {
            return RedBallLevelButtonState.Current;
        }

        if (continueTarget)
        {
            return RedBallLevelButtonState.ContinueTarget;
        }

        return completed ? RedBallLevelButtonState.Completed : RedBallLevelButtonState.Available;
    }

    public static Color GetLevelButtonColor(RedBallLevelButtonState state)
    {
        switch (state)
        {
            case RedBallLevelButtonState.Locked:
                return new Color(0.11f, 0.14f, 0.17f, 0.62f);
            case RedBallLevelButtonState.Completed:
                return new Color(0.12f, 0.58f, 0.34f, 0.9f);
            case RedBallLevelButtonState.ContinueTarget:
            case RedBallLevelButtonState.Current:
                return new Color(0.94f, 0.36f, 0.22f, 0.94f);
            default:
                return new Color(0.08f, 0.18f, 0.23f, 0.9f);
        }
    }

    public static Color GetLevelButtonTextColor(RedBallLevelButtonState state)
    {
        return state == RedBallLevelButtonState.Locked
            ? new Color(1f, 1f, 1f, 0.54f)
            : Color.white;
    }

    public static string GetLevelButtonLabel(int levelNumber, RedBallLevelButtonState state)
    {
        return GetLevelButtonLabel(levelNumber, state, false, false, false);
    }

    public static string GetLevelButtonLabel(int levelNumber, RedBallLevelButtonState state, bool clearBadge, bool allCoinsBadge, bool cleanRunBadge)
    {
        string feature = GetFeaturedLevelLabel(levelNumber);
        string stateLabel = string.Empty;
        switch (state)
        {
            case RedBallLevelButtonState.Locked:
                stateLabel = RedBallLocalization.T("level.state.locked");
                break;
            case RedBallLevelButtonState.Completed:
                stateLabel = RedBallLocalization.T("level.state.completed");
                break;
            case RedBallLevelButtonState.ContinueTarget:
                stateLabel = RedBallLocalization.T("level.state.continue");
                break;
            case RedBallLevelButtonState.Current:
                stateLabel = RedBallLocalization.T("level.state.current");
                break;
        }

        string label = levelNumber.ToString("00");
        if (!string.IsNullOrEmpty(feature))
        {
            label += "\n" + feature;
        }

        if (!string.IsNullOrEmpty(stateLabel))
        {
            label += "\n" + stateLabel;
        }

        return label;
    }

    public static string GetFeaturedLevelLabel(int levelNumber)
    {
        switch (levelNumber)
        {
            case 14:
                return RedBallLocalization.T("level.feature.14");
            case 15:
                return RedBallLocalization.T("level.feature.15");
            default:
                return string.Empty;
        }
    }

    public static string GetLevelBadgeSummary(bool clearBadge, bool allCoinsBadge, bool cleanRunBadge)
    {
        return (clearBadge ? "G" : ".") + (allCoinsBadge ? "C" : ".") + (cleanRunBadge ? "T" : ".");
    }

    public static string GetCompletionBadgeSummary(bool clearBadge, bool allCoinsBadge, bool cleanRunBadge, bool hasCoins, string cleanRunFailReason)
    {
        string clear = clearBadge ? RedBallLocalization.T("completion.clear.yes") : RedBallLocalization.T("completion.clear.no");
        string coins = !hasCoins ? RedBallLocalization.T("completion.coins.none") : allCoinsBadge ? RedBallLocalization.T("completion.coins.yes") : RedBallLocalization.T("completion.coins.no");
        string clean = cleanRunBadge ? RedBallLocalization.T("completion.clean.yes") : RedBallLocalization.T("completion.clean.no");
        if (!cleanRunBadge && !string.IsNullOrEmpty(cleanRunFailReason))
        {
            clean += " (" + RedBallLocalization.CleanReason(cleanRunFailReason) + ")";
        }

        return clear + " | " + coins + " | " + clean;
    }
}
