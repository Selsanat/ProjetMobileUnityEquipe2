using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class DataEnemy : ScriptableObject
{
    public int m_attack;
    public int m_maxPv;
    public int m_Pv;


    public Sprite m_sprite;
    public Slider m_slider;

    public entityManager.Role m_role;


    hero enemy;


    void SetEnemy()
    {
        enemy = new hero(m_role, m_maxPv, m_Pv, m_attack,0,null,0, 0);
    }
}
