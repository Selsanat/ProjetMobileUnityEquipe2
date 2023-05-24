using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using NaughtyAttributes;

[ExecuteInEditMode]
public class CardObject : MonoBehaviour
{
    [Header("DATACARD")]
    [SerializeField]private dataCard m_dataCard;


    #region Leandro
    [HorizontalLine]
    [Header("             Statistics")]
    [SerializeField] float RatioGrowHoverCard;
    private GameManager gameManager;
    public Vector3 PosBeforeDrag;
    private bool Interactible = true;

    void Awake()
    {
        gameManager = GameManager.Instance;
    }
    void OnMouseDown()
    {
        if (Interactible)
        {
            PosBeforeDrag = transform.position;
        }
    }
    void OnMouseOver()
    {
        if (Interactible)
        {
            if (Input.GetMouseButton(0))
            {
                transform.localScale = new Vector3(RatioGrowHoverCard, RatioGrowHoverCard, RatioGrowHoverCard);
            }
        }
        
    }
    void OnMouseDrag()
    {
        if (Interactible)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }
    void OnMouseExit()
    {
        if (Interactible)
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
                //Va falloir ajouter la condition
                //True si on Cible les alliés, false si non
                SelectedCard(true);
            }
        }
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
    #endregion
    Sprite SetSprite()
    {
        SpriteRenderer Sr = GetComponent<SpriteRenderer>();
        Sr.sprite = m_dataCard.CardSprite;
        return Sr.sprite;
    }

    [SerializeField][Button]
    private void OnValidate()
    {
        SetSprite();
    }


}
