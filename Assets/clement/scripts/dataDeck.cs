using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class dataDeck : ScriptableObject
{
    [SerializeField] string m_name;
    [SerializeField] DeckRole m_role;

    [SerializeField] dataCard[] dataCards;

    public string Name { get => m_name;}
    public DeckRole Role { get => m_role; set => m_role = value; }

    [Button ("Validate")]
    private void OnValidate()
    {

    }


    public enum DeckRole
    {
        Guerrier,
        Tank,
        Mage,
        Pretre,
        Arboriste,
        Debuffer,
        Paladin,
        Demoniste,
    }
}
