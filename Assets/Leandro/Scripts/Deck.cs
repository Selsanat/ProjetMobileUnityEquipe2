using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using TMPro;
using NaughtyAttributes;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Linq;
using System.Diagnostics.Tracing;

public class Deck : MonoBehaviour
{
    [SerializeField] int CarteAJouer;
    [SerializeField] float RangePourActiverCarte;
    [SerializeField] Button PiocheButton;
    [SerializeField] Button UseButton;
    [SerializeField] Button EndTurnButton;
    [SerializeField] public Button CancelButton;
    [SerializeField] int  NbCarteHandPossible;
    [SerializeField] float DecalageX;
    [SerializeField] float DecalageY;
    [SerializeField] float Rotation;
    [SerializeField] int NombrePiocheDebutTour;
    private List<CardObject> GraveYard = new List<CardObject>();
    private List<CardObject> Hand = new List<CardObject>();
    public List<CardObject> deck;

    private List<CardObject> playedCards;


    public List<Transform> cardSlots;
    public bool[] availableCardSlots;
    public TMP_Text DeckCount;
    public TMP_Text graveyardCount;

    private GameManager gameManager;

    public List<CardObject> PlayedCards { get => playedCards; private set => playedCards = value; }

    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent red cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(new Vector3(0,-50+RangePourActiverCarte / 2, 0), new Vector3(100, 100+RangePourActiverCarte/2, 5));
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawCube(cardSlots[0].transform.position, new Vector3(1, 1, 1)) ;
        for (int i = 1; i < NbCarteHandPossible + 1; i++)
        {

            if (i % 2 == 0)
            {
                Gizmos.DrawCube(new Vector3((-cardSlots[0].position.x - DecalageX * i) * -1, cardSlots[0].position.y - DecalageY * i, -i), new Vector3(1, 1, 1));
            }
            else
            {
                Gizmos.DrawCube(new Vector3(cardSlots[0].position.x - DecalageX * (i - 1)- DecalageX*2, cardSlots[0].position.y - DecalageY * (i - 1), i), new Vector3(1, 1, 1));
            }
        }

    }
    
    public void Start()
    {

        gameManager = GameManager.Instance;
        gameManager.RangePourActiverCarte = RangePourActiverCarte;
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
        PiocheButton.onClick.AddListener(DrawCard);
        UseButton.onClick.AddListener(PlayCard);
        EndTurnButton.onClick.AddListener(EndTurn);
        CancelButton.onClick.AddListener(CancelChosenCard);
        for (int i = 1; i < NbCarteHandPossible + 1; i++)
        {
            if (i % 2 == 0)
            {
                cardSlots[i].position = new Vector3((-cardSlots[0].position.x - DecalageX * i) * -1, cardSlots[0].position.y - DecalageY * i, -i);
                cardSlots[i].rotation = Quaternion.AngleAxis(Rotation * i, Vector3.back);
            }
            else
            {
                cardSlots[i].position = new Vector3(cardSlots[0].position.x - DecalageX * (i - 1) - DecalageX * 2, cardSlots[0].position.y - DecalageY * (i - 1), i);
                cardSlots[i].rotation = Quaternion.AngleAxis(Rotation * -i, Vector3.back);
            }
        }
    }

    public void ReorderZCards()
    {
        for(int i = 0; i < Hand.Count; i++)
        if (i == 0)
        {
            Hand[i].GetComponent<Renderer>().sortingOrder = 0;
        }
        else if (i % 2 == 0)
        {
            Hand[i].GetComponent<Renderer>().sortingOrder = i;
        }
        else
        {
            Hand[i].GetComponent<Renderer>().sortingOrder = -i;
        }
    }
    public void PlayCard(int Index)
    {
        if(Hand.Count > 0)
        {
            Hand[Index].gameObject.SetActive(false);
            GraveYard.Add(Hand[Index]);
            Hand.RemoveAt(Index);
            for (int i= 0; i < Hand.Count; i++)
            {
                Hand[i].transform.position = cardSlots[i].transform.position;
                Hand[i].transform.rotation = cardSlots[i].transform.rotation;
                Hand[i].Slot = cardSlots[i].transform;
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
        PlayCard(0);
    }
/*    public void DecaleCartes(int Decalage)
    {
        for(int i = 0; i < Hand.Count;i++)
        {
            Hand[i].transform.position = cardSlots[i+Decalage].transform.position;
            Hand[i].transform.rotation = cardSlots[i+Decalage].transform.rotation;
            availableCardSlots[i] = false;
            (Hand[i],Hand[i+Decalage]) = (Hand[i + Decalage],Hand[i]);
        }
    }*/
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

                    randCard.Slot = cardSlots[i];
                    Hand.Add(randCard);
                    availableCardSlots[i] = false;
                    deck.Remove(randCard);
                    if (deck.Count == 0)
                    {
                        ShuffleGraveyardToHand();
                    }
                    gameManager.Hand = Hand;
                    ReorderZCards();
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
        ReorderZCards();
        gameManager.Hand = Hand;
    }
    public void DrawCard(int number)
    {
        for (int i = 0;i < number; i++)
        {
            DrawCard();
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
    private void CacheHand()
    {
        foreach (CardObject carte in Hand)
        {
            carte.gameObject.SetActive(false);
        }
    }
    private void ShowHand()
    {
        foreach (CardObject carte in Hand)
        {
            carte.transform.localScale = new Vector3(1, 1, 1);

            carte.gameObject.SetActive(true);
            
        }
    }
    private void ReturnCardsToHand()
    {
        foreach(CardObject carte in Hand)
        {
            carte.transform.position = carte.Slot.position;
            carte.transform.rotation = carte.Slot.rotation;

        }
    }
    private void HandToGraveyard()
    {
        foreach (CardObject carte in Hand)
        {
            GraveYard.Add(carte);
        }
        Hand.Clear();
    }
    private void LibereEspacesHand() {
        for(int i = 0; i < availableCardSlots.Count();i++){
            availableCardSlots[i] = true;
        }

    }
    public void EndTurn()
    {
        CacheHand();
        LibereEspacesHand();
        HandToGraveyard();
    }

    public void StartTurn()
    {
        DrawCard(NombrePiocheDebutTour);
    }

    public void CancelChosenCard()
    {
        ShowHand();
        ReturnCardsToHand();
        gameManager.CardsInteractable = true;
        gameManager.CarteUtilisee = null;
        CancelButton.gameObject.SetActive(false);
        ReorderZCards();
    }

    [Button]
    private void BDrawCard() {
        DrawCard();
    }
    [Button]
    private void BPlayCard()
    {
        PlayCard(CarteAJouer);
    }
}
