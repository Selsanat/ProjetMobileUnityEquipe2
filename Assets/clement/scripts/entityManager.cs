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
        Mains,
        Gargouilles,
        HommesVers,
        Demon,
        Dragon

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
    [SerializeField] protected int m_armor;
    [SerializeField] protected bool m_isDebufArmor = false;
    [SerializeField] protected bool isAlive = true;
    [SerializeField] protected bool isAntiHeal = false;
    [SerializeField] protected bool isProvocation = false;
    [SerializeField] protected int m_damageMultiplier = 1;
    [SerializeField] protected int m_mana;
    [SerializeField] protected Deck m_deck;
    [SerializeField] public List<hero> heroList;
    [SerializeField] protected bool multipleTarget;
    [SerializeField] protected GameManager gameManager;

    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        Debug.Log("start");
    }
    public List<hero> getListHero() { return heroList; }


    public void takeDamage (int damage)
    {
        Debug.Log("Pv avant : " + m_Pv + m_role);
        if(m_isDebufArmor)
        {
            m_armor /= 2;
            m_isDebufArmor = false;
        }
        damage -= m_armor;
        if (damage >= 0)
            m_armor = 0;
        else
            damage = 0;
        
        m_Pv -= damage * m_damageMultiplier;

        Debug.Log("Pv apres: hero" + m_Pv + m_role);
        if (m_Pv <= 0)
        {
            isAlive = false;
        }

        if(m_role == Role.Arboriste)
        {
            gameManager.LifeArboriste = m_Pv;
        }
        else if(m_role == Role.Pretre)
        {
            gameManager.LifePretre = m_Pv;
        }
    }


}

