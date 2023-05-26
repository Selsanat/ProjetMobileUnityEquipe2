using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Fight))]
public class GameManager : MonoBehaviour
{
    
    public float RangePourActiverCarte;
    public List<CardObject> Hand;
    public CardObject CarteUtilisee = null;
    public bool CardsInteractable = true;
    public bool HasCardInHand = false;
    public InspectCard InspectUI;
    public float TempsPourClickCardInspect = 0.5f;

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
