using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EASettingManager : MonoBehaviour
{

    public static EASettingManager instance;
    public Sprite spriteMenu;
    public Sprite spriteGamePlay;

    private void Awake()
    {
        if (instance != null)
            Destroy(this);
    }

    private void Start()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

}
