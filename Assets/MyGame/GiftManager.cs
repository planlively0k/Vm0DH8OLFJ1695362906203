using System;
using UnityEngine;
using UnityEngine.UI;
public class GiftManager : MonoBehaviour
{
    private float thirtyGiftTimeSet = 1800f;
    private float thirtyGiftTime;
    private float speed;
    public Transform thirtyGift;
    private static DateTime startDate;
    private static DateTime today;
    private int hasADay;
    public Transform dailyGift;
    private void Start()
    {
        if(PlayerPrefs.GetInt("FirstTime") == 0) hasADay = 1;
        thirtyGift.GetChild(3).GetComponent<Button>().onClick.AddListener(() => ClaimThirtyGift());
        dailyGift.GetChild(3).GetComponent<Button>().onClick.AddListener(() => ClaimDailyGift());
        BeginThirtyTime();
    }
    private void Update()
    {
        thirtyGiftTime -= speed * Time.deltaTime;
        if (thirtyGiftTime < 0.1f) 
        {
            speed = 0f;
            thirtyGift.GetChild(3).gameObject.SetActive(true);
            thirtyGift.GetChild(2).GetComponent<Text>().text = "claim now!";
        }
        else
        {
            string thirtyGiftTimeS = string.Format("{0}:{01:00}", (int) thirtyGiftTime / 60, (int) thirtyGiftTime % 60);
            thirtyGift.GetChild(2).GetComponent<Text>().text = "claim after " + thirtyGiftTimeS;
        }
        if (PlayerPrefs.GetInt("FirstTime") > 0) CheckDay();
    }
    public void BeginThirtyTime()
    {
        thirtyGift.GetChild(3).gameObject.SetActive(false);
        thirtyGiftTime = thirtyGiftTimeSet;
        speed = 1f;
    }
    private void ClaimThirtyGift()
    {
        HomeUi.instance.coins += 5;
        PlayerPrefs.SetInt("Coins", HomeUi.instance.coins);
        BeginThirtyTime();
    }
    private void SetStartDate()
    {
        startDate = DateTime.Now;
        PlayerPrefs.SetString("DateInitialized", startDate.ToString());
    }
    public static string GetDaysPassed()
    {
        today = DateTime.Now;
        TimeSpan elapsed = today.Subtract(startDate);
        double days = elapsed.TotalDays;
        return days.ToString("0");
    }
    public void CheckDay()
    {
        hasADay = int.Parse(GetDaysPassed());
        if (hasADay == 1)
        {
            dailyGift.GetChild(3).gameObject.SetActive(true);
            dailyGift.GetChild(2).GetComponent<Text>().text = "claim : 5 diamonds!";
        }
        else
        {
            dailyGift.GetChild(3).gameObject.SetActive(false);
            dailyGift.GetChild(2).GetComponent<Text>().text = "tomorrow : 5 diamonds";
        }
    }
    private void ClaimDailyGift()
    {
        HomeUi.instance.coins += 5;
        PlayerPrefs.SetInt("Coins", HomeUi.instance.coins);
        SetStartDate();
        if (PlayerPrefs.GetInt("FirstTime") == 0) PlayerPrefs.SetInt("FirstTime", 1);
    }
}
