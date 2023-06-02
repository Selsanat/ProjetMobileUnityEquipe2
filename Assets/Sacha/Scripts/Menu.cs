using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Transform pos1;
    public Transform pos2;
    public List<GameObject> Startcards;

    public List<GameObject> go_credits;


    private void Start()
    {
        float caca = 0; // le retour
/*        foreach (GameObject go in Startcards)
        {
            print(caca);
            go.transform.position = new Vector3(Mathf.Lerp(pos1.position.x, pos2.position.x, caca / (Startcards.Count - 1)), go.transform.position.y, 0);
            //go.transform.eulerAngles = new Vector3();
            caca++;
        }*/
    }
    public void reloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.Instance.gameObject.GetComponent<Menu>().Start();
    }
    private void Awake()
    {
        Start();
    }
    public void Credits()
    {
        foreach(GameObject go in go_credits)
        {
            go.SetActive(!go.activeInHierarchy);
        }
    }
    
    public void Collection()
    {

    }
    public void StartGame()
    {
        SceneManager.LoadScene("levelSelector");
    }
}
