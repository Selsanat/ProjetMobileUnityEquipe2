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
    [SerializeField] Button EndTurnButton;
    [SerializeField] public Button CancelButton;
    [SerializeField] public Button PlayButton;
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





    private List<CardObject> GraveYard = new List<CardObject>();
    [SerializeField] public List<CardObject> Hand = new List<CardObject>();
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
            
        }
        else
        {
            SlidersXp[0].maxValue = herolist[0].getexperienceMAX();
            SlidersXp[0].value = gameManager.expArboriste;
            SlidersXp[1].maxValue = herolist[1].getexperienceMAX();
            SlidersXp[1].value = gameManager.expPretre;
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
    
    public void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.RangePourActiverCarte = RangePourActiverCarte;
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
        PiocheButton.onClick.AddListener(delegate { StartCoroutine(DrawCardCoroutine()); });
        CancelButton.onClick.AddListener(CancelChosenCard);
        gameManager.deck = this;
        gameManager.FM.play = this.PlayButton;
        gameManager.FM.cancel = this.CancelButton;

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
                    randCard.transform.position = cardSlots[i].position;
                    randCard.transform.rotation = cardSlots[i].rotation;

                    randCard.Slot = cardSlots[i];
                    randCard.indexHand = i;
                    Hand.Add(randCard);
                    availableCardSlots[i] = false;
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
        PlayButton.gameObject.SetActive(false);
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
        StartCoroutine(TransfoCoroutine());
    }
    [Button]
    private void PDetransfo()
    {
        StartCoroutine(DetransfoCoroutine());
    }

    public void DeplaceCardUtiliseToPlace()
    {
        Transform AllyCardTransform = GameObject.FindGameObjectsWithTag("AllyCardTransform")[0].transform;
        Transform EnnemyCardTransform = GameObject.FindGameObjectsWithTag("EnnemyCardTransform")[0].transform;
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
    IEnumerator DrawCardCoroutine()
    {
        float TempsTransition = TempsTrans;
        float timeElapsed = 0;
        CardObject card = DrawCard();
        GameObject CardGO = card.gameObject;
        CardGO.transform.localScale = new Vector3(0,0,0);
        CardGO.transform.position = Camera.main.ScreenToWorldPoint(DeckCount.transform.position);
        while (timeElapsed < TempsTransition)
        {
            CardGO.transform.rotation = Quaternion.Lerp(CardGO.transform.rotation, cardSlots[(int)Mathf.Ceil(cardSlots.Count / 2) - Hand.Count / 2 + card.indexHand].rotation, Time.deltaTime * VitesseTranspo);
            CardGO.transform.position = Vector3.Lerp(CardGO.transform.position, cardSlots[(int)Mathf.Ceil(cardSlots.Count / 2) - Hand.Count / 2 + card.indexHand].position, Time.deltaTime * VitesseTranspo);
            CardGO.transform.localScale = Vector3.Lerp(CardGO.transform.localScale, new Vector3(1,1,1), Time.deltaTime * VitesseTranspo);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        CardGO.transform.position = cardSlots[(int)Mathf.Ceil(cardSlots.Count / 2) - Hand.Count / 2 + card.indexHand].position;
        CardGO.transform.localScale = new Vector3(1, 1, 1);
    }
    IEnumerator DrawCardCoroutine(int nombreAPiocher)
    {
        for(int i = 0; i < nombreAPiocher; i++)
        {
            StartCoroutine( DrawCardCoroutine());
            yield return new WaitForSeconds(0.25f);
        }
    }
    IEnumerator DiscardCoroutine()
    {
        foreach(CardObject card in Hand.ToList())
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
    }
    public IEnumerator DiscardCoroutine(bool justeCache)
    {
        foreach (CardObject card in Hand.ToList())
        {
            StartCoroutine(DiscardOneCoroutine(card));
            yield return new WaitForSeconds(0.25f);
        }
        LibereEspacesHand();
        HandToGraveyard();

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
    public IEnumerator TransfoCoroutine()
    {
        StartCoroutine( TransposeTransparencyNegative(Background.gameObject));
        for (int i = 0; i < Hand.Count; i++)
        {
            CardObject card = Hand[i];
            StartCoroutine( TransposeAtoBRotation(card.gameObject, Quaternion.Euler(0, 90, 0)));
            card.GetComponent<SpriteRenderer>().sprite = card.DataCard.m_cardBackSprite;
            StartCoroutine(TransposeAtoBRotation(card.gameObject, Quaternion.Euler(0, 0, 0)));
            card.DataCard.m_isUpsideDown = true;
            yield return new WaitForSeconds(0.25f);
        }
        RestoreCardPosition(false);
        yield return new WaitForSeconds(0.5f);
    }
    public IEnumerator DetransfoCoroutine()
    {
        StartCoroutine(TransposeTransparency(Background.gameObject));
        for (int i = 0; i < Hand.Count;i++)
        {
            CardObject card = Hand[i];
            StartCoroutine( TransposeAtoBRotation(card.gameObject, Quaternion.Euler(0, 90,0)));
            card.GetComponent<SpriteRenderer>().sprite = card.DataCard.m_cardFrontSprite;
            StartCoroutine(TransposeAtoBRotation(card.gameObject, Quaternion.Euler(0, 0,0)));
            card.DataCard.m_isUpsideDown = true;
            yield return new WaitForSeconds(0.25f);
        }
        RestoreCardPosition(false);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(DiscardCoroutine(true));
    }
}
