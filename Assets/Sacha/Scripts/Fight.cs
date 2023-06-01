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
using UnityEngine.Rendering.Universal;

public class Fight : MonoBehaviour
{
    [SerializeField] GameManager Gm;
    
    private bool isCardSend;
    private bool m_canPlayEnemyTurn;
    private bool endturnbool = false;
    private List<Light2D> lightsAllies = new List<Light2D>();
    private List<Light2D> lightsEnnemies = new List<Light2D>();
    private List<Light2D> lights = new List<Light2D>();

    public Sprite heroSprite;
    public Sprite heroSprite2;
    public Sprite ennemy1Sprite;
    public Sprite ennemy2Sprite;
    public bool perso1 = false;
    public bool perso2 = false;
    bool test = false;
    private bool prout;
    [SerializeField] public Button play;
    [SerializeField] public Button cancel;
    [SerializeField] Button arboristeButton;
    [SerializeField] Button pretreButton;
    [SerializeField] Button ennemisButton1;
    [SerializeField] Button ennemisButton2;
    [SerializeField] Button ennemisButton3;
    [SerializeField] Button selectedButton;

    private int mana;
    [SerializeField] List<hero> heroes;
    [SerializeField] List<hero> enemies;
    [SerializeField] List<hero> selectedhero;
    dataCard selectedcard;

