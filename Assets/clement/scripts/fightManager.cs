using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class fightManager : MonoBehaviour
{
    /*private bool playerTurn1;
    private bool playerTurn2;
    private bool playerTurn3;
    private bool playerTurn4;
    private bool ennemiTurn;*/

    [SerializeField] GameObject holderCharacter;
    [SerializeField] GameObject characterSlot;
    [SerializeField] GameObject slot;

    GameObject outline;
    TextMeshProUGUI nameCharacter;
    Image imgCharacter;

    GameObject emplacement1, emplacement2, emplacement3, emplacement4;
    int characterID;
    GameManager gm => GameManager.Instance;
    public static fightManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void InitializeHeros()
    {
        hero Arboriste = new hero(entityManager.Role.Arboriste, 10, 10, 1, 0, 0, new deck(), 10);
        hero Guerrier = new hero(entityManager.Role.Guerrier, 10, 10, 1, 0, 0, new deck(), 10);
        hero Mage = new hero(entityManager.Role.Mage, 10, 10, 1, 0, 0, new deck(), 10);
        hero Paladin = new hero(entityManager.Role.Paladin, 10, 10, 1, 0, 0, new deck(), 10);
    }

}
