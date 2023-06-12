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
using Random = UnityEngine.Random;

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
    public bool isCanibalisme = false;
    public bool isProf = false;
    public bool isApo = false;
    bool test = false;
    private bool prout;
    private int ennemisTues = 0;
    [SerializeField] public Button play;
    [SerializeField] public Button cancel;
    [SerializeField] public Button arboristeButton;
    [SerializeField] public Button pretreButton;
    [SerializeField] Button ennemisButton1;
    [SerializeField] Button ennemisButton2;
    [SerializeField] Button ennemisButton3;
    [SerializeField] Button selectedButton;
    [SerializeField] Button endTurnButton;
    [SerializeField] public TextMeshProUGUI stockText;
    [SerializeField] public TextMeshProUGUI manaText;
    [SerializeField] List<GameObject> EnPrefabs;
    [SerializeField] List<GameObject> PrefabHeroes;
    [SerializeField] List<GameObject> PrefabHeroesalt;
    [SerializeField] List<GameObject> Transformed;
    [SerializeField] GameObject PrefabDmgText;
    public List<GameObject> HeroesGameObjectRef;
    public List<GameObject> HeroesAltGameObjectRef;
    public List<GameObject> EnnemiesGameObjectRef;
    public hero ennemyPlaying;
    

    public int mana;
    public int stock = 0;
    public int nbTransfo = 0;
    public int venerations = 0;
    [SerializeField] public List<hero> heroes;
    [SerializeField] public List<hero> enemies;
    [SerializeField] public List<hero> selectedhero;
    
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
    /*    void ChangerBouttonEnGameObject(Button ComponentBouton, Sprite SpritreUtilise, bool sideTrueIsAllies)
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

        }*/
    void ChangerBouttonEnGameObject(Button ComponentBouton, GameObject prefab, bool sideTrueIsAllies, float scale=0.1f, GameObject altprefab = null)
    {
        GameObject perso = GameObject.Instantiate(prefab);
        perso.transform.position = Camera.main.ScreenToWorldPoint(ComponentBouton.transform.position);
        SpriteRenderer SpritePerso = perso.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        perso.transform.parent = ComponentBouton.transform;
        perso.transform.localScale = new Vector3(scale, scale, scale);

        if (sideTrueIsAllies)
        {
            GameObject persoalt = GameObject.Instantiate(altprefab);
            persoalt.transform.position = Camera.main.ScreenToWorldPoint(ComponentBouton.transform.position);
            SpriteRenderer SpritePersoalt = persoalt.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
            persoalt.transform.parent = ComponentBouton.transform;
            persoalt.transform.localScale = new Vector3(scale, scale, scale);
            foreach (SpriteRenderer sprite in persoalt.GetComponentsInChildren<SpriteRenderer>())
            {
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);
            }
            HeroesGameObjectRef.Add(perso);
            HeroesAltGameObjectRef.Add(persoalt);
        }


        Light2D lumiere = perso.AddComponent(typeof(Light2D)) as Light2D;
        lumiere.enabled = false;

        if (ComponentBouton == ennemisButton1 || ComponentBouton == ennemisButton2 || ComponentBouton == ennemisButton3)
        {
            lightsEnnemies.Add(lumiere);
            lumiere.color = Color.red;
            EnnemiesGameObjectRef.Add(perso);
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
            Light2D lightDuBoutton = Boutton.gameObject.transform.GetChild(1).gameObject.GetComponent<Light2D>();
            lightDuBoutton.enabled = true;
        }
        
    }

    public IEnumerator CardAnimDisolve()
    {
        if (selectedhero.Count != 0)
        {
            mana -= Gm.CarteUtilisee.DataCard.m_manaCost; 
            manaText.text = mana.ToString();
            play.onClick.RemoveAllListeners();
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
    public int waveType;
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
        GameObject temporary = new GameObject() ;

        hero H1;
        hero H2;
        if (perso1 && perso2)
        {

            if (Gm.IsPretrePlayed == false)
            {
                H1 = new hero(entityManager.Role.Pretre, 50, 50, 0, 0, null, 0, 0);
                Gm.LifePretre = H1.getPv();
                Gm.IsPretrePlayed = true;
            }
            else
                H1 = new hero(entityManager.Role.Pretre, 50, Gm.LifePretre, 0, 0, null, 0, Gm.levelPretre, Gm.expPretre);
                
            if (Gm.IsArboristePlayed == false)
            {
                H2 = new hero(entityManager.Role.Arboriste, 50, Gm.LifeArboriste, 0, 0, null, 0, 0);
                Gm.LifeArboriste = H2.getPv();
                Gm.IsArboristePlayed = true;
            }
            else
                H2 = new hero(entityManager.Role.Arboriste, 50, Gm.LifeArboriste, 0, 0, null, 0, Gm.levelArboriste, Gm.expArboriste);

            temp = GameObject.Find("champ");
            temp.GetComponent<Image>().sprite = heroSprite;
            arboristeButton = temp.GetComponent<Button>();
            //ChangerBouttonEnGameObject(arboristeButton, heroSprite, true);
            ChangerBouttonEnGameObject(arboristeButton, PrefabHeroes[0], true, 0.16f, PrefabHeroesalt[0]);
            H2.m_slider = temp.GetComponentInChildren<Slider>();
            H2.m_slider.maxValue = H2.getMaxPv();
            H2.m_slider.value = H2.getPv();
            H2.stockText = temp.GetComponentInChildren<TextMeshProUGUI>();
            H2.ArmorText = temp.GetComponentsInChildren<TextMeshProUGUI>()[1];
            H2.Armor = H2.ArmorText.transform.parent.gameObject.GetComponent<Image>();
            H2.stockText.text = H2.getMana().ToString() + " / " + H2.m_manaMax;
            H2.m_buffs = H2.m_slider.transform.parent;

            temp = GameObject.Find("champ2");
            temp.GetComponent<Image>().sprite = heroSprite2;
            pretreButton = temp.GetComponent<Button>();
            //ChangerBouttonEnGameObject(pretreButton, heroSprite2, true);
            ChangerBouttonEnGameObject(pretreButton, PrefabHeroes[1], true, 0.20f, PrefabHeroesalt[1]);
            H1.m_slider = temp.GetComponentInChildren<Slider>();
            H1.m_slider.maxValue = H1.getMaxPv();
            H1.m_slider.value = H1.getPv();
            H1.stockText = temp.GetComponentInChildren<TextMeshProUGUI>();
            H1.ArmorText = temp.GetComponentsInChildren<TextMeshProUGUI>()[1];
            H1.Armor = H1.ArmorText.transform.parent.gameObject.GetComponent<Image>();
            H1.stockText.text = H1.getMana().ToString() + " / " + H1.m_manaMax;
            

            GameObject.Find("champSolo").SetActive(false);
        }
        else if(perso1)
        {
            GameObject.Find("champ2").SetActive(false);
            GameObject.Find("champ").SetActive(false);
            temp = GameObject.Find("champSolo");
            temp.GetComponent<Image>().sprite = heroSprite;
            
            arboristeButton = temp.GetComponent<Button>();
            //ChangerBouttonEnGameObject(arboristeButton, heroSprite, true);
            ChangerBouttonEnGameObject(arboristeButton, PrefabHeroes[0], true, 0.16f, PrefabHeroesalt[0]);
            if (Gm.IsArboristePlayed == false)
            {
                H2 = new hero(entityManager.Role.Arboriste, 50, 50, 0, 0, null, 0, 0);
                Gm.LifeArboriste = H2.getPv();
                Gm.IsArboristePlayed = true;
            }
            else
                H2 = new hero(entityManager.Role.Arboriste, 50, Gm.LifeArboriste, 0, 0, null, 0, Gm.levelArboriste, Gm.expArboriste);


            H2.m_slider = temp.GetComponentInChildren<Slider>();
            H2.m_slider.maxValue = H2.getMaxPv();
            H2.m_slider.value = H2.getPv();
            H2.ArmorText = temp.GetComponentsInChildren<TextMeshProUGUI>()[1];
            H2.stockText = temp.GetComponentInChildren<TextMeshProUGUI>();
            H2.Armor = H2.ArmorText.transform.parent.gameObject.GetComponent<Image>();
            H2.stockText.text = H2.getMana().ToString() + " / " + H2.m_manaMax;
            


        }
        else if(perso2)
        {
            GameObject.Find("champ2").SetActive(false);
            GameObject.Find("champ").SetActive(false);
            temp = GameObject.Find("champSolo");
            temp.GetComponent<Image>().sprite = heroSprite2;
            pretreButton = temp.GetComponent<Button>();
            //ChangerBouttonEnGameObject(pretreButton, heroSprite2, true);
            ChangerBouttonEnGameObject(pretreButton, PrefabHeroes[1], true, 0.20f, PrefabHeroesalt[1]);
            if (Gm.IsPretrePlayed == false)
            {
                H1 = new hero(entityManager.Role.Pretre, 50, 50, 0, 0, null, 0, 0);
                Gm.LifePretre = H1.getPv();
                Gm.IsPretrePlayed = true;
            }
            else
                H1 = new hero(entityManager.Role.Pretre, 50, Gm.LifePretre, 0, 0, null, 0, Gm.levelPretre, Gm.expPretre);

            H1.m_slider = temp.GetComponentInChildren<Slider>();
            H1.m_slider.maxValue = H1.getMaxPv();
            H1.m_slider.value = H1.getPv();
            H1.stockText = temp.GetComponentInChildren<TextMeshProUGUI>();
            H1.ArmorText = temp.GetComponentsInChildren<TextMeshProUGUI>()[1];
            H1.Armor = H1.ArmorText.transform.parent.gameObject.GetComponent<Image>();
            H1.stockText.text = H1.getMana().ToString() + " / " + H1.m_manaMax;
            
        }
        #endregion

        #region choix de la wave
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
            en1.m_buffs = en1.m_slider.transform.parent.GetChild(4);
            en1.setIsAlive(true);
            ChangerBouttonEnGameObject(ennemisButton1, Gm.allWave[Gm.waveCounter][waveType][0].prefab, false);
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
            en1.m_buffs = en1.m_slider.transform.parent.GetChild(4);
            en1.setIsAlive(true);

            ChangerBouttonEnGameObject(ennemisButton1, Gm.allWave[Gm.waveCounter][waveType][0].prefab, false);
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
            En2.m_buffs = En2.m_slider.transform.parent.GetChild(4);
            En2.setIsAlive(true);

            ChangerBouttonEnGameObject(ennemisButton2, Gm.allWave[Gm.waveCounter][waveType][1].prefab, false);
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
            en1.m_buffs = en1.m_slider.transform.parent.GetChild(4);
            en1.setIsAlive(true);

            ChangerBouttonEnGameObject(ennemisButton1, Gm.allWave[Gm.waveCounter][waveType][0].prefab, false);
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
            En2.m_buffs = En2.m_slider.transform.parent.GetChild(4);
            En2.setIsAlive(true);

            ChangerBouttonEnGameObject(ennemisButton2, Gm.allWave[Gm.waveCounter][waveType][1].prefab, false);

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
            En3.m_buffs = En3.m_slider.transform.parent.GetChild(4);
            En3.setIsAlive(true);

            ChangerBouttonEnGameObject(ennemisButton3, Gm.allWave[Gm.waveCounter][waveType][2].prefab, false);
        }




        #endregion

        StartTurn();

    }
    void StartTurn()
    {
        endTurnButton.GetComponentInChildren<TextMeshProUGUI>().text = "End Turn";
        endTurnButton.onClick.AddListener(() => { repMana(); });
        
        mana = 3 + Gm.manaMultiplier;
        Gm.manaMultiplier = 0;
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
                arboristeButton.onClick.AddListener(() => { StartCoroutine(Gm.deck.TransfoCoroutine(true)); E.setMana(0); E.stockText.text = E.getMana().ToString() + " / " + E.m_manaMax; isArboTransform = true; arboristeButton.onClick.RemoveAllListeners(); nbTransfo++; });

            }
            else if (E.isFull && E.getIsAlive() && E.m_role == entityManager.Role.Pretre)
            {
                pretreButton.interactable = true;
                pretreButton.onClick.AddListener(() => { StartCoroutine(Gm.deck.TransfoCoroutine(false)); E.setMana(0); E.stockText.text = E.getMana().ToString() + " / " + E.m_manaMax ; isPretreTransform = true; pretreButton.onClick.RemoveAllListeners(); nbTransfo++; });
                

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
            ennemyPlaying = En;
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
        
        play.onClick.AddListener(() => { if (conditionjouer) StartCoroutine(CardAnimDisolve()); });
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
                        
                        if (selectedcard.CardTypes == dataCard.CardType.HabemusDominum )
                        {
                            if(enemiesAtStartOfCombat[0].m_IsAttacking)
                            {
                                selectedhero.Add(enemiesAtStartOfCombat[0]);
                                switchLightSelection(ennemisButton1, false);
                            }
                                
                        }
                        else if (selectedcard.CardTypes == dataCard.CardType.DiabolusEst)
                        {
                            if (!enemiesAtStartOfCombat[0].m_IsAttacking)
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

                        
                    } 
                    

                }
                else
                {
                    if (selectedcard.CardTypes == dataCard.CardType.HabemusDominum)
                    {
                        if (enemiesAtStartOfCombat[0].m_IsAttacking)
                        {
                            selectedhero.Add(enemiesAtStartOfCombat[0]);
                            switchLightSelection(ennemisButton1, false);
                        }

                    }
                    else if (selectedcard.CardTypes == dataCard.CardType.DiabolusEst)
                    {
                        if (!enemiesAtStartOfCombat[0].m_IsAttacking)
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
                }
            });

            ennemisButton2?.onClick.AddListener(() =>
            {
                ClearSide(false);
                if (Gm.IsAnyProv)
                {
                    if (selectedcard.CardTypes == dataCard.CardType.HabemusDominum)
                    {
                        if (enemiesAtStartOfCombat[1].m_IsAttacking)
                        {
                            selectedhero.Add(enemiesAtStartOfCombat[1]);
                            switchLightSelection(ennemisButton2, false);
                        }

                    }
                    else if (selectedcard.CardTypes == dataCard.CardType.DiabolusEst)
                    {
                        if (!enemiesAtStartOfCombat[1].m_IsAttacking)
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
                }
                else
                {
                    if (selectedcard.CardTypes == dataCard.CardType.HabemusDominum)
                    {
                        if (enemiesAtStartOfCombat[1].m_IsAttacking)
                        {
                            selectedhero.Add(enemiesAtStartOfCombat[1]);
                            switchLightSelection(ennemisButton2, false);
                        }

                    }
                    else if (selectedcard.CardTypes == dataCard.CardType.DiabolusEst)
                    {
                        if (!enemiesAtStartOfCombat[1].m_IsAttacking)
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
                }
            });
            ennemisButton3?.onClick.AddListener(() =>
            {
                Deselection(false);
                if (Gm.IsAnyProv)
                {
                    if (selectedcard.CardTypes == dataCard.CardType.HabemusDominum)
                    {
                        if (enemiesAtStartOfCombat[2].m_IsAttacking)
                        {
                            selectedhero.Add(enemiesAtStartOfCombat[2]);
                            switchLightSelection(ennemisButton3, false);
                        }

                    }
                    else if (selectedcard.CardTypes == dataCard.CardType.DiabolusEst)
                    {
                        if (!enemiesAtStartOfCombat[2].m_IsAttacking)
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
                }
                else
                {
                    if (selectedcard.CardTypes == dataCard.CardType.HabemusDominum)
                    {
                        if (enemiesAtStartOfCombat[2].m_IsAttacking)
                        {
                            selectedhero.Add(enemiesAtStartOfCombat[2]);
                            switchLightSelection(ennemisButton3, false);
                        }

                    }
                    else if (selectedcard.CardTypes == dataCard.CardType.DiabolusEst)
                    {
                        if (!enemiesAtStartOfCombat[2].m_IsAttacking)
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
                }
            });


            //ennemisButton1.OnDeselect(clearCardSelected());




        }
        else
        {
            if (selectedcard.AOEEnnemies)
            {
                foreach (hero ennemy in enemiesAtStartOfCombat)
                {
                    if(ennemy.getIsAlive())
                    {
                        selectedhero.Add(ennemy);
                        ActivateSideLights(false);
                    }
                    
                }
            }
        }
        if (!selectedcard.AOEAllies && selectedcard.TargetAllies)
        {

            arboristeButton?.onClick.AddListener(() => { ClearSide(true); switchLightSelection(arboristeButton, true); if (perso2 == true) selectedhero.Add(heroes[1]); else selectedhero.Add(heroes[0]);});
            pretreButton?.onClick.AddListener(() => { switchLightSelection(pretreButton, true); ClearSide(true); selectedhero.Add(heroes[0]); });
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
            
            playCard(selectedhero);
            Deselection(false);
            Gm.deck.PlayCard(Gm.CarteUtilisee.indexHand);
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
                if(Gm.waveCounter == 12)
                {
                    WinFinalFight();
                }
                else
                    WinFight();
            }
        }
/*        PlayEnemyEffects();
        PlayEnemyTurn();
        Gm.isAbsolution = false;
        print("Je passe par la lalalalalaa");*/
    }

    void PlayPlayerEffects() 
    {
        foreach (hero h in heroes)
        {

            for (int i = 0; i < h.MyEffects?.Count; i++)
            {
                dataCard.CardEffect e = h.MyEffects[i];
                if (e.nbTour != 0)
                {
                    CardObject.DamageEffect(h, e.values);
                    e.nbTour--;
                    if (e.nbTour == 0)
                    {
                        h.MyEffects.Remove(e);
                    }
                }
                if (!CheckifHeroAreAlive())
                {
                    LooseFight();
                }
            }
            h.resetArmor();
            UpdateArmorValue(h);
            h.setArmor(h.m_nextArmor);
            print(h.m_armor + "VOILA MON ARMOR");
            h.m_nextArmor = 0;
            UpdateArmorValue(h);
        }


    }
    private void PlayEnemyEffects()
    {
        foreach (hero h in enemies)
        {
            if (h.isAlive)
            {
                if (h.MyEffects?.Count > 0||h.m_total_poison>0)
                {
                    h.takeDamage(h.m_total_poison);
                    StartCoroutine(UpdateLife(h));
                    h.m_total_poison--;
                    UpdatePoisonValue(h);
                }
            }
        }
        for (int i = 0; i < enemiesAtStartOfCombat.Count; i++)
        {
            if (enemiesAtStartOfCombat[i].getPv() <= 0)
            {
                if (i == 0 && ennemisButton1?.IsActive() == true)
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
        }

        if (!CheckifEnemyAreAlive())
        {
            lightsAllies.Clear();
            lightsEnnemies.Clear();
            if (Gm.waveCounter == 12)
            {
                WinFinalFight();
            }
            else
                WinFight();
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

        if (isArboTransform && isApo == false || isPretreTransform && isApo == false)
        {
            isArboTransform = false;
            isPretreTransform = false;
            StartCoroutine(Gm.deck.DetransfoCoroutine());
        }
        else 
            StartCoroutine(Gm.deck.DiscardCoroutine(true));
        
        stock += mana + venerations;
        venerations = 0;
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

    public IEnumerator AllerRetourCombatCorou(GameObject objet, Vector3 transform)
    {
        Vector3 pos = objet.transform.position;
        yield return StartCoroutine(Gm.deck.TransposeAtoB(objet, new Vector3(transform.x+2, transform.y, transform.z)));

        Animator Anim = objet.transform.GetChild(0).GetComponent<Animator>();
        Anim.Play("Chien_Attack");
        Anim.Play("Skeleton_Attack");
        Anim.Play("Gargouille_Attack");
        Anim.Play("Homme_Vers_Attack");
        Anim.Play("Demon_Attack");
        Anim.Play("Dragon_Attack");
        Anim.Play("Main_Geante_Attack");
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(Gm.deck.TransposeAtoB(objet, pos));
    }
    public void AllerRetourCombat(GameObject objet, Vector3 transform)
    {
        StartCoroutine(AllerRetourCombatCorou(objet, transform));
    }
    public IEnumerator AttaqueEnnemiesCorou()
    {
        Gm.AnimAtk = true;
        Gm.deck.EndTurnButton.interactable = false;
        Gm.CardsInteractable = false;
        foreach (hero En in enemiesAtStartOfCombat.ToList())
        {
            ennemyPlaying = En;
            En.EnemyAttack(heroes, false);
            if (!CheckifHeroAreAlive())
            {
                LooseFight();
                yield break;
            }
            foreach (hero h in heroes.ToList())
            {
                if (h.getIsAlive() == false)
                {
                    if (h.m_role == entityManager.Role.Arboriste)
                    {
                        arboristeButton?.onClick.RemoveAllListeners();
                        arboristeButton?.gameObject.SetActive(false);
                        /*h?.gameObject?.SetActive(false);*/
                    }
                    else
                    {
                        pretreButton?.onClick.RemoveAllListeners();
                        pretreButton.gameObject.SetActive(false);
                        /*h?.gameObject?.SetActive(false);*/
                    }
                }
            }
            if (enemies.Count > 1)
            {
                yield return new WaitForSeconds(2f);
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }
        yield return new WaitForSeconds(enemies.Count-(1.5f* (enemies.Count/2)));
        Gm.deck.EndTurnButton.interactable = true;
        Gm.CardsInteractable = true;
        Gm.AnimAtk = false;
    }
    private void PlayEnemyTurn()
    {

        PlayEnemyEffects();
        StartCoroutine(AttaqueEnnemiesCorou());
        if (!CheckifHeroAreAlive())
        {

            LooseFight();
        }
        else
        {
            StartTurn();
        }
        foreach (hero hero in heroes)
        {
            if (hero.m_tabernacleActive)
            {
                CardObject temp = new CardObject();
                temp.Tabernacle(hero);
                Destroy(temp);

            }

            hero.m_dmgTaken = 0;
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
        play.onClick.RemoveAllListeners();
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

        StartCoroutine(LoseFinalFight());
    }
    IEnumerator LoseFinalFight()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        ResetAll();
        Gm.winoulose = false;
        Gm.transi.Play("Transi");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(2);
        Gm.transi.Play("Detransi");
        
        // A appeler lorsqu'on relance
        /*FindObjectOfType<MapManager>().GenerateNewMap();*/
        Gm.SaveData();
    }

    public void WinFinalFight()
    {
        StartCoroutine(WinFinalCorou());

    }

    IEnumerator WinFinalCorou()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        ResetAll();
        Gm.transi.Play("Transi");
        yield return new WaitForSeconds(1.5f);
        Gm.winoulose = true;
        SceneManager.LoadScene(2);
        Gm.transi.Play("Detransi");
        //FindObjectOfType<MapManager>().GenerateNewMap();
        Gm.SaveData();
    }

    IEnumerator XpLerp()
    {
        Gm.CardsInteractable = false;
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
        play.onClick.RemoveAllListeners();
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
        HeroesGameObjectRef.Clear();

        selectedcard = null;
        Gm.SaveData();
        yield return new WaitUntil(() => Input.GetMouseButton(0));
        StartCoroutine(ChangeSceneApresCOmbat());

    }
    IEnumerator ChangeSceneApresCOmbat ()
    {
        Gm.transi.Play("Transi");
        yield return new WaitForSeconds(1.5f);
        Gm.transi.Play("Detransi");
        SceneManager.LoadScene(0);
        test = false;

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
    void playCard(List<hero> selected)
    {
        CardObject card = Gm.CarteUtilisee;
            for (int i = card.DataCard.nombreDexecutiion; i != 0; i--)
            {
                switch (card.DataCard.CardTypes)
                {
                    case dataCard.CardType.Damage:
                        foreach (hero hero in selected)
                        {
                         hero.takeDamage(card.DataCard.m_value);
                        print(hero.m_Pv);
                        StartCoroutine(Gm.CarteUtilisee.UpdateLife(hero));
                        StartCoroutine(DamageNumberCorou(Camera.main.ScreenToWorldPoint(hero.m_slider.transform.position), card.DataCard.m_value));
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
                            hero.setArmor(card.DataCard.m_value); // mettre la valeur de l'armure
                            Gm.FM.UpdateArmorValue(hero);
                        }
                        break;
                    case dataCard.CardType.AddMana:
                        foreach (hero hero in selected)
                        {
                            card.AddMana(1);
                        }
                        break;
                    case dataCard.CardType.AddCard:
                        Gm.deck.DrawCard(1);
                        break;
                    case dataCard.CardType.HabemusDominum:
                        foreach (hero hero in selected)
                        {
                            if (card.DataCard.m_isUpsideDown)
                            {
                                card.DiabolusEst(hero);
                            }
                            if (!card.DataCard.m_isUpsideDown)
                            {
                                card.HabemusDominum(hero);
                            }
                        }
                        break;
                    case dataCard.CardType.CultiverAme:
                        foreach (hero hero in selected)
                        {
                        if (card.DataCard.m_isUpsideDown)
                            {
                                card.CultiverFlamme();
                            }
                            if (!card.DataCard.m_isUpsideDown)
                            {
                                card.CultiverAme(hero);
                            }
                        }
                        break;
                    case dataCard.CardType.Conversion:
                        foreach (hero hero in selected)
                        {
                            if (card.DataCard.m_isUpsideDown)
                            {
                                card.Absolution();
                            }
                            if (!card.DataCard.m_isUpsideDown)
                            {
                                card.Conversion(hero);
                            }
                        }
                        break;
                    case dataCard.CardType.Benediction:
                            if (card.DataCard.m_isUpsideDown)
                            {
                                card.Apotasie();
                            }
                            if (!card.DataCard.m_isUpsideDown)
                            {
                                card.Benediction(selectedhero[0]); // mettre l ennemi selectionner
                            }
                        break;
                    case dataCard.CardType.Tabernacle:
                        foreach (hero hero in selected)
                        {
                            if (card.DataCard.m_isUpsideDown)
                            {
                                card.Belial(hero);
                            }
                            if (!card.DataCard.m_isUpsideDown)
                            {
                                card.Tabernacle(hero);
                            }
                        }
                        break;
                    case dataCard.CardType.VenererIdole:
                        foreach (hero hero in selected)
                        {
                            if (card.DataCard.m_isUpsideDown)
                            {
                                card.Blaspheme(hero);
                            }
                            if (!card.DataCard.m_isUpsideDown)
                            {
                                card.VenererIdole(hero);
                            }
                        }
                        break;
                    case dataCard.CardType.AllumerCierges:
                            if (card.DataCard.m_isUpsideDown)
                            {
                                card.IncendierCloatre();
                            }
                            if (!card.DataCard.m_isUpsideDown)
                            {
                                card.AllumerCierges(heroes[0]);
                            }
                        break;
                    case dataCard.CardType.AccueillirNecessiteux:
                        foreach (hero hero in selected)
                        {
                            if (card.DataCard.m_isUpsideDown)
                            {
                                card.MassacrerInfideles(heroes[0], selectedhero[0]);
                            }
                            if (!card.DataCard.m_isUpsideDown)
                            {
                                card.AccueillirNecessiteux(heroes[0]);
                            }
                        }
                        break;
                    case dataCard.CardType.MoxLion:
                        foreach (hero hero in selected)
                        {
                            if (card.DataCard.m_isUpsideDown)
                            {
                               
                                card.MoxAraignee(heroes[1], selectedhero[0]);
                                
                            }
                            if (!card.DataCard.m_isUpsideDown)
                            {
                                if(perso2)
                                    card.MoxLion(heroes[1]);
                                else
                                    card.MoxLion(heroes[0]);
                            }   
                        }
                        break;
                    case dataCard.CardType.MurDeRonces:
                        foreach (hero hero in selected)
                        {
                            if (card.DataCard.m_isUpsideDown)
                            {
                                card.LaissePourMort();
                            }
                            if (!card.DataCard.m_isUpsideDown)
                            {
                                card.MurDeRonces(hero);
                            }
                        }
                        break;
                    case dataCard.CardType.Cataplasme:
                        foreach (hero hero in selected)
                        {
                            if (card.DataCard.m_isUpsideDown)
                            {
                                card.Belladone(hero);
                            }
                            if (!card.DataCard.m_isUpsideDown)
                            {
                                card.Cataplasme(hero);
                            }
                        }
                        break;
                    case dataCard.CardType.SurgissementVitalique:
                        foreach (hero hero in selected)
                        {
                            if (card.DataCard.m_isUpsideDown)
                            {
                                card.RepandreMort();
                            }
                            if (!card.DataCard.m_isUpsideDown)
                            {
                                card.SurgissementVitalique();
                            }
                        }
                        break;
                    case dataCard.CardType.ArmureEcorse:
                        foreach (hero hero in selected)
                        {
                            if (card.DataCard.m_isUpsideDown)
                            {
                                card.MaleusHerbeticae(hero);
                            }
                            if (!card.DataCard.m_isUpsideDown)
                            {
                                card.ArmureEcorse(hero);
                            }
                        }
                        break;
                    case dataCard.CardType.CommunionNature:
                        if (card.DataCard.m_isUpsideDown)
                        {
                            card.Canibalisme();
                        }
                        if (!card.DataCard.m_isUpsideDown)
                        {
                            card.CommunionNature();
                        }
                        break;
                    case dataCard.CardType.SuivreEtoiles:
                        if (card.DataCard.m_isUpsideDown)
                        {
                            card.ProfanerCiel();
                        }
                        if (!card.DataCard.m_isUpsideDown)
                        {
                            card.SuivreEtoiles();
                        }
                        break;  
                    case dataCard.CardType.DormirPresDeLautre:
                        foreach (hero hero in selected)
                        {
                            if (card.DataCard.m_isUpsideDown)
                            {
                                if(perso2)
                                    card.ReveillerPourManger(heroes[1], selectedhero[0]);
                                else
                                    card.ReveillerPourManger(heroes[0], selectedhero[0]);
                            }
                            if (!card.DataCard.m_isUpsideDown)
                            {
                                card.DormirPresDeLautre(hero);
                                break;
                            }
                        }
                    break;
                }
            }
        foreach(dataCard.CardEffect effect in card.DataCard.CardEffects)
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
    IEnumerator ArmorFade(bool QuelSens, hero hero)
    {
        float valBase;
        float valFin;
        if (QuelSens)
        {
            valBase = 1f;
            valFin = 0;
        }
        else
        {
            valBase = 0;
            valFin = 1f;
        }
        float TempsTransition = 5;
        float timeElapsed = 0;
        hero.Armor.color = new Color(hero.Armor.color.r, hero.Armor.color.g, hero.Armor.color.b, valBase);
        hero.ArmorText.color = new Color(hero.ArmorText.color.r, hero.ArmorText.color.g, hero.ArmorText.color.b, valBase);
        while (timeElapsed < TempsTransition)
        {
            hero.Armor.color = Color.Lerp(hero.Armor.color, new Color(hero.Armor.color.r, hero.Armor.color.g, hero.Armor.color.b, valFin), Time.deltaTime);
            hero.ArmorText.color = Color.Lerp(hero.ArmorText.color, new Color(hero.ArmorText.color.r, hero.ArmorText.color.g, hero.ArmorText.color.b, valFin), Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        hero.Armor.color = new Color(hero.Armor.color.r, hero.Armor.color.g, hero.Armor.color.b, valFin);
        hero.ArmorText.color = new Color(hero.Armor.color.r, hero.Armor.color.g, hero.Armor.color.b, valFin);
    }
    IEnumerator Poison(bool QuelSens, hero hero)
    {
        float valBase;
        float valFin;
        if (QuelSens)
        {
            valBase = 1f;
            valFin = 0;
        }
        else
        {
            valBase = 0;
            valFin = 1f;
        }
        float TempsTransition = 5;
        float timeElapsed = 0;
        hero.m_slider.transform.parent.GetChild(4).GetComponent<Image>().color = new Color(hero.m_slider.transform.parent.GetChild(4).GetComponent<Image>().color.r, hero.m_slider.transform.parent.GetChild(4).GetComponent<Image>().color.g, hero.m_slider.transform.parent.GetChild(4).GetComponent<Image>().color.b, valBase);
        hero.m_slider.transform.parent.GetChild(4).GetChild(0).GetComponent<TMP_Text>().color = new Color(hero.m_slider.transform.parent.GetChild(4).GetChild(0).GetComponent<TMP_Text>().color.r, hero.m_slider.transform.parent.GetChild(4).GetChild(0).GetComponent<TMP_Text>().color.g, hero.m_slider.transform.parent.GetChild(4).GetChild(0).GetComponent<TMP_Text>().color.b, valBase);
        while (timeElapsed < TempsTransition)
        {
            hero.m_slider.transform.parent.GetChild(4).GetComponent<Image>().color = Color.Lerp(hero.m_slider.transform.parent.GetChild(4).GetComponent<Image>().color, new Color(hero.m_slider.transform.parent.GetChild(4).GetComponent<Image>().color.r, hero.m_slider.transform.parent.GetChild(4).GetComponent<Image>().color.g, hero.m_slider.transform.parent.GetChild(4).GetComponent<Image>().color.b, valFin), Time.deltaTime);
            hero.m_slider.transform.parent.GetChild(4).GetChild(0).GetComponent<TMP_Text>().color = Color.Lerp(hero.m_slider.transform.parent.GetChild(4).GetChild(0).GetComponent<TMP_Text>().color, new Color(hero.m_slider.transform.parent.GetChild(4).GetChild(0).GetComponent<TMP_Text>().color.r, hero.m_slider.transform.parent.GetChild(4).GetChild(0).GetComponent<TMP_Text>().color.g, hero.m_slider.transform.parent.GetChild(4).GetChild(0).GetComponent<TMP_Text>().color.b, valFin), Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        hero.m_slider.transform.parent.GetChild(4).GetComponent<Image>().color = new Color(hero.m_slider.transform.parent.GetChild(4).GetComponent<Image>().color.r, hero.m_slider.transform.parent.GetChild(4).GetComponent<Image>().color.g, hero.m_slider.transform.parent.GetChild(4).GetComponent<Image>().color.b, valFin);
        hero.m_slider.transform.parent.GetChild(4).GetChild(0).GetComponent<TMP_Text>().color = new Color(hero.m_slider.transform.parent.GetChild(4).GetComponent<Image>().color.r, hero.m_slider.transform.parent.GetChild(4).GetComponent<Image>().color.g, hero.m_slider.transform.parent.GetChild(4).GetComponent<Image>().color.b, valFin);
    }
    public void UpdateArmorValue(hero hero)
    {
        if (hero.ArmorText != null)
        {
            hero.ArmorText.text = hero.m_armor.ToString();
            if (hero.Armor.color.a < 1 && hero.m_armor != 0)
            {
                StartCoroutine(ArmorFade(false, hero));
            }
            else
            {
                if (hero.Armor.color.a > 0 && hero.m_armor == 0)
                {
                    StartCoroutine(ArmorFade(true, hero));
                }
            }
        }

    }
    public void UpdatePoisonValue(hero hero)
    {
        TMP_Text texte = hero.m_buffs.transform.GetChild(0).GetComponent<TMP_Text>();
        foreach (hero h in enemies)
        {
            if (h.isAlive)
            {
                if (h.MyEffects?.Count > 0 || h.m_total_poison > 0)
                {
                    //dédicace a clément la salope
                    for (int i = 0; i < h.MyEffects?.Count; i++)
                    {
                        h.m_total_poison += h.MyEffects[i].values;
                    }
                    h.MyEffects.Clear();
                }
            }
        }
        if (texte != null)
        {
            
            if (texte.text == "" && hero.m_total_poison>0)
            {
                StartCoroutine(Poison(false, hero));
            }
            else
            {
                if (texte.text != "" && hero.m_total_poison == 0)
                {
                    StartCoroutine(Poison(true, hero));
                    texte.text = "";
                }
            }
        }
        texte.text = hero.m_total_poison.ToString();
    }

    public IEnumerator DamageNumberCorou(GameObject objet, int damage)
    {
        GameObject texte = GameObject.Instantiate(PrefabDmgText);
        texte.transform.position = objet.transform.position;
        PrefabDmgText.transform.GetChild(0).GetComponent<TMP_Text>().text = ""+damage;
        yield return new WaitForSeconds(2);
        Destroy(texte);
    }

    public void DamageNumber(GameObject objet, int damage)
    {
        StartCoroutine(DamageNumberCorou(objet, damage));
    }
    public IEnumerator DamageNumberCorou(Vector3 objet, int damage)
    {
        GameObject texte = GameObject.Instantiate(PrefabDmgText);
        texte.transform.position = objet;
        texte.transform.position = new Vector3(texte.transform.position.x + Random.Range(-1f, 1f), texte.transform.position.y + Random.Range(-1f, 1f), texte.transform.position.z);
        PrefabDmgText.transform.GetChild(0).GetComponent<TMP_Text>().text = "" + damage;
        yield return new WaitForSeconds(2);
        Destroy(texte);
    }

    public void DamageNumber(Vector3 objet, int damage)
    {
        StartCoroutine(DamageNumberCorou(objet, damage));
    }
    public void UpdateLifeAllies()
    {
        foreach(hero hero in heroes)
        {
            StartCoroutine(UpdateLife(hero));
        }
    }
    public IEnumerator UpdateLife(hero hero)
    {
        float TempsTransition = 5f;
        float timeElapsed = 0;
        while (timeElapsed < TempsTransition)
        {
            hero.m_slider.value = Mathf.Lerp(hero.m_slider.value, hero.m_Pv, Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        hero.m_slider.value = hero.m_Pv;
    }
}
