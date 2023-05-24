using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[ExecuteInEditMode]
public class CardObject : MonoBehaviour
{
    [Header("LA CARTE EN SCRIPTABLE OBJECT")]
    [SerializeField]private card ScriptableCard;


    #region Leandro
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
        SpriteRenderer rer = GetComponent<SpriteRenderer>();
        rer.sprite = ScriptableCard.CardSprite;
        return rer.sprite;
    }

    [SerializeField][Button]
    private void OnValidate()
    {
        SetSprite();
    }
}
