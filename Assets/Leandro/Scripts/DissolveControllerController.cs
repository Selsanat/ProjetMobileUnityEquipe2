using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Color = UnityEngine.Color;

public class DissolveControllerController : MonoBehaviour
{
    [SerializeField] TMP_Text Textitre;
    [SerializeField] TMP_Text texte;
    [SerializeField] Volume Volume;
    [SerializeField] List<Light2D> Lights;
    private LensDistortion lensDistortion;
    private Bloom bloom;
    public List<DissolveController> dissolveControllers;
    public List<SpriteRenderer> SpriteRenderers;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EcranFin());
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
        yield return ValueTranspoVolume(0f, -0.75f);
        yield return ValueTranspoVolume(-0.75f, 0f);
        StartCoroutine(Light(Lights[1], 0,1f));
        StartCoroutine(Light(Lights[2], 0, 1f));
        yield return new WaitUntil(() => Input.GetMouseButton(0));
        yield return TransposeTextColo();
        StartCoroutine(Ecrit("Tel un chasseur impitoyable, vous réduisez les monstruosités à néant, laissant derrière vous un véritable charnier maccabre, victorieux dans cette danse sanglante"));
    }
}
