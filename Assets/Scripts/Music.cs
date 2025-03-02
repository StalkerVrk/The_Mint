using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    private static Music music;
    private void Awake()
    {
        if (music != null)
            Destroy(gameObject);
        else
        {
            music = this;
            DontDestroyOnLoad(transform.gameObject);
        }
    }
}
