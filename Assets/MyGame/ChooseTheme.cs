using System;
using UnityEngine;
public class ChooseTheme : MonoBehaviour
{
    public static ChooseTheme instance;
    [Header("Choose theme between 1-8 or check random")]
    public int chooseTheme;
    public bool randomtheme;
    private void Awake()
    {
       
        if (randomtheme == true)
        {
            int random = UnityEngine.Random.Range(1, 9);
            chooseTheme = random;
            instance = this;
        }
        else
        {
            instance = this;
        }
        
    }

   
}
