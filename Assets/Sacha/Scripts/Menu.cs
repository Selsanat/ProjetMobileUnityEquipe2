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


    public float speed = 1;
    //public SpriteRenderer space;
    bool acsend = false;

    /*void Update()
    {
        if (!acsend)
        {
            if(space.color.a >= 0) 
            {
                space.color = new Color(space.color.r, space.color.g, space.color.b, space.color.a - 0.02f * speed);
            }else
            {
                acsend = !acsend;
            }
        }
        else
        {
            if (space.color.a <= 1)
            {
                space.color = new Color(space.color.r, space.color.g, space.color.b, space.color.a + 0.02f * speed);
            }
            else
            {
                acsend = !acsend;
            }
        }
    }*/

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
        gameObject.SetActive(false);
    }
}
