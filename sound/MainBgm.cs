using UnityEngine;
using System.Collections;

public class MainBgm : MonoBehaviour
{
    public static AudioSource Mainbgm;
    void Start()
    {
        if (Mainbgm != null)
            Destroy(gameObject);
        else
        {
            Mainbgm = gameObject.GetComponent<AudioSource>();
            Mainbgm.mute = false;
            DontDestroyOnLoad(gameObject);
        }

    }


}