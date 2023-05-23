using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CardObject : MonoBehaviour
{
    [SerializeField]private card ScriptableCard;


    Sprite SetSprite()
    {
        SpriteRenderer rer = GetComponent<SpriteRenderer>();
        rer.sprite = ScriptableCard.CardSprite;
        return rer.sprite;
    }

    [Button]
    private void OnValidate()
    {
        SetSprite();
    }
}
