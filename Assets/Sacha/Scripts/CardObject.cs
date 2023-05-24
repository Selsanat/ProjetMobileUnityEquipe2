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
    private Vector3 PosBeforeDrag;

    void Awake()
    {
        gameManager = GameManager.Instance;
    }
    void OnDrawGizmosSelected()
    {
    }
    void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            transform.localScale = new Vector3(RatioGrowHoverCard, RatioGrowHoverCard, RatioGrowHoverCard);
        }
        
    }
    void OnMouseDrag()
    {
        transform.position = Input.mousePosition;
    }
    void OnMouseExit()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }
    
    void OnMouseUp()
    {
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        PosBeforeDrag = transform.position;
    }
    public void OnDrag(PointerEventData eventData)
    {
        
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.position.y  < gameManager.RangePourActiverCarte)
        {
            transform.position = PosBeforeDrag;
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
