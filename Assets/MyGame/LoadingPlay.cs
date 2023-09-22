using System.Collections;
using System.Collections.Generic;
using EaAds;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPlay : MonoBehaviour
{
    public Image sliderLoading;
    public Text txtLoading;

    public void UpdateLoading(DataProgress dataProgress) {
        sliderLoading.fillAmount = dataProgress.progressFloat;
        txtLoading.text = dataProgress.progressPersen;
    }
}
