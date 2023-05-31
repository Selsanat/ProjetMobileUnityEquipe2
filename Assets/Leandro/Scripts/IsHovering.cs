using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IsHovering : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameManager.isHoverButton = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        gameManager.isHoverButton = true;
    }

}
