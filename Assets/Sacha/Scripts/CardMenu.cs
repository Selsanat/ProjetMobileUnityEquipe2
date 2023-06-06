using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.Events;

public class CardMenu : MonoBehaviour
{
    public Vector3 StartPos;
    Transform WhereIwasPos;
    public Transform UpToDrag;

    public Vector3 test;


    public UnityEvent MouseUpEvent;

    BoxCollider2D hitBox2D;

    public bool yeah = false;

    float avancement;
    public float speed = 0.02f;

    public float timeToStart = 0;

    DissolveController dissolve;

    private void Start()
    {
        dissolve = GetComponent<DissolveController>();
        StartPos = transform.position;
        hitBox2D = GetComponent<BoxCollider2D>();
    }

    private void OnMouseDrag()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    private void OnMouseUp()
    {
        WhereIwasPos = this.transform;
        if (transform.position.y > UpToDrag.position.y)
        {
            StartCoroutine(gocard());
        }else
        {
            yeah = true;
        }
    }

    IEnumerator gocard()
    {
        dissolve.isDissolving = true;
        yield return new WaitUntil(() => dissolve.dissolveAmount < 0);
        MouseUpEvent.Invoke();
    }

    private void Update()
    {
        if (yeah)
        {
            avancement += speed;
            transform.position = Vector3.Lerp(transform.position, StartPos, avancement);
            if(avancement > 1)
            {
                avancement = 0;
                transform.position = StartPos;
                yeah = false;    
            }
        }
    }
}
