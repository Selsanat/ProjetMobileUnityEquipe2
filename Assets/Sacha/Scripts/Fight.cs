using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight : MonoBehaviour
{
    public main hand;

    public entityManager manager;

    public int mana;

    public List<CardObject> playedCards;


    [Button]
    void EndHeroTurn()
    {
        foreach (CardObject card in playedCards)
        {
            if(card.HeroToAttack == null) { card.HeroToAttack.Add(FindObjectOfType<hero>()); }
            playCard(card.DataCard,card.HeroToAttack);
        }
    }

    void playCard(dataCard card, List<hero> selected)
    {

        switch (card.CardType)
        {
            case 0:
                Debug.LogError("CARTE TYPE UNDIFINED");
                break;
            case (dataCard.cardType)1:
                foreach(hero hero in selected)
                {
                    card.takeDamage(hero);
                }
                break;
            case (dataCard.cardType)2:
                foreach (hero hero in selected)
                {
                    card.heal(hero);
                }
                break;
            case (dataCard.cardType)3:
                foreach (hero hero in selected)
                {
                    card.BuffDamage(hero);
                }
                break;
        }
    }

   /* public enum cardType
    {
        undifined = 0,
        Damage = 1,
        Heal = 2,
        BuffDamage = 3,
        BuffHeal = 4,
        Block = 5
    }*/
}
