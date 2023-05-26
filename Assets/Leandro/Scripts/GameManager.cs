using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(Fight))]
public class GameManager : MonoBehaviour
{
    
    
    public List<CardObject> Hand;
    public entityManager entityManager;
    public Fight FM;


    public CardObject CarteUtilisee = null;
    public bool HasCardInHand = false;
    public float RangePourActiverCarte;

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
        entityManager = FindObjectOfType<entityManager>();

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
