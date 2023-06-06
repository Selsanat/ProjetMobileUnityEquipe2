using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouttonSelectionTower : MonoBehaviour
{
    [SerializeField] public bool isDruid;

    void OnMouseUpAsButton()
    {
        FindObjectOfType<Tower>().selection(isDruid);
    }
}

