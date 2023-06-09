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
    [SerializeField] public GameObject AutreUI;
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
            foreach(CardObject card in gameManager.deck.Hand)
            {
                card.gameObject.transform.localScale = new Vector3(1, 1, 1);
                
            }
            AutreUI.SetActive(true);
            UI.SetActive(false);
            
        }
        
    }
}
