using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class TutoManager : MonoBehaviour
{
    public string prevName;
    public string nextName;
    private string name;
    public Animator animator;


    private void Start()
    {
        string path = Application.persistentDataPath + "/saveData.fun";

        if (File.Exists(path))
        {
            SceneManager.LoadScene(1);
        }

    }
    public void ChangeScenePrev()
    {
        name = prevName;
        animator.SetTrigger("fadeout");
    }

    public void ChangeSceneNext()
    {
        name = nextName;
        animator.SetTrigger("fadeout");
    }

    public void skip()
    {
        SceneManager.LoadScene(1);
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(name);
    }
}