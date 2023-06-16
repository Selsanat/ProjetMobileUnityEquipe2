using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutoManager : MonoBehaviour
{
    public string prevName;
    public string nextName;
    private string name;
    public Animator animator;

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

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(name);
    }
}