using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutoManager : MonoBehaviour
{
    public string prevSceneName;
    public string nextSceneName;

    public void ChangePrevScene()
    {
        SceneManager.LoadScene(prevSceneName);
    }

    public void ChangeNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
        
    }
}