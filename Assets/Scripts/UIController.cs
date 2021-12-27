using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GameObject.Find("BGM").GetComponent<AudioSource>();
    }


    public void ModifyVolume(float vol)
    {
        audioSource.volume = vol;
    }
}
