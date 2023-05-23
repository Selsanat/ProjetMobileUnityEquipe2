using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using TMPro;
using NaughtyAttributes;


public class Deck : MonoBehaviour
{
    private List<GameObject> GraveYard = new List<GameObject>();
    private List<GameObject> Hand = new List<GameObject>();
    public List<GameObject> deck;
    public Transform[] cardSlots;
    public bool[] availableCardSlots;
    public TMP_Text DeckCount;
    public TMP_Text graveyardCount;

    public void PlayFirstCard()
    {
        if(Hand.Count > 0)
        {
            GraveYard.Add(Hand[0]);
            GraveYard[0].SetActive(false);
            Hand.RemoveAt(0);       
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
            GameObject randCard = deck[UnityEngine.Random.Range(0, deck.Count)];

            for (int i = 0; i < availableCardSlots.Length; i++)
            {
                if (availableCardSlots[i] == true)
                {
                    randCard.SetActive(true);
                    randCard.transform.position = cardSlots[i].position;
                    Hand.Add(randCard);
                    availableCardSlots[i] = false;
                    deck.Remove(randCard);
                    return;
                }
            }
        }
        else
        {
            print("Shuffle");
            ShuffleGraveyardToHand();
            DrawCard();
        }
    }
    public List<GameObject> Shuffle(List<GameObject> liste)
    {
        List<GameObject> retour = new List<GameObject>();
        while (liste.Count != 0)
        {
            GameObject elem = liste[UnityEngine.Random.Range(0, liste.Count)];
            retour.Add(elem);
            liste.Remove(elem);
        }
        return retour;
    }
    public void ShuffleGraveyardToHand()
    {
        deck = Shuffle(GraveYard);
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
        PlayFirstCard();
    }
}
