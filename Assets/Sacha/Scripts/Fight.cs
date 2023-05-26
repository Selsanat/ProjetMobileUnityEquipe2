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
    private Deck deck;

    private bool isCardSend;
    private bool m_canPlayEnemyTurn;
    private bool endturnbool = false;


    private entityManager manager;

    private int mana;


    public List<hero> entities;
    List<hero> heroes;
    List<hero> enemies;

    List<dataCard.CardEffect> heroesEffects;
    List<dataCard.CardEffect> enemyEffects;


    List<hero> selectedhero;
    dataCard selectedcard;

    Coroutine coroutine;

    public bool IsCardSend { get => isCardSend; set => isCardSend = value; }

    private void Start()
    {
        Gm = GameManager.Instance;
        deck = FindObjectOfType<Deck>();
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

        foreach (hero E in entities)
        {
            if (E.m_role != 0)
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
    public void Cardsend(CardObject card)
    {
        selectedcard = card.DataCard;
        selectedhero = card.heroToAttack;
        isCardSend = true;
    }

    public IEnumerator turnwait()
    {
        while (!endturnbool)
        {
            yield return new WaitUntil(() => isCardSend);
            playCard(selectedcard, selectedhero);
            isCardSend = false;
            if (!CheckifEnemyAreAlive())
            {
                WinFight();
            }
        }
        PlayPlayerEffects();
        PlayEnemyTurn();

    }

    void PlayPlayerEffects()
    {
        for(int i = 0; i < heroesEffects.Count; i++)
        {
            dataCard.CardEffect E = heroesEffects[i];
            if (E.nbTour != 0)
            {
                E.nbTour--;
                if(E.nbTour == 0)
                {
                    heroesEffects.Remove(E);
                }
            }
            foreach (dataCard.CardType turnEffect in E.effects)
            {
                switch (turnEffect)
                {
                    //BRUH LA MEME CHOSE PUTAIN
                }
            }

            if (!CheckifHeroAreAlive())
            {
                LooseFight();
            }
        }
    }
        private void PlayEnemyEffects()
        {
            throw new NotImplementedException();
        }

        [Button]
        void EndButton()
        {
            endturnbool = !endturnbool;
        }

        private void PlayEnemyTurn()
        {
            Debug.Log("Ennemyturn");
            StopCoroutine(coroutine);
            foreach (hero En in enemies)
            {
                En.EnemyAttack(heroes, enemies);
                if (!CheckifHeroAreAlive())
                {
                    LooseFight();
                }
                else
                {
                    StartTurn();
                }
            }
            PlayEnemyEffects();
        }


        private void LooseFight()
        {
            Debug.Log("Loosedfight");
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
            foreach (hero En in enemies)
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
            foreach (dataCard.CardType cardT in card.CardTypes)
            {
                switch (cardT)
                {
                    case 0:
                        Debug.LogError("CARTE TYPE UNDIFINED");
                        break;
                    case (dataCard.CardType)1:
                        foreach (hero hero in selected)
                        {
                            card.takeDamage(hero);
                        }
                        break;
                    case (dataCard.CardType)2:
                        foreach (hero hero in selected)
                        {
                            card.heal(hero);
                        }
                        break;
                    case (dataCard.CardType)3:
                        foreach (hero hero in selected)
                        {
                            card.BuffDamage(hero);
                        }
                        break;
                    case (dataCard.CardType)4:
                        foreach (hero hero in selected)
                        {

                        }
                        break;
                    case (dataCard.CardType)5:
                        foreach (hero hero in selected)
                        {

                        }
                        break;
                    case (dataCard.CardType)6:
                        foreach (hero hero in selected)
                        {

                        }
                        break;
                    case (dataCard.CardType)7:
                        foreach (hero hero in selected)
                        {

                        }
                        break;
                }
            }

        }

        /* public enum CardType
         {
             undifined = 0,
             Damage = 1,
             Heal = 2,
             BuffDamage = 3,
             BuffHeal = 4,
             Block = 5
         }*/



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
}
