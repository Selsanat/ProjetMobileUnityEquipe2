using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMenu : MonoBehaviour
{

    BoxCollider2D hitBox2D;

    private void Start()
    {
        hitBox2D = GetComponent<BoxCollider2D>();
    }

    private void OnMouseDrag()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}
