using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectCard : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] public GameObject UI;
    [SerializeField] public Image Image;
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
