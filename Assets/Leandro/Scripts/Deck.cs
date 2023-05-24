using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using TMPro;
using NaughtyAttributes;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Deck : MonoBehaviour
{
    [SerializeField] int CarteAJouer;
    [SerializeField] float RangePourActiverCarte;
    [SerializeField] Button Pioche;
    [SerializeField] Button Use;
    [SerializeField] float DecalageX;
    [SerializeField] float DecalageY;
    private List<CardObject> GraveYard = new List<CardObject>();
    private List<CardObject> Hand = new List<CardObject>();
    public List<CardObject> deck;
    public Transform[] cardSlots;
    public bool[] availableCardSlots;
    public TMP_Text DeckCount;
    public TMP_Text graveyardCount;

    private GameManager gameManager;

    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent red cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(new Vector3(0,-150+RangePourActiverCarte / 2, 0), new Vector3(300, 300+RangePourActiverCarte/2, 5));
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        for (int i = 0; i < 10; i++)
        {
            if (i % 2 == 0)
            {
                Gizmos.DrawCube(new Vector3(cardSlots[0].position.x - DecalageX*i, cardSlots[0].position.y - DecalageY*i, i), new Vector3(1,1, 1));
            }
            else
            {
                Gizmos.DrawCube(new Vector3((cardSlots[0].position.x - DecalageX * i)*-1, cardSlots[0].position.y - DecalageY * i, i-1), new Vector3(1, 1, 1));
            }
        }

    }
    public void Awake()
    {
        gameManager = GameManager.Instance;
        gameManager.RangePourActiverCarte = RangePourActiverCarte;
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
        Pioche.onClick.AddListener(DrawCard);
        Use.onClick.AddListener(PlayCard);
        for (int i = 0; i < 10; i++)
        {
            if (i % 2 == 0)
            {
                cardSlots[i].position = new Vector3(cardSlots[0].position.x - DecalageX * i, cardSlots[0].position.y - DecalageY * i, i);
            }
            else
            {
                cardSlots[i].position = new Vector3((cardSlots[0].position.x - DecalageX * i) * -1, cardSlots[0].position.y - DecalageY * i, -i);
            }
        }
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
        gameManager.Hand = Hand;
    }
    public void PlayCard()
    {
        if (Hand.Count > 0)
        {
            Hand[0].gameObject.SetActive(false);
            GraveYard.Add(Hand[0]);
            Hand.RemoveAt(0);
            for (int i = 0; i < Hand.Count; i++)
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
        gameManager.Hand = Hand;
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
                    gameManager.Hand = Hand;
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
        gameManager.Hand = Hand;
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
