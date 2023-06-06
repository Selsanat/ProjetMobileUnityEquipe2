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
using System.Linq;
using TMPro;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;
using Slider = UnityEngine.UI.Slider;
using Map;
using DG.Tweening.Core.Easing;

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
    public bool isPretreTransform = false;
    public bool isArboTransform = false;
    bool test = false;
    private bool prout;
    private int ennemisTues = 0;
    [SerializeField] public Button play;
    [SerializeField] public Button cancel;
    [SerializeField] Button arboristeButton;
    [SerializeField] Button pretreButton;
    [SerializeField] Button ennemisButton1;
    [SerializeField] Button ennemisButton2;
    [SerializeField] Button ennemisButton3;
    [SerializeField] Button selectedButton;
    [SerializeField] Button endTurnButton;
    [SerializeField] TextMeshProUGUI stockText;
    [SerializeField] TextMeshProUGUI manaText;

    private int mana;
    private int stock = 0;
    [SerializeField] List<hero> heroes;
    [SerializeField] List<hero> enemies;
    [SerializeField] List<hero> selectedhero;
    
    public List<hero> enemiesAtStartOfCombat;
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
        enemiesAtStartOfCombat = new List<hero>();
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
        
        if (ComponentBouton == ennemisButton1 || ComponentBouton == ennemisButton2 || ComponentBouton == ennemisButton3)
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
    void switchLightSelection(Button Boutton, bool isChamp)
    {
        if(isChamp == false)
        {
            Light2D lightDuBoutton = Boutton.gameObject.transform.GetChild(4).gameObject.GetComponent<Light2D>();
            lightDuBoutton.enabled = true;
        }
        else
        {
            Light2D lightDuBoutton = Boutton.gameObject.transform.GetChild(2).gameObject.GetComponent<Light2D>();
            lightDuBoutton.enabled = true;
        }
        
    }

    public IEnumerator CardAnimDisolve()
    {
        if (selectedhero.Count != 0)
        {
            play.gameObject.SetActive(false);
            cancel.gameObject.SetActive(false);
            DissolveController dissolveController = Gm.CarteUtilisee.GetComponent<DissolveController>();
            Gm.CarteUtilisee.canvas.gameObject.SetActive(false);
            dissolveController.isDissolving = true;
            yield return new WaitUntil(() => dissolveController.dissolveAmount < 0);
            dissolveController.isDissolving = false;
            dissolveController.dissolveAmount = 1;
            Gm.CarteUtilisee.canvas.gameObject.SetActive(false);
            isCardSend = true;
        }
    }
    #endregion
    public void StartFight()
    {
        manaText = GameObject.Find("ManaText").GetComponent<TextMeshProUGUI>();
        manaText.text = mana.ToString();
        stockText = GameObject.Find("StockText").GetComponent<TextMeshProUGUI>();
        stockText.text = stock.ToString();
        endTurnButton = GameObject.Find("EndTurnButton").GetComponent<Button>();
        endTurnButton.onClick.AddListener(() => { repMana(); });
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
                H2 = new hero(entityManager.Role.Arboriste, 50, Gm.LifeArboriste, 0, 0, null, 0, Gm.levelArboriste, Gm.expArboriste);

            temp = GameObject.Find("champ");
            temp.GetComponent<Image>().sprite = heroSprite;
            arboristeButton = temp.GetComponent<Button>();
            ChangerBouttonEnGameObject(arboristeButton, heroSprite, true);
            H2.m_slider = temp.GetComponentInChildren<Slider>();
            H2.m_slider.maxValue = H2.getMaxPv();
            H2.m_slider.value = H2.getPv();
            H2.stockText = temp.GetComponentInChildren<TextMeshProUGUI>();
            H2.stockText.text = H2.getMana().ToString() + " / " + H2.m_manaMax;

            temp = GameObject.Find("champ2");
            temp.GetComponent<Image>().sprite = heroSprite2;
            pretreButton = temp.GetComponent<Button>();
            GameObject.Find("champSolo").SetActive(false);
            ChangerBouttonEnGameObject(pretreButton, heroSprite2, true);
            H1.m_slider = temp.GetComponentInChildren<Slider>();
            H1.m_slider.maxValue = H1.getMaxPv();
            H1.m_slider.value = H1.getPv();
            H1.stockText = temp.GetComponentInChildren<TextMeshProUGUI>();
            H1.stockText.text = H1.getMana().ToString() + " / " + H1.m_manaMax;
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
            }
            else
                H2 = new hero(entityManager.Role.Pretre, 50, Gm.LifeArboriste, 0, 0, null, 0, Gm.levelArboriste, Gm.expArboriste);


            H2.m_slider = temp.GetComponentInChildren<Slider>();
            H2.m_slider.maxValue = H2.getMaxPv();
            H2.m_slider.value = H2.getPv();
            H2.stockText = temp.GetComponentInChildren<TextMeshProUGUI>();
            H2.stockText.text = H2.getMana().ToString() + " / " + H2.m_manaMax;


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
            H1.stockText = temp.GetComponentInChildren<TextMeshProUGUI>();
            H1.stockText.text = H1.getMana().ToString() + " / " + H1.m_manaMax;
        }
        #endregion

        #region choix de la wave
        int waveType = UnityEngine.Random.Range(0, Gm.allWave[Gm.waveCounter].Count -1);
        Debug.Log("waveType : " + waveType);
        hero en1;
        hero En2;
        hero En3;



        if (Gm.allWave[Gm.waveCounter][waveType].Count == 1)
        {
            en1 = Gm.allWave[Gm.waveCounter][waveType][0].SetEnemy();
            temp = GameObject.Find("enemy1");
            temp.GetComponent<Image>().sprite = ennemy1Sprite;
            ennemisButton1 = temp.GetComponent<Button>();
            en1.m_slider = temp.GetComponentInChildren<Slider>();
            en1.m_slider.maxValue = en1.getMaxPv();
            en1.m_slider.value = en1.getPv();
            en1.m_valueText = temp.GetComponentInChildren<TextMeshProUGUI>();
            en1.m_spriteFocus = temp.GetComponentsInChildren<Image>()[3];
            en1.m_spriteTypeAttack = temp.GetComponentsInChildren<Image>()[4];
            en1.setIsAlive(true);
            ChangerBouttonEnGameObject(ennemisButton1, en1.m_sprite, false);
            GameObject.Find("enemy2").SetActive(false);
            GameObject.Find("enemy3").SetActive(false);
        }
        else if (Gm.allWave[Gm.waveCounter][waveType].Count == 2)
        {
            en1 = Gm.allWave[Gm.waveCounter][waveType][0].SetEnemy();
            temp = GameObject.Find("enemy1");
            temp.GetComponent<Image>().sprite = ennemy1Sprite;
            ennemisButton1 = temp.GetComponent<Button>();
            en1.m_slider = temp.GetComponentInChildren<Slider>();
            en1.m_slider.maxValue = en1.getMaxPv();
            en1.m_slider.value = en1.getPv();
            en1.m_valueText = temp.GetComponentInChildren<TextMeshProUGUI>();
            en1.m_spriteFocus = temp.GetComponentsInChildren<Image>()[3];
            en1.m_spriteTypeAttack = temp.GetComponentsInChildren<Image>()[4];
            en1.setIsAlive(true);

            ChangerBouttonEnGameObject(ennemisButton1, en1.m_sprite, false);
            En2 = Gm.allWave[Gm.waveCounter][waveType][1].SetEnemy();
            temp = GameObject.Find("enemy2");
            temp.GetComponent<Image>().sprite = ennemy2Sprite;
            ennemisButton2 = temp.GetComponent<Button>();
            En2.m_slider = temp.GetComponentInChildren<Slider>();
            En2.m_slider.maxValue = En2.getMaxPv();
            En2.m_slider.value = En2.getPv();
            En2.m_valueText = temp.GetComponentInChildren<TextMeshProUGUI>();
            En2.m_spriteFocus = temp.GetComponentsInChildren<Image>()[3];
            En2.m_spriteTypeAttack = temp.GetComponentsInChildren<Image>()[4];
            En2.setIsAlive(true);

            ChangerBouttonEnGameObject(ennemisButton2, En2.m_sprite, false);
            GameObject.Find("enemy3").SetActive(false);

        }
        else if (Gm.allWave[Gm.waveCounter][waveType].Count == 3)
        {
            en1 = Gm.allWave[Gm.waveCounter][waveType][0].SetEnemy();
            temp = GameObject.Find("enemy1");
            temp.GetComponent<Image>().sprite = ennemy1Sprite;
            ennemisButton1 = temp.GetComponent<Button>();
            en1.m_slider = temp.GetComponentInChildren<Slider>();
            en1.m_slider.maxValue = en1.getMaxPv();
            en1.m_slider.value = en1.getPv();
            en1.m_valueText = temp.GetComponentInChildren<TextMeshProUGUI>();
            en1.m_spriteFocus = temp.GetComponentsInChildren<Image>()[3];
            en1.m_spriteTypeAttack = temp.GetComponentsInChildren<Image>()[4];
            en1.setIsAlive(true);

            ChangerBouttonEnGameObject(ennemisButton1, en1.m_sprite, false);
            En2 = Gm.allWave[Gm.waveCounter][waveType][1].SetEnemy();
            temp = GameObject.Find("enemy2");
            temp.GetComponent<Image>().sprite = ennemy2Sprite;
            ennemisButton2 = temp.GetComponent<Button>();
            En2.m_slider = temp.GetComponentInChildren<Slider>();
            En2.m_slider.maxValue = En2.getMaxPv();
            En2.m_slider.value = En2.getPv();
            En2.m_valueText = temp.GetComponentInChildren<TextMeshProUGUI>();
            En2.m_spriteFocus = temp.GetComponentsInChildren<Image>()[3];
            En2.m_spriteTypeAttack = temp.GetComponentsInChildren<Image>()[4];
            En2.setIsAlive(true);

            ChangerBouttonEnGameObject(ennemisButton2, En2.m_sprite, false);

            En3 = Gm.allWave[Gm.waveCounter][waveType][2].SetEnemy();
            temp = GameObject.Find("enemy3");
            temp.GetComponent<Image>().sprite = ennemy2Sprite;
            ennemisButton3 = temp.GetComponent<Button>();
            En3.m_slider = temp.GetComponentInChildren<Slider>();
            En3.m_slider.maxValue = En3.getMaxPv();
            En3.m_slider.value = En3.getPv();
            En3.m_valueText = temp.GetComponentInChildren<TextMeshProUGUI>();
            En3.m_spriteFocus = temp.GetComponentsInChildren<Image>()[3];
            En3.m_spriteTypeAttack = temp.GetComponentsInChildren<Image>()[4];
            En3.setIsAlive(true);

            ChangerBouttonEnGameObject(ennemisButton3, En3.m_sprite, false);
        }




        #endregion

        StartTurn();

    }
    void StartTurn()
    {
        endTurnButton.GetComponentInChildren<TextMeshProUGUI>().text = "End Turn";
        endTurnButton.onClick.AddListener(() => { repMana(); });
        mana = 3;
        manaText.text = mana.ToString();
        endturnbool = false;
        Gm.deck.StartTurn();
        heroes.Clear();
        enemies.Clear();
        
        pretreButton?.onClick.RemoveAllListeners();
        arboristeButton?.onClick.RemoveAllListeners();

        foreach (hero E in Gm.entityManager.getListHero())
        {
            if (E.m_role == entityManager.Role.Pretre || E.m_role == entityManager.Role.Arboriste )
            {
                if(E.getIsAlive())
                    heroes.Add(E);
            }
            else
            {
                    enemies.Add(E);
            }
            if(E.isFull && E.getIsAlive() && E.m_role == entityManager.Role.Arboriste)
            {
                arboristeButton.interactable = true;
                arboristeButton.onClick.AddListener(() => { StartCoroutine(Gm.deck.TransfoCoroutine(true)); E.setMana(0); E.stockText.text = E.getMana().ToString() + " / " + E.m_manaMax; isArboTransform = true; arboristeButton.interactable = false; });
            }
            else if (E.isFull && E.getIsAlive() && E.m_role == entityManager.Role.Pretre)
            {
                pretreButton.interactable = true;
                pretreButton.onClick.AddListener(() => { StartCoroutine(Gm.deck.TransfoCoroutine(false)); E.setMana(0); E.stockText.text = E.getMana().ToString() + " / " + E.m_manaMax; isPretreTransform = true; pretreButton.interactable = false; });
            }
        }
        

        if (enemiesAtStartOfCombat.Count== 0)
        {
            enemiesAtStartOfCombat = enemies.ToList();
        }
        else
        {
            enemies.Clear();
            foreach (hero En in enemiesAtStartOfCombat.ToList())
            {
                if (En.getIsAlive())
                {
                    enemies.Add(En);
                }
            }
        }
        foreach (hero En in enemiesAtStartOfCombat.ToList())
        {
            En.EnemyAttack(heroes, true);
        }

    }




    public void Cardsend(CardObject card, int index)
    {
        pretreButton?.onClick.RemoveAllListeners();
        arboristeButton?.onClick.RemoveAllListeners();
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
        //True si : La carte n'est pas null et qu'elle a une cible. Si elle n'en a pas, elle se lance si C'est une carte D'AOE Alli�e qui cible pas d'ennemies, ou inversement.
        //[WIP]je dois le changer[WIP]
        bool conditionjouer = Gm.CarteUtilisee != null;//&& selectedhero != null&& ((selectedcard.AOEAllies && !selectedcard.TargetEnnemies) || (selectedcard.AOEEnnemies && !selectedcard.TargetAllies));
        play.onClick.AddListener(() => { if(conditionjouer) StartCoroutine(CardAnimDisolve());});
        //[WIP]je dois le changer[WIP]

        if (!selectedcard.AOEEnnemies && selectedcard.TargetEnnemies)
        {
            ennemisButton1?.onClick.AddListener(() => 
            {
                Deselection(false);
                if (Gm.IsAnyProv) 
                { 
                    if(enemiesAtStartOfCombat[0].getIsProvocation()) 
                    { 
                        selectedhero.Add(enemiesAtStartOfCombat[0]); 
                        switchLightSelection(ennemisButton1, false); 
                    } 
                    

                }
                else
                { 
                    selectedhero.Add(enemiesAtStartOfCombat[0]); 
                    switchLightSelection(ennemisButton1, false);
                }
            });

            ennemisButton2?.onClick.AddListener(() =>
            {
                ClearSide(false);
                if (Gm.IsAnyProv)
                {
                    if (enemiesAtStartOfCombat[1].getIsProvocation())
                    {
                        selectedhero.Add(enemiesAtStartOfCombat[1]);
                        switchLightSelection(ennemisButton2, false);
                    }
                }
                else
                {
                    selectedhero.Add(enemiesAtStartOfCombat[1]);
                    switchLightSelection(ennemisButton2, false);
                }
            });
            ennemisButton3?.onClick.AddListener(() =>
            {
                Deselection(false);
                if (Gm.IsAnyProv)
                {
                    if (enemiesAtStartOfCombat[2].getIsProvocation())
                    {
                        selectedhero.Add(enemiesAtStartOfCombat[2]);
                        switchLightSelection(ennemisButton3, false);
                    }
                }
                else
                {
                    selectedhero.Add(enemiesAtStartOfCombat[2]);
                    switchLightSelection(ennemisButton3, false);
                }
            });


            //ennemisButton1.OnDeselect(clearCardSelected());




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
            arboristeButton?.onClick.AddListener(() => { ClearSide(true); selectedhero.Add(heroes[0]); switchLightSelection(arboristeButton, true); });
            pretreButton?.onClick.AddListener(() => { switchLightSelection(pretreButton, true); ClearSide(true); ; if (perso1 == true) selectedhero.Add(heroes[1]); else selectedhero.Add(heroes[0]); });
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
            for (int i = 0; i < enemiesAtStartOfCombat.Count; i++)
            {
                if(enemiesAtStartOfCombat[i].getPv() <= 0)
                {
                    if(i == 0 && ennemisButton1?.IsActive() == true)
                    {
                        Debug.Log("ennemi 1 mort");
                        ennemisButton1?.onClick.RemoveAllListeners();
                        ennemisButton1?.gameObject.SetActive(false);
                        enemies.Remove(enemiesAtStartOfCombat[i]);
                    }
                    else if (i == 1 && ennemisButton2?.IsActive() == true)
                    {
                        Debug.Log("ennemi 2 mort");
                        ennemisButton2?.onClick.RemoveAllListeners();
                        ennemisButton2?.gameObject.SetActive(false);
                        enemies.Remove(enemiesAtStartOfCombat[i]);


                    }
                    else if (i == 2 && ennemisButton3?.IsActive() == true)
                    {
                        Debug.Log("ennemi 3 mort");
                        ennemisButton3?.onClick.RemoveAllListeners();
                        ennemisButton3?.gameObject.SetActive(false);
                        enemies.Remove(enemiesAtStartOfCombat[i]);


                    }
                }
                enemiesAtStartOfCombat[i].resetArmor();
            }   

            if (!CheckifEnemyAreAlive())
            {
                lightsAllies.Clear();
                lightsEnnemies.Clear();
                //Gm.waveCounter == 12
                if (true)
                {
                    WinFinalFight();
                }
                else
                    WinFight();
            }
        }
        PlayEnemyEffects();
        PlayEnemyTurn();
    }

    void PlayPlayerEffects() 
    {
        foreach (hero h in heroes)
        {
            for (int i = 0; i < h?.MyEffects?.Count; i++)
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

                
                h.resetArmor();
                if (!CheckifHeroAreAlive())
                {
                    LooseFight();
                }
            }
        }

    }
    private void PlayEnemyEffects()
    {
        foreach (hero h in enemies.ToList())
        {
            for (int i = 0; i < h.MyEffects?.Count; i++)
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

    public void repMana()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        if (isArboTransform)
        {
            isArboTransform = false;
            StartCoroutine(Gm.deck.DetransfoCoroutine(true));
        }
        if (isPretreTransform)
        {
            isPretreTransform = false;
            StartCoroutine(Gm.deck.DetransfoCoroutine(false));
        }
        else 
            StartCoroutine(Gm.deck.DiscardCoroutine(true));
        
        stock += mana;
        mana = 0;
        stockText.text = stock.ToString();
        manaText.text = mana.ToString();

        endTurnButton?.onClick.RemoveAllListeners();
        endTurnButton.GetComponentInChildren<TextMeshProUGUI>().text = "SKIP";
        endTurnButton?.onClick.AddListener(() => { chargeMana(3); });
        
        
        foreach (hero h in heroes)
        {
            if(h.m_role == hero.Role.Arboriste && h.getMana() != h.m_manaMax)
            {
                arboristeButton?.onClick.RemoveAllListeners();
                arboristeButton?.onClick.AddListener(() => { chargeMana(0); });
                h.stockText.text = h.getMana().ToString() + " / " + h.m_manaMax;
            }
            else if (h.m_role == hero.Role.Pretre && h.getMana() != h.m_manaMax)
            {

                pretreButton?.onClick.RemoveAllListeners();
                pretreButton?.onClick.AddListener(() => { chargeMana(1); });
                h.stockText.text = h.getMana().ToString() + " / " + h.m_manaMax;
            }   
            
        }
        
        
    }

    public void chargeMana(int x)
    {
        if (x == 0)
        {
            foreach (hero h in heroes)
            {
                if (h.getMana() == h.m_manaMax)
                    break;
                if (h.m_role == hero.Role.Arboriste)
                {
                    while(stock >= 1)
                    {
                        h.setMana(h.getMana() + 1);
                        stock--;
                        if (h.getMana() == h.m_manaMax)
                        {
                            h.isFull = true;
                            break;

                        }
                    }
                    h.stockText.text = h.getMana().ToString() + " / " + h.m_manaMax;
                    
                }
            }
            
        }
        else if (x == 1)
        {
            foreach (hero h in heroes)
            {
                if (h.m_role == hero.Role.Pretre)
                {
                    if(h.getMana() == h.m_manaMax)
                        break;

                    while (stock >= 1)
                    {
                        h.setMana(h.getMana() + 1);
                        stock--;
                        if (h.getMana() == h.m_manaMax)
                        {
                            h.isFull = true;
                            break;

                        }
                    }
                    h.stockText.text = h.getMana().ToString() + " / " + h.m_manaMax;

                }
            }
        }
        

        stockText.text = stock.ToString();
        pretreButton?.onClick.RemoveAllListeners();
        arboristeButton?.onClick.RemoveAllListeners();
        endTurnButton?.onClick.RemoveAllListeners();

        PlayEnemyTurn();

    }



    private void PlayEnemyTurn()
    {

        
        foreach (hero En in enemiesAtStartOfCombat.ToList())
        {
            En.EnemyAttack(heroes, false);
            if (!CheckifHeroAreAlive())
            {
                LooseFight();
                return;
            }
            foreach (hero h in heroes.ToList())
            {
                if (h.getIsAlive() == false)
                {
                    if (h.m_role == entityManager.Role.Arboriste)
                    {
                        arboristeButton?.onClick.RemoveAllListeners();
                        arboristeButton?.gameObject.SetActive(false);
                        h.gameObject.SetActive(false);
                    }
                    else
                    {
                        pretreButton?.onClick.RemoveAllListeners();
                        pretreButton.gameObject.SetActive(false);
                        h.gameObject.SetActive(false);


                    }
                }

            }

        }
        if (!CheckifHeroAreAlive())
        {

            LooseFight();
        }
        else
        {
            StartTurn();
        }
        PlayPlayerEffects();
    }

    public void ResetAll()
    {
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
        ennemisButton3 = null;
        arboristeButton = null;
        pretreButton = null;
        stock = 0;
        heroes.Clear();
        enemies.Clear();
        selectedhero.Clear();
        enemiesAtStartOfCombat.Clear();
        Gm.levelArboriste = 0;
        Gm.levelPretre = 0;
        Gm.expPretre = 0;
        Gm.expArboriste = 0;
        Gm.LifeArboriste = 50;
        Gm.LifePretre = 50;
        Gm.IsArboristePlayed = false;
        Gm.IsPretrePlayed = false;
        Gm.waveCounter = 0;
        Gm.IsAnyProv = false;
        Gm.CarteUtilisee = null;
        Gm.CardsInteractable = true;
        Gm.HasCardInHand = false;
        Gm.debuffDraw = 0;
        Gm.isHoverButton = false;
    }
    private void LooseFight()
    {
        Debug.Log("Loosedfight");
        StopCoroutine(coroutine);
        ResetAll();
        SceneManager.LoadScene(0);
        FindObjectOfType<MapManager>().GenerateNewMap();

    }

    public void WinFinalFight()
    {
        StartCoroutine(realwin());
    }
    IEnumerator realwin()
    {
        Debug.Log("WinFinalFight");
        StopCoroutine(coroutine);
        ResetAll();
        Gm.transi.Play("Transi");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(2);
        Gm.transi.Play("Detransi");
        
        FindObjectOfType<MapManager>().GenerateNewMap();
    }

    IEnumerator XpLerp()
    {
        int lvldruid =0;
        int lvlpriest=0;
        bool lvlUpDruid = false;
        bool lvlUpPriest = false;
        foreach (hero hero in heroes)
        {
            if (hero.m_role == entityManager.Role.Arboriste)
            {
                lvldruid = Gm.levelArboriste;
            }
            else
            {
                lvlpriest = Gm.levelPretre;
            }
            hero.gainExperience((int)((2*enemiesAtStartOfCombat.Count)/heroes.Count));
            if (hero.m_role == entityManager.Role.Arboriste)
            {
                lvlUpDruid = Gm.levelArboriste > lvldruid;
            }
            else
            {
                lvlUpPriest = Gm.levelArboriste > lvlpriest;
            }
        }

        #region AnimationBarreXP
        yield return new WaitUntil(() => Input.GetMouseButton(0));
        float TempsTransition = 5;
        float timeElapsed = 0;

        if (heroes.Count == 1)
        {

            if (heroes[0].m_role == entityManager.Role.Arboriste)
            {
                lvlUpDruid = Gm.deck.SlidersXp[2].value > Gm.expArboriste||lvlUpDruid;
            }
            else
            {
                lvlUpPriest = Gm.deck.SlidersXp[2].value > Gm.expPretre||lvlUpPriest;
            }
        }
        else
        {
            lvlUpDruid = Gm.deck.SlidersXp[0].value > Gm.expArboriste || lvlUpDruid;
            lvlUpPriest = Gm.deck.SlidersXp[1].value > Gm.expPretre|| lvlUpPriest;

        }
        while (timeElapsed < TempsTransition)
        {
            if (heroes.Count == 1)
            {
                
                if (heroes[0].m_role == entityManager.Role.Arboriste)
                {
                    if (lvlUpDruid)
                    {
                        Gm.deck.SlidersXp[2].value = Mathf.Lerp(Gm.deck.SlidersXp[2].value, Gm.deck.SlidersXp[2].maxValue, Time.deltaTime * 1.5f);
                    }
                    else
                    {
                        Gm.deck.SlidersXp[2].value = Mathf.Lerp(Gm.deck.SlidersXp[2].value, Gm.expArboriste, Time.deltaTime * 1.5f);
                    }
                }
                else
                {
                    if (lvlUpPriest)
                    {
                        Gm.deck.SlidersXp[2].value = Mathf.Lerp(Gm.deck.SlidersXp[2].value, Gm.deck.SlidersXp[2].maxValue, Time.deltaTime * 1.5f);
                    }
                    else
                    {
                        Gm.deck.SlidersXp[2].value = Mathf.Lerp(Gm.deck.SlidersXp[2].value, Gm.expPretre, Time.deltaTime * 1.5f);
                    }

                }
            }
            else
            {
                if (lvlUpDruid)
                {
                    Gm.deck.SlidersXp[0].value = Mathf.Lerp(Gm.deck.SlidersXp[0].value, Gm.deck.SlidersXp[0].maxValue, Time.deltaTime * 1.5f);

                }
                else
                {
                    Gm.deck.SlidersXp[0].value = Mathf.Lerp(Gm.deck.SlidersXp[0].value, Gm.expArboriste, Time.deltaTime * 1.5f);
                }
                if (lvlUpPriest)
                {
                    Gm.deck.SlidersXp[1].value = Mathf.Lerp(Gm.deck.SlidersXp[1].value, Gm.deck.SlidersXp[1].maxValue, Time.deltaTime * 1.5f);
                }
                else
                {
                    Gm.deck.SlidersXp[1].value = Mathf.Lerp(Gm.deck.SlidersXp[1].value, Gm.expPretre, Time.deltaTime * 1.5f);
                }
                
            }
            
            timeElapsed += Time.deltaTime * 1.5f;
            yield return null;
        }
        TempsTransition = 5;
        timeElapsed = 0;
        if (lvlUpDruid||lvlUpPriest)
        {
            if (heroes.Count == 1)
            {

                if (heroes[0].m_role == entityManager.Role.Arboriste)
                {
                    if (lvlUpDruid)
                    {
                        Gm.deck.SlidersXp[2].value = 0;
                    }
                }
                else
                {
                    if (lvlUpPriest)
                    {
                        Gm.deck.SlidersXp[2].value = 0;
                    }

                }
                Gm.deck.SlidersXp[2].maxValue = heroes[0].getexperienceMAX();
            }
            else
            {
                if (lvlUpDruid)
                {
                    Gm.deck.SlidersXp[0].value = 0;
                    Gm.deck.SlidersXp[0].maxValue = heroes[0].getexperienceMAX();

                }
                if (lvlUpPriest)
                {
                    Gm.deck.SlidersXp[1].value = 0;
                    Gm.deck.SlidersXp[1].maxValue = heroes[1].getexperienceMAX();
                }

            }
            while (timeElapsed < TempsTransition)
            {
                if (heroes.Count == 1)
                {

                    if (heroes[0].m_role == entityManager.Role.Arboriste)
                    {
                        if (lvlUpDruid)
                        {
                            Gm.deck.SlidersXp[2].value = Mathf.Lerp(Gm.deck.SlidersXp[2].value, Gm.expArboriste, Time.deltaTime * 1.5f);
                        }
                    }
                    else
                    {
                        if (lvlUpPriest)
                        {
                            Gm.deck.SlidersXp[2].value = Mathf.Lerp(Gm.deck.SlidersXp[2].value, Gm.expPretre, Time.deltaTime * 1.5f);
                        }

                    }
                }
                else
                {
                    if (lvlUpDruid)
                    {
                        Gm.deck.SlidersXp[0].value = Mathf.Lerp(Gm.deck.SlidersXp[0].value, Gm.expArboriste, Time.deltaTime * 1.5f);

                    }
                    if (lvlUpPriest)
                    {
                        Gm.deck.SlidersXp[1].value = Mathf.Lerp(Gm.deck.SlidersXp[1].value, Gm.expPretre, Time.deltaTime * 1.5f);
                    }

                }

                timeElapsed += Time.deltaTime * 1.5f;
                yield return null;
            }
        }
        if (heroes.Count == 1)
        {
            if (heroes[0].m_role == entityManager.Role.Arboriste)
            {
                    Gm.deck.SlidersXp[2].value = Gm.expArboriste;
            }
            else
            {
                Gm.deck.SlidersXp[2].value = Gm.expPretre;
            }
           
        }
        else
        {
            Gm.deck.SlidersXp[0].value = Gm.expArboriste;
            Gm.deck.SlidersXp[1].value = Gm.expPretre;
        }
        #endregion
        yield return new WaitUntil(() => Input.GetMouseButton(0));
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
        ennemisButton3 = null;
        arboristeButton = null;
        pretreButton = null;
        stock = 0;
        heroes.Clear();
        enemies.Clear();
        selectedhero.Clear();
        enemiesAtStartOfCombat.Clear();
        
        selectedcard = null;
        yield return new WaitUntil(() => Input.GetMouseButton(0));
        StartCoroutine(ChangeSceneApresCOmbat());
        test = false;

    }
    IEnumerator ChangeSceneApresCOmbat (){
            Gm.transi.Play("Transi");
            yield return new WaitForSeconds(1.5f);
            Gm.transi.Play("Detransi");
            SceneManager.LoadScene(0);
    }
    private void WinFight()
    {
        StopCoroutine(coroutine);
        Gm.deck.AfficheSideUiXP(perso1 && perso2);
        Gm.deck.SetBonneBarreXp(heroes);
        Debug.Log("WIIIIIIIIIIIIIIIIIIIIIIIIIIIIN");
        StartCoroutine(XpLerp());


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


    public bool CheckifEnemyAreAlive()//TRUE = min ONE ALIVE
    {
        foreach (hero En in enemiesAtStartOfCombat)
        {
            if (En.getIsAlive())
            {
                return true;
            }
            else
            {
                
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
                            hero.takeDamage(card.m_value);
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
                    case dataCard.CardType.Poison://le personnage recoit les degats du poison avant de jouer puis � chaque tour il subit un point de moins
                        foreach (hero hero in selected)
                        {
                            //DONTDO
                        }
                        break;
                    case dataCard.CardType.Steal://inflige X degat et soigne X � un autre personnage
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

}
