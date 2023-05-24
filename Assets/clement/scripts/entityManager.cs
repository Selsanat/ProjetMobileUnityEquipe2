using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class entityManager : MonoBehaviour
{
    [SerializeField] protected int m_Pv;
    [SerializeField] protected int m_attack;
    [SerializeField] protected int m_speed;
    [SerializeField] protected int m_buff;
    [SerializeField] protected int m_nerf;
    [SerializeField] protected bool isAlive;

    protected void takeDamage (int damage)
    {
        m_Pv -= damage;

        if (m_Pv <= 0)
        {
            isAlive = false;
        }
    }
}

public class hero : entityManager
{
    hero(int Pv, int speed, int buff, int nerf)
    {
        m_Pv = Pv;
        m_attack = 0;
        m_speed = speed;
        m_buff = buff;
        m_nerf = nerf;
        isAlive = true;
    }
}

public class enemy : entityManager
{
    enemy(int Pv, int attack, int speed, int buff, int nerf)
    {
        m_Pv = Pv;
        m_attack = attack;
        m_speed = speed;
        m_buff = buff;
        m_nerf = nerf;
        isAlive = true;
    }
}
