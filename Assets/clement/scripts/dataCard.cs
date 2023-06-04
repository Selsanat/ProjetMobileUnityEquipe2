using DG.Tweening.Core.Easing;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEditor;
using UnityEngine;
using static entityManager;
using static UnityEngine.EventSystems.EventTrigger;
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

    public void heal(hero hero, int value)
    {
        hero.setPv(hero.getPv() + value);
        if (hero.getPv() < hero.getMaxPv())
            hero.setPv(hero.getMaxPv());
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

    public void takeDamage(hero hero, int value)
    {
        Debug.Log("Pv avant : " + hero.getPv());
        hero.setPv(hero.getPv()+value);
        Debug.Log("Pv apres : " + hero.getPv());

        if (hero.getPv() <= 0)
        {
            hero.setIsAlive(false);
        }
        hero.setVarHero();
    }

    public void AddArmor(hero hero)
    {
        hero.setArmor(m_value);
    }
    public void AddArmor(hero hero, int value)
    {
        hero.setArmor(value);
    }

    public void AddMana(int value)
    {
        GM.FM.mana += value;
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

    public void Venerate(hero hero, int value)
    {
        hero.setVenerate(hero.getVenerate() + value);
    }

    public void Steal(hero enemy, hero ally)
    {
        int currentEnemyLife = enemy.getPv();
        enemy.setPv(currentEnemyLife -= m_value);

        int currentAllyLife = ally.getPv();
        ally.setPv(currentAllyLife += m_value);
    }

    public void Conversion(hero hero)
    {
        hero.setArmor(hero.getArmor()-1);
        hero.setPv(hero.getPv() + 1);
    }

    public void Absolution(hero hero)
    {
/*        foreach (hero enemy in GM.FM.Enemies)
        {

        }*/
        throw new NotImplementedException();
    }

    public void Benediction(hero ally, hero enemy)
    {
        enemy.takeDamage(ally.getVenerate());
    }
    public void Tabernacle(hero ally) //appeler cette fonction à chaque fois que le joueur prend des degats
    {
        //int damageReceive = ...;
        //AddArmor(ally, damageReceive);
    }

    public void Belial()
    {
        throw new NotImplementedException();

    }
    public void VenererIdole()
    {
        throw new NotImplementedException();

    }
    public void Blaspheme()
    {
        throw new NotImplementedException();

    }
    public void AllumerCierges()
    {
        throw new NotImplementedException();

    }
    public void IncendierCloatre()
    {
        throw new NotImplementedException();

    }
    public void AccueillirNecessiteux()
    {
        throw new NotImplementedException();

    }
    public void MassacrerInfideles()
    {
        throw new NotImplementedException();

    }
    public void MoxLion(hero arboriste)
    {
        if (GM.FM.mana > 0)
        {
            AddArmor(arboriste, GM.FM.mana * 2);
            GM.FM.mana = 0;
            //prochain tour donne GM.FM.mana * 2
        }

    }

    public void MoxAraignee(hero arboriste, hero enemy)
    {
        if (GM.FM.mana > 0)
        {
            AddArmor(arboriste, GM.FM.mana * 2);
            takeDamage(enemy, GM.FM.mana * 2);
            GM.FM.mana = 0;
            //prochain tour donne GM.FM.mana * 2
        }
    }

    public void MurDeRonces()
    {
        throw new NotImplementedException();

    }

    public void LaissePourMort()
    {
        throw new NotImplementedException();

    }

    public void Cataplasme(hero hero)
    {
        heal(hero);
        //enlever les malus
    }

    public void Belladone(hero enemy)
    {
        if (enemy.getArmor() > 0)
        {
            enemy.setArmor(0);
            takeDamage(enemy, 8);
        }

    }

    public void SurgissementVitalique(hero arboriste)
    {
        if  (arboriste.getVenerate() > 0)
        {
            AddMana(arboriste.getVenerate());
            arboriste.setVenerate(0);
            AddCard();
        }
    }

    public void RepandreMort()
    {
        throw new NotImplementedException();

    }

    public void ArmureEcorse()
    {
        throw new NotImplementedException();

    }

    public void MaleusHerbeticae(hero arboriste)
    {
        foreach (hero enemy in GM.FM.Enemies)
        {
            //poison
        }
        AddArmor(arboriste, 7);
    }

    public void CommunionNature()
    {
        throw new NotImplementedException();

    }

    public void Canibalisme()
    {
        throw new NotImplementedException();

    }
    public void SuivreEtoiles(hero arboriste)
    {
        Venerate(arboriste, 2);
        bool drawCard = false;
        foreach (hero ally in GM.FM.Heroes)
        {
/*            if (ally.canTranscend)
            {
                drawCard = true;
                break;
            }*/
        }
        if (drawCard)
        {
            AddCard();
        }


    }
    public void ProfanerCiel()
    {
        throw new NotImplementedException();

    }
    public void DormirPresDeLautre(hero ally, hero arboriste)
    {
        Venerate(arboriste, 4);
        heal(ally,4);
        AddArmor(ally,4);

    }
    public void ReveillerPourManger(hero enemy, hero ally)
    {
        takeDamage(enemy, 6);
        heal(ally,6);
        Venerate(ally,6);
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
        AddCard,
        Poison,
        HabemusDominum,//?? inflique 6 blessure a chaque enemy qui va attaquer 
        DiabolusEst,//?? inflique 12 blessure a chaque enemy qui ne va pas attaquer 
        CultiverAme,//sauve une carte de la defausse pour un tour et lui double ses stats de soin ou de bloquage si elle en a
        CultiverFlamme,//sauve une carte de la defausse pour un tour et lui double ses stats de soin, de bloquage ou de degat si elle en a
        Conversion, //transforme un point d'armure en point de vie
        Absolution, //effet actif sur tout le combat, quand des soins ou du bloquage est utilise la meme valeur est egalement renvoye en degats a tout les ennemis
        Benediction, //inflige des degats correspondant au nombre de point de veneration puis pioche une carte
        Apotasie,//?? inflige 4 blessures a tout les monstres
        Tabernacle,//lorsque le hero est blesse de X degats il recoit X armure
        Belial,//inflige 6 a tout les monstres lorsque joue et ajoute 3 de soin et 4 de veneration au pretre lorsqu'un monstre est tue
        VenererIdole,//soigne de 3 le hero cible et ajoute 2 point de veneration
        Blaspheme,//??
        AllumerCierges,//pioche l'equivalent de point de veneration du pretre
        IncendierCloatre,//inflige 15 degats au monstre avec le plus de vie (si plusieurs -> random sur les monstres)
        AccueillirNecessiteux,//bloque d'autant qu'il y a de monstres qui attaque
        MassacrerInfideles,//??
        MoxLion,//bloque du double de mana possede puis vide le mana pour donner le double au prochain tour
        MoxAraignee,//bloque puis inflige a un enemy le double du mana possede puis redonne autant de mana que le joueur avait avant l'utilisation de la carte
        MurDeRonces,//bloque de 6 et empoisonne de 2 chaque monstre qui attaque
        LaissePourMort,//pour chaque monstre qui va attaquer empoisonne de 6
        Cataplasme,//heal de 2 et supprime les effets negatifs (poison, debuff)
        Belladone,//retire l'armure de la cible et lui inflige 8 de blessure
        SurgissementVitalique,//transforme les points de veneration de l'arboriste en mana et pioche une carte
        RepandreMort,//?? pour chaque ally transcend inflige 5 de poison a tout les monstres 
        ArmureEcorse,//bloque de 7 et ajoute 7 d'amure au prochain tour
        MaleusHerbeticae,//empoisonne de 4 tout les monstres et bloque de 7
        CommunionNature,//au prochain tour gagne 3 mana et 3 points de veneration
        Canibalisme,//effet actif sur tout le combat, quand un monstre meurt donne 3 veneration et soigne de 3
        SuivreEtoiles,//ajoute 2 veneration et si un ally peut transcender alors pioche 2 cartes
        ProfanerCiel,//effet actif sur tout le combat, si ally a au moins un point de veneration il peut transcender
        DormirPresDeLautre,// ajoute 4 de veneration, soigne et bloque de 4 sur une cible
        ReveillerPourManger,// inflige 6 et si la cible meurt soigne et venere de 6
    }

}






