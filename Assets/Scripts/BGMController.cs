using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    static bool isHave = false;
    void Start()
    {
        if(!isHave)
        {
            DontDestroyOnLoad(gameObject);
            isHave = true;
        }
    }
}
