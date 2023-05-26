using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static OneLine.Example.SlicesTest;


public class Fight : MonoBehaviour
{
    [SerializeField] GameManager Gm;
    [SerializeField] Deck deck;
    
    private bool isCardSend;
    private bool m_canPlayEnemyTurn;
    private bool endturnbool = false;

    public Sprite heroSprite;
    public Sprite heroSprite2;
    public Sprite ennemy1Sprite;
    public Sprite ennemy2Sprite;
    public bool perso1 = false;
    public bool perso2 = false;
    public Image image;
    bool test = false;



    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
    }

    private entityManager entityManager;

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

    private void Update()
    {
        if(test == false)
        {
            if (SceneManager.GetActiveScene().name == "TestSceneSacha2")
            {
                StartFight();

                test = true;
            }
        }
        
    }

    private void Start()
    {
        Gm = GameManager.Instance;
        entityManager = Gm.entityManager;
        heroes = new List<hero>();
        enemies = new List<hero>();
        //StartFight();
        //StartTurn();

    }
    public void StartFight()
    {
        GameObject.Find("enemy1").GetComponent<Image>().sprite = ennemy1Sprite;
        GameObject.Find("enemy2").GetComponent<Image>().sprite = ennemy2Sprite;
        hero H1;
        hero H2;
        if (perso1 && perso2)
        {
            GameObject.Find("champ").GetComponent<Image>().sprite = heroSprite;
            GameObject.Find("champ2").GetComponent<Image>().sprite = heroSprite2;
            GameObject.Find("champSolo").SetActive(false);
            int countArbo = 0;
            int countPretre = 0;
            foreach (hero camp in entityManager.getListHero())
            {
                if (camp.m_role == entityManager.Role.Pretre)
                    countPretre++;
                if (camp.m_role == entityManager.Role.Arboriste)
                    countArbo++;
            }

            if (countPretre == 0)
                H1 = new hero(entityManager.Role.Pretre, 10, 10, 0, 0, null, 0);

            if (countArbo == 0)
                H2 = new hero(entityManager.Role.Arboriste, 10, 10, 0, 0, null, 0);
        }
        else if(perso1)
        {
            GameObject.Find("champ2").SetActive(false);
            GameObject.Find("champ").SetActive(false);
            GameObject.Find("champSolo").GetComponent<Image>().sprite = heroSprite;
            int count = 0;
            foreach (hero camp in entityManager.getListHero())
            {
                if (camp.m_role == entityManager.Role.Arboriste)
                    count++;
            }
            if (count == 0)
                H1 = new hero(entityManager.Role.Arboriste, 10, 10, 0, 0, null, 0);
            


        }
        else if(perso2)
        {
            GameObject.Find("champ2").SetActive(false);
            GameObject.Find("champ").SetActive(false);
            GameObject.Find("champSolo").GetComponent<Image>().sprite = heroSprite2;
            int count = 0;
            foreach (hero camp in entityManager.getListHero())
            {
                if (camp.m_role == entityManager.Role.Pretre)
                    count++;
            }
            if (count == 0)
                H1 = new hero(entityManager.Role.Pretre, 10, 10, 0, 0, null, 0);
        }

        //goEn1.AddComponent<>();



        hero en1 = new hero(entityManager.Role.Squellettes, 10, 10, 0, 0, null, 0);
        
        hero En2 = new hero(entityManager.Role.Squellettes, 10, 10, 0, 0, null, 0);

    }
    void StartTurn()
    {
        endturnbool = false;
        deck.StartTurn();
        heroes.Clear();
        enemies.Clear();

        foreach (hero E in entityManager.getListHero())
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
                    case (dataCard.CardType)2:
                        foreach (hero hero in selected)
                        {
                            card.takeDamage(hero);
                        }
                        break;
                    case (dataCard.CardType)1:
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
