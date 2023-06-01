using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using NaughtyAttributes;
using Unity.VisualScripting.Dependencies.Sqlite;
using DG.Tweening.Core.Easing;

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


    public List<hero> heroToAttack; //always Start Null
    public bool stayInHand = false;

    void Awake()
    {
        gameManager = GameManager.Instance;
        TempsClick = gameManager.TempsPourClickCardInspect;
        BaseColliderDimensions = this.GetComponent<BoxCollider2D>().size;
        rendeureur = GetComponent<Renderer>();
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
                this.GetComponent<BoxCollider2D>().size = BaseColliderDimensions;
                gameManager.deck.ReorderZCards();
               rendeureur.sortingOrder = 10;
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
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
            gameManager.deck.ReorderZCards();

        }
    }

    void SelectedCard(bool Side1, bool Side2)
    {
        gameManager.CardsInteractable  = false;
        Transform AllyCardTransform = GameObject.FindGameObjectsWithTag("AllyCardTransform")[0].transform;
        Transform EnnemyCardTransform = GameObject.FindGameObjectsWithTag("EnnemyCardTransform")[0].transform;
        if (Side1 && Side2)
        {
            transform.position = Vector3.Lerp(AllyCardTransform.position, EnnemyCardTransform.transform.position, 0.5f);
            transform.rotation = Quaternion.Lerp(AllyCardTransform.rotation, EnnemyCardTransform.transform.rotation, 0.5f);
        }
        else if (Side1)
        {
            transform.position = AllyCardTransform.position; 
            transform.rotation = AllyCardTransform.rotation;
        }
        else
        {
            transform.position = EnnemyCardTransform.transform.position;
            transform.rotation = EnnemyCardTransform.transform.rotation;
        }
        transform.localScale = new Vector3(2,2, 2);
        rendeureur.sortingOrder = 10;
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
                }
            }
            else
            {
                gameManager.CarteUtilisee = this;
                gameManager.FM.Cardsend(this, indexHand);
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


}
