using DG.Tweening.Core.Easing;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static entityManager;

[System.Serializable]

[CreateAssetMenu]
public class dataCard : ScriptableObject
{
    #region STATISTIC
    [Header("Basic Stats")]
    [SerializeField] int m_manaCost; //mana
    [SerializeField] int m_attack; //les nb dmgs
    [SerializeField] int m_heal; //les nb heal
    [SerializeField] public int m_index; //index de la carte dans la liste
    [SerializeField] List<CardType> m_cardTypes;

    [SerializeField] List<CardEffect> m_cardEffects;

    #endregion
    #region TARGET
    [Header("Define Target")]
    [SerializeField] public bool TargetAllies;
    [SerializeField] public bool TargetEnnemies;
    [SerializeField] public bool AOEAllies;
    [SerializeField] public bool AOEEnnemies;

    #endregion

    [Header("MISCELLANEAOUS")]
    [SerializeField] bool m_isDeleteOnTurn;
    [SerializeField] bool m_isUpsideDown;
    [SerializeField] bool m_isBonusCard;


    [Header("TEXT")]
    [InfoBox("/!/ ATTENTION /!/ NE PAS DONNER DEUX FOIS LE MÊME NOM A DEUX CARTE SOUS PEINE DE LAG")]
    [SerializeField] string m_cardName;
    [SerializeField] string m_onCardExplain;
    [SerializeField] string m_FullCardExplain;

    [Header("looking")]
    [SerializeField] Sprite m_cardFrontSprite;
    [SerializeField] Sprite m_cardBackSprite;

    public Sprite CardSprite { get => m_cardFrontSprite; private set => m_cardFrontSprite = value; }
    public List<CardType> CardTypes { get => m_cardTypes; set => m_cardTypes = value; }

    void OnValidate()
    {
        if(this.name != m_cardName && m_cardName != null)
        {         
            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(this), m_cardName);
        }
    }

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
        Debug.Log("Pv avant : " + hero.getPv());
        int currentPv = hero.getPv();
        currentPv -= m_attack;
        hero.setPv(currentPv);
        Debug.Log("Pv apres : " + hero.getPv());

        if (currentPv <= 0)
        {
            hero.setIsAlive(false);
        }
        hero.setVarHero();
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

        [SerializeField] public List<CardType> effects;
        [SerializeField] public List<int> values;
        [SerializeField] public int nbTour;
        [SerializeField] public bool nextTour; //l'effet se fait sur le tour suivant
        [SerializeField] public bool KeepCard;

    }
    public enum CardType
    {
        Heal,
        Damage,
        Injury, //degat sur plusieurs tours
        Block,
        EmptyMana,
        DoubleShieldMana, //donne autant de shield que le joueur a de mana
        DoubleHeal, //double le heal du joueur
        DoubleInjury, //double les blessures infliges
        InjuryOnAttack, //si ennemie attaque il prend des blessures
        ArmureAsDamage, //armure devient des degats qu'on inflige a tout les ennemies
        AttackAsArmure, // les degats inflige aux ennemis donne de armure
        DamageAsArmur, // les degats subis aux ennemis donne de armure
        DeleteNerf, // supprime tout les effets negatifs
        DeleteArmor, // retire l'armure des ennemies
        GainMana,
        GainCard,
        BlockAsHeal, //chaque degat bloque devient un heal
        AllWoundAsInjury, //Inflige autant de blessures que de points de vie manquants � tout les ennemis
        MonsterDead, //quand un monstre meurt      
    }

}






