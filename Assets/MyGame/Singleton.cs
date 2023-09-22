using UnityEngine;
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
	private static T instance;
	public static T Instance => instance;
	public static bool IsInitialized => (Object)instance != (Object)null;
	protected virtual void Awake()
	{
		if ((Object)instance != (Object)null) UnityEngine.Debug.LogError("[Singleton] Trying to instantiate a second instance of a singleton class");
		else instance = (T)this;
	}
	protected virtual void OnDestroy()
	{
		if (instance == this) instance = null;
	}
}
