using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Fight))]
public class GameManager : MonoBehaviour
{
    
    
    public List<CardObject> Hand;
    public entityManager entityManager;
    public Fight FM;


    public CardObject CarteUtilisee = null;
    public bool CardsInteractable = true;
    public bool HasCardInHand = false;
    public float RangePourActiverCarte;
    public InspectCard InspectUI;
    public float TempsPourClickCardInspect = 0.5f;
    public Deck deck;
    public int LifeArboriste = 50;
    public int LifePretre = 50;
    public bool IsArboristePlayed = false;
    public bool IsPretrePlayed = false;
    public bool isHoverButton = false;
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
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }
/*    public void CardSended(CardObject card)
    {
        CarteUtilisee = card;
        FM.Cardsend(card);
    }*/
}
