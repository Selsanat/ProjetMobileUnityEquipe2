using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.EventSystems;

public class Fight : MonoBehaviour
{
    [SerializeField] GameManager Gm;
    [SerializeField] Deck deck;

    private bool m_caca;
    private bool m_canPlayEnemyTurn;
    private bool endturnbool = false;


    private entityManager manager;

    private int mana;


    public List<hero> entities;
    List<hero> heroes;
    List<hero> enemies;

    List<hero> selectedhero;
    dataCard selectedcard;

    Coroutine coroutine;

    public bool Caca { get => m_caca; set => m_caca = value; }

    private void Start()
    {
        Gm = FindObjectOfType<GameManager>();
        heroes = new List<hero>();
        enemies = new List<hero>();
        StartTurn();

    }
    void StartTurn()
    {
        endturnbool = false;
        deck.StartTurn();
        heroes.Clear();
        enemies.Clear();

        foreach(hero E in entities)
        {
            if(E.m_role != 0)
            {
                heroes.Add(E);
            }
            else
            {
                enemies.Add(E);
            }
        }
        coroutine = StartCoroutine(turnwait());
    }

    [Button]
    public void leandrogo()
    {
        selectedcard = Gm.CarteUtilisee.DataCard;
        selectedhero = Gm.CarteUtilisee.HeroToAttack;
        m_caca = true;
    }

    public IEnumerator turnwait()
    {
        while (!endturnbool)
        {
            yield return new WaitUntil(() => m_caca);
            playCard(selectedcard, selectedhero);
            m_caca = false;
            if (!CheckifEnemyAreAlive())
            {
                WinFight();
            }
        }

        PlayEnemyTurn();

    }
    [Button]
    void EndButton()
    {
        endturnbool = !endturnbool;
    }
    /*    [Button]
        void Turn()
        {
            //Pioche4
            //FIND WAY TO WAIT
            //EndHeroTurn();
        }*/

    /*    [Button]
        void EndHeroTurn()
        {
            deck.EndTurn();
            foreach (CardObject card in deck.PlayedCards)
            {
                if(card.HeroToAttack == null) { card.HeroToAttack.Add(FindObjectOfType<hero>()); }
                playCard(card.DataCard,card.HeroToAttack);
            }
            if (CheckifEnemyAreAlive())
            {
                PlayEnemyTurn();
            }
            else
            {
                WinFight();
            }
        }*/
    private void PlayEnemyTurn()
    {
        StopCoroutine(coroutine);
        foreach (hero En in enemies)
        {
            En.EnemyAttack(heroes);
            if (!CheckifHeroAreAlive())
            {
                LooseFight();
            }
            else
            {
                StartTurn();
            }
        }
    }

    private void LooseFight()
    {
        StopCoroutine(coroutine);
        throw new NotImplementedException();
    }
    private void WinFight()
    {
        StopCoroutine(coroutine);
        Debug.Log("WIIIIIIIIIIIIIIIIIIIIIIIIIIIIN");
    }

    bool CheckifHeroAreAlive()//TRUE = min ONE ALIVE
    {
        foreach (hero En in heroes)
        {
            if (En.getIsAlive())
            {
                return true;
            }
        }
        return false;
    }


    bool CheckifEnemyAreAlive()//TRUE = min ONE ALIVE
    {
        foreach(hero En in enemies)
        {
            if (En.getIsAlive())
            {
                return true;
            } 
        }
        return false;
    }
    void playCard(dataCard card, List<hero> selected)
    {

        switch (card.CardType)
        {
            case 0:
                Debug.LogError("CARTE TYPE UNDIFINED");
                break;
            case (dataCard.cardType)1:
                foreach(hero hero in selected)
                {
                    card.takeDamage(hero);
                }
                break;
            case (dataCard.cardType)2:
                foreach (hero hero in selected)
                {
                    card.heal(hero);
                }
                break;
            case (dataCard.cardType)3:
                foreach (hero hero in selected)
                {
                    card.BuffDamage(hero);
                }
                break;
        }
    }

   /* public enum cardType
    {
        undifined = 0,
        Damage = 1,
        Heal = 2,
        BuffDamage = 3,
        BuffHeal = 4,
        Block = 5
    }*/
}
