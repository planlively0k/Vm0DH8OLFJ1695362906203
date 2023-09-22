using UnityEngine;
public class RotateLogo : MonoBehaviour
{
    [SerializeField] int speed;
    private void Update() { transform.Rotate(0, 0, speed * Time.deltaTime); }
}
