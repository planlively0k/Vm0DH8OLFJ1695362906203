using UnityEngine;
using UnityEngine.UI;
public class AchievementSetting : MonoBehaviour
{
    public GameObject[] achievementItems;
    private int[] achievementLockeds;
    private int[] achievementClaimeds;
    private int[] achievementRewards;
    private int bestScore;
    private void OnEnable()
    {
        achievementLockeds = new int[achievementItems.Length];
        achievementRewards = new int[achievementItems.Length];
        achievementClaimeds = new int[achievementItems.Length];
        CheckData();
        for (int i = 0; i < achievementLockeds.Length; i++) achievementRewards[i] = int.Parse(achievementItems[i].transform.GetChild(2).GetChild(0).GetComponent<Text>().text);
        if (PlayerPrefs.GetInt("BallUnlockedCount") >= 5 && achievementClaimeds[0] == 0) PlayerPrefs.SetInt("AchievementUnlocked" + 0, 1);
        if (PlayerPrefs.GetInt("BallUnlockedCount") >= BallManager.instance.ballButtons.Length && achievementClaimeds[1] == 0) PlayerPrefs.SetInt("AchievementUnlocked" + 1, 1);
        if (bestScore >= 100 && achievementClaimeds[2] == 0) PlayerPrefs.SetInt("AchievementUnlocked" + 2, 1);
        if (bestScore >= 250 && achievementClaimeds[3] == 0) PlayerPrefs.SetInt("AchievementUnlocked" + 3, 1);
        if (bestScore >= 500 && achievementClaimeds[4] == 0) PlayerPrefs.SetInt("AchievementUnlocked" + 4, 1);
        if (bestScore >= 1000 && achievementClaimeds[5] == 0) PlayerPrefs.SetInt("AchievementUnlocked" + 5, 1);
        if (PlayerPrefs.GetInt("SongUnlockedCount") >= 5 && achievementClaimeds[6] == 0) PlayerPrefs.SetInt("AchievementUnlocked" + 6, 1);
        if (PlayerPrefs.GetInt("SongUnlockedCount") >= SongManager.instance.songCount && achievementClaimeds[7] == 0) PlayerPrefs.SetInt("AchievementUnlocked" + 7, 1);
    }
    private void Update()
    {
        CheckData();
    }
    private void CheckData()
    {
        bestScore = PlayerPrefs.GetInt("bestScore");
        for (int i = 0; i < achievementLockeds.Length; i++)
        {
            achievementLockeds[i] = PlayerPrefs.GetInt("AchievementUnlocked" + i);
            achievementClaimeds[i] = PlayerPrefs.GetInt("AchievementClaimed" + i);
            if (achievementLockeds[i] == 0)
            {
                achievementItems[i].transform.GetChild(3).gameObject.SetActive(true);
                achievementItems[i].transform.GetChild(4).gameObject.SetActive(false);
            }
            else if (achievementLockeds[i] == 1)
            {
                if (achievementClaimeds[i] == 0)
                {
                    achievementItems[i].transform.GetChild(3).gameObject.SetActive(false);
                    achievementItems[i].transform.GetChild(4).gameObject.SetActive(true);
                }
                else
                {
                    achievementItems[i].transform.GetChild(3).gameObject.SetActive(false);
                    achievementItems[i].transform.GetChild(4).gameObject.SetActive(false);
                }
            }
        }
    }
    public void ClaimAchievement(int numb)
    {
        HomeUi.instance.coins += achievementRewards[numb];
        PlayerPrefs.SetInt("Coins", HomeUi.instance.coins);
        PlayerPrefs.SetInt("AchievementClaimed" + numb, 1);
    }
}
