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
                return new Color(0.1f, 0.12f, 0.14f, 0.42f);
            case RedBallLevelButtonState.Completed:
                return new Color(0.18f, 0.58f, 0.32f, 0.76f);
            case RedBallLevelButtonState.ContinueTarget:
            case RedBallLevelButtonState.Current:
                return new Color(1f, 0.76f, 0.18f, 0.82f);
            default:
                return new Color(0.08f, 0.13f, 0.18f, 0.66f);
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
        string badges = GetLevelBadgeSummary(clearBadge, allCoinsBadge, cleanRunBadge);
        switch (state)
        {
            case RedBallLevelButtonState.Locked:
                return levelNumber + "\nKilit";
            case RedBallLevelButtonState.Completed:
                return levelNumber + "\nGecti\n" + badges;
            case RedBallLevelButtonState.ContinueTarget:
                return levelNumber + "\nDevam\n" + badges;
            case RedBallLevelButtonState.Current:
                return levelNumber + "\nOynuyor\n" + badges;
            default:
                return levelNumber + "\n" + badges;
        }
    }

    public static string GetLevelBadgeSummary(bool clearBadge, bool allCoinsBadge, bool cleanRunBadge)
    {
        return (clearBadge ? "G" : ".") + (allCoinsBadge ? "C" : ".") + (cleanRunBadge ? "T" : ".");
    }

    public static string GetCompletionBadgeSummary(bool clearBadge, bool allCoinsBadge, bool cleanRunBadge, bool hasCoins, string cleanRunFailReason)
    {
        string clear = clearBadge ? "Gecis var" : "Gecis yok";
        string coins = !hasCoins ? "Coin yok" : allCoinsBadge ? "Tum coin var" : "Coin eksik";
        string clean = cleanRunBadge ? "Temiz var" : "Temiz yok";
        if (!cleanRunBadge && !string.IsNullOrEmpty(cleanRunFailReason))
        {
            clean += " (" + cleanRunFailReason + ")";
        }

        return clear + " | " + coins + " | " + clean;
    }
}
