using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoutonFinSelection : MonoBehaviour
{
    [SerializeField] bool IsDruid;
    private bool isClickable = true;
    // Start is called before the first frame update
    void OnMouseUpAsButton()
    {
        if (isClickable)
        {
            StartCoroutine(FindObjectOfType<Campsite>().Soigne(IsDruid));
            isClickable = false;
        }
    }
}
