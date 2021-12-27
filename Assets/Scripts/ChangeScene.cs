using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void Loadsceneasync(int Sceneindex)
    {
        SceneManager.LoadSceneAsync(Sceneindex);
    }

    public void Loadscene(int Sceneindex)
    {
        SceneManager.LoadScene(Sceneindex);
    }
}
