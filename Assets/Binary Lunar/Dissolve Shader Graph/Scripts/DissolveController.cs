using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveController : MonoBehaviour
{
    public float dissolveAmount;
    public float dissolveSpeed;
    public bool isDissolving;
    [ColorUsageAttribute(true,true)]
    public Color outColor;
    [ColorUsageAttribute(true, true)]
    public Color inColor;

    public Material mat;


    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
        if (gameObject.tag != "Player")
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            FindObjectOfType<DissolveControllerController>()?.dissolveControllers.Add(this);
            FindObjectOfType<DissolveControllerController>()?.SpriteRenderers.Add(sprite);
            

            if (FindObjectOfType<DissolveControllerController>() != null)
            {
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);
            }
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            isDissolving = true;

        if (Input.GetKeyDown(KeyCode.S))
            isDissolving = false;

        if (isDissolving)
        {
            DissolveOut(dissolveSpeed, outColor);
        }

        if (!isDissolving)
        {
            DissolveIn(dissolveSpeed, inColor);
        }

        mat.SetFloat("_DissolveAmount", dissolveAmount);


    }



    public void DissolveOut(float speed, Color color)
    {
        mat.SetColor("_DissolveColor", color);
        if (dissolveAmount > -0.1)
            dissolveAmount -= Time.deltaTime * speed;
    }

    public void DissolveIn(float speed, Color color)
    {
        mat.SetColor("_DissolveColor", color);
        if (dissolveAmount < 1)
            dissolveAmount += Time.deltaTime * dissolveSpeed;
    }
}
