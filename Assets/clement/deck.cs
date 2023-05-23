using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deck : MonoBehaviour
{

    [SerializeField] int m_maxCard;
    [SerializeField] int m_currentCard;
    [SerializeField] List<card> m_listCard;


    void Start()
    {
        m_currentCard = m_maxCard;
    }

    public void addCardToDeck(card card)
    {
        if(m_currentCard < m_maxCard)
            m_listCard.Add(card);
    }
    public void removeCardToDeck(card card)
    {
        if(m_listCard.Contains(card))
            m_listCard.Remove(card);
    }

    public void drawCard()
    {
        if(m_currentCard >= 1)
        {
            card drawCard = m_listCard[Random.Range(0, m_currentCard)];
            removeCardToDeck(drawCard);
        }
    }


}
