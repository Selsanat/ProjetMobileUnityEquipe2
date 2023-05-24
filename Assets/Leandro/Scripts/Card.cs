using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] int RatioGrowHoverCard;
    void OnMouseOver()
    {
        transform.localScale = new Vector3(RatioGrowHoverCard, RatioGrowHoverCard, RatioGrowHoverCard);
    }

    void OnMouseExit()
    {
        transform.localScale = new Vector3(1, 1, 1) ;
    }
}
