using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using TMPro;
using NaughtyAttributes;


public class Deck : MonoBehaviour
{
    [SerializeField] int CarteAJouer;
    private List<CardObject> GraveYard = new List<CardObject>();
    private List<CardObject> Hand = new List<CardObject>();
    public List<CardObject> deck;
    public Transform[] cardSlots;
    public bool[] availableCardSlots;
    public TMP_Text DeckCount;
    public TMP_Text graveyardCount;

    public void Awake()
    {
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
    }
    public void PlayCard(int Index)
    {
        if(Hand.Count > 0)
        {
            Hand[Index].gameObject.SetActive(false);
            GraveYard.Add(Hand[Index]);
            Hand.RemoveAt(Index);
            for (int i= Index; i < Hand.Count; i++)
            {
                Hand[i].transform.position = cardSlots[i].transform.position;
                Hand[i].transform.rotation = cardSlots[i].transform.rotation;
                availableCardSlots[i] = false;
            }
            availableCardSlots[Hand.Count] = true;


        }
        else
        {
            print("Pas De Cartes bouffon");
        }
    }
    public void DrawCard()
    {
        print("draw");
        if (deck.Count >= 1)
        {
            CardObject randCard = deck[UnityEngine.Random.Range(0, deck.Count)];

            for (int i = 0; i < availableCardSlots.Length; i++)
            {
                if (availableCardSlots[i] == true)
                {
                    randCard.gameObject.SetActive(true);
                    randCard.transform.position = cardSlots[i].position;
                    randCard.transform.rotation = cardSlots[i].rotation;
                    Hand.Add(randCard);
                    availableCardSlots[i] = false;
                    deck.Remove(randCard);
                    if (deck.Count == 0)
                    {
                        ShuffleGraveyardToHand();
                    }
                    return;
                }
            }
        }
        else
        {
            if (GraveYard.Count > 0) {
                print("Shuffle");
                ShuffleGraveyardToHand();
                DrawCard();
            }
            else
            {
                print("Graveyard vide idiot");
            }

        }
    }
    public List<CardObject> Shuffle(List<CardObject> liste)
    {
        List<CardObject> retour = new List<CardObject>();
        while (liste.Count != 0)
        {
            CardObject elem = liste[UnityEngine.Random.Range(0, liste.Count)];
            retour.Add(elem);
            liste.Remove(elem);
        }
        return retour;
    }
    public void ShuffleGraveyardToHand()
    {
        foreach(CardObject Carte in Shuffle(GraveYard))
        deck.Add(Carte);
        GraveYard.Clear();
    }

    private void Update()
    {
        DeckCount.text = deck.Count.ToString();
        graveyardCount.text = GraveYard.Count.ToString();
    }

    [Button]
    private void IDrawCard() {
        DrawCard();
    }
    [Button]
    private void IPlayCard()
    {
        PlayCard(CarteAJouer);
    }
}
