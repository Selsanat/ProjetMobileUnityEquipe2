using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class champSelector : MonoBehaviour
{
    bool selectedArboriste = false;
    bool selectedPretre = false;
    public List<GameObject> objects = new List<GameObject>();
    public Button buttonArboriste;
    public Button buttonPretre;

    public Button start;
    public Sprite notSelectedArboriste;
    public Sprite notSelectedPretre;
    public Sprite SelectedArboriste;
    public Sprite SelectedPretre;
    [SerializeField] GameManager gameManager;

    public TextMeshProUGUI ArboLife;
    public TextMeshProUGUI PretreLife;


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
            StartCoroutine(TransiLevel());

            
        }
        
    }


    public void setctive()
    {
        foreach (GameObject go in objects)
        {
            go.SetActive(true);
            if (go.CompareTag("arbo"))
            {
                Slider slider;
                slider = go.GetComponentInChildren<Slider>();
                slider.value = gameManager.expArboriste;
                ArboLife.text = "PV : " + gameManager.LifeArboriste.ToString() + " \nNiveau :" + gameManager.levelArboriste;
            }
            if (go.CompareTag("pretre"))
            {
                Slider slider;
                slider = go.GetComponentInChildren<Slider>();
                slider.value = gameManager.levelPretre;
                PretreLife.text = "PV : " + gameManager.LifePretre.ToString() + "\nNiveau :" + gameManager.levelPretre;
            }

        }
/*        buttonArboriste.gameObject.SetActive(true);
        buttonPretre.gameObject.SetActive(true);
        start.gameObject.SetActive(true);*/

    }
    IEnumerator TransiLevel()
    {
        gameManager.transi.Play("Transi");
        yield return new WaitForSeconds(1.5f);
        gameManager.transi.Play("Detransi");
        Fight fight = FindObjectOfType<Fight>();
        fight.perso1 = selectedArboriste;
        fight.perso2 = selectedPretre;
        foreach (GameObject go in objects)
        {
            go.SetActive(false);
        }
/*        buttonArboriste.gameObject.SetActive(false);
        buttonPretre.gameObject.SetActive(false);
        start.gameObject.SetActive(false);*/
        SceneManager.LoadScene(1);
    }



}
