/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deck : MonoBehaviour
{

    [SerializeField] int m_maxCard;
    [SerializeField] int m_currentCard;
    [SerializeField] List<dataCard> m_listCard;
    [SerializeField] List<dataCard> m_listCardStart;


    void Start()
    {
        m_currentCard = m_maxCard;
        Random.InitState((int)System.DateTime.Now.Ticks);
    }
    
    public void addCardToDeck(dataCard card)
    {
        if(m_currentCard < m_maxCard)
        {
            m_listCard.Add(card);
            m_currentCard++;
        }
    }
    public void removeCardToDeck(dataCard card)
    {
        if(m_listCard.Contains(card))
        {
            m_listCard.Remove(card);
            m_currentCard--;
        }
    }

    public dataCard drawCard()
    {
        if(m_currentCard >= 1)
        {
            dataCard drawCard = m_listCard[Random.Range(0, m_currentCard)];
            removeCardToDeck(drawCard);
            m_currentCard--;
            return drawCard;
        }
        else
        {
            reloadDeck();
            dataCard drawCard = m_listCard[Random.Range(0, m_currentCard)];
            removeCardToDeck(drawCard);
            m_currentCard--;
            return drawCard;
        }
    }


    public void reloadDeck()
    {
        m_listCard.Clear();
        m_listCard = m_listCardStart;
        m_currentCard = m_maxCard;

    }


}
*/