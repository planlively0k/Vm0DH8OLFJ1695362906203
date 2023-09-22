using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarmSong : MonoBehaviour
{
    public Button btnWatchAds;
    // Start is called before the first frame update
    void Start()
    {
        btnWatchAds.onClick.AddListener(() => {
           if(AdsManagerWrapper.INSTANCE)
                AdsManagerWrapper.INSTANCE.ShowRewards((onAdLoded) => {}, onAdFailedToLoad => {}, (irewards) => {
                    SongManager.instance.unlockByAds();
                });
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
