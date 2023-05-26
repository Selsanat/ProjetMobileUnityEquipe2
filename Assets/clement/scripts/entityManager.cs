using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class entityManager : MonoBehaviour
{

    public enum Role 
    {
        Enemy,
/*        Guerrier,
        Tank,*/
        Mage,
        Pretre,
        Arboriste,
        Debuffer,
        ChienEnemy,
        Squellettes,
        Mains

/*        Paladin,
        Demoniste,*/
    }

    [SerializeField] public Role m_role;
    [SerializeField] protected int m_maxPv;
    [SerializeField] protected int m_Pv;
    [SerializeField] protected int m_attack;
    [SerializeField] protected int m_speed;
    [SerializeField] protected int m_buff;
    [SerializeField] protected int m_nerf;
    [SerializeField] protected bool isAlive = true;
    [SerializeField] protected int m_mana;
    [SerializeField] protected Deck m_deck;
    [SerializeField] protected List<hero> heroList;
    [SerializeField] protected bool multipleTarget;

    public List<hero> getListHero() { return heroList; }


    public void takeDamage (int damage)
    {
        m_Pv -= damage;

        if (m_Pv <= 0)
        {
            isAlive = false;
        }
    }
}