    Coroutine coroutine;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        
    }

    public bool IsCardSend { get => isCardSend; set => isCardSend = value; }

    private void Update()
    {
        if (test == false)
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                test = true;
                StartFight();
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (!Gm.isHoverButton)
            {
                if (selectedcard != null)
                {
                    Deselection(false);
                }

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
    #region MerdeLeandro
    void ChangerBouttonEnGameObject(Button ComponentBouton, Sprite SpritreUtilise, bool sideTrueIsAllies)
    {
        GameObject perso = new GameObject();
        perso.transform.position = Camera.main.ScreenToWorldPoint(ComponentBouton.transform.position);
        SpriteRenderer SpritePerso = perso.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        SpritePerso.sprite = SpritreUtilise;
        perso.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        perso.transform.parent = ComponentBouton.transform;
        Light2D lumiere = perso.AddComponent(typeof(Light2D)) as Light2D;
        lumiere.enabled = false;
        
        if (ComponentBouton == ennemisButton1 || ComponentBouton == ennemisButton2)
        {
            lightsEnnemies.Add(lumiere);
            lumiere.color = Color.red;
            return;
        }
        else
        {
            lumiere.color = Color.green;
        }
        lightsAllies.Add(lumiere);
        
    }

    public void CancelCard()
    {
        ClearSide(true);
        ClearSide(false);
        ennemisButton1?.onClick.RemoveAllListeners();
        ennemisButton2?.onClick.RemoveAllListeners();
        ennemisButton3?.onClick.RemoveAllListeners();
        arboristeButton?.onClick.RemoveAllListeners();
        pretreButton?.onClick.RemoveAllListeners();
        Deselection(true);
    }
    void Deselection(bool ForceDeselec)
    {
        if (!selectedcard.AOEAllies || ForceDeselec)
        {
            foreach (Light2D light in lightsAllies)
            {
                light.enabled = false;
            }
            ClearSide(true);
        }
        if (!selectedcard.AOEEnnemies || ForceDeselec)
        {
            foreach (Light2D light in lightsEnnemies)
            {
                light.enabled = false;
            }
            ClearSide(false);
        }

    }
    void ClearSide(bool sideTrueIsAllies)
    {
        List<hero> listeretour = new List<hero>();
        foreach(hero hero in selectedhero)
        {
            if (hero.m_role == entityManager.Role.Pretre || hero.m_role == entityManager.Role.Arboriste)
            {
                if (!sideTrueIsAllies)
                {
                    listeretour.Add(hero);
                }
            }
            else
            {
                if (sideTrueIsAllies)
                {
                    listeretour.Add(hero);
                }
            }
        }
        selectedhero.Clear();
        selectedhero = listeretour;
    }
    public void ActivateSideLights(bool sideTrueIsAllies)
    {
        if (sideTrueIsAllies){
            foreach (Light2D light in lightsAllies)
            {
                light.enabled = true;
            }
        }
        else
        {
            foreach (Light2D light in lightsEnnemies)
            {
                light.enabled = true;
            }
        }

    }
    void switchLightSelection(Button Boutton)
    {
        Light2D lightDuBoutton = Boutton.gameObject.transform.GetChild(1).gameObject.GetComponent<Light2D>();
        lightDuBoutton.enabled = true;
    }

    public IEnumerator CardAnimDisolve()
    {
        play.gameObject.SetActive(false);
        cancel.gameObject.SetActive(false);
        DissolveController dissolveController = Gm.CarteUtilisee.GetComponent<DissolveController>();
        
        dissolveController.isDissolving = true;
        yield return new WaitUntil(() => dissolveController.dissolveAmount < 0);
        dissolveController.isDissolving = false;
        dissolveController.dissolveAmount = 1;
        isCardSend = true;
    }
    #endregion
    public void StartFight()
    {

        #region Set Up des personnages
        GameObject temp;

        hero H1;
        hero H2;
        if (perso1 && perso2)
        {

            if (Gm.IsPretrePlayed == false)
            {
                H1 = new hero(entityManager.Role.Pretre, 50, 50, 0, 0, null, 0);
                Gm.LifePretre = H1.getPv();
                Gm.IsPretrePlayed = true;
            }
            else
                H1 = new hero(entityManager.Role.Pretre, 50, Gm.LifePretre, 0, 0, null, 0, Gm.levelPretre, Gm.expPretre);
                
            if (Gm.IsArboristePlayed == false)
            {
                H2 = new hero(entityManager.Role.Arboriste, 50, Gm.LifeArboriste, 0, 0, null, 0);
                Gm.LifeArboriste = H2.getPv();
                Gm.IsArboristePlayed = true;
            }
            else
                H2 = new hero(entityManager.Role.Pretre, 50, Gm.LifeArboriste, 0, 0, null, 0, Gm.levelArboriste, Gm.expArboriste);

            temp = GameObject.Find("champ");
            temp.GetComponent<Image>().sprite = heroSprite;
            arboristeButton = temp.GetComponent<Button>();
            ChangerBouttonEnGameObject(arboristeButton, heroSprite, true);
            H2.m_slider = temp.GetComponentInChildren<Slider>();
            H2.m_slider.maxValue = H2.getMaxPv();
            H2.m_slider.value = H2.getPv();

            temp = GameObject.Find("champ2");
            temp.GetComponent<Image>().sprite = heroSprite2;
            pretreButton = temp.GetComponent<Button>();
            GameObject.Find("champSolo").SetActive(false);
            ChangerBouttonEnGameObject(pretreButton, heroSprite2, true);
            H1.m_slider = temp.GetComponentInChildren<Slider>();
            H1.m_slider.maxValue = H1.getMaxPv();
            H1.m_slider.value = H1.getPv();
        }
        else if(perso1)
        {
            GameObject.Find("champ2").SetActive(false);
            GameObject.Find("champ").SetActive(false);
            temp = GameObject.Find("champSolo");
            temp.GetComponent<Image>().sprite = heroSprite;
            
            arboristeButton = temp.GetComponent<Button>();
            ChangerBouttonEnGameObject(arboristeButton, heroSprite, true);
            if (Gm.IsArboristePlayed == false)
            {
                H2 = new hero(entityManager.Role.Arboriste, 50, 50, 0, 0, null, 0);
                Gm.LifeArboriste = H2.getPv();
                Gm.IsArboristePlayed = true;
                Debug.Log("creer arboriste");
            }
            else
                H2 = new hero(entityManager.Role.Pretre, 50, Gm.LifeArboriste, 0, 0, null, 0, Gm.levelArboriste, Gm.expArboriste);


            H2.m_slider = temp.GetComponentInChildren<Slider>();
            H2.m_slider.maxValue = H2.getMaxPv();
            H2.m_slider.value = H2.getPv();


        }
        else if(perso2)
        {
            GameObject.Find("champ2").SetActive(false);
            GameObject.Find("champ").SetActive(false);
            temp = GameObject.Find("champSolo");
            temp.GetComponent<Image>().sprite = heroSprite2;
            pretreButton = temp.GetComponent<Button>();
            ChangerBouttonEnGameObject(pretreButton, heroSprite2, true);
            if (Gm.IsPretrePlayed == false)
            {
                H1 = new hero(entityManager.Role.Pretre, 50, 50, 0, 0, null, 0);
                Gm.LifePretre = H1.getPv();
                Gm.IsPretrePlayed = true;
            }
            else
                H1 = new hero(entityManager.Role.Pretre, 50, Gm.LifePretre, 0, 0, null, 0, Gm.levelPretre, Gm.expPretre);

            H1.m_slider = temp.GetComponentInChildren<Slider>();
            H1.m_slider.maxValue = H1.getMaxPv();
            H1.m_slider.value = H1.getPv();
        }
        #endregion

        #region choix de la wave
        int waveType = UnityEngine.Random.Range(0, Gm.allWave[Gm.waveCounter].Count -1);
        Debug.Log("waveType : " + waveType);
        hero en1;
        hero En2;
        hero En3;

        

        if(Gm.allWave[Gm.waveCounter][waveType].Count == 1)
        {
            en1 = Gm.allWave[Gm.waveCounter][waveType][0];
            Gm.entityManager.heroList.Add(en1);
            temp = GameObject.Find("enemy1");
            temp.GetComponent<Image>().sprite = ennemy1Sprite;
            ennemisButton1 = temp.GetComponent<Button>();
            en1.m_slider = temp.GetComponentInChildren<Slider>();
            en1.m_slider.maxValue = en1.getMaxPv();
            en1.m_slider.value = en1.getPv();
            Debug.Log(en1.getPv());
            ChangerBouttonEnGameObject(ennemisButton1, en1.m_sprite, false);
            GameObject.Find("enemy2").SetActive(false);
            GameObject.Find("enemy3").SetActive(false);
        }
        else if (Gm.allWave[Gm.waveCounter][waveType].Count == 2)
        {
            en1 = Gm.allWave[Gm.waveCounter][waveType][0];
            Gm.entityManager.heroList.Add(en1);
            temp = GameObject.Find("enemy1");
            temp.GetComponent<Image>().sprite = ennemy1Sprite;
            ennemisButton1 = temp.GetComponent<Button>();
            en1.m_slider = temp.GetComponentInChildren<Slider>();
            en1.m_slider.maxValue = en1.getMaxPv();
            en1.m_slider.value = en1.getPv();
            ChangerBouttonEnGameObject(ennemisButton1, en1.m_sprite, false);
            Debug.Log(en1.getPv());
            En2 = Gm.allWave[Gm.waveCounter][waveType][1];
            Gm.entityManager.heroList.Add(En2);
            temp = GameObject.Find("enemy2");
            temp.GetComponent<Image>().sprite = ennemy2Sprite;
            ennemisButton2 = temp.GetComponent<Button>();
            En2.m_slider = temp.GetComponentInChildren<Slider>();
            En2.m_slider.maxValue = En2.getMaxPv();
            En2.m_slider.value = En2.getPv();
            ChangerBouttonEnGameObject(ennemisButton2, En2.m_sprite, false);
            GameObject.Find("enemy3").SetActive(false);

        }
        else if (Gm.allWave[Gm.waveCounter][waveType].Count == 3)
        {
            en1 = Gm.allWave[Gm.waveCounter][waveType][0];
            Gm.entityManager.heroList.Add(en1);
            temp = GameObject.Find("enemy1");
            temp.GetComponent<Image>().sprite = ennemy1Sprite;
            ennemisButton1 = temp.GetComponent<Button>();
            en1.m_slider = temp.GetComponentInChildren<Slider>();
            en1.m_slider.maxValue = en1.getMaxPv();
            en1.m_slider.value = en1.getPv();
            ChangerBouttonEnGameObject(ennemisButton1, en1.m_sprite, false);
            Debug.Log(en1.getPv());
            En2 = Gm.allWave[Gm.waveCounter][waveType][1];
            Gm.entityManager.heroList.Add(En2);
            temp = GameObject.Find("enemy2");
            temp.GetComponent<Image>().sprite = ennemy2Sprite;
            ennemisButton2 = temp.GetComponent<Button>();
            En2.m_slider = temp.GetComponentInChildren<Slider>();
            En2.m_slider.maxValue = En2.getMaxPv();
            En2.m_slider.value = En2.getPv();
            ChangerBouttonEnGameObject(ennemisButton2, En2.m_sprite, false);

            En3 = Gm.allWave[Gm.waveCounter][waveType][2];
            Gm.entityManager.heroList.Add(En3);
            temp = GameObject.Find("enemy3");
            temp.GetComponent<Image>().sprite = ennemy2Sprite;
            ennemisButton3 = temp.GetComponent<Button>();
            En3.m_slider = temp.GetComponentInChildren<Slider>();
            En3.m_slider.maxValue = En3.getMaxPv();
            En3.m_slider.value = En3.getPv();
            ChangerBouttonEnGameObject(ennemisButton3, En3.m_sprite, false);
        }




        #endregion


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
    }




    public void Cardsend(CardObject card, int index)
    {
        card.DataCard.m_index = card.indexHand;
        selectedcard = card.DataCard;
/*        if(play == null)
        {
            play = GameObject.Find("Play").GetComponent<Button>();
        }
        if (cancel == null)
        {
            print("testAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            cancel = GameObject.Find("Cancel").GetComponent<Button>();
        }*/

        //condition a voir en fonction des besoins
        //True si : La carte n'est pas null et qu'elle a une cible. Si elle n'en a pas, elle se lance si C'est une carte D'AOE Alliée qui cible pas d'ennemies, ou inversement.
        //[WIP]je dois le changer[WIP]
        bool conditionjouer = Gm.CarteUtilisee != null || selectedhero != null || ((selectedcard.AOEAllies && !selectedcard.TargetEnnemies) || (selectedcard.AOEEnnemies && !selectedcard.TargetAllies));
        play.onClick.AddListener(() => { if(conditionjouer) StartCoroutine(CardAnimDisolve());});
        //[WIP]je dois le changer[WIP]

        if (!selectedcard.AOEEnnemies && selectedcard.TargetEnnemies)
        {
            ennemisButton1?.onClick.AddListener(() => 
            {
                Deselection(false);
                if (Gm.IsAnyProv) 
                { 
                    if(enemies[0].getIsProvocation()) 
                    { 
                        selectedhero.Add(enemies[0]); 
                        switchLightSelection(ennemisButton1); 
                    } 
                } 
                else 
                { 
                    selectedhero.Add(enemies[0]); 
                    switchLightSelection(ennemisButton1);
                }
            });


            //ennemisButton1.OnDeselect(clearCardSelected());


            ennemisButton2?.onClick.AddListener(() => 
            { 
                ClearSide(false);
                if (Gm.IsAnyProv) 
                { 
                    if (enemies[1].getIsProvocation()) 
                    { 
                        selectedhero.Add(enemies[1]); 
                        switchLightSelection(ennemisButton2); 
                    } 
                } 
                else 
                { 
                    selectedhero.Add(enemies[1]); 
                    switchLightSelection(ennemisButton2); 
                }
            });
            ennemisButton3?.onClick.AddListener(() =>
            {
                Deselection(false);
                if (Gm.IsAnyProv)
                {
                    if (enemies[2].getIsProvocation())
                    {
                        selectedhero.Add(enemies[2]);
                        switchLightSelection(ennemisButton3);
                    }
                }
                else
                {
                    selectedhero.Add(enemies[2]);
                    switchLightSelection(ennemisButton3);
                }
            });

        }
        else
        {
            if (selectedcard.AOEEnnemies)
            {
                foreach (hero ennemy in enemies)
                {
                    selectedhero.Add(ennemy);
                    ActivateSideLights(false);
                }
            }
        }
        if (!selectedcard.AOEAllies && selectedcard.TargetAllies)
        {
            arboristeButton?.onClick.AddListener(() => { ClearSide(true); selectedhero.Add(heroes[0]); switchLightSelection(arboristeButton); });
            pretreButton?.onClick.AddListener(() => { switchLightSelection(pretreButton); ClearSide(true); ; if (perso1 == true) selectedhero.Add(heroes[1]); else selectedhero.Add(heroes[0]); });
        }
        else
        {
            if (selectedcard.AOEAllies)
            {
                foreach (hero hero in heroes)
                {
                    selectedhero.Add(hero);
                    ActivateSideLights(true);
                }
            }
        }
        coroutine = StartCoroutine(turnwait());
    }

    public IEnumerator turnwait()
    {
        while (!endturnbool)
        {
            yield return new WaitUntil(() => isCardSend);

            playCard(selectedcard, selectedhero);
            Deselection(false);
            Gm.deck.PlayCard(selectedcard.m_index);
            isCardSend = false;
            for (int i = 0; i < enemies.Count; i++)
            {
                if(enemies[i].getPv() <= 0)
                {
                    if(i == 0)
                    {
                        ennemisButton1?.onClick.RemoveAllListeners();
                        ennemisButton1.gameObject.SetActive(false);
                    }
                    else
                    {
                        ennemisButton2?.onClick.RemoveAllListeners();
                        ennemisButton2.gameObject.SetActive(false);
                    }
                }
                enemies[i].resetArmor();
            }   

            if (!CheckifEnemyAreAlive())
            {
                lightsAllies.Clear();
                lightsEnnemies.Clear();
                WinFight();
            }
        }
        PlayPlayerEffects();
        PlayEnemyTurn();

    }

    void PlayPlayerEffects() 
    {
        foreach (hero h in heroes)
        {
            for (int i = 0; i < h.MyEffects.Count; i++)
            {
                dataCard.CardEffect e = h.MyEffects[i];
                if (e.nbTour != 0)
                {
                    e.nbTour--;
                    if (e.nbTour == 0)
                    {
                        h.MyEffects.Remove(e);
                    }
                    int howFar = 0;
                    foreach(dataCard.CardType c in e.effects)
                    {
                        switch (c)
                        {
                            case dataCard.CardType.Damage:
                                dataCard.DamageEffect(h, e.values[howFar]);
                                break;
                            case dataCard.CardType.Poison:
                                dataCard.DamageEffect(h, e.values[howFar]);
                                break;
                        }
                        howFar++;
                    }
                }

                if (h.getPv() <= 0)
                {
                    if (i == 0)
                    {
                        if (perso1)
                        {
                            arboristeButton?.onClick.RemoveAllListeners();
                            arboristeButton.gameObject.SetActive(false);
                        }
                        else
                        {
                            pretreButton?.onClick.RemoveAllListeners();
                            pretreButton.gameObject.SetActive(false);
                        }

                    }
                    else
                    {
                        pretreButton?.onClick.RemoveAllListeners();
                        pretreButton.gameObject.SetActive(false);
                    }
                }
                if (!CheckifHeroAreAlive())
                {
                    LooseFight();
                }
                heroes[i].resetArmor();
            }
        }
    }
    private void PlayEnemyEffects()
    {
        foreach (hero h in enemies)
        {
            for (int i = 0; i < h.MyEffects.Count; i++)
            {
                dataCard.CardEffect e = h.MyEffects[i];
                if (e.nbTour != 0)
                {
                    e.nbTour--;
                    if (e.nbTour == 0)
                    {
                        h.MyEffects.Remove(e);
                    }
                    int howFar = 0;
                    foreach(dataCard.CardType c in e.effects)
                    {
                        switch (c)
                        {
                            case dataCard.CardType.Damage:
                                dataCard.DamageEffect(h, e.values[howFar]);
                                break;
                            case dataCard.CardType.Poison:
                                dataCard.DamageEffect(h, e.values[howFar]);
                                break;
                        }
                        howFar++;
                    }
                }
            }
        }

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
        
        foreach(hero hero in heroes)
        {
            hero.gainExperience(20);
        }
        SceneManager.LoadScene(0);
        Gm.waveCounter++;
        Gm.Hand.Clear();
        Gm.deck = null;
        Gm.entityManager.getListHero().Clear();
        ennemisButton1?.onClick.RemoveAllListeners();
        ennemisButton2?.onClick.RemoveAllListeners();
        ennemisButton3?.onClick.RemoveAllListeners();
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
        test = false;
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
            for (int i = card.nombreDexecutiion; i != 0; i--)
            {
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
                    case dataCard.CardType.AddArmor:
                        foreach (hero hero in selected)
                        {
                            hero.setArmor(2); // mettre la valeur de l'armure
                        }
                        break;
                    case dataCard.CardType.AddMana:
                        foreach (hero hero in selected)
                        {
                            card.AddMana(hero);
                        }
                        break;
                    case dataCard.CardType.AddCard: //pioche une carte
                        Gm.deck.DrawCard(1);
                        break;
                    case dataCard.CardType.UpgradeCard://la carte ne va pas dans la defausse elle reste sur la table et s'ameliore au fur et a mesure de la partie, Leur prix peut baisser, leurs stats augmenter...
                        foreach (hero hero in selected)
                        {
                            
                        }
                        break;
                    case dataCard.CardType.ChangeCardMana://change le mana d'une carte
                        foreach (hero hero in selected)
                        {

                        }
                        break;
                    case dataCard.CardType.ChangeCardDamage://change le damage d'une carte
                        foreach (hero hero in selected)
                        {

                        }
                        break;
                    case dataCard.CardType.FromNow://les effets de cette carte dure jusqu'a la fin du combat
                        foreach (hero hero in selected)
                        {

                        }
                        break;
                    case dataCard.CardType.Venerate://augmente la barre de veneration d'un allie
                        foreach (hero hero in selected)
                        {

                        }
                        break;
                    case dataCard.CardType.Poison://le personnage recoit les degats du poison avant de jouer puis à chaque tour il subit un point de moins
                        foreach (hero hero in selected)
                        {
                            //DONTDO
                        }
                        break;
                    case dataCard.CardType.Steal://inflige X degat et soigne X à un autre personnage
                        foreach (hero hero in selected)
                        {

                        }
                        break;
                }
            }
        }
        foreach(dataCard.CardEffect effect in card.CardEffects)
        {
            foreach(hero h in selectedhero)
            {
                h.MyEffects.Add(effect);
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

}
