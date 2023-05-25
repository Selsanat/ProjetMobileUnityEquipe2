using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public float RangePourActiverCarte;
    public List<CardObject> Hand;
    public CardObject CarteUtilisee = null;
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                print("Game manager is null");
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

}
