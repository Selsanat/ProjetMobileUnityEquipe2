using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoutonFinSelectionTower : MonoBehaviour
{
    [SerializeField] bool IsDruid;
    // Start is called before the first frame update
    private bool isClickable = true;
    void OnMouseUpAsButton()
    {
        if (isClickable)
        {
            StartCoroutine(FindObjectOfType<Tower>().Soigne(IsDruid));
            isClickable = false;
        }
        
    }
}
