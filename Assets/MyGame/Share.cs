using System;
using System.Collections;
using System.IO;
using UnityEngine;
public class Share : MonoBehaviour
{
    public static Share instance;
    private void Awake() { instance = this;  }
    public void ShareThisGame()
    {
        var appName = Application.productName;
        string textToShare = "Download now!" + "\n\n" + CustomEAUtils.getLinkRateApp();
        string screenShotPath = Application.persistentDataPath + "/" + appName + ".png";
        if (File.Exists(screenShotPath)) File.Delete(screenShotPath);
        ScreenCapture.CaptureScreenshot(appName + ".png");
        StartCoroutine(DelayedShare(screenShotPath, textToShare));
        if(AdsManagerWrapper.INSTANCE)
			AdsManagerWrapper.INSTANCE.ShowInterstitial((onAdLoded) => {}, onAdFailedToLoad => {});
        // AdmobAd.instance.ShowInterstitial();
    }
    IEnumerator DelayedShare(string screenShotPath, string text)
    {
        while (!File.Exists(screenShotPath)) yield return new WaitForSeconds(.05f);
        SharePlugin.Share(text, screenShotPath, "", "", "image/png", true, "");
    }
    private float width
    {
        get
        {
            return Screen.width;
        }
    }
    private float height
    {
        get
        {
            return Screen.height;
        }
    }
    private void Screenshot()
    {
        StartCoroutine(GetScreenshot());
    }
    private IEnumerator GetScreenshot()
    {
        yield return new WaitForEndOfFrame();
        Texture2D screenshot;
        screenshot = new Texture2D((int)width, (int)height, TextureFormat.ARGB32, false);
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);
        screenshot.Apply();
        Save_Screenshot(screenshot);
    }
    private void Save_Screenshot(Texture2D screenshot)
    {
        string screenShotPath = Application.persistentDataPath + "/" + DateTime.Now.ToString("dd-MM-yyyy_HH:mm:ss") + "_" + Application.productName;
        File.WriteAllBytes(screenShotPath, screenshot.EncodeToPNG());
        StartCoroutine(DelayedShare_Image(screenShotPath));
    }
    private void Clear_SavedScreenShots()
    {
        string path = Application.persistentDataPath;
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] info = dir.GetFiles("*.png");
        foreach (FileInfo f in info)
        {
            File.Delete(f.FullName);
        }
    }
    private IEnumerator DelayedShare_Image(string screenShotPath)
    {
        while (!File.Exists(screenShotPath))
        {
            yield return new WaitForSeconds(.05f);
        }
        NativeShare_Image(screenShotPath);
    }
    private void NativeShare_Image(string screenShotPath)
    {
        string text = "";
        string subject = "";
        string url = "";
        string title = "Select sharing app";

#if UNITY_ANDROID
        subject = "Test subject.";
        text = "Test text";
#endif

#if UNITY_IOS
        subject = "Test subject.";
        text = "Test text";
#endif
        SharePlugin.Share(text, screenShotPath, url, subject, "image/png", true, title);
    }
}
