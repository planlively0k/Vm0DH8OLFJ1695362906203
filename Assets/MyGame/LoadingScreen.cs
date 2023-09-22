using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadingScreen : MonoBehaviour
{
	private void Start() { Invoke("LoadScene", 1.5f); }
	private void LoadScene() { SceneManager.LoadScene(1, LoadSceneMode.Single); }
}
