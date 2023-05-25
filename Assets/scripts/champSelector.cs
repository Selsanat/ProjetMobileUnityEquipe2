using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class champSelector : MonoBehaviour
{
    bool selectedArboriste = false;
    bool selectedPretre = false;
    public Button buttonArboriste;
    public Button buttonPretre;
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

}
