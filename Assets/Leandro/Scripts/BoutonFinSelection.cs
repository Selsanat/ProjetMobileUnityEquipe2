using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoutonFinSelection : MonoBehaviour
{
    [SerializeField] bool IsDruid;
    // Start is called before the first frame update
    void OnMouseUpAsButton()
    {
        StartCoroutine(FindObjectOfType<Campsite>().Soigne(IsDruid));
    }
}
