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
    [SerializeField] List<CardType> m_cardTypes;
    

    #endregion
    [SerializeField] bool m_isDeleteOnTurn;
    [SerializeField] bool m_isUpsideDown;
    [SerializeField] bool m_isBonusCard;

    [Header("looking")]
    [SerializeField] Sprite m_cardSprite;

    public Sprite CardSprite { get => m_cardSprite; private set => m_cardSprite = value; }
    public List<CardType> CardTypes { get => m_cardTypes; set => m_cardTypes = value; }

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

    [System.Serializable]
    public struct CardEffect
    {
        [SerializeField] List<CardType> effects;
        [SerializeField] int value;
        [SerializeField] int nbTour;
        [SerializeField] bool KeepCard;

    }
    public enum CardType
    {
        Heal,
        Damage,
        Injury, //d�gat sur plusieurs tours
        Block,
        EmptyMana,
        DoubleShieldMana, //donne autant de shield que le joueur avait de mana
        DoubleHeal,
        DoubleInjury,
        InjuryOnAttack,
        ArmureAsDamage,
        DeleteNerf,
        DeleteArmor,




        undifined,
        BuffDamage, 
        BuffHeal,
        
    }

}






