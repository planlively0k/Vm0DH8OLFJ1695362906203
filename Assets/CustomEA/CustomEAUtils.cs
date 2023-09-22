using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using EaAds;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class JSONData {
    public List<SongJson> ListSong { get; set; }
}

[System.Serializable]
public class SongJson {
    public string title;
    public string urlSong;
    public int bpm;
}


public class CustomEAUtils 
{
   public static async Task<JSONData> GetJsonDataFromServer(string url, Action<DataProgress> progresss)
    {
        var dataProgress = new DataProgress();
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            // Send request and wait
            www.SendWebRequest();

            // While the request is still processing...
            while (!www.isDone)
            {
                var progresssPersen = string.Format("{0:0}%", www.downloadProgress * 100);

                dataProgress.progressPersen = progresssPersen;
                dataProgress.progressFloat = www.downloadProgress;

                progresss.Invoke(dataProgress);
                await Task.Delay(100);  // Tunggu sedikit sebelum memeriksa lagi.
            }

            // If there are network errors
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                return null;
            }
            else
            {
                // Parse JSON response
                return JsonConvert.DeserializeObject<JSONData>(www.downloadHandler.text);
            }
        }
    }

    public static JSONData GetJsonDataFromLocal()
    {
        TextAsset jsonTextAsset = Resources.Load<TextAsset>("JsonData");
        // Parse JSON response
        return JsonConvert.DeserializeObject<JSONData>(jsonTextAsset.text);
    }

     public static async Task<AudioClip> LoadAudioFromUrl(string url, Action<DataProgress> progresss)
    {
        var dataProgress = new DataProgress();
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            www.SendWebRequest();

            while (!www.isDone)
            {
                var progresssPersen = string.Format("{0:0}%", www.downloadProgress * 100);

                dataProgress.progressPersen = progresssPersen;
                dataProgress.progressFloat = www.downloadProgress;

                progresss.Invoke(dataProgress);
                await Task.Delay(100);
            }

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                return null;
            }
            else
            {
                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);
                return audioClip;
            }
        }
    }

    public static AudioClip LoadAudioFromLocal(string audioFilePath, Action<DataProgress> progresss)
    {

        /*
        string absolutePath = Path.Combine(Application.dataPath, relativePath);
        var dataProgress = new DataProgress();
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip("file://" + absolutePath, AudioType.WAV))
        {
            request.SendWebRequest().completed += operation =>
            {
                // Report full progress
                dataProgress.progressPersen = "100%";
                dataProgress.progressFloat = 10;

                progresss.Invoke(dataProgress);
                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Failed to load audio at path: {absolutePath}, Error: {request.error}");
                }
            };

            // Check progress and report it
            while (!request.isDone)
            {
                await Task.Delay(100); // Delay to prevent freezing
                var progresssPersen = string.Format("{0:0}%", request.downloadProgress * 100);

                dataProgress.progressPersen = progresssPersen;
                dataProgress.progressFloat = request.downloadProgress;

                progresss.Invoke(dataProgress);
                await Task.Delay(100);
            }

            return DownloadHandlerAudioClip.GetContent(request);
        }
        */
         AudioClip audioClip = Resources.Load<AudioClip>(audioFilePath);
         return audioClip;
        
    }

    public static string getLinkRateApp () {
        return "https://play.google.com/store/apps/details?id="+Application.identifier;
    }
}
