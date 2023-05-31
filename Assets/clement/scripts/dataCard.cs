using DG.Tweening.Core.Easing;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static entityManager;

[System.Serializable]
[ExecuteInEditMode]
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
    public List<CardEffect> CardEffects { get => m_cardEffects; set => m_cardEffects = value; }

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
        Damage,
        Heal,
        Armor,
        AddMana,
        AddCard,//pioche une carte
        UpgradeCard,//la carte ne va pas dans la defausse elle reste sur la table et s'ameliore au fur et a mesure de la partie, Leur prix peut baisser, leurs stats augmenter...
        ChangeCardMana,//change le mana d'une carte
        ChangeDamage,//change le damage d'une carte
        FromNow,//les effets de cette carte dure jusqu'a la fin du combat
        Venerate,//augmente la barre de veneration d'un allie
        Transcend,//un personnage avec assez de points de veneration peut se transcender
        Poison,//le personnage recoit les degats du poison avant de jouer puis à chaque tour il subit un point de moins
        Steal,//inflige X degat et soigne X à un autre personnage
    }

}






