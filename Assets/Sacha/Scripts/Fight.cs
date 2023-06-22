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
using static UnityEngine.Rendering.DebugUI;
using PathCreation.Examples;

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
    private bool isFirstTurn = true;
    private int ennemisTues = 0;
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
            if (SceneManager.GetActiveScene().buildIndex == 2)
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
                if (light != null)
                {
                    light.enabled = false;
                }
                
            }
            ClearSide(true);
        }
        if (!selectedcard.AOEEnnemies || ForceDeselec)
        {
            foreach (Light2D light in lightsEnnemies)
            {
                if (light != null)
                {
                    light.enabled = false;
                }
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
            Light2D lightDuBoutton = Boutton.gameObject.transform.GetChild(8).gameObject.GetComponent<Light2D>();
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
            mana -= Gm.CarteUtilisee.DataCard.m_manaCost; 
            manaText.text = mana.ToString();
            cancel.gameObject.SetActive(false);
            Gm.CarteUtilisee.DisableHealthbar( true);
            DissolveController dissolveController = Gm.CarteUtilisee.GetComponent<DissolveController>();
            Gm.CarteUtilisee.canvas.gameObject.SetActive(false);
            dissolveController.isDissolving = true;
            yield return new WaitUntil(() => dissolveController.dissolveAmount < 0);
            dissolveController.isDissolving = false;
            dissolveController.dissolveAmount = 1;
            Gm.CarteUtilisee.canvas.gameObject.SetActive(false);
            isCardSend = true;
            Gm.deck.EndTurnButton.interactable = true;


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
                H1 = new hero(entityManager.Role.Pretre, 40, 40, 0, 0, null, 0, 0);
                Gm.LifePretre = H1.getPv();
                Gm.IsPretrePlayed = true;
            }
            else
                H1 = new hero(entityManager.Role.Pretre, 40, Gm.LifePretre, 0, 0, null, 0, Gm.levelPretre, Gm.expPretre);
                
            if (Gm.IsArboristePlayed == false)
            {
                H2 = new hero(entityManager.Role.Arboriste, 40, 40, 0, 0, null, 0, 0);
                Gm.LifeArboriste = H2.getPv();
                Gm.IsArboristePlayed = true;
            }
            else
                H2 = new hero(entityManager.Role.Arboriste, 40, Gm.LifeArboriste, 0, 0, null, 0, Gm.levelArboriste, Gm.expArboriste);

            temp = GameObject.Find("champ");
            temp.GetComponent<Image>().sprite = heroSprite;
            arboristeButton = temp.GetComponent<Button>();
            //ChangerBouttonEnGameObject(arboristeButton, heroSprite, true);
            ChangerBouttonEnGameObject(arboristeButton, PrefabHeroes[0], true, 0.16f, PrefabHeroesalt[0]);
            H2.m_slider = temp.GetComponentInChildren<Slider>();
            H2.m_slider.maxValue = 40;
            H2.m_slider.value = H2.getPv();
            H2.pvText = GameObject.Find("pvChamp").GetComponent<TextMeshProUGUI>();
            H2.pvText.text = H2.getPv().ToString() + " / " + H2.getMaxPv().ToString();
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
            H1.pvText = GameObject.Find("pvChamp2").GetComponent<TextMeshProUGUI>();
            H1.pvText.text = H1.getPv().ToString() + " / " + H1.getMaxPv().ToString();
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
                H2 = new hero(entityManager.Role.Arboriste, 40, 40, 0, 0, null, 0, 0);
                Gm.LifeArboriste = H2.getPv();
                Gm.IsArboristePlayed = true;
            }
            else
                H2 = new hero(entityManager.Role.Arboriste, 40, Gm.LifeArboriste, 0, 0, null, 0, Gm.levelArboriste, Gm.expArboriste);


            H2.m_slider = temp.GetComponentInChildren<Slider>();
            H2.m_slider.maxValue = H2.getMaxPv();
            H2.m_slider.value = H2.getPv();
            H2.pvText = GameObject.Find("pvChampSolo").GetComponent<TextMeshProUGUI>();
            H2.pvText.text = H2.getPv().ToString() + " / " + H2.getMaxPv().ToString();
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
                H1 = new hero(entityManager.Role.Pretre, 40, 40, 0, 0, null, 0, 0);
                Gm.LifePretre = H1.getPv();
                Gm.IsPretrePlayed = true;
            }
            else
                H1 = new hero(entityManager.Role.Pretre, 40, Gm.LifePretre, 0, 0, null, 0, Gm.levelPretre, Gm.expPretre);

            H1.m_slider = temp.GetComponentInChildren<Slider>();
            H1.m_slider.maxValue = H1.getMaxPv();
            H1.m_slider.value = H1.getPv();
            H1.pvText = GameObject.Find("pvChampSolo").GetComponent<TextMeshProUGUI>();
            H1.pvText.text = H1.getPv().ToString() + " / " + H1.getMaxPv().ToString();
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
            temp.GetComponent<Image>().rectTransform.sizeDelta = Gm.allWave[Gm.waveCounter][waveType][0].m_sprite.bounds.size * 100f;
            ennemisButton1 = temp.GetComponent<Button>();
            en1.m_slider = temp.GetComponentInChildren<Slider>();
            en1.m_slider.maxValue = en1.getMaxPv();
            en1.m_slider.value = en1.getPv();
            en1.pvText = GameObject.Find("pvEn").GetComponent<TextMeshProUGUI>();
            en1.pvText.text = en1.getPv().ToString() + " / " + en1.getMaxPv().ToString();
            en1.pvText = GameObject.Find("pvEn").GetComponent<TextMeshProUGUI>();
            en1.ArmorText = GameObject.Find("shieldTextEn").GetComponent<TextMeshProUGUI>();
            en1.ArmorText.text = en1.getArmor().ToString();
            en1.Armor = GameObject.Find("shieldEn").GetComponent<Image>();
            
            en1.m_valueText = temp.GetComponentInChildren<TextMeshProUGUI>();
            en1.m_spriteFocus = temp.GetComponentsInChildren<Image>()[3];
            en1.m_spriteTypeAttack = temp.GetComponentsInChildren<Image>()[4];
            en1.m_buffs = en1.m_slider.transform.parent.GetChild(4);
            en1.setIsAlive(true);


            switch (Gm.allWave[Gm.waveCounter][waveType][0].m_role)
            {
                case entityManager.Role.ChienEnemy:
                    ChangerBouttonEnGameObject(ennemisButton1, Gm.allWave[Gm.waveCounter][waveType][0].prefab, false, 0.2f);
                    break;
                case entityManager.Role.Mains:
                    ChangerBouttonEnGameObject(ennemisButton1, Gm.allWave[Gm.waveCounter][waveType][0].prefab, false, 0.2f);
                    break;
                case entityManager.Role.Dragon:
                    ChangerBouttonEnGameObject(ennemisButton1, Gm.allWave[Gm.waveCounter][waveType][0].prefab, false, 0.05f);
                    break;

                case entityManager.Role.Gargouilles:
                    ChangerBouttonEnGameObject(ennemisButton1, Gm.allWave[Gm.waveCounter][waveType][0].prefab, false,0.05f);
                    break;
                case entityManager.Role.Demon:
                    ChangerBouttonEnGameObject(ennemisButton1, Gm.allWave[Gm.waveCounter][waveType][0].prefab, false, 0.05f);
                    break;
                default:
                    ChangerBouttonEnGameObject(ennemisButton1, Gm.allWave[Gm.waveCounter][waveType][0].prefab, false);
                    break;
            }


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
            en1.pvText = GameObject.Find("pvEn").GetComponent<TextMeshProUGUI>();
            en1.pvText.text = en1.getPv().ToString() + " / " + en1.getMaxPv().ToString();
            en1.m_valueText = temp.GetComponentInChildren<TextMeshProUGUI>();
            en1.ArmorText = GameObject.Find("shieldTextEn").GetComponent<TextMeshProUGUI>();
            en1.Armor = GameObject.Find("shieldEn").GetComponent<Image>();

            en1.ArmorText.text = en1.getArmor().ToString();
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
            En2.pvText = GameObject.Find("pvEn2").GetComponent<TextMeshProUGUI>();
            En2.pvText.text = En2.getPv().ToString() + " / " + En2.getMaxPv().ToString();
            En2.m_valueText = temp.GetComponentInChildren<TextMeshProUGUI>();
            En2.Armor = GameObject.Find("shieldEn").GetComponent<Image>();

            En2.ArmorText = GameObject.Find("shieldTextEn2").GetComponent<TextMeshProUGUI>();
            En2.ArmorText.text = En2.getArmor().ToString();
            En2.m_spriteFocus = temp.GetComponentsInChildren<Image>()[3];
            En2.m_spriteTypeAttack = temp.GetComponentsInChildren<Image>()[4];
            En2.m_buffs = En2.m_slider.transform.parent.GetChild(4);
            En2.setIsAlive(true);

            switch (Gm.allWave[Gm.waveCounter][waveType][1].m_role)
            {
                case entityManager.Role.ChienEnemy:
                    ChangerBouttonEnGameObject(ennemisButton2, Gm.allWave[Gm.waveCounter][waveType][1].prefab, false, 0.2f);
                    break;
                case entityManager.Role.Mains:
                    ChangerBouttonEnGameObject(ennemisButton2, Gm.allWave[Gm.waveCounter][waveType][1].prefab, false, 0.2f);
                    break;
                case entityManager.Role.Dragon:
                    ChangerBouttonEnGameObject(ennemisButton2, Gm.allWave[Gm.waveCounter][waveType][1].prefab, false, 0.05f);
                    break;

                case entityManager.Role.Gargouilles:
                    ChangerBouttonEnGameObject(ennemisButton2, Gm.allWave[Gm.waveCounter][waveType][1].prefab, false, 0.05f);
                    break;
                case entityManager.Role.Demon:
                    ChangerBouttonEnGameObject(ennemisButton2, Gm.allWave[Gm.waveCounter][waveType][1].prefab, false, 0.05f);
                    break;
                default:
                    ChangerBouttonEnGameObject(ennemisButton2, Gm.allWave[Gm.waveCounter][waveType][1].prefab, false);
                    break;
            }

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
            en1.pvText = GameObject.Find("pvEn").GetComponent<TextMeshProUGUI>();
            en1.pvText.text = en1.getPv().ToString() + " / " + en1.getMaxPv().ToString();
            en1.m_valueText = temp.GetComponentInChildren<TextMeshProUGUI>();
            en1.ArmorText = GameObject.Find("shieldTextEn").GetComponent<TextMeshProUGUI>();
            en1.ArmorText.text = en1.getArmor().ToString();
            en1.Armor = GameObject.Find("shieldEn").GetComponent<Image>();

            en1.m_spriteFocus = temp.GetComponentsInChildren<Image>()[3];
            en1.m_spriteTypeAttack = temp.GetComponentsInChildren<Image>()[4];
            en1.m_buffs = en1.m_slider.transform.parent.GetChild(4);
            en1.setIsAlive(true);

            switch (Gm.allWave[Gm.waveCounter][waveType][0].m_role)
            {
                case entityManager.Role.ChienEnemy:
                    ChangerBouttonEnGameObject(ennemisButton1, Gm.allWave[Gm.waveCounter][waveType][0].prefab, false, 0.2f);
                    break;
                case entityManager.Role.Mains:
                    ChangerBouttonEnGameObject(ennemisButton1, Gm.allWave[Gm.waveCounter][waveType][0].prefab, false, 0.2f);
                    break;
                case entityManager.Role.Dragon:
                    ChangerBouttonEnGameObject(ennemisButton1, Gm.allWave[Gm.waveCounter][waveType][0].prefab, false, 0.05f);
                    break;

                case entityManager.Role.Gargouilles:
                    ChangerBouttonEnGameObject(ennemisButton1, Gm.allWave[Gm.waveCounter][waveType][0].prefab, false, 0.05f);
                    break;
                case entityManager.Role.Demon:
                    ChangerBouttonEnGameObject(ennemisButton1, Gm.allWave[Gm.waveCounter][waveType][0].prefab, false, 0.05f);
                    break;
                default:
                    ChangerBouttonEnGameObject(ennemisButton1, Gm.allWave[Gm.waveCounter][waveType][0].prefab, false);
                    break;
            }

            En2 = Gm.allWave[Gm.waveCounter][waveType][1].SetEnemy();
            temp = GameObject.Find("enemy2");
            temp.GetComponent<Image>().sprite = ennemy2Sprite;
            ennemisButton2 = temp.GetComponent<Button>();
            En2.m_slider = temp.GetComponentInChildren<Slider>();
            En2.m_slider.maxValue = En2.getMaxPv();
            En2.m_slider.value = En2.getPv();
            En2.pvText = GameObject.Find("pvEn2").GetComponent<TextMeshProUGUI>();
            En2.pvText.text = En2.getPv().ToString() + " / " + En2.getMaxPv().ToString();
            En2.m_valueText = temp.GetComponentInChildren<TextMeshProUGUI>();
            En2.m_valueText = temp.GetComponentInChildren<TextMeshProUGUI>();
            En2.ArmorText = GameObject.Find("shieldTextEn2").GetComponent<TextMeshProUGUI>();
            En2.Armor = GameObject.Find("shieldEn2").GetComponent<Image>();

            En2.m_spriteFocus = temp.GetComponentsInChildren<Image>()[3];
            En2.m_spriteTypeAttack = temp.GetComponentsInChildren<Image>()[4];
            En2.m_buffs = En2.m_slider.transform.parent.GetChild(4);
            En2.setIsAlive(true);

            switch (Gm.allWave[Gm.waveCounter][waveType][1].m_role)
            {
                case entityManager.Role.ChienEnemy:
                    ChangerBouttonEnGameObject(ennemisButton2, Gm.allWave[Gm.waveCounter][waveType][1].prefab, false, 0.2f);
                    break;
                case entityManager.Role.Mains:
                    ChangerBouttonEnGameObject(ennemisButton2, Gm.allWave[Gm.waveCounter][waveType][1].prefab, false, 0.2f);
                    break;
                case entityManager.Role.Dragon:
                    ChangerBouttonEnGameObject(ennemisButton2, Gm.allWave[Gm.waveCounter][waveType][1].prefab, false, 0.05f);
                    break;

                case entityManager.Role.Gargouilles:
                    ChangerBouttonEnGameObject(ennemisButton2, Gm.allWave[Gm.waveCounter][waveType][1].prefab, false, 0.05f);
                    break;
                case entityManager.Role.Demon:
                    ChangerBouttonEnGameObject(ennemisButton2, Gm.allWave[Gm.waveCounter][waveType][1].prefab, false, 0.05f);
                    break;
                default:
                    ChangerBouttonEnGameObject(ennemisButton2, Gm.allWave[Gm.waveCounter][waveType][1].prefab, false);
                    break;
            }


            En3 = Gm.allWave[Gm.waveCounter][waveType][2].SetEnemy();
            temp = GameObject.Find("enemy3");
            temp.GetComponent<Image>().sprite = ennemy2Sprite;
            ennemisButton3 = temp.GetComponent<Button>();
            En3.m_slider = temp.GetComponentInChildren<Slider>();
            En3.m_slider.maxValue = En3.getMaxPv();
            En3.m_slider.value = En3.getPv();
            En3.pvText = GameObject.Find("pvEn3").GetComponent<TextMeshProUGUI>();
            En3.pvText.text = En3.getPv().ToString() + " / " + En3.getMaxPv().ToString();
            En3.ArmorText = GameObject.Find("shieldTextEn3").GetComponent<TextMeshProUGUI>();
            En3.ArmorText.text = En3.getArmor().ToString();
            En3.m_valueText = temp.GetComponentInChildren<TextMeshProUGUI>();
            En3.Armor = GameObject.Find("shieldEn3").GetComponent<Image>();

            En3.m_spriteFocus = temp.GetComponentsInChildren<Image>()[3];
            En3.m_spriteTypeAttack = temp.GetComponentsInChildren<Image>()[4];
            En3.m_buffs = En3.m_slider.transform.parent.GetChild(4);
            En3.setIsAlive(true);

            switch (Gm.allWave[Gm.waveCounter][waveType][2].m_role)
            {
                case entityManager.Role.ChienEnemy:
                    ChangerBouttonEnGameObject(ennemisButton3, Gm.allWave[Gm.waveCounter][waveType][2].prefab, false, 0.2f);
                    break;
                case entityManager.Role.Mains:
                    ChangerBouttonEnGameObject(ennemisButton3, Gm.allWave[Gm.waveCounter][waveType][2].prefab, false, 0.2f);
                    break;
                case entityManager.Role.Dragon:
                    ChangerBouttonEnGameObject(ennemisButton3, Gm.allWave[Gm.waveCounter][waveType][2].prefab, false, 0.05f);
                    break;

                case entityManager.Role.Gargouilles:
                    ChangerBouttonEnGameObject(ennemisButton3, Gm.allWave[Gm.waveCounter][waveType][2].prefab, false, 0.05f);
                    break;
                case entityManager.Role.Demon:
                    ChangerBouttonEnGameObject(ennemisButton3, Gm.allWave[Gm.waveCounter][waveType][2].prefab, false, 0.05f);
                    break;
                default:
                    ChangerBouttonEnGameObject(ennemisButton3, Gm.allWave[Gm.waveCounter][waveType][2].prefab, false);
                    break;
            }

        }




        #endregion

        List<CardObject> list = new List<CardObject>();
        list = FindObjectsOfType<CardObject>().ToList();
        foreach (CardObject c in list)
        {
            c.DataCard.m_isUpsideDown = false;
            c.gameObject.SetActive(false);
        }
        GameObject.FindGameObjectWithTag("pretreLight").GetComponent<Light2D>().enabled = false;
        GameObject.FindGameObjectWithTag("arboLight").GetComponent<Light2D>().enabled = false;

        StartTurn();
        

    }
    void StartTurn()
    {
        //endTurnButton.GetComponentInChildren<TextMeshProUGUI>().text = "End Turn";
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
                            
                arboristeButton.onClick.AddListener(() => { StartCoroutine(Gm.deck.TransfoCoroutine(true)); E.setMana(0); E.stockText.text = E.getMana().ToString() + " / " + E.m_manaMax; isArboTransform = true; arboristeButton.onClick.RemoveAllListeners(); nbTransfo++; E.isFull = false; Gm.TranscendanceAchivement(); GameObject.FindGameObjectWithTag("arboLight").GetComponent<Light2D>().enabled = false; });

            }
            else if (E.isFull && E.getIsAlive() && E.m_role == entityManager.Role.Pretre)
            {
                pretreButton.interactable = true;
                pretreButton.onClick.AddListener(() => { StartCoroutine(Gm.deck.TransfoCoroutine(false)); E.setMana(0); E.stockText.text = E.getMana().ToString() + " / " + E.m_manaMax ; isPretreTransform = true; pretreButton.onClick.RemoveAllListeners(); nbTransfo++; E.isFull = false; Gm.TranscendanceAchivement(); GameObject.FindGameObjectWithTag("pretreLight").GetComponent<Light2D>().enabled = false; });
                

            }
            if (isPretreTransform && isArboTransform)
                Gm.TranscendanceBothHeroAchivement();
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
        
        if(isFirstTurn)
        {
            foreach (hero En in enemiesAtStartOfCombat.ToList())
            {
                ennemyPlaying = En;
                En.EnemyAttack(heroes, true);
            }
            isFirstTurn = false;
        }

    }




    public void Cardsend(CardObject card, int index)
    {
        
        pretreButton?.onClick.RemoveAllListeners();
        arboristeButton?.onClick.RemoveAllListeners();
        card.DataCard.m_index = card.indexHand;
        if (card.DataCard.m_isUpsideDown)
        {
            selectedcard = card.DataCard.BackCard;
        }
        else
        {
            selectedcard = card.DataCard;

        }
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
        
        //play.onClick.AddListener(() => { if (conditionjouer) StartCoroutine(CardAnimDisolve()); });
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
                                StartCoroutine(CardAnimDisolve());
                                switchLightSelection(ennemisButton1, false);
                            }
                                
                        }
                        else if (selectedcard.CardTypes == dataCard.CardType.DiabolusEst)
                        {
                            if (!enemiesAtStartOfCombat[0].m_IsAttacking)
                            {
                                selectedhero.Add(enemiesAtStartOfCombat[0]);
                                StartCoroutine(CardAnimDisolve());
                                switchLightSelection(ennemisButton1, false);
                            }

                        }
                        else
                        {
                            selectedhero.Add(enemiesAtStartOfCombat[0]);
                            StartCoroutine(CardAnimDisolve());
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
                            StartCoroutine(CardAnimDisolve());
                            switchLightSelection(ennemisButton1, false);
                        }

                    }
                    else if (selectedcard.CardTypes == dataCard.CardType.DiabolusEst)
                    {
                        if (!enemiesAtStartOfCombat[0].m_IsAttacking)
                        {
                            selectedhero.Add(enemiesAtStartOfCombat[0]);
                            StartCoroutine(CardAnimDisolve());
                            switchLightSelection(ennemisButton1, false);
                        }

                    }
                    else
                    {
                        selectedhero.Add(enemiesAtStartOfCombat[0]);
                        StartCoroutine(CardAnimDisolve());
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
                            StartCoroutine(CardAnimDisolve());
                            switchLightSelection(ennemisButton2, false);
                        }

                    }
                    else if (selectedcard.CardTypes == dataCard.CardType.DiabolusEst)
                    {
                        if (!enemiesAtStartOfCombat[1].m_IsAttacking)
                        {
                            selectedhero.Add(enemiesAtStartOfCombat[1]);
                            StartCoroutine(CardAnimDisolve());
                            switchLightSelection(ennemisButton2, false);
                        }

                    }
                    else
                    {
                        selectedhero.Add(enemiesAtStartOfCombat[1]);
                        StartCoroutine(CardAnimDisolve());
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
                            StartCoroutine(CardAnimDisolve());
                            switchLightSelection(ennemisButton2, false);
                        }

                    }
                    else if (selectedcard.CardTypes == dataCard.CardType.DiabolusEst)
                    {
                        if (!enemiesAtStartOfCombat[1].m_IsAttacking)
                        {
                            selectedhero.Add(enemiesAtStartOfCombat[1]);
                            StartCoroutine(CardAnimDisolve());
                            switchLightSelection(ennemisButton2, false);
                        }

                    }

                    else
                    {
                        selectedhero.Add(enemiesAtStartOfCombat[1]);
                        StartCoroutine(CardAnimDisolve());
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
                            StartCoroutine(CardAnimDisolve());
                            switchLightSelection(ennemisButton3, false);
                        }

                    }
                    else if (selectedcard.CardTypes == dataCard.CardType.DiabolusEst)
                    {
                        if (!enemiesAtStartOfCombat[2].m_IsAttacking)
                        {
                            selectedhero.Add(enemiesAtStartOfCombat[2]);
                            StartCoroutine(CardAnimDisolve());
                            switchLightSelection(ennemisButton3, false);
                        }

                    }
                    else
                    {
                        selectedhero.Add(enemiesAtStartOfCombat[2]);
                        StartCoroutine(CardAnimDisolve());
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
                StartCoroutine(CardAnimDisolve());
            }
        }
        if (!selectedcard.AOEAllies && selectedcard.TargetAllies)
        {
            arboristeButton?.onClick.AddListener(() => { ClearSide(true); switchLightSelection(arboristeButton, true); if (perso2 == true) selectedhero.Add(heroes[1]); else selectedhero.Add(heroes[0]); StartCoroutine(CardAnimDisolve()); });
            pretreButton?.onClick.AddListener(() => { switchLightSelection(pretreButton, true); ClearSide(true); selectedhero.Add(heroes[0]); StartCoroutine(CardAnimDisolve()); });
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
                StartCoroutine(CardAnimDisolve());
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
            }

            foreach (hero E in heroes)
            {
                if (E.isFull && E.getIsAlive() && E.m_role == entityManager.Role.Arboriste)
                {
                    arboristeButton?.onClick.RemoveAllListeners();
                    arboristeButton.interactable = true;
                    arboristeButton.onClick.AddListener(() => { StartCoroutine(Gm.deck.TransfoCoroutine(true)); E.setMana(0); E.stockText.text = E.getMana().ToString() + " / " + E.m_manaMax; isArboTransform = true; arboristeButton.onClick.RemoveAllListeners(); nbTransfo++; E.isFull = false; Gm.TranscendanceAchivement(); });

                }
                else if (E.isFull && E.getIsAlive() && E.m_role == entityManager.Role.Pretre)
                {
                    pretreButton?.onClick.RemoveAllListeners();
                    pretreButton.interactable = true;
                    pretreButton.onClick.AddListener(() => { StartCoroutine(Gm.deck.TransfoCoroutine(false)); E.setMana(0); E.stockText.text = E.getMana().ToString() + " / " + E.m_manaMax; isPretreTransform = true; pretreButton.onClick.RemoveAllListeners(); nbTransfo++; E.isFull = false; Gm.TranscendanceAchivement(); });


                }
            }

            if (isPretreTransform && isArboTransform)
                Gm.TranscendanceBothHeroAchivement();

            if (!CheckifEnemyAreAlive())
            {
                lightsAllies.Clear();
                lightsEnnemies.Clear();
                if(Gm.waveCounter == 11)
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
            if (h.ArmorText != null)
                h.ArmorText.text = h.getArmor().ToString();

            if (h.m_role == entityManager.Role.Arboriste || h.m_role == entityManager.Role.Pretre)
                UpdateArmorValue(h);

            h.setArmor(h.m_nextArmor);
            print(h.m_armor + "VOILA MON ARMOR");
            h.m_nextArmor = 0;

            if (h.ArmorText != null)
                h.ArmorText.text = h.getArmor().ToString();

            if(h.m_role == entityManager.Role.Arboriste || h.m_role == entityManager.Role.Pretre)
            {
                UpdateArmorValue(h);

            }
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
            if (Gm.waveCounter == 11)
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
    public IEnumerator animMana()
    {

        FindObjectOfType<ParticleSystem>().GetComponent<PathFollower>().addPath();

        yield return new WaitForSeconds(0.85f);
        FindObjectOfType<ParticleSystem>().GetComponent<PathFollower>().removePath();
        stockText.text = stock.ToString();
        endTurnButton?.onClick.RemoveAllListeners();
        endTurnButton?.onClick.AddListener(() => { chargeMana(3); });


        foreach (hero h in heroes)
        {
            if (h.m_role == hero.Role.Arboriste && h.getMana() != h.m_manaMax)
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


        yield return null;
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
        manaText.text = mana.ToString();

        
        StartCoroutine(animMana());
        
    }

    public void chargeMana(int x)
    {
        if (x == 0)
        {
            foreach (hero h in heroes)
            {
                
                if (h.m_role == hero.Role.Arboriste)
                {
                    while(stock >= 1)
                    {
                        h.setMana(h.getMana() + 1);
                        stock--;
                        if (h.getMana() == h.m_manaMax)
                        {
                            h.isFull = true;
                            GameObject.FindGameObjectWithTag("arboLight").GetComponent<Light2D>().enabled = true;
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
                    

                    while (stock >= 1)
                    {
                        h.setMana(h.getMana() + 1);
                        stock--;
                        if (h.getMana() == h.m_manaMax)
                        {
                            h.isFull = true;
                            GameObject.FindGameObjectWithTag("pretreLight").GetComponent<Light2D>().enabled = true;
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
        yield return StartCoroutine(Gm.deck.TransposeAtoB(objet, new Vector3(transform.x+2, transform.y-3.5f, transform.z)));

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
            En.resetArmor();
            En.ArmorText.text = En.getArmor().ToString();

            ennemyPlaying = En;
            En.EnemyAttack(heroes, false);
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

            

            if (!CheckifHeroAreAlive())
            {
                yield return new WaitForSeconds(3f);
                LooseFight();
                yield break;
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
        foreach (hero En in enemiesAtStartOfCombat.ToList())
        {
            ennemyPlaying = En;
            En.EnemyAttack(heroes, true);
        }

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
    private void PlayEnemyTurn()
    {
        PlayEnemyEffects();
        StartCoroutine(AttaqueEnnemiesCorou());
        
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
        Gm.LifeArboriste = 40;
        Gm.LifePretre = 40;
        Gm.IsArboristePlayed = false;
        Gm.IsPretrePlayed = false;
        Gm.waveCounter = 0;
        Gm.IsAnyProv = false;
        Gm.CarteUtilisee = null;
        Gm.CardsInteractable = true;
        Gm.HasCardInHand = false;
        Gm.debuffDraw = 0;
        Gm.isHoverButton = false;
        Gm.needToResetMap = true;
        enemiesAtStartOfCombat.Clear();
        HeroesGameObjectRef.Clear();
        HeroesAltGameObjectRef.Clear();
        
        Gm.SaveData();
    }
    private void LooseFight()
    {
        Gm.DeathAchivement();
        StartCoroutine(LoseFinalFight());
    }
    IEnumerator LoseFinalFight()
    {
        
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        isFirstTurn = true;
        isApo = false;
        isCanibalisme = false;
        isProf = false;
        ResetAll();
        Gm.winoulose = false;
        Gm.transi.Play("Transi");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(3);
        Gm.transi.Play("Detransi");
        
    }

    public void WinFinalFight()
    {
        Gm.WinAchivement();
        Gm.RestAchivement();
        StartCoroutine(WinFinalCorou());

    }

    IEnumerator WinFinalCorou()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        isFirstTurn = true;
        isApo = false;
        isCanibalisme = false;
        isProf = false;

        ResetAll();
        Gm.SaveData();
        Gm.transi.Play("Transi");
        yield return new WaitForSeconds(1.5f);
        Gm.winoulose = true;
        SceneManager.LoadScene(3);
        Gm.transi.Play("Detransi");
        //FindObjectOfType<MapManager>().GenerateNewMap();
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
            hero.gainExperience((int)((4 * enemiesAtStartOfCombat.Count)/heroes.Count));
            if (hero.m_role == entityManager.Role.Arboriste)
            {
                lvlUpDruid = Gm.levelArboriste > lvldruid;
            }
            else
            {
                lvlUpPriest = Gm.levelPretre > lvlpriest;
            }
            if (Gm.levelArboriste >= 6 && Gm.levelPretre >= 6)
                Gm.PeakPerformanceAchivement();
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
                Gm.deck.SlidersXp[2].transform.GetChild(3).GetComponent<TMP_Text>().text = Mathf.Round(Gm.deck.SlidersXp[2].value) + "/" + Mathf.Round(Gm.deck.SlidersXp[2].maxValue);
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
                Gm.deck.SlidersXp[0].transform.GetChild(3).GetComponent<TMP_Text>().text = Mathf.Round(Gm.deck.SlidersXp[0].value) + "/" + Mathf.Round(Gm.deck.SlidersXp[0].maxValue);
                Gm.deck.SlidersXp[1].transform.GetChild(3).GetComponent<TMP_Text>().text = Mathf.Round(Gm.deck.SlidersXp[1].value) + "/" + Mathf.Round(Gm.deck.SlidersXp[1].maxValue);
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
                        Gm.deck.SlidersXp[2].transform.GetChild(4).GetComponent<TMP_Text>().text = "LVL " + Gm.levelArboriste;
                    }
                }
                else
                {
                    if (lvlUpPriest)
                    {
                        Gm.deck.SlidersXp[2].value = 0;
                        Gm.deck.SlidersXp[2].transform.GetChild(4).GetComponent<TMP_Text>().text = "LVL " + Gm.levelPretre;
                    }

                }
                Gm.deck.SlidersXp[2].maxValue = heroes[0].getexperienceMAX();
                Gm.deck.SlidersXp[2].transform.GetChild(3).GetComponent<TMP_Text>().text = Mathf.Round(Gm.deck.SlidersXp[2].value) + "/" + Mathf.Round(Gm.deck.SlidersXp[2].maxValue);
            }
            else
            {
                if (lvlUpDruid)
                {
                    Gm.deck.SlidersXp[0].value = 0;
                    Gm.deck.SlidersXp[0].maxValue = heroes[1].getexperienceMAX();

                }
                if (lvlUpPriest)
                {
                    Gm.deck.SlidersXp[1].value = 0;
                    Gm.deck.SlidersXp[1].maxValue = heroes[0].getexperienceMAX();
                }
                Gm.deck.SlidersXp[0].transform.GetChild(3).GetComponent<TMP_Text>().text = Mathf.Round(Gm.deck.SlidersXp[0].value) + "/" + Mathf.Round(Gm.deck.SlidersXp[0].maxValue);
                Gm.deck.SlidersXp[1].transform.GetChild(3).GetComponent<TMP_Text>().text = Mathf.Round(Gm.deck.SlidersXp[1].value) + "/" + Mathf.Round(Gm.deck.SlidersXp[1].maxValue);
                Gm.deck.SlidersXp[0].transform.GetChild(4).GetComponent<TMP_Text>().text = "LVL " + Gm.levelArboriste;
                Gm.deck.SlidersXp[1].transform.GetChild(4).GetComponent<TMP_Text>().text = "LVL " + Gm.levelPretre;
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
                    Gm.deck.SlidersXp[2].transform.GetChild(3).GetComponent<TMP_Text>().text = Mathf.Round(Gm.deck.SlidersXp[2].value) + "/" + Mathf.Round(Gm.deck.SlidersXp[2].maxValue);
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
                    Gm.deck.SlidersXp[0].transform.GetChild(3).GetComponent<TMP_Text>().text = Mathf.Round(Gm.deck.SlidersXp[0].value) + "/" + Mathf.Round(Gm.deck.SlidersXp[0].maxValue);
                    Gm.deck.SlidersXp[1].transform.GetChild(3).GetComponent<TMP_Text>().text = Mathf.Round(Gm.deck.SlidersXp[1].value) + "/" + Mathf.Round(Gm.deck.SlidersXp[1].maxValue);

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
            Gm.deck.SlidersXp[2].transform.GetChild(3).GetComponent<TMP_Text>().text = Mathf.Round(Gm.deck.SlidersXp[2].value) + "/" + Mathf.Round(Gm.deck.SlidersXp[2].maxValue);
        }
        else
        {
            Gm.deck.SlidersXp[0].value = Gm.expArboriste;
            Gm.deck.SlidersXp[1].value = Gm.expPretre;
            Gm.deck.SlidersXp[0].transform.GetChild(3).GetComponent<TMP_Text>().text = Mathf.Round(Gm.deck.SlidersXp[0].value) + "/" + Mathf.Round(Gm.deck.SlidersXp[0].maxValue);
            Gm.deck.SlidersXp[1].transform.GetChild(3).GetComponent<TMP_Text>().text = Mathf.Round(Gm.deck.SlidersXp[1].value) + "/" + Mathf.Round(Gm.deck.SlidersXp[1].maxValue);
        }
        #endregion
        yield return new WaitUntil(() => Input.GetMouseButton(0));
        Gm.waveCounter++;
        Gm.Hand.Clear();
        Gm.deck = null;
        Gm.entityManager.getListHero().Clear();
        ennemisButton1?.onClick.RemoveAllListeners();
        ennemisButton2?.onClick.RemoveAllListeners();
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
        HeroesGameObjectRef.Clear();
        HeroesAltGameObjectRef.Clear();

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
        SceneManager.LoadScene(1);
        test = false;

    }
    private void WinFight()
    {
        if(perso1 && !perso2)
        {
            if (heroes[0].m_role == entityManager.Role.Pretre)
                Gm.MiracleAchivement();
            else if (heroes[0].m_role == entityManager.Role.Arboriste)
                Gm.GrowthAchivement();
        }

        StopCoroutine(coroutine);
        Gm.deck.AfficheSideUiXP(perso1 && perso2 && GameManager.Instance.LifeArboriste>0 && GameManager.Instance.LifePretre > 0);
        Gm.deck.SetBonneBarreXp(heroes);
        Debug.Log("WIIIIIIIIIIIIIIIIIIIIIIIIIIIIN");
        isFirstTurn = true;
        isApo = false;
        isCanibalisme = false;
        isProf = false;
        StartCoroutine(XpLerp());
        MapPlayerTracker.Instance.setPlayerToNode(MapPlayerTracker.Instance._currentNode);
        MapPlayerTracker.Instance.mapManager.SaveMap();
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
                            if(hero.ArmorText != null)
                                hero.ArmorText.text = hero.m_armor.ToString();

                            if(hero.m_role == entityManager.Role.Pretre || hero.m_role == entityManager.Role.Arboriste)
                                Gm.FM.UpdateArmorValue(hero);

                            if (Gm.isAbsolution)
                            {
                                foreach (hero enemy in enemiesAtStartOfCombat)
                                {
                                    if (enemy.getIsAlive())
                                    {
                                        enemy.takeDamage(card.DataCard.m_value);
                                    }
                                }

                            }
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
                        if (card.DataCard.m_isUpsideDown)
                        {
                            card.CultiverFlamme();
                        }
                        foreach (hero hero in selected)
                        {
                        
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
                            
                            if (!card.DataCard.m_isUpsideDown)
                            {
                                card.VenererIdole(hero);
                            }
                            if (card.DataCard.m_isUpsideDown)
                            {
                                card.Blaspheme(hero);
                            }
                        }
                        break;
                    case dataCard.CardType.AllumerCierges:
                            if (card.DataCard.m_isUpsideDown)
                            {
                                card.IncendierCloatre();
                            }
                            foreach (hero hero in selected)
                            {
                                if (!card.DataCard.m_isUpsideDown)
                                {
                                    card.AllumerCierges(hero);
                                }
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
                                card.AccueillirNecessiteux(hero);
                            }
                        }
                        break;
                    case dataCard.CardType.MoxLion:
                        foreach (hero hero in selected)
                        {
                            if (card.DataCard.m_isUpsideDown)
                            {
                                if (perso2)
                                    card.MoxAraignee(heroes[1], selectedhero[0]);

                                else
                                    card.MoxAraignee(heroes[0], selectedhero[0]);

                            }

                            if (!card.DataCard.m_isUpsideDown)
                            {
                                card.MoxLion(hero);
                            }   
                        }
                        break;
                    case dataCard.CardType.MurDeRonces:
                        if (card.DataCard.m_isUpsideDown)
                        {
                            card.LaissePourMort();
                        }
                        foreach (hero hero in selected)
                        {
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
                            if (card.DataCard.m_isUpsideDown)
                            {
                                card.RepandreMort();
                            }
                            if (!card.DataCard.m_isUpsideDown)
                            {
                                card.SurgissementVitalique();
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
        PrefabDmgText.transform.GetChild(0).GetComponent<TMP_Text>().text = damage.ToString();
        GameObject texte = GameObject.Instantiate(PrefabDmgText);
        texte.transform.position = objet.transform.position;
        yield return new WaitForSeconds(2);
        Destroy(texte);
    }

    public void DamageNumber(GameObject objet, int damage)
    {
        StartCoroutine(DamageNumberCorou(objet, damage));
    }
    public IEnumerator DamageNumberCorou(Vector3 objet, int damage)
    {
        PrefabDmgText.transform.GetChild(0).GetComponent<TMP_Text>().text = damage.ToString();
        GameObject texte = GameObject.Instantiate(PrefabDmgText);
        texte.transform.position = new Vector3(objet.x, objet.y - 2, objet.z);
        texte.transform.position = new Vector3(texte.transform.position.x + Random.Range(-1f, 1f), texte.transform.position.y + Random.Range(-1f, 1f), texte.transform.position.z);
        PrefabDmgText.transform.GetChild(0).GetComponent<TMP_Text>().text = damage.ToString();
        yield return new WaitForSeconds(2);
        Destroy(texte);
    }

    public void DamageNumber(Vector3 objet, int damage)
    {
        Vector3 offset = new Vector3(0, 4, 0);
        StartCoroutine(DamageNumberCorou(objet + offset, damage));
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
        
    }
}
