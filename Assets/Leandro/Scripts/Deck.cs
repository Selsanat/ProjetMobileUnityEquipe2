using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using TMPro;
using NaughtyAttributes;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Slider = UnityEngine.UI.Slider;
using Image = UnityEngine.UI.Image;

using Color = UnityEngine.Color;
using Unity.VisualScripting;
using System.Linq;
using System.Diagnostics.Tracing;
using UnityEngine.UIElements;
using DG.Tweening.Core.Easing;
using System.Drawing;
using static entityManager;

public class Deck : MonoBehaviour
{
    [SerializeField] int CarteAJouer;
    [SerializeField] float RangePourActiverCarte;
    [SerializeField] Button PiocheButton;
    [SerializeField] Button UseButton;
    [SerializeField] public  Button EndTurnButton;
    [SerializeField] public Button CancelButton;
    [SerializeField] int  NbCarteHandPossible;
    [SerializeField] float DecalageX;
    [SerializeField] float DecalageY;
    [SerializeField] float Rotation;
    [SerializeField] int NombrePiocheDebutTour = 4;
    [SerializeField] Transform MilieuPlaceCard;
    [SerializeField] float VitesseTranspo = 10f;
    [SerializeField] float TempsTrans = 0.5f;

    [SerializeField] public GameObject UiXpSolo;
    [SerializeField] public GameObject UiXpDuo;
    [SerializeField] public SpriteRenderer Background;
    [SerializeField] public SpriteRenderer BackgroundAlt;
    [SerializeField] public List<Slider> SlidersXp;
    [SerializeField] public List<Image>  SpriteRenderers;
    public Transform AoeEmplacement;





    private List<CardObject> GraveYard = new List<CardObject>();
    [SerializeField] public List<CardObject> Hand = new List<CardObject>();
    public List<CardObject> deckDruid;
    public List<CardObject> deckPriest;
    public List<CardObject> deckBaseDruid;
    public List<CardObject> deckBasePriest;
    public List<CardObject> deck;

    private List<CardObject> playedCards;
    Coroutine RestoreCardPos;
    Coroutine RestoreCardRot;


    public List<Transform> cardSlots;
    public bool[] availableCardSlots;
    public TMP_Text DeckCount;
    public TMP_Text graveyardCount;

    [SerializeField] private GameManager gameManager;

    public List<CardObject> PlayedCards { get => playedCards; private set => playedCards = value; }

    public void AfficheSideUiXP(bool TrueIfDuo)
    {
        SpriteRenderers[3].gameObject.SetActive(true);
        if (TrueIfDuo)
        {
            UiXpDuo.SetActive(true);
        }
        else
        {
            UiXpSolo.SetActive(true);
            if (gameManager.FM.perso2)
            {
                SpriteRenderers[2].sprite = SpriteRenderers[1].sprite;
            }
            else
            {
                SpriteRenderers[2].sprite = SpriteRenderers[0].sprite;
            }
        }
    }

    public void SetBonneBarreXp(List<hero> herolist)
    {
        if (herolist.Count == 1)
        {
            if (herolist[0].m_role == Role.Arboriste)
            {
                SlidersXp[2].maxValue = herolist[0].getexperienceMAX();
                SlidersXp[2].value = gameManager.expArboriste;

                

            }
            else
            {
                SlidersXp[2].maxValue = herolist[0].getexperienceMAX();
                SlidersXp[2].value = gameManager.expPretre;
            }
            SlidersXp[2].transform.GetChild(3).GetComponent<TMP_Text>().text = SlidersXp[2].value + "/" + SlidersXp[2].maxValue;
            gameManager.deck.SlidersXp[2].transform.GetChild(4).GetComponent<TMP_Text>().text = "LVL " + gameManager.levelArboriste;

        }
        else
        {
            SlidersXp[0].maxValue = herolist[0].getexperienceMAX();
            SlidersXp[0].value = gameManager.expArboriste;
            SlidersXp[0].transform.GetChild(3).GetComponent<TMP_Text>().text = SlidersXp[0].value + "/" + SlidersXp[0].maxValue;
            SlidersXp[1].maxValue = herolist[1].getexperienceMAX();
            SlidersXp[1].value = gameManager.expPretre;
            SlidersXp[1].transform.GetChild(3).GetComponent<TMP_Text>().text = SlidersXp[1].value + "/" + SlidersXp[1].maxValue;
            gameManager.deck.SlidersXp[0].transform.GetChild(4).GetComponent<TMP_Text>().text = "LVL " + gameManager.levelArboriste;
            gameManager.deck.SlidersXp[1].transform.GetChild(4).GetComponent<TMP_Text>().text = "LVL " + gameManager.levelPretre;
        }
    }
    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent red cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(new Vector3(0,-50+RangePourActiverCarte / 2, 0), new Vector3(100, 100+RangePourActiverCarte/2, 5));
        Gizmos.color = new Color(0, 0, 1, 0.5f);

