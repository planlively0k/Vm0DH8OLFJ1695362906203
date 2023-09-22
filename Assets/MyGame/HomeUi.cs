using UnityEngine;
using UnityEngine.UI;
public class HomeUi : MonoBehaviour
{
    public static HomeUi instance;
    public GameObject homeScreen, settingScreen, giftScreen, achievementScreen, ballScreen, helpScreen, shopScreen;
    public Image soundButton, musicButton;
    public Sprite[] soundButtons = new Sprite[2];
    public Sprite[] musicButtons = new Sprite[2];
    [HideInInspector] public int coins;
    public Text coinsText;
    public Text txtAppName;
    private void Awake() { instance = this; }
    private void Start()
    {
        if(PlayerPrefs.GetInt("FisrtPlay") == 0)
        {
            PlayerPrefs.SetInt("Music", 1);
            PlayerPrefs.SetInt("FisrtPlay", 1);
        }
        txtAppName.text = Application.productName;
    }
    private void Update()
    {
        if (PlayerPrefs.GetInt("Music") == 0)
        {
            musicButton.sprite = musicButtons[0];
            musicButton.transform.GetChild(1).GetComponent<Text>().text = "Off";
        }
        else
        {
            musicButton.sprite = musicButtons[1];
            musicButton.transform.GetChild(1).GetComponent<Text>().text = "On";
        }
        coins = PlayerPrefs.GetInt("Coins");
        coinsText.text = coins.ToString();
    }
    public void SettingScreen()
    {
        if (settingScreen.activeSelf)
        {
            homeScreen.SetActive(true);
            settingScreen.SetActive(false);
        }
        else
        {
            homeScreen.SetActive(false);
            settingScreen.SetActive(true);
        }
    }
    public void GiftScreen()
    {
        if (giftScreen.activeSelf)
        {
            homeScreen.SetActive(true);
            giftScreen.SetActive(false);
        }
        else
        {
            homeScreen.SetActive(false);
            giftScreen.SetActive(true);
        }
    }
    public void AchievementScreen()
    {
        if (achievementScreen.activeSelf)
        {
            homeScreen.SetActive(true);
            achievementScreen.SetActive(false);
        }
        else
        {
            homeScreen.SetActive(false);
            achievementScreen.SetActive(true);
        }
    }
    public void BallScreen()
    {
        if (ballScreen.activeSelf)
        {
            homeScreen.SetActive(true);
            ballScreen.SetActive(false);
        }
        else
        {
            homeScreen.SetActive(false);
            ballScreen.SetActive(true);
        }
    }
    public void HelpScreen()
    {
        if (helpScreen.activeSelf)
        {
            settingScreen.SetActive(true);
            helpScreen.SetActive(false);
        }
        else
        {
            settingScreen.SetActive(false);
            helpScreen.SetActive(true);
        }
    }
    public void ShopScreen()
    {
        if (shopScreen.activeSelf)
        {
            settingScreen.SetActive(true);
            shopScreen.SetActive(false);
        }
        else
        {
            settingScreen.SetActive(false);
            shopScreen.SetActive(true);
        }
    }
    public void RateThisGame()
    {
        Application.OpenURL(CustomEAUtils.getLinkRateApp());
    }
    public void MusicSetting()
    {
        if (PlayerPrefs.GetInt("Music") == 0) PlayerPrefs.SetInt("Music", 1);
        else PlayerPrefs.SetInt("Music", 0);
    }
}
