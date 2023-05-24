using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class card : ScriptableObject
{
    #region STATISTIC
    [Header("Basic Stats")]
    [SerializeField] int m_manaCost; //mana
    [SerializeField] int m_attack; //les nb dmgs
    [SerializeField] int m_heal; //les nb heal
    #region BOOST
    [Header("Buff Stats")]
    [SerializeField] int m_boostSpeed; //nb boost speed
    [SerializeField] int m_boostAttack; 
    [SerializeField] int m_boostCrit;
    [SerializeField] int m_boostCritDmg;
    [SerializeField] int m_boostDef;
    [SerializeField] int m_boostPv;
    [SerializeField] int m_boostMana;
    #endregion
    #endregion
    [SerializeField] bool m_isDeleteOnTurn;
    [SerializeField] bool m_isUpsideDown;
    [SerializeField] bool m_isBonusCard;

    [Header("looking")]
    [SerializeField] Sprite m_cardSprite;

    public Sprite CardSprite { get => m_cardSprite; private set => m_cardSprite = value; }

    public bool getIsDeleteOnTurn()
    {
        return m_isDeleteOnTurn;
    }
    public void setIsDeletOnTurn()
    {
        m_isDeleteOnTurn = true;
    }


    public void heal(hero hero)
    {
        hero.m_Pv += m_heal;
        if (hero.m_maxPv < hero.m_Pv)
            hero.m_Pv = hero.m_maxPv;
    }

    public void takeDamage(hero hero)
    {
        hero.m_Pv -= m_attack; 

        if (hero.m_Pv <= 0)
        {
            hero.isAlive = false;
        }
    }

    public void draw(hero hero, int amout)
    {
        hero.deck.draw();
    }



}






