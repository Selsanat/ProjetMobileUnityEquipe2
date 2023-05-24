using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using NaughtyAttributes;
[ExecuteInEditMode]
public class CardObject : MonoBehaviour
{
    [Header("LA CARTE EN SCRIPTABLE OBJECT")]
    [SerializeField]private card ScriptableCard;


    #region Leandro
    [HorizontalLine]
    [Header("             Statistics")]
    [SerializeField] float RatioGrowHoverCard;
    void OnMouseOver()
    {
        transform.localScale = new Vector3(RatioGrowHoverCard, RatioGrowHoverCard, RatioGrowHoverCard);
    }

    void OnMouseExit()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }
    #endregion
    Sprite SetSprite()
    {
        SpriteRenderer Sr = GetComponent<SpriteRenderer>();
        Sr.sprite = ScriptableCard.CardSprite;
        return Sr.sprite;
    }

    [SerializeField][Button]
    private void OnValidate()
    {
        SetSprite();
    }
}
