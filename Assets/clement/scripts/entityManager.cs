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
    #region GET & SET
    public int getMaxPv() { return m_maxPv; }
    public int getPv() { return m_Pv; }
    public void setPv(int pv) { m_Pv = pv; }
    public int getSpeed() { return m_speed; }
    public void setSpeed(int speed) { m_speed = speed; }
    public int getBuff() { return m_buff; }
    public void setBuff(int buff) { m_buff = buff; }
    public int getNerf() { return m_nerf; }
    public void setNerf(int nerf) { m_nerf = nerf; }
    public int getMana() { return m_mana; }
    public void setMana(int mana) { m_mana = mana; }
    public deck GetDeck() { return m_deck; }
    public void setDeck(deck deck) { m_deck = deck; }
    public bool getIsAlive() { return isAlive; }
    public void setIsAlive(bool set) { isAlive = set; }
    #endregion
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
