using Map;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

[RequireComponent(typeof(Fight))]
public class GameManager : MonoBehaviour
{
    
    
    public List<CardObject> Hand;
    public entityManager entityManager;
    public Fight FM;
    public List<DataEnemy> enemiesData;
    public List<List<List<DataEnemy>>> allWave = new List<List<List<DataEnemy>>>(); 
    public CardObject CarteUtilisee = null;
    public bool CardsInteractable = true;
    public bool HasCardInHand = false;
    public float RangePourActiverCarte;
    public InspectCard InspectUI;
    public float TempsPourClickCardInspect = 0.5f;
    public Deck deck;
    public hero pretre;
    public int debuffDraw = 0;
    public bool IsAnyProv = false;
    public int waveCounter = 0;
    public bool isManaMultiplier = false;
    
    public int manaMultiplier = 0;
    public bool isAbsolution = false;
    public bool winoulose;

    public bool isHoverButton = false;
    public Animator transi;
    public bool AnimAtk = false;
    public MapPlayerTracker maptracker;
    //private public MapNode _currentNode;

    #region perso
    #region Arboriste
    public int LifeArboriste = 50;
    public bool IsArboristePlayed = false;
    public int levelArboriste = 0;
    public int expArboriste = 0;

    #endregion

    #region Pretre

    public int LifePretre = 50;
    public bool IsPretrePlayed = false;
    public int levelPretre = 0;
    public int expPretre = 0;

    #endregion
    #endregion

