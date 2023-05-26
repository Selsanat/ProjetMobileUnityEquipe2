using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hero : entityManager
{

 
    public hero(Role role, int maxPV, int Pv, int attack, int nerf, Deck deck, int mana)
    {
        m_role = role;
        m_maxPv = maxPV;
        m_Pv = Pv;
        m_attack = attack;
        m_buff = 0;
        m_nerf = nerf;
        isAlive = true;
        m_deck = deck;
        m_mana = mana;
        int a = Random.Range(0, 1);
        if (a == 0) { multipleTarget = false; }
        else { multipleTarget = true; }
        if(gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }   
        gameManager.entityManager.heroList.Add(this);

    }
    #region GET & SET
    public int getMaxPv() { return m_maxPv; }
    public int getPv() { return m_Pv; }
    public void setPv(int pv) { m_Pv = pv; }
    public int getBuff() { return m_buff; }
    public void setBuff(int buff) { m_buff = buff; }
    public int getNerf() { return m_nerf; }
    public void setNerf(int nerf) { m_nerf = nerf; }
    public int getMana() { return m_mana; }
    public void setMana(int mana) { m_mana = mana; }
    public Deck GetDeck() { return m_deck; }
    public void setDeck(Deck deck) { m_deck = deck; }
    public bool getIsAlive() { return isAlive; }
    public void setIsAlive(bool set) { isAlive = set; }
    public void setFullLife() { this.m_Pv = this.m_maxPv; }
    #endregion




    public void EnemyAttack(List<hero> heroesToAttack, List<hero> listEnnemis)
    {

        switch(this.m_role)
        {
            case Role.ChienEnemy:
                chienIA(heroesToAttack);
                break;
            case Role.Squellettes:
                squelettes(heroesToAttack);
                break;

            default:
                return;
        }

        if (multipleTarget)
        {
            foreach (hero hero in heroesToAttack)
            {
                hero.takeDamage(3);
            }
        }
        else
        {
            hero old = new hero(entityManager.Role.Arboriste, 99999, 99999, 0, 0, new Deck(), 10);
            foreach (hero heroooo in heroesToAttack)
            {
                if (heroooo.getPv() < old.getPv())
                {
                    old = heroooo;
                }
            }
            old.takeDamage(3);
        }

    }

    #region IA
    public void chienIA(List<hero> heroesToAttack)
    {
        int firtAttack = 65;
        int secondAttack = 25;
        int thridAttack = 10;
        int dmg = 5;
        int AOEDmg = 3;
        int totalWeight = firtAttack + secondAttack + thridAttack;
        float diceRoll = Random.Range(0f, totalWeight);

        if (thridAttack >= diceRoll)
        {
            hero temp = heroesToAttack[0];

            foreach (hero champ in heroesToAttack)
            {
                if (champ.getPv() < temp.getPv())
                    temp = champ;
            }
            temp.takeDamage(dmg);
        }
        else if(secondAttack >= diceRoll)
        {
            foreach (hero hero in heroesToAttack)
            {
                hero.takeDamage(AOEDmg);
            }
        }
        else if (firtAttack >= diceRoll)
        {
            //add buff
        }

    }


    public void squelettes(List<hero> heroesToAttack)
    {
        int firtAttack = 60;
        int secondAttack = 40;
        int dmg = 3;
        int armor = 3;
        int totalWeight = firtAttack + secondAttack;
        float diceRoll = Random.Range(0f, totalWeight);

        foreach (hero champ in heroesToAttack)
        {
            if (champ.getPv() <= dmg)
            {
                champ.takeDamage(dmg);
                return;

            }
        }

        if (secondAttack >= diceRoll)
        {
            //buff
        }
        else if (firtAttack >= diceRoll)
        {
            hero temp = heroesToAttack[0];

            foreach (hero champ in heroesToAttack)
            {
                if (champ.getPv() < temp.getPv())
                    temp = champ;
            }
            temp.takeDamage(5);
        }
    }

    public void main(List<hero> heroesToAttack, List<hero> listEnnemies)
    {
        int nbMain = 0;

        foreach (hero hero in listEnnemies)
        {
            if (hero.m_role == Role.Mains)
                nbMain++;
        }

        int dmg = 5 * nbMain;
        int armor = 9;
        int firtAttack = 55;
        int secondAttack = 25;
        int thridAttack = 20;
        int totalWeight = firtAttack + secondAttack + thridAttack;
        float diceRoll = Random.Range(0f, totalWeight);
        


        if (thridAttack >= diceRoll)
        {
            hero temp = heroesToAttack[0];

            foreach (hero champ in heroesToAttack)
            {
                if (champ.getPv() < temp.getPv())
                    temp = champ;
            }
            temp.takeDamage(dmg);
        }
        else if (secondAttack >= diceRoll)
        {
            //nerf draw card
        }
        else if (firtAttack >= diceRoll)
        {
            //buff armor
        }




    }
    #endregion

}

/*public class enemy : entityManager
{
    enemy(int maxPV, int Pv, int attack, int buff, int nerf, int mana)
    {
        m_maxPv = maxPV;
        m_Pv = Pv;
        m_attack = attack;
        m_buff = buff;
        m_nerf = nerf;
        m_mana = mana;
        isAlive = true;
    }
    #region GET & SET
    public int getMaxPv() { return m_maxPv; }
    public int getPv() { return m_Pv; }
    public void setPv(int pv) { m_Pv = pv; }
    public int getBuff() { return m_buff; }
    public void setBuff(int buff) { m_buff = buff; }
    public int getNerf() { return m_nerf; }
    public void setNerf(int nerf) { m_nerf = nerf; }
    public int getMana() { return m_mana; }
    public void setMana(int mana) { m_mana = mana; }
    public deck GetDeck() { return m_deck; }
    public void setDeck(deck deck) { m_deck = deck; }
    public bool getIsAlive() { return isAlive; }
    public void setIsAlive(bool set) { isAlive = set; }
    #endregion
}
*/
