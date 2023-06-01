using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

[RequireComponent(typeof(Fight))]
public class GameManager : MonoBehaviour
{
    
    
    public List<CardObject> Hand;
    public entityManager entityManager;
    public Fight FM;
    public List<hero> ennemiesPrefabs;
    public List<List<List<hero>>> allWave = new List<List<List<hero>>>(); 
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
    
    public bool isHoverButton = false;

    #region Heros
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

        foreach(var wave in allWave)
        {
            foreach(var wave2 in wave)
            {
                foreach(var wave3 in wave2)
                {
                    print(wave3);
                }
            }
        }   


    }

    #region Set Up Wave
    public void wave1()
    {
        List<hero> wave = new List<hero>();
        List<hero> wave2 = new List<hero>();
        List<hero> wave3 = new List<hero>();
        List<hero> wave4 = new List<hero>();
        List<List<hero>> waveChose = new List<List<hero>>
        {
            wave,
            wave2,
            wave3,
            wave4
        };
        waveChose[0].Add(ennemiesPrefabs[6]);

        waveChose[1].Add(ennemiesPrefabs[6]);
        waveChose[1].Add(ennemiesPrefabs[6]);

        waveChose[2].Add(ennemiesPrefabs[6]);
        waveChose[2].Add(ennemiesPrefabs[6]);
        waveChose[2].Add(ennemiesPrefabs[6]);

        waveChose[3].Add(ennemiesPrefabs[0]);

        allWave.Add(waveChose);



    }

    public void wave2()
    {
        List<hero> wave = new List<hero>();
        List<hero> wave2 = new List<hero>();
        List<hero> wave3 = new List<hero>();
        List<hero> wave4 = new List<hero>();
        List<List<hero>> waveChose = new List<List<hero>>
        {
            wave,
            wave2,
            wave3,
            wave4
        };
        waveChose[0].Add(ennemiesPrefabs[6]);
        waveChose[0].Add(ennemiesPrefabs[0]);

        waveChose[1].Add(ennemiesPrefabs[6]);
        waveChose[1].Add(ennemiesPrefabs[6]);
        waveChose[1].Add(ennemiesPrefabs[0]);

        waveChose[2].Add(ennemiesPrefabs[0]);
        waveChose[2].Add(ennemiesPrefabs[0]);

        allWave.Add(waveChose);

    }

    public void wave3()
    {
        List<hero> wave = new List<hero>();
        List<hero> wave2 = new List<hero>();
        List<hero> wave3 = new List<hero>();
        List<hero> wave4 = new List<hero>();
        List<List<hero>> waveChose = new List<List<hero>>
        {
            wave,
            wave2,
            wave3,
            wave4
        };
        waveChose[0].Add(ennemiesPrefabs[6]);
        waveChose[0].Add(ennemiesPrefabs[0]);
        waveChose[0].Add(ennemiesPrefabs[0]);

        waveChose[1].Add(ennemiesPrefabs[0]);
        waveChose[1].Add(ennemiesPrefabs[0]);
        waveChose[1].Add(ennemiesPrefabs[0]);

        waveChose[2].Add(ennemiesPrefabs[0]);
        waveChose[2].Add(ennemiesPrefabs[5]);

        waveChose[3].Add(ennemiesPrefabs[6]);
        waveChose[3].Add(ennemiesPrefabs[6]);
        waveChose[3].Add(ennemiesPrefabs[5]);

        allWave.Add(waveChose);
    }

    public void wave4()
    {
        List<hero> wave = new List<hero>();
        List<hero> wave2 = new List<hero>();
        List<hero> wave3 = new List<hero>();
        List<hero> wave4 = new List<hero>();
        List<List<hero>> waveChose = new List<List<hero>>
        {
            wave,
            wave2,
            wave3,
            wave4
        };
        waveChose[0].Add(ennemiesPrefabs[0]);
        waveChose[0].Add(ennemiesPrefabs[5]);

        waveChose[1].Add(ennemiesPrefabs[0]);
        waveChose[1].Add(ennemiesPrefabs[0]);
        waveChose[1].Add(ennemiesPrefabs[5]);

        waveChose[2].Add(ennemiesPrefabs[0]);
        waveChose[2].Add(ennemiesPrefabs[5]);

        waveChose[3].Add(ennemiesPrefabs[0]);
        waveChose[3].Add(ennemiesPrefabs[5]);
        waveChose[3].Add(ennemiesPrefabs[5]);

        allWave.Add(waveChose);
    }

    public void wave5()
    {
        List<hero> wave = new List<hero>();
        List<hero> wave2 = new List<hero>();
        List<hero> wave3 = new List<hero>();
        List<hero> wave4 = new List<hero>();
        List<List<hero>> waveChose = new List<List<hero>>
        {
            wave,
            wave2,
            wave3,
            wave4
        };
        waveChose[0].Add(ennemiesPrefabs[5]);
        waveChose[0].Add(ennemiesPrefabs[5]);
        waveChose[0].Add(ennemiesPrefabs[5]);

        waveChose[1].Add(ennemiesPrefabs[0]);
        waveChose[1].Add(ennemiesPrefabs[5]);
        waveChose[1].Add(ennemiesPrefabs[3]);

        waveChose[2].Add(ennemiesPrefabs[5]);
        waveChose[2].Add(ennemiesPrefabs[5]);
        waveChose[2].Add(ennemiesPrefabs[3]);

        waveChose[3].Add(ennemiesPrefabs[3]);
        waveChose[3].Add(ennemiesPrefabs[3]);

        allWave.Add(waveChose);
    }

    public void wave6()
    {
        List<hero> wave = new List<hero>();
        List<hero> wave2 = new List<hero>();
        List<hero> wave3 = new List<hero>();
        List<hero> wave4 = new List<hero>();
        List<List<hero>> waveChose = new List<List<hero>>
        {
            wave,
            wave2,
            wave3,
            wave4
        };
        waveChose[0].Add(ennemiesPrefabs[5]);
        waveChose[0].Add(ennemiesPrefabs[5]);
        waveChose[0].Add(ennemiesPrefabs[0]);

        waveChose[1].Add(ennemiesPrefabs[3]);
        waveChose[1].Add(ennemiesPrefabs[3]);
        waveChose[1].Add(ennemiesPrefabs[5]);

        waveChose[2].Add(ennemiesPrefabs[5]);
        waveChose[2].Add(ennemiesPrefabs[5]);
        waveChose[2].Add(ennemiesPrefabs[3]);

        waveChose[3].Add(ennemiesPrefabs[3]);
        waveChose[3].Add(ennemiesPrefabs[0]);
        waveChose[3].Add(ennemiesPrefabs[5]);

        allWave.Add(waveChose);
    }

    public void wave7()
    {
        List<hero> wave = new List<hero>();
        List<hero> wave2 = new List<hero>();
        List<hero> wave3 = new List<hero>();
        List<hero> wave4 = new List<hero>();
        List<List<hero>> waveChose = new List<List<hero>>
        {
            wave,
            wave2,
            wave3,
            wave4
        };
        waveChose[0].Add(ennemiesPrefabs[5]);
        waveChose[0].Add(ennemiesPrefabs[3]);
        waveChose[0].Add(ennemiesPrefabs[3]);

        waveChose[1].Add(ennemiesPrefabs[5]);
        waveChose[1].Add(ennemiesPrefabs[5]);
        waveChose[1].Add(ennemiesPrefabs[4]);

        waveChose[2].Add(ennemiesPrefabs[5]);
        waveChose[2].Add(ennemiesPrefabs[3]);
        waveChose[2].Add(ennemiesPrefabs[4]);

        waveChose[3].Add(ennemiesPrefabs[3]);
        waveChose[3].Add(ennemiesPrefabs[3]);
        waveChose[3].Add(ennemiesPrefabs[3]);

        allWave.Add(waveChose);
    }

    public void wave8()
    {
        List<hero> wave = new List<hero>();
        List<hero> wave2 = new List<hero>();
        List<hero> wave3 = new List<hero>();
        List<hero> wave4 = new List<hero>();
        List<List<hero>> waveChose = new List<List<hero>>
        {
            wave,
            wave2,
            wave3,
            wave4
        };
        waveChose[0].Add(ennemiesPrefabs[3]);
        waveChose[0].Add(ennemiesPrefabs[3]);
        waveChose[0].Add(ennemiesPrefabs[4]);

        waveChose[1].Add(ennemiesPrefabs[4]);
        waveChose[1].Add(ennemiesPrefabs[4]);

        waveChose[2].Add(ennemiesPrefabs[3]);
        waveChose[2].Add(ennemiesPrefabs[4]);
        waveChose[2].Add(ennemiesPrefabs[4]);

        waveChose[3].Add(ennemiesPrefabs[4]);
        waveChose[3].Add(ennemiesPrefabs[4]);
        waveChose[3].Add(ennemiesPrefabs[4]);

        allWave.Add(waveChose);
    }
    public void wave9()
    {
        List<hero> wave = new List<hero>();
        List<hero> wave2 = new List<hero>();
        List<hero> wave3 = new List<hero>();
        List<List<hero>> waveChose = new List<List<hero>>
        {
            wave,
            wave2,
            wave3
        };
        waveChose[0].Add(ennemiesPrefabs[3]);
        waveChose[0].Add(ennemiesPrefabs[1]);

        waveChose[1].Add(ennemiesPrefabs[3]);
        waveChose[1].Add(ennemiesPrefabs[3]);
        waveChose[1].Add(ennemiesPrefabs[1]);

        waveChose[2].Add(ennemiesPrefabs[4]);
        waveChose[2].Add(ennemiesPrefabs[1]);


        allWave.Add(waveChose);
    }
    public void wave10()
    {
        List<hero> wave = new List<hero>();
        List<hero> wave2 = new List<hero>();
        List<hero> wave3 = new List<hero>();
        List<List<hero>> waveChose = new List<List<hero>>
        {
            wave,
            wave2,
            wave3
        };
        waveChose[0].Add(ennemiesPrefabs[3]);
        waveChose[0].Add(ennemiesPrefabs[4]);
        waveChose[0].Add(ennemiesPrefabs[1]);

        waveChose[1].Add(ennemiesPrefabs[4]);
        waveChose[1].Add(ennemiesPrefabs[4]);
        waveChose[1].Add(ennemiesPrefabs[1]);

        waveChose[2].Add(ennemiesPrefabs[1]);
        waveChose[2].Add(ennemiesPrefabs[1]);


        allWave.Add(waveChose);
    }
    public void wave11()
    {
        List<hero> wave = new List<hero>();

        List<List<hero>> waveChose = new List<List<hero>>();
        waveChose.Add(wave);
        
        waveChose[0].Add(ennemiesPrefabs[1]);
        waveChose[0].Add(ennemiesPrefabs[1]);
        waveChose[0].Add(ennemiesPrefabs[1]);

        

        allWave.Add(waveChose);
    }

    public void wave12()
    {
        List<hero> wave = new List<hero>();

        List<List<hero>> waveChose = new List<List<hero>>();
        waveChose.Add(wave);

        waveChose[0].Add(ennemiesPrefabs[2]);
        



        allWave.Add(waveChose);
    }


    #endregion
    /*    public void CardSended(CardObject card)
        {
            CarteUtilisee = card;
            FM.Cardsend(card);
        }*/
}
