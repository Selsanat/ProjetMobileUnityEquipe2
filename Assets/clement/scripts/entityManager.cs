using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class entityManager : MonoBehaviour
{
    [SerializeField] int m_Pv;
    [SerializeField] int m_speed;
    [SerializeField] int m_def;
    [SerializeField] int m_buff;
    [SerializeField] int m_nerf;
    [SerializeField] bool m_condition;
    [SerializeField] bool isStillAlive;

    protected void takeDamage (int damage)
    {
        m_Pv -= damage;

        if (m_Pv <= 0)
        {
            isStillAlive = false;
        }
    }
}

public class hero : entityManager
{
    hero()
    {

    }
}
