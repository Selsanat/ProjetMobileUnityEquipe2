using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using NaughtyAttributes;
//using Unity.VisualScripting.Dependencies.Sqlite;
using DG.Tweening.Core.Easing;
using Unity.VisualScripting;
using TMPro;

[ExecuteInEditMode]
public class CardObject : MonoBehaviour
{
    [Header("DATACARD")]
    [SerializeField]private dataCard m_dataCard;
    public dataCard DataCard { get => m_dataCard;}

    [Header("             Statistics")]
    [SerializeField] float RatioGrowHoverCard;
    private GameManager gameManager;
    public Vector3 PosBeforeDrag;
    public Transform Slot;
    public Vector2 BaseColliderDimensions;
    public float TempsClick;
    public Renderer rendeureur;
    public int indexHand;
    public Canvas canvas;
    public TMP_Text Description;
    public TMP_Text Name;


    public List<hero> heroToAttack; //always Start Null


    public bool MenuCarde = false;

    public void RemettreCardSlot()
    {
        Transform trans = gameManager.deck.GetTransformSlotFromCard(this);
        transform.position = trans.position;
        transform.rotation = trans.rotation;
    }

    void Awake()
    {
        gameManager = GameManager.Instance;
        if (!MenuCarde)
        {
            TempsClick = gameManager.TempsPourClickCardInspect;
        }
        BaseColliderDimensions = this.GetComponent<BoxCollider2D>().size;
        rendeureur = GetComponent<Renderer>();
        canvas = transform.GetChild(0).GetComponent<Canvas>();
        Description = canvas.transform.GetChild(0).GetComponent<TMP_Text>();
        Description.text = DataCard.Description;
        Name = canvas.transform.GetChild(1).GetComponent<TMP_Text>();
        Name.text = DataCard.Name;
    }
    void OnMouseDown()
    {
        if (gameManager.CardsInteractable  && !gameManager.HasCardInHand)
        {
            PosBeforeDrag = transform.position;
            TempsClick = Time.time;
        }
    }
/*    void OnMouseUpAsButton()
    {
    }*/
    void OnMouseOver()
    {
        if (gameManager.CardsInteractable  && !gameManager.HasCardInHand)
        {
            if (Input.GetMouseButton(0))
            {

                transform.localScale = new Vector3(RatioGrowHoverCard, RatioGrowHoverCard, RatioGrowHoverCard);
                transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0,0);
                Transform trans = gameManager.deck.GetTransformSlotFromCard(this);
                transform.position = new Vector3(trans.position.x, trans.position.y + 1, trans.position.z);
                this.GetComponent<BoxCollider2D>().size = BaseColliderDimensions;
                gameManager.deck.ReorderZCards();
               rendeureur.sortingOrder = 10;
                canvas.sortingOrder = 10;
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
                RemettreCardSlot();
                gameManager.deck.ReorderZCards();
            }
        }
        
    }
    void OnMouseDrag()
    {
        if (gameManager.CardsInteractable )
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            gameManager.HasCardInHand = true;
        }
    }
    void OnMouseExit()
    {
        if (gameManager.CardsInteractable  && !gameManager.HasCardInHand)
        {
            transform.localScale = new Vector3(1, 1, 1);
            RemettreCardSlot();
            gameManager.deck.ReorderZCards();

        }
    }

    void SelectedCard(bool Side1, bool Side2)
    {
        gameManager.CardsInteractable  = false;

        gameManager.deck.DeplaceCardUtiliseToPlace();
        transform.localScale = new Vector3(2,2, 2);
        rendeureur.sortingOrder = 10;
        canvas.sortingOrder = 10;
        HideHandExceptThis();
    }

    [Button]
    void OnMouseUp()
    {
        if (gameManager.CardsInteractable )
        {
            
            transform.localScale = new Vector3(1, 1, 1);
            gameManager.deck.ReorderZCards();
            if (transform.position.y < gameManager.RangePourActiverCarte)
            {
                transform.position = PosBeforeDrag;
                if (Time.time - TempsClick < gameManager.TempsPourClickCardInspect)
                {
                    gameManager.CardsInteractable = false;
                    //print(gameManager.InspectUI.Image.sprite);
                    print(this.GetComponent<SpriteRenderer>().sprite);
                    gameManager.InspectUI.Image.sprite = this.GetComponent<SpriteRenderer>().sprite;
                    gameManager.InspectUI.UI.SetActive(true);
                    gameManager.InspectUI.Name.text = this.DataCard.Name;
                    gameManager.InspectUI.description.text = this.DataCard.Description;
                    RemettreCardSlot();

                }
            }
            else
            {
                gameManager.CarteUtilisee = this;
                gameManager.FM.Cardsend(this, indexHand);
                Slot = this.gameObject.transform;
                FindObjectOfType<Deck>().CancelButton.gameObject.SetActive(true) ;
                FindObjectOfType<Deck>().PlayButton.gameObject.SetActive(true) ;
                SelectedCard(DataCard.TargetAllies, DataCard.TargetEnnemies) ;
            }
        }
        gameManager.HasCardInHand = false;

    }

    void HideHandExceptThis()
    {
        foreach(CardObject Carte in gameManager.Hand)
        {
            Carte.gameObject.SetActive(false);
        }
        this.gameObject.SetActive(true);
    }
    void ShowHand()
    {
        foreach (CardObject Carte in gameManager.Hand)
        {
            Carte.gameObject.SetActive(true);
        }
    }
    Sprite SetSprite()
    {
        SpriteRenderer Sr = GetComponent<SpriteRenderer>();
        Sr.sprite = m_dataCard.CardSprite;
        return Sr.sprite;
    }

    [SerializeField][Button]
    private void OnValidate()
    {
       //SetSprite();
    }

    private void Update()
    {
        //print(gameManager.HasCardInHand);
    }

    IEnumerator TransposeAtoB(GameObject objetABouger, Vector3 position)
    {
        for (int i =0; i <100; i++)
        {
            gameManager.deck.TransposeAtoB(objetABouger, position);
            yield return null;
        }
        
    }
}
