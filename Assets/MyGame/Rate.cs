using UnityEngine;
public class Rate : MonoBehaviour
{
    public GameObject rateDialog;
    public void OpenRate() { rateDialog.SetActive(true);  }
    public void RateNow()  { Application.OpenURL(""); }
    public void NoThanks() {  rateDialog.SetActive(false); }
}
