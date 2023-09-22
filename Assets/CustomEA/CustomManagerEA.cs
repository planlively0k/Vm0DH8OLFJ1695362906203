using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomManagerEA : MonoBehaviour
{

    public static CustomManagerEA instance;
    public List<SongJson> listSong;


    private void Awake() {
        if(instance) {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    
}
