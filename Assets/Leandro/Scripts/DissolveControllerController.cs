using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Color = UnityEngine.Color;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DissolveControllerController : MonoBehaviour
{
    [SerializeField] TMP_Text Textitre;
    [SerializeField] TMP_Text texte;
    [SerializeField] Volume Volume;
    [SerializeField] List<Light2D> Lights;
    [SerializeField] List<GameObject> Persos;
    [SerializeField] List<Material> Materials;
    private LensDistortion lensDistortion;
    private Bloom bloom;
    private ChromaticAberration chromaticAberation;
    private DepthOfField depthOfField;
    public List<DissolveController> dissolveControllers;
    public List<SpriteRenderer> SpriteRenderers;
    public Image Asombrir;


    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.winoulose)
        {
            StartCoroutine(EcranFin());
        }
        else
        {
            StartCoroutine(EcranMort());
            
        }
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public IEnumerator ValueTranspoVolume(float depart, float arrivee)
    {
        float TempsTransition = 5;
        float timeElapsed = 0;
        LensDistortion tmp;
        if(Volume.profile.TryGet<LensDistortion>(out tmp)){
            lensDistortion = tmp;
        }
        lensDistortion.intensity.value = depart;
        while (timeElapsed < TempsTransition)
        {
            lensDistortion.intensity.value = Mathf.Lerp(lensDistortion.intensity.value, arrivee, Time.deltaTime * 4);
            timeElapsed += Time.deltaTime*4;
            yield return null;
        }
        lensDistortion.intensity.value = arrivee;
    }
    public IEnumerator ValueTranspoBloom(float depart, float arrivee)
    {
        float TempsTransition = 5;
        float timeElapsed = 0;
        Bloom tmp;
        if (Volume.profile.TryGet<Bloom>(out tmp))
        {
            bloom = tmp;
        }
        bloom.intensity.value = depart;
        while (timeElapsed < TempsTransition)
        {
            bloom.intensity.value = Mathf.Lerp(bloom.intensity.value, arrivee, Time.deltaTime * 4);
            timeElapsed += Time.deltaTime * 4;
            yield return null;
        }
        bloom.intensity.value = arrivee;
    }
    public IEnumerator DepthOfField()
    {
        float TempsTransition = 5;
        float timeElapsed = 0;
        DepthOfField tmp;
        if (Volume.profile.TryGet<DepthOfField>(out tmp))
        {
            depthOfField = tmp;
        }
        depthOfField.focalLength.value = 1;
        while (timeElapsed < TempsTransition)
        {
            depthOfField.focalLength.value = Mathf.Lerp(depthOfField.focalLength.value, 300, Time.deltaTime * 4);
            timeElapsed += Time.deltaTime * 4;
            yield return null;
        }
        depthOfField.focalLength.value = 300;
    }
    public IEnumerator ChromaticAberation(bool sens)
    {
        float TempsTransition = 5;
        float timeElapsed = 0;
        ChromaticAberration tmp;
        if (Volume.profile.TryGet<ChromaticAberration>(out tmp))
        {
            chromaticAberation = tmp;
        }
        if (sens)
        {
            chromaticAberation.intensity.value = 0;
            while (timeElapsed < TempsTransition)
            {
                chromaticAberation.intensity.value = Mathf.Lerp(chromaticAberation.intensity.value, 1, Time.deltaTime * 4);
                timeElapsed += Time.deltaTime * 4;
                yield return null;
            }
            chromaticAberation.intensity.value = 1;
        }
        else
        {
            bloom.intensity.value = 1;
            while (timeElapsed < TempsTransition)
            {
                chromaticAberation.intensity.value = Mathf.Lerp(chromaticAberation.intensity.value, 0, Time.deltaTime * 4);
                timeElapsed += Time.deltaTime * 4;
                yield return null;
            }
            chromaticAberation.intensity.value = 0;
        }

    }
    public IEnumerator Light(Light2D light,float depart, float arrivee)
    {
        float TempsTransition = 5;
        float timeElapsed = 0;
        light.intensity = depart;
        while (timeElapsed < TempsTransition)
        {
            light.intensity = Mathf.Lerp(light.intensity, arrivee, Time.deltaTime * 4);
            timeElapsed += Time.deltaTime * 4;
            yield return null;
        }
        light.intensity = arrivee;
    }
    public IEnumerator TransposeTextColo()
    {
        float TempsTransition = 5;
        float timeElapsed = 0;
        Textitre.color = new Color(Textitre.color.r, Textitre.color.g, Textitre.color.b, 0);
        while (timeElapsed < TempsTransition)
        {
            Textitre.color = Color.Lerp(Textitre.color, new Color(Textitre.color.r, Textitre.color.g, Textitre.color.b, 1), Time.deltaTime * 1);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        Textitre.color = new Color(Textitre.color.r, Textitre.color.g, Textitre.color.b, 1);
    }
    public IEnumerator TranspoEnnemis(SpriteRenderer sprite)
    {
        float TempsTransition = 5;
        float timeElapsed = 0;
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);
        while (timeElapsed < TempsTransition)
        {
            sprite.color = Color.Lerp(sprite.color, new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1), Time.deltaTime * 1);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
    }
    public IEnumerator TranspoFond()
    {
        float TempsTransition = 5;
        float timeElapsed = 0;
        Asombrir.color = new Color(Asombrir.color.r, Asombrir.color.g, Asombrir.color.b, 0);
        while (timeElapsed < TempsTransition)
        {
            Asombrir.color = Color.Lerp(Asombrir.color, new Color(Asombrir.color.r, Asombrir.color.g, Asombrir.color.b, 0.9f), Time.deltaTime * 1);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        Asombrir.color = new Color(Asombrir.color.r, Asombrir.color.g, Asombrir.color.b, 0.9f);
    }
    public IEnumerator Smooth()
    {
        foreach (SpriteRenderer sprote in SpriteRenderers)
        {
            StartCoroutine(TranspoEnnemis(sprote));
            yield return null;
        }
    }
    IEnumerator Ecrit(string blabla)
    {
        texte.text = "";
        for (int i = 0; i < blabla.Length; i++)
        {
            texte.text += blabla[i];
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator EcranFin()
    {
        ChromaticAberration tmp;
        if (Volume.profile.TryGet<ChromaticAberration>(out tmp))
        {
            chromaticAberation = tmp;
        }
        chromaticAberation.intensity.value = 1;
        foreach (GameObject perso in Persos)
        {
            perso.GetComponent<SpriteRenderer>().material = Materials[0];
        }
        yield return new WaitForSeconds(1);
        yield return Smooth();
        yield return new WaitUntil(() => Input.GetMouseButton(0));

        foreach (DissolveController controller in dissolveControllers)
        {
            controller.dissolveSpeed = Random.Range(0.5f, 2f);
            controller.isDissolving = true;
        }
        StartCoroutine(Light(Lights[0],0, 0.5f));
        StartCoroutine(ValueTranspoBloom(0, 3f));
        StartCoroutine(ChromaticAberation(false));
        yield return ValueTranspoVolume(0f, -0.75f);
        yield return ValueTranspoVolume(-0.75f, 0f);
        StartCoroutine(Light(Lights[1], 0,1f));
        StartCoroutine(Light(Lights[2], 0, 1f));
        yield return new WaitUntil(() => Input.GetMouseButton(0));
        yield return TransposeTextColo();
        StartCoroutine(Ecrit("Tel un chasseur impitoyable, vous réduisez les monstruosités à néant, laissant derrière vous un véritable charnier maccabre, victorieux dans cette danse sanglante"));
    }

    IEnumerator EcranMort()
    {

        yield return new WaitForSeconds(1);
        
        yield return new WaitUntil(() => Input.GetMouseButton(0));

        StartCoroutine(Light(Lights[0], 0, 0.5f));
        Bloom tmp;
        if (Volume.profile.TryGet<Bloom>(out tmp))
        {
            bloom = tmp;
        }
        bloom.tint.value = new Color(183, 83, 83);
        StartCoroutine(ValueTranspoBloom(0, 3f));
        yield return new WaitForSeconds(1);
        yield return Light(Lights[1], 0, 1f);
        yield return Light(Lights[2], 0, 1f);
        Persos[0].GetComponent<DissolveController>().isDissolving = true;
        Persos[1].GetComponent<DissolveController>().isDissolving = true;
        StartCoroutine(ChromaticAberation(true));
        yield return ValueTranspoVolume(0f, -0.75f);
        StartCoroutine(Smooth());
        StartCoroutine(DepthOfField());
        yield return ValueTranspoVolume(-0.75f, 0f);



        yield return new WaitUntil(() => Input.GetMouseButton(0));
        StartCoroutine(TranspoFond());
        Textitre.text = "Vous avez succombé";
        yield return TransposeTextColo();
        StartCoroutine(Ecrit("Les monstres sans pitié dévorent votre chair et broient votre espoir, laissant derrière eux un tableau grotesque d'agonie et de désolation."));
        yield return new WaitUntil(() => Input.GetMouseButton(0));
        GameManager.Instance.transi.Play("Transi");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(0);
        GameManager.Instance.transi.Play("Detransi");
    }
}
