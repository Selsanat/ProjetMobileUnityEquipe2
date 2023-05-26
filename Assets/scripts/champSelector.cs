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
    

    public void selectArboriste()
    {
        selectedArboriste = !selectedArboriste;
        if (selectedArboriste)
            buttonArboriste.image.sprite = SelectedArboriste;
        else
            buttonArboriste.image.sprite = notSelectedArboriste;
    }
    public void selectPretre()
    {
        selectedPretre = !selectedPretre;
        if (selectedPretre)
            buttonPretre.image.sprite = SelectedPretre;
        else
            buttonPretre.image.sprite = notSelectedPretre;
    }


    public void startNewLevel()
    {
        Fight fight;
        fight = GetComponent<Fight>();
        fight.perso1 = selectedArboriste;
        fight.perso2 = selectedPretre;
        SceneManager.LoadScene("TestSceneSacha2");
    }

}
