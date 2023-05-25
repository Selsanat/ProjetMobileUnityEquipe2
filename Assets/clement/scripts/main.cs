using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour
{
    [SerializeField] int m_cardToDraw = 4;
    [SerializeField] int m_currentCard;
    [SerializeField] List<dataCard> m_listCard;
    [SerializeField] deck deck;


    public void Start()
    {
        deck = FindObjectOfType<deck>();
    }
    public void addCardToHand(dataCard card)
    {
        m_listCard.Add(card);
        m_currentCard++;
    }
    public void addCardToHand(dataCard card, int number)
    {
        for (int i = 0; i <= number; i++)
        {
            m_listCard.Add(card);
            m_currentCard++;
        }
    }
    public void addCardToHand(int number)
    {
        for (int i = 0; i <= number; i++)
        {
            m_listCard.Add(m_listCard[Random.Range(0,m_listCard.Count)]);
            m_currentCard++;
        }
    }

    public void tour()
    {
        foreach(dataCard card in m_listCard)
        {
            if (card.getIsDeleteOnTurn())
                m_listCard.Remove(card);
        }

        for (int i = 0; i < m_cardToDraw; i++)
            addCardToHand(deck.drawCard());
    }


}
