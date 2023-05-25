using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

[CreateAssetMenu]
public class dataCard : ScriptableObject
{
    #region STATISTIC
    [Header("Basic Stats")]
    [SerializeField] int m_manaCost; //mana
    [SerializeField] int m_attack; //les nb dmgs
    [SerializeField] int m_heal; //les nb heal
    [SerializeField] cardType m_cardType; // Damage /Heal / Buffs
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
    public cardType CardType { get => m_cardType; set => m_cardType = value; }

    public bool getIsDeleteOnTurn()
    {
        return m_isDeleteOnTurn;
    }
    public void setIsDeletOnTurn()
    {
        m_isDeleteOnTurn = true;
    }

    #region CARD EFFECTS
    public void heal(hero hero)
    {
        int currentPv = hero.getPv() + m_heal;
        int currentMaxPv = hero.getMaxPv();
        hero.setPv(currentPv);
        if (currentPv < currentMaxPv)
            hero.setPv(currentMaxPv);

    }

    public void takeDamage(hero hero)
    {

        int currentPv = hero.getPv();
        currentPv -= m_attack;

        if (currentPv <= 0)
        {
            hero.setIsAlive(false);
        }
    }
    internal void BuffDamage(hero hero)
    {
        throw new NotImplementedException();
    }


    #endregion

    public void draw(hero hero, int amout)
    {
        hero.GetDeck();
    }



    public enum cardType
    {
        undifined,
        Damage,
        Heal,
        BuffDamage, 
        BuffHeal,
        Block
    }

}






