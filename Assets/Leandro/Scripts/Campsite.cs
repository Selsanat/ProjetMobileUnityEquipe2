using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine.SceneManagement;

public class Campsite : MonoBehaviour
{
    [SerializeField] Light2D pretre;
    [SerializeField] Light2D druide;
    [SerializeField] List<Light2D> LightsAEteindre = new List<Light2D>();
    [SerializeField] float VitesseTranspo = 10f;
    [SerializeField] float TempsTrans = 0.5f;
    private GameManager gameManager;
    public TMP_Text texte;
    private Coroutine inst;
    // Start is called before the first frame update

    private void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        druide.gameObject.SetActive(false);
        pretre.gameObject.SetActive(false);
        inst = StartCoroutine(Ecrit("Choisissez un personnage a soigner"));
    }
    public IEnumerator Soigne(bool isDruid)
    {
        string str = "Le personnage a �t� soign� !(";
        if (isDruid)
        {
            gameManager.LifeArboriste += 25;
            if (gameManager.LifeArboriste > 50)
                gameManager.LifeArboriste = 50;

            str += "" + gameManager.LifeArboriste;
        }
        else 
        {
            gameManager.LifePretre += 25;
            if (gameManager.LifePretre > 50)
                gameManager.LifePretre = 50;
            str += "" + gameManager.LifePretre;
        }
        str += "PV)\nCliquer pour continuer";
        if (inst != null)
        {
            StopCoroutine(inst);
        }
        inst = StartCoroutine(Ecrit(str));
        yield return new WaitUntil(() => Input.GetMouseButton(0)) ;
        StartCoroutine(EteindreLumieres());

    }
    IEnumerator EteindreLumieres()
    {
        druide.gameObject.SetActive(true);
        pretre.gameObject.SetActive(true);
        float TempsTransition = TempsTrans;
        float timeElapsed = 0;
            while (timeElapsed < TempsTransition)
            {
                pretre.intensity = Mathf.Lerp(pretre.intensity, 0, Time.deltaTime * VitesseTranspo);
                druide.intensity = Mathf.Lerp(druide.intensity, 0, Time.deltaTime * VitesseTranspo);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        druide.gameObject.SetActive(false);
        pretre.gameObject.SetActive(false);
        if (inst != null)
        {
            StopCoroutine(inst);
        }
        inst = StartCoroutine(Ecrit("Bonne chance pour la suite..."));
        yield return new WaitUntil(() => Input.GetMouseButton(0));
        foreach (Light2D light in LightsAEteindre)
        {
            TempsTransition = TempsTrans;
            timeElapsed = 0;
            while (timeElapsed < TempsTransition)
            {
                light.intensity = Mathf.Lerp(light.intensity, 0, Time.deltaTime * VitesseTranspo);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
        yield return new WaitForSeconds(1);
        gameManager.transi.Play("Detransi");
        SceneManager.LoadScene(0);

    }
    IEnumerator SoigneEtchangeScreen(bool isDruid)
    {
        yield return null;
    }
    public void selection(bool isDruid)
    {
        StartCoroutine(FadeNDefade(isDruid));
    }
    IEnumerator Ecrit(string blabla)
    {
        texte.text = "";
        for(int i = 0; i< blabla.Length; i++)
        {
            texte.text += blabla[i];
            yield return new WaitForSeconds(0.02f);
        }
    }
    IEnumerator FadeNDefade(bool isDruid)
    {
        druide.gameObject.SetActive(true);
        pretre.gameObject.SetActive(true);
        /*float TempsTransition = TempsTrans;
        float timeElapsed = 0;*/
/*        if (isDruid)
        {
            while (timeElapsed < TempsTransition)
            {
                pretre.intensity = Mathf.Lerp(pretre.intensity, 0, Time.deltaTime * VitesseTranspo);
                druide.intensity = Mathf.Lerp(druide.intensity, 25, Time.deltaTime * VitesseTranspo);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (timeElapsed < TempsTransition)
            {
                pretre.intensity = Mathf.Lerp(pretre.intensity, 25, Time.deltaTime * VitesseTranspo);
                druide.intensity = Mathf.Lerp(druide.intensity, 0, Time.deltaTime * VitesseTranspo);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }*/
        if (isDruid)
        {
            pretre.intensity = 0;
            druide.intensity = 25;
            pretre.gameObject.SetActive(false);
            string str = "Soignez le druide ? (";
            str += "" + gameManager.LifeArboriste;
            str += "PV)\nCliquer pour continuer. . .";
            if (inst != null)
            {
                StopCoroutine(inst);
            }
            inst = StartCoroutine(Ecrit(str));
            yield return null;
        }
        else
        {
            pretre.intensity = 25;
            druide.intensity = 0;
            druide.gameObject.SetActive(false);
            string str = "Soignez le pretre ? (";
            str += "" + gameManager.LifePretre;
            str += "PV)\nCliquer pour continuer. . .";
            if (inst != null)
            {
                StopCoroutine(inst);
            }
            inst = StartCoroutine(Ecrit(str));
            yield return null;
        }
    }
}
