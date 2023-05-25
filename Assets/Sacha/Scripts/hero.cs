using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hero : entityManager
{
    public hero(Role role, int maxPV, int Pv, int buff, int nerf, Deck deck, int mana)
    {
        m_role = role;
        m_maxPv = maxPV;
        m_Pv = Pv;
        m_attack = 0;
        m_buff = buff;
        m_nerf = nerf;
        isAlive = true;
        m_deck = deck;
        m_mana = mana;

        int a = Random.Range(0, 1);
        if (a == 0) { multipleTarget = false; }
        else { multipleTarget = true; }
    }
    #region GET & SET
    public int getMaxPv() { return m_maxPv; }
    public int getPv() { return m_Pv; }
    public void setPv(int pv) { m_Pv = pv; }
    public int getBuff() { return m_buff; }
    public void setBuff(int buff) { m_buff = buff; }
    public int getNerf() { return m_nerf; }
    public void setNerf(int nerf) { m_nerf = nerf; }
    public int getMana() { return m_mana; }
    public void setMana(int mana) { m_mana = mana; }
    public Deck GetDeck() { return m_deck; }
    public void setDeck(Deck deck) { m_deck = deck; }
    public bool getIsAlive() { return isAlive; }
    public void setIsAlive(bool set) { isAlive = set; }
    #endregion




    public void EnemyAttack(List<hero> heroesToAttack)
    {
        if (multipleTarget)
        {
            foreach (hero hero in heroesToAttack)
            {
                hero.takeDamage(3);
            }
        }
        else
        {
            hero old = new hero(entityManager.Role.Arboriste, 99999, 99999, 0, 0, new Deck(), 10);
            foreach (hero heroooo in heroesToAttack)
            {
                if (heroooo.getPv() < old.getPv())
                {
                    old = heroooo;
                }
            }
            old.takeDamage(3);
        }

    }


}

/*public class enemy : entityManager
{
    enemy(int maxPV, int Pv, int attack, int buff, int nerf, int mana)
    {
        m_maxPv = maxPV;
        m_Pv = Pv;
        m_attack = attack;
        m_buff = buff;
        m_nerf = nerf;
        m_mana = mana;
        isAlive = true;
    }
    #region GET & SET
    public int getMaxPv() { return m_maxPv; }
    public int getPv() { return m_Pv; }
    public void setPv(int pv) { m_Pv = pv; }
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
*/
