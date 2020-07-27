using UnityEngine;
using GooglePlayGames;

public class Achievements : MonoBehaviour
{
    public void OpenAchievementPanel()
    {
        Social.ShowAchievementsUI();
    }

    public void UpdateIncremental()
    {
        PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_increment, 1, null);
    }

    public void UnlockRegular()
    {
        Social.ReportProgress(GPGSIds.achievement_regular, 100f, null);
    }
}
