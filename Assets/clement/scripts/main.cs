using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour
{
    [SerializeField] int m_cardToDraw;
    [SerializeField] int m_currentCard;
    [SerializeField] List<card> m_listCard;
    [SerializeField] deck deck;


    public void Start()
    {
        deck = FindObjectOfType<deck>();
    }
    public void addCardToHand(card card)
    {
        m_listCard.Add(card);
        m_currentCard++;
    }

    public void tour()
    {
        foreach(card card in m_listCard)
        {
            if (card.getIsDeleteOnTurn())
                m_listCard.Remove(card);
        }

        for (int i = 0; i < m_cardToDraw; i++)
            addCardToHand(deck.drawCard());
    }


}