    public hero ennemy1;
    public hero ennemy2;
    public hero ennemy3;

    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                print("Game manager is null");
            return _instance;
        }
    }
    


    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }
    #endregion
    private void Start()
    {

        transi = transform.GetChild(0)?.gameObject.GetComponent<Animator>();
        FM = FindObjectOfType<Fight>();
        entityManager = FindObjectOfType<entityManager>();
        wave1();
        wave2();
        wave3();
        wave4();
        wave5();
        wave6();
        wave7();
        wave8();
        wave9();
        wave10();
        wave11();
        wave12();
        string path = Application.persistentDataPath + "/saveData.fun";
        if (File.Exists(path))
        {
            print("Load");
            LoadData();
        }
        else
        {

            SaveData();
        }

        if(maptracker != null)
            maptracker = FindObjectOfType<MapPlayerTracker>();


    }

    public void SaveData()
    {
        savingData data = new savingData(LifeArboriste, LifePretre, waveCounter, levelArboriste, levelPretre, expArboriste, expPretre, IsPretrePlayed, IsArboristePlayed);
        savingSysteme.SaveData(data);
    }

    public void LoadData()
    {
        savingData data = savingSysteme.LoadData();
        LifeArboriste = data.lifeArbo;
        LifePretre = data.lifePretre;
        waveCounter = data.waveCount;
        levelArboriste = data.levelArbo;
        levelPretre = data.levelPretre;
        expArboriste = data.expArbo;
        expPretre = data.expPretre;
        IsPretrePlayed = data.isPretrePlayed;
        IsArboristePlayed = data.isArboPlayed;
    }

    #region Set Up Wave
    public void wave1()
    {
        List<DataEnemy> wave = new List<DataEnemy>();
        List<DataEnemy> wave2 = new List<DataEnemy>();
        List<DataEnemy> wave3 = new List<DataEnemy>();
        List<DataEnemy> wave4 = new List<DataEnemy>();
        List<List<DataEnemy>> waveChose = new List<List<DataEnemy>>
        {
            wave,
            wave2,
            wave3,
            wave4
        };
        waveChose[0].Add(enemiesData[6]);

        waveChose[1].Add(enemiesData[6]);
        waveChose[1].Add(enemiesData[6]);

        waveChose[2].Add(enemiesData[6]);
        waveChose[2].Add(enemiesData[6]);
        waveChose[2].Add(enemiesData[6]);

        waveChose[3].Add(enemiesData[5]);

        allWave.Add(waveChose);



    }

    public void wave2()
    {
        List<DataEnemy> wave = new List<DataEnemy>();
        List<DataEnemy> wave2 = new List<DataEnemy>();
        List<DataEnemy> wave3 = new List<DataEnemy>();
        List<DataEnemy> wave4 = new List<DataEnemy>();
        List<List<DataEnemy>> waveChose = new List<List<DataEnemy>>
        {
            wave,
            wave2,
            wave3,
            wave4
        };
        waveChose[0].Add(enemiesData[6]);
        waveChose[0].Add(enemiesData[5]);

        waveChose[1].Add(enemiesData[6]);
        waveChose[1].Add(enemiesData[6]);
        waveChose[1].Add(enemiesData[5]);

        waveChose[2].Add(enemiesData[5]);
        waveChose[2].Add(enemiesData[5]);

        allWave.Add(waveChose);

    }

    public void wave3()
    {
        List<DataEnemy> wave = new List<DataEnemy>();
        List<DataEnemy> wave2 = new List<DataEnemy>();
        List<DataEnemy> wave3 = new List<DataEnemy>();
        List<DataEnemy> wave4 = new List<DataEnemy>();
        List<List<DataEnemy>> waveChose = new List<List<DataEnemy>>
        {
            wave,
            wave2,
            wave3,
            wave4
        };
        waveChose[0].Add(enemiesData[6]);
        waveChose[0].Add(enemiesData[5]);
        waveChose[0].Add(enemiesData[5]);

        waveChose[1].Add(enemiesData[5]);
        waveChose[1].Add(enemiesData[5]);
        waveChose[1].Add(enemiesData[5]);

        waveChose[2].Add(enemiesData[5]);
        waveChose[2].Add(enemiesData[0]);

        waveChose[3].Add(enemiesData[6]);
        waveChose[3].Add(enemiesData[6]);
        waveChose[3].Add(enemiesData[0]);

        allWave.Add(waveChose);
    }

    public void wave4()
    {
        List<DataEnemy> wave = new List<DataEnemy>();
        List<DataEnemy> wave2 = new List<DataEnemy>();
        List<DataEnemy> wave3 = new List<DataEnemy>();
        List<DataEnemy> wave4 = new List<DataEnemy>();
        List<List<DataEnemy>> waveChose = new List<List<DataEnemy>>
        {
            wave,
            wave2,
            wave3,
            wave4
        };
        waveChose[0].Add(enemiesData[5]);
        waveChose[0].Add(enemiesData[0]);

        waveChose[1].Add(enemiesData[5]);
        waveChose[1].Add(enemiesData[5]);
        waveChose[1].Add(enemiesData[0]);

        waveChose[2].Add(enemiesData[5]);
        waveChose[2].Add(enemiesData[0]);
        waveChose[2].Add(enemiesData[0]);


        allWave.Add(waveChose);
    }

    public void wave5()
    {
        List<DataEnemy> wave = new List<DataEnemy>();
        List<DataEnemy> wave2 = new List<DataEnemy>();
        List<DataEnemy> wave3 = new List<DataEnemy>();
        List<DataEnemy> wave4 = new List<DataEnemy>();
        List<List<DataEnemy>> waveChose = new List<List<DataEnemy>>
        {
            wave,
            wave2,
            wave3,
            wave4
        };
        waveChose[0].Add(enemiesData[0]);
        waveChose[0].Add(enemiesData[0]);
        waveChose[0].Add(enemiesData[0]);

        waveChose[1].Add(enemiesData[5]);
        waveChose[1].Add(enemiesData[0]);
        waveChose[1].Add(enemiesData[2]);

        waveChose[2].Add(enemiesData[0]);
        waveChose[2].Add(enemiesData[0]);
        waveChose[2].Add(enemiesData[2]);

        waveChose[3].Add(enemiesData[2]);
        waveChose[3].Add(enemiesData[2]);

        allWave.Add(waveChose);
    }

    public void wave6()
    {
        List<DataEnemy> wave = new List<DataEnemy>();
        List<DataEnemy> wave2 = new List<DataEnemy>();
        List<DataEnemy> wave3 = new List<DataEnemy>();
        List<DataEnemy> wave4 = new List<DataEnemy>();
        List<List<DataEnemy>> waveChose = new List<List<DataEnemy>>
        {
            wave,
            wave2,
            wave3,
            wave4
        };
        waveChose[0].Add(enemiesData[0]);
        waveChose[0].Add(enemiesData[0]);
        waveChose[0].Add(enemiesData[5]);

        waveChose[1].Add(enemiesData[2]);
        waveChose[1].Add(enemiesData[2]);
        waveChose[1].Add(enemiesData[0]);

        waveChose[2].Add(enemiesData[0]);
        waveChose[2].Add(enemiesData[5]);
        waveChose[2].Add(enemiesData[2]);

        waveChose[3].Add(enemiesData[0]);
        waveChose[3].Add(enemiesData[0]);
        waveChose[3].Add(enemiesData[2]);

        allWave.Add(waveChose);
    }

    public void wave7()
    {
        List<DataEnemy> wave = new List<DataEnemy>();
        List<DataEnemy> wave2 = new List<DataEnemy>();
        List<DataEnemy> wave3 = new List<DataEnemy>();
        List<DataEnemy> wave4 = new List<DataEnemy>();
        List<List<DataEnemy>> waveChose = new List<List<DataEnemy>>
        {
            wave,
            wave2,
            wave3,
            wave4
        };
        waveChose[0].Add(enemiesData[0]);
        waveChose[0].Add(enemiesData[2]);
        waveChose[0].Add(enemiesData[2]);

        waveChose[1].Add(enemiesData[0]);
        waveChose[1].Add(enemiesData[0]);
        waveChose[1].Add(enemiesData[1]);

        waveChose[2].Add(enemiesData[0]);
        waveChose[2].Add(enemiesData[2]);
        waveChose[2].Add(enemiesData[1]);

        waveChose[3].Add(enemiesData[2]);
        waveChose[3].Add(enemiesData[2]);
        waveChose[3].Add(enemiesData[2]);

        allWave.Add(waveChose);
    }

    public void wave8()
    {
        List<DataEnemy> wave = new List<DataEnemy>();
        List<DataEnemy> wave2 = new List<DataEnemy>();
        List<DataEnemy> wave3 = new List<DataEnemy>();
        List<DataEnemy> wave4 = new List<DataEnemy>();
        List<List<DataEnemy>> waveChose = new List<List<DataEnemy>>
        {
            wave,
            wave2,
            wave3,
            wave4
        };
        waveChose[0].Add(enemiesData[2]);
        waveChose[0].Add(enemiesData[2]);
        waveChose[0].Add(enemiesData[1]);

        waveChose[1].Add(enemiesData[1]);
        waveChose[1].Add(enemiesData[1]);

        waveChose[2].Add(enemiesData[2]);
        waveChose[2].Add(enemiesData[1]);
        waveChose[2].Add(enemiesData[1]);

        waveChose[3].Add(enemiesData[1]);
        waveChose[3].Add(enemiesData[1]);
        waveChose[3].Add(enemiesData[1]);

        allWave.Add(waveChose);
    }
    public void wave9()
    {
        List<DataEnemy> wave = new List<DataEnemy>();
        List<DataEnemy> wave2 = new List<DataEnemy>();
        List<DataEnemy> wave3 = new List<DataEnemy>();
        List<List<DataEnemy>> waveChose = new List<List<DataEnemy>>
        {
            wave,
            wave2,
            wave3
        };
        waveChose[0].Add(enemiesData[2]);
        waveChose[0].Add(enemiesData[4]);

        waveChose[1].Add(enemiesData[2]);
        waveChose[1].Add(enemiesData[2]);
        waveChose[1].Add(enemiesData[4]);

        waveChose[2].Add(enemiesData[1]);
        waveChose[2].Add(enemiesData[4]);


        allWave.Add(waveChose);
    }
    public void wave10()
    {
        List<DataEnemy> wave = new List<DataEnemy>();
        List<DataEnemy> wave2 = new List<DataEnemy>();
        List<DataEnemy> wave3 = new List<DataEnemy>();
        List<List<DataEnemy>> waveChose = new List<List<DataEnemy>>
        {
            wave,
            wave2,
            wave3
        };
        waveChose[0].Add(enemiesData[2]);
        waveChose[0].Add(enemiesData[1]);
        waveChose[0].Add(enemiesData[4]);

        waveChose[1].Add(enemiesData[1]);
        waveChose[1].Add(enemiesData[1]);
        waveChose[1].Add(enemiesData[4]);

        waveChose[2].Add(enemiesData[4]);
        waveChose[2].Add(enemiesData[4]);


        allWave.Add(waveChose);
    }
    public void wave11()
    {
        List<DataEnemy> wave = new List<DataEnemy>();

        List<List<DataEnemy>> waveChose = new List<List<DataEnemy>>();
        waveChose.Add(wave);
        
        waveChose[0].Add(enemiesData[4]);
        waveChose[0].Add(enemiesData[4]);
        waveChose[0].Add(enemiesData[4]);

        

        allWave.Add(waveChose);
    }

    public void wave12()
    {
        List<DataEnemy> wave = new List<DataEnemy>();

        List<List<DataEnemy>> waveChose = new List<List<DataEnemy>>();
        waveChose.Add(wave);

        waveChose[0].Add(enemiesData[3]);
        



        allWave.Add(waveChose);
    }


    #endregion

}