        if (Hand.Count % 2 == 0)
        {
            Gizmos.DrawCube(new Vector3((MilieuPlaceCard.position.x +DecalageX), MilieuPlaceCard.position.y, 0), new Vector3(1, 1, 1));
        }
        else
        {
            Gizmos.DrawCube(MilieuPlaceCard.transform.position, new Vector3(1, 1, 1));
        }
        for (int i = 1; i < NbCarteHandPossible; i++)
        {
            if (i == 1 && Hand.Count % 2 != 0)
            {
                Gizmos.DrawCube(new Vector3((MilieuPlaceCard.position.x - DecalageX-DecalageX), MilieuPlaceCard.position.y -DecalageY, -i), new Vector3(1, 1, 1));
            }
            else
            {
                if (i % 2 == 0)
                {
                    if (Hand.Count % 2 == 0)
                    {
                        Gizmos.DrawCube(new Vector3((-MilieuPlaceCard.position.x - DecalageX * i) * -1 + DecalageX, MilieuPlaceCard.position.y - DecalageY * i, -i), new Vector3(1, 1, 1));
                    }
                    else
                    {
                        Gizmos.DrawCube(new Vector3((-MilieuPlaceCard.position.x - DecalageX * i) * -1, MilieuPlaceCard.position.y - DecalageY * i, -i), new Vector3(1, 1, 1));
                    }
                     
                }
                else
                {
                    if (Hand.Count % 2 != 0)
                    {
                        Gizmos.DrawCube(new Vector3(MilieuPlaceCard.position.x - DecalageX * (i - 1) - DecalageX * 2, MilieuPlaceCard.position.y - DecalageY - DecalageY * (i - 1), i), new Vector3(1, 1, 1));
                    }
                    else
                    {
                        Gizmos.DrawCube(new Vector3(MilieuPlaceCard.position.x - DecalageX * (i - 1) - DecalageX * 2+DecalageX, MilieuPlaceCard.position.y - DecalageY * (i - 1), i), new Vector3(1, 1, 1));
                    }
                }
            }

        }

    }

    public void rearangecardslots()
    {
        cardSlots[0].position = MilieuPlaceCard.position;
        if (Hand.Count % 2 == 0)
        {
            cardSlots[0].position = new Vector3((MilieuPlaceCard.position.x + DecalageX), MilieuPlaceCard.position.y, 0);
        }
        else
        {
            cardSlots[0].position = MilieuPlaceCard.transform.position;
        }
            cardSlots[0].rotation = Quaternion.AngleAxis(0, Vector3.back);

        for (int i = 1; i < NbCarteHandPossible; i++)
        {
            if (i == 1 && Hand.Count % 2 != 0)
            {
                cardSlots[i].position = new Vector3((MilieuPlaceCard.position.x - DecalageX - DecalageX), MilieuPlaceCard.position.y - DecalageY, -i);
                cardSlots[i].rotation = Quaternion.AngleAxis(0, Vector3.back);
            }
            else
            {
                if (i % 2 == 0)
                {
                    if (Hand.Count % 2 == 0)
                    {
                        cardSlots[i].position = new Vector3((-MilieuPlaceCard.position.x - DecalageX * i) * -1 + DecalageX, MilieuPlaceCard.position.y - DecalageY * i, -i);
                    }
                    else
                    {
                        cardSlots[i].position = new Vector3((-MilieuPlaceCard.position.x - DecalageX * i) * -1, MilieuPlaceCard.position.y - DecalageY * i, -i);
                    }
                        
                    cardSlots[i].rotation = Quaternion.AngleAxis(Rotation * i, Vector3.back);
                }
                else
                {
                    if (Hand.Count % 2 != 0)
                    {
                        cardSlots[i].position = new Vector3(MilieuPlaceCard.position.x - DecalageX * (i - 1) - DecalageX * 2, MilieuPlaceCard.position.y - DecalageY - DecalageY * (i - 1), i);


                    }
                    else {
                        cardSlots[i].position = new Vector3(MilieuPlaceCard.position.x - DecalageX * (i - 1) - DecalageX * 2 + DecalageX, MilieuPlaceCard.position.y - DecalageY * (i - 1), i);
                    }
                    cardSlots[i].rotation = Quaternion.AngleAxis(Rotation * -i, Vector3.back);
                }
            }


        }
        List<Transform> newTempList = new List<Transform>();
        for (int i = NbCarteHandPossible; i > 0; i--)
        {
            if (i % 2 != 0)
                newTempList.Add(cardSlots[i]);

        }
        for (int i = 0; i < NbCarteHandPossible; i++)
        {
            if (i % 2 == 0)
                newTempList.Add(cardSlots[i]);
        }

        cardSlots = newTempList;
        ReorderZCards();

    }

    public void RestoreCardPosition(bool hard)
    {
        if (RestoreCardPos != null)
        {
            StopCoroutine(RestoreCardPos);
        }
        if (RestoreCardRot != null)
        {
            StopCoroutine(RestoreCardRot);
        }
        for (int i = 0; i < Hand.Count; i++)
        {
            if (Hand[i] != gameManager?.CarteUtilisee)
            {
                if (hard)
                {
                    Hand[i].gameObject.transform.position = cardSlots[(int)Mathf.Ceil(cardSlots.Count / 2) - Hand.Count / 2 + i].position;
                    Hand[i].gameObject.transform.rotation = cardSlots[(int)Mathf.Ceil(cardSlots.Count / 2) - Hand.Count / 2 + i].rotation;
                }
                else
                {
                    if (Hand[i].gameObject.transform.position != cardSlots[(int)Mathf.Ceil(cardSlots.Count / 2) - Hand.Count / 2 + i].position)
                    {
                        RestoreCardPos = StartCoroutine(TransposeAtoB(Hand[i].gameObject, cardSlots[(int)Mathf.Ceil(cardSlots.Count / 2) - Hand.Count / 2 + i].position,2f));
                    }
                    if (Hand[i].gameObject.transform.rotation != cardSlots[(int)Mathf.Ceil(cardSlots.Count / 2) - Hand.Count / 2 + i].rotation)
                    {
                        RestoreCardRot = StartCoroutine(TransposeAtoBRotation(Hand[i].gameObject, cardSlots[(int)Mathf.Ceil(cardSlots.Count / 2) - Hand.Count / 2 + i].rotation,2f));
                    }
                }
            }
        }
    }
    public void ConstruireDeck()
    {
        if (gameManager.FM.perso1)
        {
            for (int i = 0; i < gameManager.levelArboriste + 2; i++)
            {
                if (i < 8)
                {
                    deck.Add(deckDruid[i]);
                }
            }
            for (int i = 0; i < deckBaseDruid.Count() - (gameManager.levelArboriste + 2); i++)
            {

                    deck.Add(deckBaseDruid[i]);
                
            }
        }
        if (gameManager.FM.perso2)
        {
            for(int i = 0; i < gameManager.levelPretre + 2; i++)
            {
                if (i < 8)
                {
                    deck.Add(deckPriest[i]);
                }
            }
            for (int i = 0; i < deckBasePriest.Count() - (gameManager.levelPretre + 2); i++)
            {
                deck.Add(deckBasePriest[i]);
            }
        }
    }
    public void Start()
    {
        foreach (CardObject card in deck)
        {
            card.DataCard.m_isUpsideDown = false;
        }
        gameManager = GameManager.Instance;
        gameManager.RangePourActiverCarte = RangePourActiverCarte;
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
        PiocheButton.onClick.AddListener(delegate { StartCoroutine(DrawCardCoroutine()); });
        CancelButton.onClick.AddListener(() => { CancelChosenCard();}) ;
        gameManager.deck = this;
        gameManager.FM.cancel = this.CancelButton;
        deck.Clear();
        ConstruireDeck();
        rearangecardslots();
    }
    public void ReorderZCards()
    {
        for(int i = 0; i < Hand.Count; i++)
        {
                Hand[i].GetComponent<Renderer>().sortingOrder =i;
            Hand[i].GetComponent<CardObject>().canvas.sortingOrder = i;
        }
        
    }
    public void PlayCard(int Index)
    {
        if(Hand.Count > 0)
        {
            Hand[Index].transform.localScale = new Vector3(1, 1, 1);
            Hand[Index].gameObject.SetActive(false);
            for (int i = Hand[Index].indexHand; i < Hand.Count; i++)
            {
                Hand[i].indexHand -= 1;
            }
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
        gameManager.CarteUtilisee = null;
        rearangecardslots();
        /*RestoreCardPosition(true);*/
        ReorderZCards();
        gameManager.Hand = Hand;
        CancelChosenCard(true);
        gameManager.isHoverButton = false;
    }
    public Transform GetTransformSlotFromCard(CardObject card)
    {
        return (cardSlots[(int)Mathf.Ceil(cardSlots.Count / 2) - Hand.Count / 2 + card.indexHand]);
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
    public CardObject DrawCard()
    {
        if (deck.Count >= 1 && availableCardSlots.Contains(true))
        {
            CardObject randCard = deck[UnityEngine.Random.Range(0, deck.Count)];
            randCard.DataCard.GM = gameManager;

            for (int i = 0; i < availableCardSlots.Length - gameManager.debuffDraw; i++)
            {
                if (availableCardSlots[i] == true)
                {

                    randCard.gameObject.SetActive(true);
                    randCard.canvas.gameObject.SetActive(true);
                    randCard.transform.position = cardSlots[i].position;
                    randCard.transform.rotation = cardSlots[i].rotation;

                    randCard.Slot = cardSlots[i];
                    randCard.indexHand = 0;
                    List<CardObject> NewHand = new List<CardObject>();
                    NewHand.Add(randCard);
                    foreach(CardObject card in Hand)
                    {
                        card.indexHand++;
                        NewHand.Add(card);
                    }
                    Hand = NewHand.ToList();
                    NewHand.Clear();
                    availableCardSlots[Hand.Count-1] = false;
                    deck.Remove(randCard);
                    if (deck.Count == 0)
                    {
                        ShuffleGraveyardToHand();
                    }
                    gameManager.Hand = Hand;

                    rearangecardslots();
                    RestoreCardPosition(false);
                    ReorderZCards();
                    return randCard;
                }
            }

        }
        else
        {
            if (GraveYard.Count > 0) {
                ShuffleGraveyardToHand();
                DrawCard();
            }
            else
            {
                print("Graveyard vide idiot ou plus de place dans la main idiot");
            }

        }
        gameManager.Hand = Hand;
        gameManager.debuffDraw = 0;
        return (Hand[0]);
    }
    public CardObject DrawCard(int number)
    {
        for (int i = 0; i < number; i++)
        {
            DrawCard();
        }
        return Hand[0];
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
            if (gameManager?.CarteUtilisee!= carte)
            {
                carte.transform.localScale = new Vector3(1, 1, 1);

                carte.gameObject.SetActive(true);
            }
            StartCoroutine(TransposeTransparency(carte.gameObject));
        }
        rearangecardslots();
        RestoreCardPosition(false);
        ReorderZCards();
    }
    private void HandToGraveyard()
    {
        foreach (CardObject carte in Hand)
        {
            if (!carte.stayInHand)
            {
            GraveYard.Add(carte);
            }
            carte.stayInHand = false;
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
        StartCoroutine(DiscardCoroutine());
    }

    public void StartTurn()
    {
        StartCoroutine(DrawCardCoroutine(NombrePiocheDebutTour));
    }
    public void CancelChosenCard(bool TrueIfPlay)
    {
        if (gameManager.FM.CheckifEnemyAreAlive())
        {
           ShowHand();
        }
        RestoreCardPosition(true);
        if (!TrueIfPlay)
        {

            StartCoroutine(TransposeAtoBScale(gameManager.CarteUtilisee.gameObject, new Vector3(1, 1, 1)));
            StartCoroutine(TransposeAtoB(gameManager.CarteUtilisee.gameObject, cardSlots[(int)Mathf.Ceil(cardSlots.Count / 2) - Hand.Count / 2 + gameManager.CarteUtilisee.indexHand].position));
            StartCoroutine(TransposeAtoBRotation(gameManager.CarteUtilisee.gameObject, cardSlots[(int)Mathf.Ceil(cardSlots.Count / 2) - Hand.Count / 2 + gameManager.CarteUtilisee.indexHand].rotation));
        }
        gameManager.CardsInteractable = true;
        gameManager.CarteUtilisee = null;
        CancelButton.gameObject.SetActive(false);
        rearangecardslots();

        ReorderZCards();
        gameManager.FM.CancelCard();
        gameManager.isHoverButton = false;
    }
    public void CancelChosenCard()
    {
        CancelChosenCard(false);
    }
    [Button]
    private void BDrawCard() {
        DrawCard();
    }
    [Button]
    private void BPlayCard()
    {
        //PlayCard(CarteAJouer);
    }
    [Button]
    private void BTransfo()
    {
        //StartCoroutine(TransfoCoroutine());
    }
    [Button]
    private void PDetransfo()
    {
        //StartCoroutine(DetransfoCoroutine());
    }

    public void DeplaceCardUtiliseToPlace()
    {
        Transform AllyCardTransform = GameObject.FindGameObjectsWithTag("AllyCardTransform")[0].transform;
        Transform EnnemyCardTransform = GameObject.FindGameObjectsWithTag("EnnemyCardTransform")[0].transform;

        if(gameManager.CarteUtilisee.DataCard.m_isUpsideDown)
        {
            if (gameManager.CarteUtilisee.DataCard.BackCard.TargetAllies && gameManager.CarteUtilisee.DataCard.BackCard.TargetEnnemies)
            {
                StartCoroutine(TransposeAtoB(gameManager.CarteUtilisee.gameObject, Vector3.Lerp(AllyCardTransform.position, EnnemyCardTransform.position, 0.5f)));
                StartCoroutine(TransposeAtoBRotation(gameManager.CarteUtilisee.gameObject, Quaternion.Lerp(AllyCardTransform.rotation, EnnemyCardTransform.rotation, 0.5f)));
            }
            if (gameManager.CarteUtilisee.DataCard.BackCard.TargetAllies && !gameManager.CarteUtilisee.DataCard.BackCard.TargetEnnemies)
            {
                StartCoroutine(TransposeAtoB(gameManager.CarteUtilisee.gameObject, AllyCardTransform.position));
                StartCoroutine(TransposeAtoBRotation(gameManager.CarteUtilisee.gameObject, AllyCardTransform.rotation));
            }
            if (gameManager.CarteUtilisee.DataCard.BackCard.TargetEnnemies && !gameManager.CarteUtilisee.DataCard.BackCard.TargetAllies)
            {

                StartCoroutine(TransposeAtoB(gameManager.CarteUtilisee.gameObject, EnnemyCardTransform.position));
                StartCoroutine(TransposeAtoBRotation(gameManager.CarteUtilisee.gameObject, EnnemyCardTransform.rotation));
            }

        }
        else
        {
            if (gameManager.CarteUtilisee.DataCard.TargetAllies && gameManager.CarteUtilisee.DataCard.TargetEnnemies)
            {
                StartCoroutine(TransposeAtoB(gameManager.CarteUtilisee.gameObject, Vector3.Lerp(AllyCardTransform.position, EnnemyCardTransform.position, 0.5f)));
                StartCoroutine(TransposeAtoBRotation(gameManager.CarteUtilisee.gameObject, Quaternion.Lerp(AllyCardTransform.rotation, EnnemyCardTransform.rotation, 0.5f)));
            }
            if (gameManager.CarteUtilisee.DataCard.TargetAllies && !gameManager.CarteUtilisee.DataCard.TargetEnnemies)
            {
                StartCoroutine(TransposeAtoB(gameManager.CarteUtilisee.gameObject, AllyCardTransform.position));
                StartCoroutine(TransposeAtoBRotation(gameManager.CarteUtilisee.gameObject, AllyCardTransform.rotation));
            }
            if (gameManager.CarteUtilisee.DataCard.TargetEnnemies && !gameManager.CarteUtilisee.DataCard.TargetAllies)
            {
                StartCoroutine(TransposeAtoB(gameManager.CarteUtilisee.gameObject, EnnemyCardTransform.position));
                StartCoroutine(TransposeAtoBRotation(gameManager.CarteUtilisee.gameObject, EnnemyCardTransform.rotation));
            }
        }
        

    }

    public IEnumerator TransposeAtoB(GameObject objetABouger, Vector3 position, float vitesse = 1f)
    {
        float TempsTransition = TempsTrans;
        float timeElapsed = 0;
        while (timeElapsed< TempsTransition)
        {
            objetABouger.transform.position = Vector3.Lerp(objetABouger.transform.position, position, Time.deltaTime * VitesseTranspo*vitesse);
            timeElapsed += Time.deltaTime*vitesse;
            yield return null;
        }
        objetABouger.transform.position = position;

    }
    public IEnumerator TransposeAtoBRotation(GameObject objetABouger, Quaternion position, float vitesse = 1f)
    {
        float TempsTransition = TempsTrans;
        float timeElapsed = 0;
        while (timeElapsed < TempsTransition)
        {
            objetABouger.transform.rotation = Quaternion.Lerp(objetABouger.transform.rotation, position, Time.deltaTime * VitesseTranspo*vitesse);
            timeElapsed += Time.deltaTime*vitesse;
            yield return null;
        }
        objetABouger.transform.rotation = position;
    }

    public IEnumerator TransposeAtoBScale(GameObject objetABouger, Vector3 position) {
        float TempsTransition = TempsTrans;
        float timeElapsed = 0;
        while (timeElapsed < TempsTransition)
        {
            objetABouger.transform.localScale = Vector3.Lerp(objetABouger.transform.localScale, position, Time.deltaTime * VitesseTranspo);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        objetABouger.transform.localScale = position;
    }

    public IEnumerator  TransposeTransparency(GameObject objetABouger)
    {
        float TempsTransition = TempsTrans;
        float timeElapsed = 0;
        SpriteRenderer mesh = objetABouger.GetComponent<SpriteRenderer>();
        mesh.color = new Color(mesh.color.r, mesh.color.g, mesh.color.b,0);
        while (timeElapsed < TempsTransition)
        {
            mesh.color = Color.Lerp(mesh.color, new Color(mesh.color.r, mesh.color.g, mesh.color.b, 1), Time.deltaTime * VitesseTranspo);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        objetABouger.GetComponent<SpriteRenderer>().color = new Color(mesh.color.r, mesh.color.g, mesh.color.b, 1);
    }
    public IEnumerator TransposeTransparency(SpriteRenderer objetABouger)
    {
        float TempsTransition = TempsTrans;
        float timeElapsed = 0;
        SpriteRenderer mesh = objetABouger.GetComponent<SpriteRenderer>();
        mesh.color = new Color(mesh.color.r, mesh.color.g, mesh.color.b, 0);
        while (timeElapsed < TempsTransition)
        {
            mesh.color = Color.Lerp(mesh.color, new Color(mesh.color.r, mesh.color.g, mesh.color.b, 1), Time.deltaTime * VitesseTranspo);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        objetABouger.GetComponent<SpriteRenderer>().color = new Color(mesh.color.r, mesh.color.g, mesh.color.b, 1);
    }
    public IEnumerator TransposeTransparencyNegative(GameObject objetABouger)
    {
        float TempsTransition = TempsTrans;
        float timeElapsed = 0;
        SpriteRenderer mesh = objetABouger.GetComponent<SpriteRenderer>();
        mesh.color = new Color(mesh.color.r, mesh.color.g, mesh.color.b, 1);
        while (timeElapsed < TempsTransition)
        {
            mesh.color = Color.Lerp(mesh.color, new Color(mesh.color.r, mesh.color.g, mesh.color.b, 0), Time.deltaTime * VitesseTranspo);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        objetABouger.GetComponent<SpriteRenderer>().color = new Color(mesh.color.r, mesh.color.g, mesh.color.b, 0);
    }
    public IEnumerator TransposeTransparencyNegative(SpriteRenderer objetABouger)
    {
        float TempsTransition = TempsTrans;
        float timeElapsed = 0;
        SpriteRenderer mesh = objetABouger.GetComponent<SpriteRenderer>();
        mesh.color = new Color(mesh.color.r, mesh.color.g, mesh.color.b, 1);
        while (timeElapsed < TempsTransition)
        {
            mesh.color = Color.Lerp(mesh.color, new Color(mesh.color.r, mesh.color.g, mesh.color.b, 0), Time.deltaTime * VitesseTranspo);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        objetABouger.GetComponent<SpriteRenderer>().color = new Color(mesh.color.r, mesh.color.g, mesh.color.b, 0);
    }
    public void DrawCardTest(int iterations)
    {
        StartCoroutine(DrawCardCoroutine(iterations));
    }
    public IEnumerator DrawCardCoroutine()
    {
        float TempsTransition = TempsTrans;
        float timeElapsed = 0;
        CardObject card = DrawCard();
        GameObject CardGO = card.gameObject;
        CardGO.transform.localScale = new Vector3(0,0,0);
        CardGO.transform.position = Camera.main.ScreenToWorldPoint(DeckCount.transform.position);
        while (timeElapsed < TempsTransition)
        {
            CardGO.transform.rotation = Quaternion.Lerp(CardGO.transform.rotation, cardSlots[(int)Mathf.Ceil(cardSlots.Count / 2) - Hand.Count / 2].rotation, Time.deltaTime * VitesseTranspo);
            CardGO.transform.position = Vector3.Lerp(CardGO.transform.position, cardSlots[(int)Mathf.Ceil(cardSlots.Count / 2) - Hand.Count / 2].position, Time.deltaTime * VitesseTranspo);
            CardGO.transform.localScale = Vector3.Lerp(CardGO.transform.localScale, new Vector3(1,1,1), Time.deltaTime * VitesseTranspo);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        CardGO.transform.position = cardSlots[(int)Mathf.Ceil(cardSlots.Count / 2) - Hand.Count / 2 + card.indexHand].position;
        CardGO.transform.localScale = new Vector3(1, 1, 1);
    }
    public IEnumerator DrawCardCoroutine(int nombreAPiocher)
    {
        EndTurnButton.interactable = false;
        gameManager.CardsInteractable = false;
        for (int i = 0; i < nombreAPiocher; i++)
        {
            StartCoroutine( DrawCardCoroutine());
            yield return new WaitForSeconds(0.25f);
        }
        if (!gameManager.AnimAtk)
        {
            EndTurnButton.interactable = true;
            gameManager.CardsInteractable = true;
        }

    }
    IEnumerator DiscardCoroutine()
    {
        
        foreach (CardObject card in Hand.ToList())
        {
            float TempsTransition = TempsTrans;
            float timeElapsed = 0;
            GameObject CardGO = card.gameObject;
            while (timeElapsed < TempsTransition)
            {
                CardGO.transform.position = Vector3.Lerp(CardGO.transform.position, Camera.main.ScreenToWorldPoint(graveyardCount.transform.position), Time.deltaTime * VitesseTranspo);
                CardGO.transform.localScale = Vector3.Lerp(CardGO.transform.localScale, new Vector3(0, 0, 0), Time.deltaTime * VitesseTranspo);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
            card.gameObject.SetActive(false);
            CardGO.transform.position = Camera.main.ScreenToWorldPoint(graveyardCount.transform.position);
            CardGO.transform.localScale = new Vector3(1, 1, 1);
        }
        yield return new WaitForSeconds(0.5f);
        LibereEspacesHand();
        HandToGraveyard();
        yield return DrawCardCoroutine(NombrePiocheDebutTour);
        EndTurnButton.interactable = true;
    }
    public IEnumerator DiscardCoroutine(bool justeCache)
    {
        EndTurnButton.interactable = false;
        gameManager.CardsInteractable = false;
        List<CardObject> ReversedCardlist = Hand.ToList();
        ReversedCardlist.Reverse(); 
        foreach (CardObject card in ReversedCardlist)
        {
            StartCoroutine(DiscardOneCoroutine(card));
            yield return new WaitForSeconds(0.25f);
        }
        LibereEspacesHand();
        HandToGraveyard();
        gameManager.CardsInteractable = true;
        EndTurnButton.interactable = true;

        //yield return DrawCardCoroutine(NombrePiocheDebutTour);
    }

    IEnumerator DiscardOneCoroutine(CardObject card)
    {
        float TempsTransition = TempsTrans;
        float timeElapsed = 0;
        GameObject CardGO = card.gameObject;
        while (timeElapsed < TempsTransition)
        {
            CardGO.transform.position = Vector3.Lerp(CardGO.transform.position, Camera.main.ScreenToWorldPoint(graveyardCount.transform.position), Time.deltaTime * VitesseTranspo);
            CardGO.transform.localScale = Vector3.Lerp(CardGO.transform.localScale, new Vector3(0, 0, 0), Time.deltaTime * VitesseTranspo);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        card.gameObject.SetActive(false);
        CardGO.transform.position = Camera.main.ScreenToWorldPoint(graveyardCount.transform.position);
        CardGO.transform.localScale = new Vector3(1, 1, 1);
    }

    IEnumerator TourneCarte90(CardObject card)
    {
        yield return (TransposeAtoBRotation(card.gameObject, Quaternion.Euler(0, 90, 0)));
        yield return (TransposeAtoBRotation(card.gameObject, Quaternion.Euler(0, 0, 0)));
    }
    public IEnumerator TransfoCoroutine(bool trueIfDruid)
    {
        EndTurnButton.interactable = false;
        gameManager.CardsInteractable = false;
        StartCoroutine( TransposeTransparencyNegative(Background.gameObject));
        int index = 0;
        if (!trueIfDruid)
        {
            index = 1;
        }

        foreach (SpriteRenderer sprite in gameManager.FM.HeroesAltGameObjectRef[index].GetComponentsInChildren<SpriteRenderer>())
        {
            StartCoroutine(TransposeTransparency(sprite));
        }
        foreach (SpriteRenderer sprite in gameManager.FM.HeroesGameObjectRef[index].GetComponentsInChildren<SpriteRenderer>())
        {
            StartCoroutine(TransposeTransparencyNegative(sprite));
        }

        for (int i = 0; i < Hand.Count; i++)
        {
            CardObject card = Hand[i];
            if (trueIfDruid == card.DataCard.isDruidCard && !card.DataCard.isBaseCard)
            {
                StartCoroutine(TourneCarte90(card));
                yield return new WaitForSeconds(0.25f);
                card.Name.text = card.DataCard.BackCard.Name;
                card.Description.text = card.DataCard.BackCard.Description;
                card.GetComponent<SpriteRenderer>().sprite = card.DataCard.m_cardBackSprite;
                card.DataCard.m_isUpsideDown = true;
                card.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = card.DataCard.BackCard.m_manaCost.ToString();
            }
        }
        for (int i = 0; i < GraveYard.Count; i++)
        {
            CardObject card = GraveYard[i];
            if (trueIfDruid == card.DataCard.isDruidCard && !card.DataCard.isBaseCard)
            {
                StartCoroutine(TourneCarte90(card));
                yield return new WaitForSeconds(0.25f);
                card.Name.text = card.DataCard.BackCard.Name;
                card.Description.text = card.DataCard.BackCard.Description;
                card.GetComponent<SpriteRenderer>().sprite = card.DataCard.m_cardBackSprite;
                card.DataCard.m_isUpsideDown = true;
                card.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = card.DataCard.BackCard.m_manaCost.ToString();
            }
        }
        for (int i = 0; i < deck.Count; i++)
        {
            CardObject card = deck[i];
            if (trueIfDruid == card.DataCard.isDruidCard && !card.DataCard.isBaseCard)
            {
                StartCoroutine(TourneCarte90(card));
                yield return new WaitForSeconds(0.25f);
                card.Name.text = card.DataCard.BackCard.Name;
                card.Description.text = card.DataCard.BackCard.Description;
                card.GetComponent<SpriteRenderer>().sprite = card.DataCard.m_cardBackSprite;
                card.DataCard.m_isUpsideDown = true;
                card.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = card.DataCard.BackCard.m_manaCost.ToString();
            }
        }
        RestoreCardPosition(false);
        yield return new WaitForSeconds(0.5f);
        EndTurnButton.interactable = true;
        gameManager.CardsInteractable = true;
    }
    public IEnumerator DetransfoCoroutine()
    {
        EndTurnButton.interactable = false;
        gameManager.CardsInteractable = false;
        StartCoroutine(TransposeTransparency(Background.gameObject));
        if (GameManager.Instance.FM.perso1)
        {
            foreach (SpriteRenderer sprite in gameManager.FM.HeroesAltGameObjectRef[0].GetComponentsInChildren<SpriteRenderer>())
            {
                StartCoroutine(TransposeTransparencyNegative(sprite));
            }
            foreach (SpriteRenderer sprite in gameManager.FM.HeroesGameObjectRef[0].GetComponentsInChildren<SpriteRenderer>())
            {
                StartCoroutine(TransposeTransparency(sprite));
            }
            if (GameManager.Instance.FM.perso2)
            {
                foreach (SpriteRenderer sprite in gameManager.FM.HeroesGameObjectRef[1].GetComponentsInChildren<SpriteRenderer>())
                {
                    StartCoroutine(TransposeTransparency(sprite));
                }
                foreach (SpriteRenderer sprite in gameManager.FM.HeroesAltGameObjectRef[1].GetComponentsInChildren<SpriteRenderer>())
                {
                    StartCoroutine(TransposeTransparencyNegative(sprite));
                }
            }
        }

        for (int i = 0; i < Hand.Count;i++)
        {
            CardObject card = Hand[i];
            if (card.DataCard.m_isUpsideDown)
            {
                StartCoroutine(TourneCarte90(card));
                yield return new WaitForSeconds(0.25f);
                card.Name.text = card.DataCard.name;
                card.Description.text = card.DataCard.Description;
                card.GetComponent<SpriteRenderer>().sprite = card.DataCard.m_cardFrontSprite;
                card.DataCard.m_isUpsideDown = false;
                card.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = card.DataCard.m_manaCost.ToString();
            }
        }
        for (int i = 0; i < GraveYard.Count; i++)
        {
            CardObject card = GraveYard[i];
            if (card.DataCard.m_isUpsideDown)
            {
                StartCoroutine(TourneCarte90(card));
                yield return new WaitForSeconds(0.25f);
                card.Name.text = card.DataCard.name;
                card.Description.text = card.DataCard.Description;
                card.GetComponent<SpriteRenderer>().sprite = card.DataCard.m_cardFrontSprite;
                card.DataCard.m_isUpsideDown = false;
                card.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = card.DataCard.m_manaCost.ToString();
            }
        }
        for (int i = 0; i < deck.Count; i++)
        {
            CardObject card = deck[i];
            if (card.DataCard.m_isUpsideDown)
            {
                StartCoroutine(TourneCarte90(card));
                yield return new WaitForSeconds(0.25f);
                card.Name.text = card.DataCard.name;
                card.Description.text = card.DataCard.Description;
                card.GetComponent<SpriteRenderer>().sprite = card.DataCard.m_cardFrontSprite;
                card.DataCard.m_isUpsideDown = false;
                card.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = card.DataCard.m_manaCost.ToString();
            }
        }
        RestoreCardPosition(false);
        StartCoroutine(DiscardCoroutine(true));
    }
}
