using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

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

    public Sprite en1;
    public Sprite en2;
    public Sprite en3;
    int wavetype = 0;
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
        wavetype = UnityEngine.Random.Range(0, gameManager.allWave[gameManager.waveCounter].Count - 1);
        gameManager.FM.waveType = wavetype;

        int encount = 0;
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
            if (go.CompareTag("enemy"))
            {
                if(encount > gameManager.allWave[gameManager.waveCounter][wavetype].Count - 1)
                {
                    go.SetActive(false);
                    break;
                }
                go.GetComponent<Image>().sprite = gameManager.allWave[gameManager.waveCounter][wavetype][encount].m_sprite;
                go.GetComponentInChildren<TextMeshProUGUI>().text = gameManager.enemiesData[encount].m_role.ToString() + " \nPV : "  + gameManager.allWave[gameManager.waveCounter][wavetype][encount].m_Pv;
                encount++;
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
