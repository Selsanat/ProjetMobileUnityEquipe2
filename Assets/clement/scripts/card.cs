using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card")]
public class card : ScriptableObject
{
    [SerializeField] int m_mana;
    [SerializeField] int m_speed;
    [SerializeField] int m_attack;
    [SerializeField] int m_boostSpeed;
    [SerializeField] int m_boostAttack;
    [SerializeField] int m_boostCrit;
    [SerializeField] int m_boostCritDmg;
    [SerializeField] int m_boostDef;
    [SerializeField] int m_boostPv;
    [SerializeField] int m_boostMana;
    [SerializeField] bool m_isDeleteOnTurn;


    public bool getIsDeleteOnTurn()
    {
        return m_isDeleteOnTurn;
    }
    public void setIsDeletOnTurn()
    {
        m_isDeleteOnTurn = true;
    }

}


