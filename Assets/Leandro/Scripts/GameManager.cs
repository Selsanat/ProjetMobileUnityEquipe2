using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(Fight))]
public class GameManager : MonoBehaviour
{
    
    public float RangePourActiverCarte;
    public List<CardObject> Hand;
    public CardObject CarteUtilisee = null;
    public bool HasCardInHand = false;

    public Fight FM;
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
    private void Start()
    {
        FM = FindObjectOfType<Fight>();

    }


    private void Awake()
    {

        _instance = this;
    }
/*    public void CardSended(CardObject card)
    {
        CarteUtilisee = card;
        FM.Cardsend(card);
    }*/
}
