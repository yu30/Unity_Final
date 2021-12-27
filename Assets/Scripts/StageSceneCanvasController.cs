using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSceneCanvasController : MonoBehaviour
{
    Canvas settingCanvas;
    Canvas escCanvas;
    Canvas tabCanvas;

    void Start()
    {
        settingCanvas = GameObject.Find("SettingCanvas").GetComponent<Canvas>();
        escCanvas = GameObject.Find("ESCCanvas").GetComponent<Canvas>();
        tabCanvas = GameObject.Find("TabCanvas").GetComponent<Canvas>();
        settingCanvas.enabled = false;
        escCanvas.enabled = false;
        tabCanvas.enabled = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            tabCanvas.enabled = true;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            escCanvas.enabled = true;
        }
    }

    public void opensettingCanvas()
    {
        settingCanvas.enabled = true;
    }
    public void closesettingCanvas()
    {
        settingCanvas.enabled = false;
    }

    public void closeescCanvas()
    {
        escCanvas.enabled = false;
    }
    public void closetabCanvas()
    {
        tabCanvas.enabled = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
