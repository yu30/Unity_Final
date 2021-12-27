using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneCanvasController : MonoBehaviour
{
    Canvas settingCanvas;
    // Start is called before the first frame update
    void Start()
    {
        settingCanvas = GameObject.Find("SettingCanvas").GetComponent<Canvas>();
        settingCanvas.enabled = false;
    }

    public void opensettingCanvas()
    {
        settingCanvas.enabled = true;
    }
    public void closesettingCanvas()
    {
        settingCanvas.enabled = false;
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
