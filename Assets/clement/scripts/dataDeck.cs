using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class dataDeck : ScriptableObject
{
    [SerializeField] dataCard[] dataCards;


    [Button ("Validate")]
    private void OnValidate()
    {

    }

}
