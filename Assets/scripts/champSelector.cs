using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class champSelector : MonoBehaviour
{
    bool selectedArboriste = false;
    bool selectedPretre = false;
    public Button buttonArboriste;
    public Button buttonPretre;

    public Button start;
    public Sprite notSelectedArboriste;
    public Sprite notSelectedPretre;
    public Sprite SelectedArboriste;
    public Sprite SelectedPretre;
    [SerializeField] GameManager gameManager;
    
    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }   
    public void selectArboriste()
    {
        if(gameManager.LifeArboriste <= 0)
            return;

        selectedArboriste = !selectedArboriste;
        if (selectedArboriste)
            buttonArboriste.image.sprite = SelectedArboriste;
        else
            buttonArboriste.image.sprite = notSelectedArboriste;
    }
    public void selectPretre()
    {
        if(gameManager.LifePretre <= 0)
            return;

        selectedPretre = !selectedPretre;
        if (selectedPretre)
            buttonPretre.image.sprite = SelectedPretre;
        else
            buttonPretre.image.sprite = notSelectedPretre;
    }


    public void startNewLevel()
    {
        if(selectedArboriste || selectedPretre)
        {
            Fight fight = FindObjectOfType<Fight>();
            fight.perso1 = selectedArboriste;
            fight.perso2 = selectedPretre;
            buttonArboriste.gameObject.SetActive(false);
            buttonPretre.gameObject.SetActive(false);
            start.gameObject.SetActive(false);
            SceneManager.LoadScene(0);
            
        }
        
    }


    public void setctive()
    {
        buttonArboriste.gameObject.SetActive(true);
        buttonPretre.gameObject.SetActive(true);
        start.gameObject.SetActive(true);
    }

}
