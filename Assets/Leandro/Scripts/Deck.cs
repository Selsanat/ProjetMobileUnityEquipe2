using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using TMPro;


public class Deck : MonoBehaviour
{
    public List<GameObject> CardList = new List<GameObject>();
    public List<GameObject> deck;
    public Transform[] cardSlots;
    public bool[] availableCardSlots;
    public TMP_Text textCount;
    public void DrawCard()
    {
        if (deck.Count >= 1)
        {
            GameObject randCard = deck[UnityEngine.Random.Range(0, deck.Count)];

            for (int i = 0; i < availableCardSlots.Length; i++)
            {
                if (availableCardSlots[i] == true)
                {
                    randCard.SetActive(true);
                    randCard.transform.position = cardSlots[i].position;
                    availableCardSlots[i] = false;
                    deck.Remove(randCard);
                    return;
                }
            }
        }
    }

    private void Update()
    {
        textCount.text = deck.Count.ToString();
    }
}
