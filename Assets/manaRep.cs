using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class manaRep : MonoBehaviour
{

    [SerializeField] Button Skip;
    [SerializeField] Button arbo;
    [SerializeField] Button pretre;
    [SerializeField] GameObject manaRepGm;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        manaRepGm.SetActive(false);

    }


    public void active()
    {
        manaRepGm.SetActive(true);  
    }

    public void skipingOnclick()
    {

    }

    public void arboristeOnclick()
    {

    }

    public void pretreOnclick()
    {

    }
}
