using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class NewTutoManager : MonoBehaviour
{
    public GameObject[] parts;

    private GameObject currentPart;

    public Animator animator;

    private void Start()
    {
        string path = Application.persistentDataPath + "/saveData.fun";

        if (File.Exists(path))
        {
            //SceneManager.LoadScene(1);
        }

        currentPart = parts[0];
    }

    public void ChangePart(GameObject pPart)
    {
        //animator.SetTrigger("fadeout");
        currentPart.SetActive(false);
        currentPart = pPart;
        pPart.SetActive(true);
    }

    public void Skip()
    {
        SceneManager.LoadScene(1);
    }

    public void OnFadeComplete()
    {
        //animator.SetTrigger("fadein");
    }
}
