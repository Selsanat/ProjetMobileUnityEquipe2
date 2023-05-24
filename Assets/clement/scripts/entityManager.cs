using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class entityManager : MonoBehaviour
{
    public enum Role 
    {
        Guerrier,
        Tank,
        Mage,
        Prêtre,
        Arboriste,
        Debuffer,
        Paladin,
        Demoniste,
    }

    [SerializeField] public Role m_role;
    [SerializeField] protected int m_maxPv;
    [SerializeField] protected int m_Pv;
    [SerializeField] protected int m_attack;
    [SerializeField] protected int m_speed;
    [SerializeField] protected int m_buff;
    [SerializeField] protected int m_nerf;
    [SerializeField] protected bool isAlive;
    [SerializeField] protected int m_mana;
    [SerializeField] protected deck m_deck;

    #region Get Set
    public int MaxPv { get => m_maxPv; private set => m_maxPv = value; }
    public int Pv { get => m_Pv; private set => m_Pv = value; }
    public int Attack { get => m_attack; private set => m_attack = value; }
    public int Speed { get => m_speed; private set => m_speed = value; }
    public int Buff { get => m_buff; private set => m_buff = value; }
    public int Nerf { get => m_nerf; private set => m_nerf = value; }
    public bool IsAlive { get => isAlive; private set => isAlive = value; }
    public int Mana { get => m_mana; private set => m_mana = value; }
    public deck Deck { get => m_deck; private set => m_deck = value; }
    #endregion

    public void takeDamage (int damage)
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
    hero(Role role, int maxPV, int Pv, int speed, int buff, int nerf, deck deck, int mana)
    {
        m_role = role;
        m_maxPv = maxPV;
        m_Pv = Pv;
        m_attack = 0;
        m_speed = speed;
        m_buff = buff;
        m_nerf = nerf;
        isAlive = true;
        m_deck = deck;
        m_mana = mana;
    }
}

public class enemy : entityManager
{
    enemy(int maxPV, int Pv, int attack, int speed, int buff, int nerf, int mana)
    {
        m_maxPv = maxPV;
        m_Pv = Pv;
        m_attack = attack;
        m_speed = speed;
        m_buff = buff;
        m_nerf = nerf;
        m_mana = mana;
        isAlive = true;
    }
}
