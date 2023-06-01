using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InspectCard : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] public GameObject UI;
    [SerializeField] public Image Image;
    [SerializeField] public TMP_Text Name;
    [SerializeField] public TMP_Text description;
    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.InspectUI = this;
    }
    void OnMouseUpAsButton()
    {
        if (UI.activeSelf)
        {
            gameManager.CardsInteractable = true;
            gameManager.HasCardInHand = false;
            UI.SetActive(false);
            
        }
        
    }
}
