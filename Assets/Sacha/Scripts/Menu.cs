using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public List<GameObject> toSwitch;

    void toggle()
    {
        foreach(GameObject go in toSwitch)
        {
            go.SetActive(!go.active);
        }
    }
    
    public void Credits()
    {
        toggle();
    }
    public void Collection()
    {

    }
    public void StartGame()
    {
        SceneManager.LoadScene("levelSelector");
    }
}
