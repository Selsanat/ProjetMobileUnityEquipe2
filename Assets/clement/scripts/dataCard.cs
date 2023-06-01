using DG.Tweening.Core.Easing;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static entityManager;
using static UnityEngine.Rendering.DebugUI;

[System.Serializable]
[ExecuteInEditMode]
[CreateAssetMenu]
public class dataCard : ScriptableObject
{
    #region STATISTIC
    [Header("Basic Stats")]
    [SerializeField] int m_manaCost; //mana
    [SerializeField] int m_value;
    [SerializeField] public int m_index; //index de la carte dans la liste
    [SerializeField] public int nombreDexecutiion = 1; //nb de fois que la cartee s'execute
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

    [SerializeField] string m_onCardExplain;
    [SerializeField] string m_FullCardExplain;

    [Header("looking")]
    [SerializeField] Sprite m_cardFrontSprite;
    [SerializeField] Sprite m_cardBackSprite;
    [SerializeField] public String Description;
    [SerializeField] public String Name;

    private GameManager GM;

    public Sprite CardSprite { get => m_cardFrontSprite; private set => m_cardFrontSprite = value; }
    public List<CardType> CardTypes { get => m_cardTypes; set => m_cardTypes = value; }
    public List<CardEffect> CardEffects { get => m_cardEffects; set => m_cardEffects = value; }

    void Start ()
    {
        GM = GameManager.Instance;
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
        int currentPv = hero.getPv() + m_value;
        int currentMaxPv = hero.getMaxPv();
        hero.setPv(currentPv);
        if (currentPv < currentMaxPv)
            hero.setPv(currentMaxPv);

    }

    public void takeDamage(hero hero) 
    {
        Debug.Log("Pv avant : " + hero.getPv());
        int currentPv = hero.getPv();
        currentPv -= m_value;
        hero.setPv(currentPv);
        Debug.Log("Pv apres : " + hero.getPv());

        if (currentPv <= 0)
        {
            hero.setIsAlive(false);
        }
        hero.setVarHero();
    }

    public void AddArmor(hero hero)
    {
        hero.setArmor(m_value);
    }

    public void AddMana(hero hero)
    {
        int currentMana = hero.getMana();
        currentMana+= m_value;
        hero.setMana(currentMana);
    }

    public void AddCard ()
    {
        GM.deck.DrawCard();
    }

    public void KeepCardInHand (CardObject cardToKeep)
    {
        cardToKeep.stayInHand = true;
    }

    public void ChangeCardMana (CardObject card)
    {
        card.DataCard.m_manaCost += m_value;
    }

    public void ChangeCardDamage (CardObject card)
    {
        card.DataCard.m_value += m_value;
    }

    public void FromNow()
    {
        throw new NotImplementedException();
    }

    public void Venerate()
    {
        throw new NotImplementedException();
    }

    public void Transcend()
    {
        throw new NotImplementedException();
    }

    public void Steal(hero enemy, hero ally)
    {
        int currentEnemyLife = enemy.getPv();
        enemy.setPv(currentEnemyLife -= m_value);

        int currentAllyLife = ally.getPv();
        ally.setPv(currentAllyLife += m_value);
    }

    public static void DamageEffect(hero h, int value)
    {
        h.setPv(h.getPv() - value);
    }

    public static void HealEffect(hero h, int value)
    {
        h.setPv(h.getPv() + value);
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
        AddArmor,
        AddMana,
        AddCard,//pioche une carte
        KeepCardInHand,//permet a une autre carte de ne pas passer dans la defausse a la fin du tour
        ChangeCardMana,//change le cout de mana d'une carte
        ChangeCardDamage,//change le damage d'une carte
        FromNow,//les effets de cette carte dure jusqu'a la fin du combat
        Venerate,//augmente la barre de veneration d'un allie
        Poison,//le personnage recoit les degats du poison avant de jouer puis à chaque tour il subit un point de moins
        Steal,//inflige X degat et soigne X à un autre personnage
    }

}






