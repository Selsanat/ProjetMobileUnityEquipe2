using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using NaughtyAttributes;
using Unity.VisualScripting.Dependencies.Sqlite;

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
    public bool Interactible = true;
    public Transform Slot;
    public Vector2 BaseColliderDimensions;



    private List<hero> heroToAttack; //always Start Null
    public List<hero> HeroToAttack { get => heroToAttack; set => heroToAttack = value; }

    void Awake()
    {
        gameManager = GameManager.Instance;
        BaseColliderDimensions = this.GetComponent<BoxCollider2D>().size;
    }
    void OnMouseDown()
    {
        if (Interactible && !gameManager.HasCardInHand)
        {
            PosBeforeDrag = transform.position;
        }
    }
    void OnMouseOver()
    {
        if (Interactible && !gameManager.HasCardInHand)
        {
            if (Input.GetMouseButton(0))
            {

                transform.localScale = new Vector3(RatioGrowHoverCard, RatioGrowHoverCard, RatioGrowHoverCard);
                this.GetComponent<BoxCollider2D>().size = BaseColliderDimensions;
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
        
    }
    void OnMouseDrag()
    {
        if (Interactible)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            gameManager.HasCardInHand = true;
        }
    }
    void OnMouseExit()
    {
        if (Interactible && !gameManager.HasCardInHand)
        {
            transform.localScale = new Vector3(1, 1, 1);
            
        }
    }

    void SelectedCard(bool Side)
    {
        Interactible = false;
        if (Side)
        {
            transform.position = GameObject.FindGameObjectsWithTag("AllyCardTransform")[0].transform.position;
            transform.rotation = GameObject.FindGameObjectsWithTag("AllyCardTransform")[0].transform.rotation;
        }
        else
        {
            transform.position = GameObject.FindGameObjectsWithTag("EnnemyCardTransform")[0].transform.position;
            transform.rotation = GameObject.FindGameObjectsWithTag("EnnemyCardTransform")[0].transform.rotation;
        }
        transform.localScale = new Vector3(2,2, 2);
        HideHandExceptThis();
    }
    
    void OnMouseUp()
    {
        if (Interactible)
        {
            
            transform.localScale = new Vector3(1, 1, 1);
            if (transform.position.y < gameManager.RangePourActiverCarte)
            {
                transform.position = PosBeforeDrag;
            }
            else
            {
                gameManager.CarteUtilisee = this;
                FindObjectOfType<Deck>().CancelButton.gameObject.SetActive(true) ;
                SelectedCard(true);
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
        print(gameManager.HasCardInHand);
    }


}
