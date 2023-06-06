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
    [SerializeField] public int m_value;
    [SerializeField] public int m_index; //index de la carte dans la liste
    [SerializeField] public int nombreDexecutiion = 1; //nb de fois que la cartee s'execute
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
    [SerializeField] bool m_isDeleteOnTurn;
    [SerializeField] public bool m_isUpsideDown;
    [SerializeField] bool m_isBonusCard;

    [SerializeField] string m_onCardExplain;
    [SerializeField] string m_FullCardExplain;

    [Header("looking")]
    [SerializeField] public Sprite m_cardFrontSprite;
    [SerializeField] public Sprite m_cardBackSprite;
    [SerializeField] public String Description;
    [SerializeField] public String Name;

    private GameManager GM;

    public Sprite CardSprite { get => m_cardFrontSprite; private set => m_cardFrontSprite = value; }
    public CardType CardTypes { get => m_cardTypes; set => m_cardTypes = value; }
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
        if(GM.isAbsolution)
        {
            foreach (hero enemy in GM.FM.enemiesAtStartOfCombat)
            {
                if(enemy.getIsAlive())
                {
                    takeDamage(enemy, m_value);
                }
            }

        }

    }

    public void heal(hero hero, int value)
    {
        hero.setPv(hero.getPv() + value);
        if (hero.getPv() < hero.getMaxPv())
            hero.setPv(hero.getMaxPv());
        if (GM.isAbsolution)
        {
            foreach (hero enemy in GM.FM.enemiesAtStartOfCombat)
            {
                if (enemy.getIsAlive())
                {
                    takeDamage(enemy, value);
                }
            }

        }
    }

    public void takeDamage(hero hero) 
    {
        if (hero.m_isDebufArmor)
        {
            hero.m_armor /= 2;
            hero.m_isDebufArmor = false;
        }
        m_value -= hero.m_armor;
        if (m_value >= 0)
            hero.m_armor = 0;
        else
            m_value = 0;

        hero.m_Pv -= m_value * hero.m_damageMultiplier;
        hero.m_slider.value = hero.m_Pv;
        Debug.Log("Pv apres: " + hero.m_Pv + " " + hero.m_role);
        if (hero.m_Pv <= 0)
        {
            hero.isAlive = false;
        }

        hero.setVarHero();
    }

    public void takeDamage(hero hero, int value)
    {
        if (hero.m_isDebufArmor)
        {
            hero.m_armor /= 2;
            hero.m_isDebufArmor = false;
        }
        value -= hero.m_armor;
        if (value >= 0)
            hero.m_armor = 0;
        else
            value = 0;

        hero.m_Pv -= value * hero.m_damageMultiplier;
        hero.m_slider.value = hero.m_Pv;
        Debug.Log("Pv apres: " + hero.m_Pv + " " + hero.m_role);
        if (hero.m_Pv <= 0)
        {
            if(GM.FM.isCanibalisme)
            {
                Venerate(hero, 3);
                heal(hero, 3);
            }
            if(GM.FM.isProf)
            {
                foreach(hero champ in GM.FM.heroes)
                {
                    champ.m_manaMax = 1;
                    if(champ.m_mana > 1)
                    {
                        champ.m_mana = 1;
                        champ.stockText.text = champ.m_mana.ToString() + " / " + champ.m_manaMax;
                    }
                }
            }
            hero.isAlive = false;
        }

        hero.setVarHero();
    }

    public void AddArmor(hero hero)
    {
        hero.setArmor(m_value);
        if (GM.isAbsolution)
        {
            foreach (hero enemy in GM.FM.enemiesAtStartOfCombat)
            {
                if (enemy.getIsAlive())
                {
                    takeDamage(enemy, m_value);
                }
            }

        }
    }
    public void AddArmor(hero hero, int value)
    {
        hero.setArmor(value);
        if (GM.isAbsolution)
        {
            foreach (hero enemy in GM.FM.enemiesAtStartOfCombat)
            {
                if (enemy.getIsAlive())
                {
                    takeDamage(enemy, value);
                }
            }

        }
    }

    public void AddMana(int value)
    {
        GM.FM.mana += value;
    }

    public void AddCard ()
    {
        GM.deck.DrawCard();
    }

    public void AddCard(int value)
    {
        GM.deck.DrawCard(value);
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
        GM.FM.stock += value;

        GM.FM.stockText.text = GM.FM.stock.ToString();
    }

    public int Steal(hero enemy, hero ally, int value)
    {
        int surplusToReturn = 0;
        enemy.setPv(enemy.getPv() - value);
        if (enemy.getPv() <= 0)
        {
            enemy.setIsAlive(false);
        }

        ally.setPv(ally.getPv() + value);
        if (ally.getPv() > ally.getMaxPv())
        {
            surplusToReturn = ally.getPv() - ally.getMaxPv();
            ally.setPv(ally.getMaxPv());
        }
        return surplusToReturn;
    }

    public void HabemusDominum(hero enemy)
    {
        //detecter l'attaque d'un enemy
        takeDamage(enemy, 6);
    }

    public void DiabolusEst(hero enemy)
    {
        //detecter l'attaque d'un enemy

        takeDamage(enemy, 12);
    }

    public void CultiverAme(dataCard data)
    {
        throw new NotImplementedException();
    }

    public void CultiverFlamme()
    {
        throw new NotImplementedException();
    }

    public void Conversion(hero hero)
    {
        while(hero.getArmor()>0)
        {
            if (hero.getPv() == hero.getMaxPv())
                break;
            hero.setArmor(hero.getArmor() - 1);
            hero.setPv(hero.getPv() + 1);
        }
    }

    public void Absolution()
    {
        GM.isAbsolution = true;
    }

    public void Benediction(hero ally, hero enemy)
    {
        if (ally.getVenerate()>0)
        {
            enemy.takeDamage(ally.getVenerate());
            AddCard();
        }
    }

    public void Apotasie()
    {
        foreach (hero enemy in GM.FM.enemiesAtStartOfCombat)
        {
            if (enemy.getIsAlive())
            {
                takeDamage(enemy, 4);
            }
        }
        GM.FM.isApo = true;
    }

    public void Tabernacle(hero ally) //appeler cette fonction à chaque fois que le joueur prend des degats
    {
        AddArmor(ally, ally.m_dmgTaken);
        
    }

    public void Belial(hero pretre)
    {
        foreach (hero enemy in GM.FM.enemiesAtStartOfCombat)
        {
            if (enemy.getIsAlive())
            {
                enemy.setPv(enemy.getPv() - 6);
                if (enemy.getPv() <= 0)
                {
                    enemy.setIsAlive(false);
                    heal(pretre, 3);
                    Venerate(pretre, 4);
                }
            }
        }
    }
    public void VenererIdole(hero ally)
    {
        heal(ally, 3);
        Venerate(ally, 2);
    }
    public void Blaspheme(hero ally)
    {
        foreach (hero enemy in GM.FM.enemiesAtStartOfCombat)
        {
            if (enemy.getIsAlive())
            {
                Steal(enemy, ally, 5);
            }
        }

    }
    public void AllumerCierges(hero pretre)
    {
        if (pretre.getVenerate() > 0)
        {
            AddCard(pretre.getVenerate());
        }

    }
    public void IncendierCloatre()
    {
        hero enemyWithMostPV = null;
        foreach (hero enemy in GM.FM.enemiesAtStartOfCombat)
        {
            if (enemy.getIsAlive())
            {
                if ((enemyWithMostPV == null) || (enemy.getPv() > enemyWithMostPV.getPv()))
                {
                    enemyWithMostPV = enemy;
                }
            }
                
        }
        takeDamage(enemyWithMostPV, 15);
    }
    public void AccueillirNecessiteux(hero hero )
    {
        int nb = 0;
        foreach (hero enemies in GM.FM.enemiesAtStartOfCombat)
        {
            if (enemies.getIsAlive() && enemies.m_IsAttacking)
            {
                nb++;
            }
        }
        AddArmor(hero, nb);
    }
    public void MassacrerInfideles(hero ally, hero enemy)
    {
        int surplus = Steal(enemy, ally,7);
        takeDamage(enemy, surplus * 3);

    }
    public void MoxLion(hero ally)
    {
        if (GM.FM.mana > 0)
        {
            AddArmor(ally, GM.FM.mana * 2);
            GM.FM.mana = 0;
            GM.isManaMultiplier = true;
        }

    }

    public void MoxAraignee(hero ally, hero enemy)
    {
        if (GM.FM.mana > 0)
        {
            AddArmor(ally, GM.FM.mana * 2);
            takeDamage(enemy, GM.FM.mana * 2);
            GM.FM.mana = 0;
            GM.isManaMultiplier = true;

        }
    }

    public void MurDeRonces(hero hero)//ajouté le poison
    {
        int nb = 0;
        CardEffect card = new CardEffect();
        card.nbTour = 2;
        card.effects = CardType.Poison;
        card.values = 2;
        foreach (hero enemies in GM.FM.enemiesAtStartOfCombat)
        {
            if (enemies.getIsAlive() && enemies.m_IsAttacking)
            {
                nb++;
                enemies.MyEffects.Add(card);
            }
        }
        AddArmor(hero, nb * 6);

    }

    public void LaissePourMort()//ajouté le poison
    {
        CardEffect card = new CardEffect();
        card.nbTour = 2;
        card.effects = CardType.Poison;
        card.values = 6;
        foreach (hero enemies in GM.FM.enemiesAtStartOfCombat)
        {
            if (enemies.getIsAlive() && enemies.m_IsAttacking)
            {

                enemies.MyEffects.Add(card);

            }
        }

    }

    public void Cataplasme(hero hero)
    {
        heal(hero,2);
        GM.debuffDraw = 0;
        hero.m_isDebufArmor = false;
        hero.m_damageMultiplier = 1;
        hero.isAntiHeal = false;
    }

    public void Belladone(hero enemy)
    {
        if (enemy.getArmor() > 0)
        {
            enemy.setArmor(0);
        }
        takeDamage(enemy, 8);

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
        CardEffect card = new CardEffect();
        card.nbTour = 2;
        card.effects = CardType.Poison;
        card.values = 5;
        for (int i = 0; i < GM.FM.nbTransfo; i++)
        {
            foreach (hero enemies in GM.FM.enemiesAtStartOfCombat)
            {
                if (enemies.getIsAlive())
                {
                    enemies.MyEffects.Add(card);
                }
            }
        }
        
        

    }

    public void ArmureEcorse(hero ally)
    {
        AddArmor(ally, 7);
        ally.m_nextArmor = 7;
    }

    public void MaleusHerbeticae(hero ally)//poison
    {
        CardEffect card = new CardEffect();
        card.nbTour = 2;
        card.effects = CardType.Poison;
        card.values = 4;
        foreach (hero enemy in GM.FM.enemiesAtStartOfCombat)
        {
            if (enemy.getIsAlive())
            {
                enemy.MyEffects.Add(card);
            }
        }
        AddArmor(ally, 7);
    }

    public void CommunionNature(hero hero)
    {
        Venerate(hero, 3);

    }

    public void Canibalisme()
    {
        GM.FM.isCanibalisme = true;

    }
    public void SuivreEtoiles(hero ally)
    {
        Venerate(ally, 2);
        foreach (hero hero in GM.FM.heroes)
        {
           if (hero.getMana() == hero.m_manaMax)
           {
                AddCard(2);
                return;

           }
        }



    }
    public void ProfanerCiel()
    {
        GM.FM.isProf = true;

    }
    public void DormirPresDeLautre(hero ally, hero arboriste)
    {
        Venerate(arboriste, 4);
        heal(ally,4);
        AddArmor(ally,4);

    }
    public void ReveillerPourManger(hero enemy, hero ally)
    {
        enemy.setPv(enemy.getPv() - 6);
        if (enemy.getPv() <= 0)
        {
            enemy.setIsAlive(false);
            heal(ally,6);
            Venerate(ally,6);
        }
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






