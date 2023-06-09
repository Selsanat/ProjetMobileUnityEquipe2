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
    [SerializeField] public int m_manaCost; //mana
    [SerializeField] public int m_value;
    [SerializeField] public int m_index; //index de la carte dans la liste
    [SerializeField] public int nombreDexecutiion = 1; //nb de fois que la cartee s'execute
    [SerializeField] public int unlockLevel;
    [SerializeField] CardType m_cardTypes;

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
    [SerializeField] public bool m_isDeleteOnTurn;
    [SerializeField] public bool m_isUpsideDown;
    [SerializeField] bool m_isBonusCard;

    [SerializeField] string m_onCardExplain;
    [SerializeField] string m_FullCardExplain;
    [SerializeField] public bool isDruidCard;
    [Header("looking")]
    [SerializeField] public String Description;
    [SerializeField] public String Name;
    [SerializeField] public Sprite m_cardFrontSprite;
    [SerializeField] public Sprite m_cardBackSprite;

    [SerializeField] public dataCard BackCard;


    public GameManager GM;

    public Sprite CardSprite { get => m_cardFrontSprite; private set => m_cardFrontSprite = value; }
    public CardType CardTypes { get => m_cardTypes; set => m_cardTypes = value; }
    public List<CardEffect> CardEffects { get => m_cardEffects; set => m_cardEffects = value; }

    void Start ()
    {
        GM = FindObjectOfType<GameManager>();
    }



    [System.Serializable]
    public struct CardEffect
    {

        [SerializeField] public CardType effects;
        [SerializeField] public int values;
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
        HabemusDominum,//inflique 6 blessure a un enemy qui va attaquer 
        DiabolusEst,//inflique 12 blessure a un enemy qui ne va pas attaquer 
        CultiverAme,//sauve une carte de la defausse pour un tour et lui double ses stats de soin ou de bloquage si elle en a
        CultiverFlamme,//sauve une carte de la defausse pour un tour et lui double ses stats de soin, de bloquage ou de degat si elle en a
        Conversion, //transforme un point d'armure en point de vie
        Absolution, //effet actif sur tout le combat, quand des soins ou du bloquage est utilise la meme valeur est egalement renvoye en degats a tout les ennemis
        Benediction, //inflige des degats correspondant au nombre de point de veneration puis pioche une carte
        Apotasie,//inflige 4 blessures a tout les monstres, lorsqu'un hero se transforme ça dure pour tout le combat
        Tabernacle,//lorsque le hero est blesse de X degats il recoit X armure
        Belial,//inflige 6 a tout les monstres lorsque joue et ajoute 3 de soin et 4 de veneration au pretre lorsqu'un monstre est tue
        VenererIdole,//soigne de 3 le hero cible et ajoute 2 point de veneration
        Blaspheme,//vole 5 points de vie à tout les monstres, s'il a moins de 5 pv ou si tu es full pv ça marche aussi
        AllumerCierges,//pioche l'equivalent de point de veneration du pretre
        IncendierCloatre,//inflige 15 degats au monstre avec le plus de vie (si plusieurs -> random sur les monstres)
        AccueillirNecessiteux,//bloque d'autant qu'il y a de monstres qui attaque
        MassacrerInfideles,//vole 7 points de vie à un monstre, et le surplus lui est renvoyé en dmg *3
        MoxLion,//bloque du double de mana possede puis vide le mana pour donner le double au prochain tour
        MoxAraignee,//bloque puis inflige a un enemy le double du mana possede puis redonne autant de mana que le joueur avait avant l'utilisation de la carte
        MurDeRonces,//bloque de 6 et empoisonne de 2 chaque monstre qui attaque
        LaissePourMort,//pour chaque monstre qui va attaquer empoisonne de 6
        Cataplasme,//heal de 2 et supprime les effets negatifs (poison, debuff)
        Belladone,//retire l'armure de la cible et lui inflige 8 de blessure
        SurgissementVitalique,//transforme les points de veneration de l'arboriste en mana et pioche une carte
        RepandreMort,//chaque transformation depuis le début de la partie inflige 3 de poison à chaque monstres
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






