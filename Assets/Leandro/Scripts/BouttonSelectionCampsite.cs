using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouttonSelectionCampsite : MonoBehaviour
{
    [SerializeField] public bool isDruid;

    void OnMouseUpAsButton()
    {
        FindObjectOfType<Campsite>().selection(isDruid);
    }
}

