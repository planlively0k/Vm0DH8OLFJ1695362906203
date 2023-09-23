using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OneSignalSDK;

public class EAOnesignalModul : MonoBehaviour
{
        private string ONESIGNAL_ID = "9be403f0-9121-4828-9b55-a615da195744";
    // Start is called before the first frame update
    void Start()
    {
        OneSignal.Default.Initialize(ONESIGNAL_ID);
    }

}
