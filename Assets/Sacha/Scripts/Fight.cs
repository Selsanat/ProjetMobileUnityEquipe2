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
using Unity.VisualScripting;

public class Fight : MonoBehaviour
{
    [SerializeField] GameManager Gm;
    
    private bool isCardSend;
    private bool m_canPlayEnemyTurn;
    private bool endturnbool = false;

    public Sprite heroSprite;
    public Sprite heroSprite2;
    public Sprite ennemy1Sprite;
    public Sprite ennemy2Sprite;
    public bool perso1 = false;
    public bool perso2 = false;
    bool test = false;
    [SerializeField] public Button play;
    [SerializeField] Button arboristeButton;
    [SerializeField] Button pretreButton;
    [SerializeField] Button ennemisButton1;
    [SerializeField] Button ennemisButton2;
    [SerializeField] Button selectedButton;



    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
    }


    private int mana;



    [SerializeField] List<hero> heroes;
    [SerializeField] List<hero> enemies;

    List<dataCard.CardEffect> heroesEffects;
    List<dataCard.CardEffect> enemyEffects;


    [SerializeField] List<hero> selectedhero;
    dataCard selectedcard;

    Coroutine coroutine;

    public bool IsCardSend { get => isCardSend; set => isCardSend = value; }

    private void Update()
    {
        if(test == false)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                test = true;
                StartFight();
            }
        }
        
    }

    private void Start()
    {
        Gm = GameManager.Instance;
        heroes = new List<hero>();
        enemies = new List<hero>();
        //StartFight();
        //StartTurn();

    }
    public void StartFight()
    {
        GameObject temp = GameObject.Find("enemy1");
        temp.GetComponent<Image>().sprite = ennemy1Sprite;
        ennemisButton1 = temp.GetComponent<Button>();
        temp = GameObject.Find("enemy2");
        temp.GetComponent<Image>().sprite = ennemy2Sprite;
        ennemisButton2 = temp.GetComponent<Button>();
        hero H1;
        hero H2;
        if (perso1 && perso2)
        {
            temp = GameObject.Find("champ");
            temp.GetComponent<Image>().sprite = heroSprite;
            arboristeButton = temp.GetComponent<Button>();
            
            temp = GameObject.Find("champ2");
            temp.GetComponent<Image>().sprite = heroSprite2;
            pretreButton = temp.GetComponent<Button>();
            GameObject.Find("champSolo").SetActive(false);
            int countArbo = 0;
            int countPretre = 0;
            foreach (hero camp in Gm.entityManager.getListHero())
            {
                if (camp.m_role == entityManager.Role.Pretre)
                    countPretre++;
                if (camp.m_role == entityManager.Role.Arboriste)
                    countArbo++;
            }

            if (countPretre == 0)
                H1 = new hero(entityManager.Role.Pretre, 50, 50, 0, 0, null, 0);

            if (countArbo == 0)
                H2 = new hero(entityManager.Role.Arboriste, 50, 50, 0, 0, null, 0);
        }
        else if(perso1)
        {
            GameObject.Find("champ2").SetActive(false);
            GameObject.Find("champ").SetActive(false);
            temp = GameObject.Find("champSolo");
            temp.GetComponent<Image>().sprite = heroSprite;
            arboristeButton = temp.GetComponent<Button>();
            int count = 0;
            foreach (hero camp in Gm.entityManager.getListHero())
            {
                if (camp.m_role == entityManager.Role.Arboriste)
                    count++;
            }
            if (count == 0)
                H1 = new hero(entityManager.Role.Arboriste, 50, 50, 0, 0, null, 0);
            else
            {

            }
            


        }
        else if(perso2)
        {
            GameObject.Find("champ2").SetActive(false);
            GameObject.Find("champ").SetActive(false);
            temp = GameObject.Find("champSolo");
            temp.GetComponent<Image>().sprite = heroSprite2;
            pretreButton = temp.GetComponent<Button>();
            int count = 0;
            foreach (hero camp in Gm.entityManager.getListHero())
            {
                if (camp.m_role == entityManager.Role.Pretre)
                    count++;
            }
            if (count == 0)
                H1 = new hero(entityManager.Role.Pretre, 50, 50, 0, 0, null, 0);
        }

        //goEn1.AddComponent<>();



        hero en1 = new hero(entityManager.Role.Squellettes, 50, 50, 0, 0, null, 0);
        
        hero En2 = new hero(entityManager.Role.Squellettes, 50, 50, 0, 0, null, 0);

        
        StartTurn();

    }
    void StartTurn()
    {
        endturnbool = false;
        Gm.deck.StartTurn();
        heroes.Clear();
        enemies.Clear();

        foreach (hero E in Gm.entityManager.getListHero())
        {
            if (E.m_role == entityManager.Role.Pretre || E.m_role == entityManager.Role.Arboriste)
            {
                heroes.Add(E);
            }
            else
            {
                enemies.Add(E);
            }
        }
        ennemisButton1.onClick.AddListener(() => { selectedhero.Clear(); selectedhero.Add(enemies[0]); });
        //ennemisButton1.OnDeselect(clearCardSelected());
        ennemisButton2.onClick.AddListener(() => { selectedhero.Clear(); selectedhero.Add(enemies[1]); });

        arboristeButton?.onClick.AddListener(() => { selectedhero.Clear(); selectedhero.Add(heroes[0]); });
        pretreButton?.onClick.AddListener(() => { selectedhero.Clear(); selectedhero.Add(heroes[1]); }); 
        coroutine = StartCoroutine(turnwait());
    }

    [Button]
    public void Cardsend(CardObject card, int index)
    {
        Debug.Log("card send");
        card.DataCard.m_index = card.indexHand;
        selectedcard = card.DataCard;
        if(play == null)
        {
            play = GameObject.Find("Play").GetComponent<Button>();
        }
        play.onClick.AddListener(() => { if(selectedhero != null) isCardSend = true; });
    }

    public IEnumerator turnwait()
    {
        while (!endturnbool)
        {
            yield return new WaitUntil(() => isCardSend);

            playCard(selectedcard, selectedhero);
            Gm.deck.PlayCard(selectedcard.m_index);
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
        

        SceneManager.LoadScene(1);
       /* 
        Gm.Hand.Clear();
        foreach (hero E in Gm.entityManager.getListHero())
        {
            if (E.m_role != entityManager.Role.Pretre || E.m_role != entityManager.Role.Arboriste)
            {
                Gm.entityManager.heroList.Remove(E);
            }
        }
        ennemisButton1.onClick.RemoveAllListeners();
        ennemisButton2.onClick.RemoveAllListeners();
        arboristeButton?.onClick.RemoveAllListeners();
        pretreButton?.onClick.RemoveAllListeners();
        ennemisButton1 = null;
        ennemisButton2 = null;
        arboristeButton = null;
        pretreButton = null;

        heroes.Clear();
        enemies.Clear();
        selectedhero.Clear();
        selectedcard = null;
        test = false;*/
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
            else
            {
                Gm.entityManager.heroList.Remove(En);
                selectedhero.Remove(En);
                
            }
                
        }
        return false;
    }
    void playCard(dataCard card, List<hero> selected)
    {
       
        foreach (dataCard.CardType cardT in card.CardTypes)
        {
            Debug.Log("card type" + cardT);
            Debug.Log("enemy count" + selected.Count);
            switch (cardT)
            {
                case dataCard.CardType.Damage:
                    foreach (hero hero in selected)
                    {
                        Debug.Log("card damage");
                        card.takeDamage(hero);
                    }
                    break;
                case dataCard.CardType.Heal:
                    foreach (hero hero in selected)
                    {
                        card.heal(hero);
                    }
                    break;
                case dataCard.CardType.GainMana:
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
    public void clearCardSelected()
    {
        selectedhero.Clear();
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (eventData.selectedObject == ennemisButton1)
        {
            selectedButton = ennemisButton1;
            selectedhero.Clear();
            selectedhero.Add(enemies[0]);
        }
        else if (eventData.selectedObject == ennemisButton2)
        {
            selectedhero.Clear();
            selectedhero.Add(enemies[1]);
            selectedButton = ennemisButton2;
        }
        else if(eventData.selectedObject == arboristeButton)
        {
            selectedhero.Clear();
            selectedhero.Add(heroes[0]);
            selectedButton = arboristeButton;
        }
        else if(eventData.selectedObject == pretreButton)
        {
            selectedhero.Clear();
            selectedhero.Add(heroes[1]);
            selectedButton = pretreButton;
        }
        else
        {
            selectedButton = null;
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
