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
        m_armor = 0;
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

    public int getArmor() { return m_armor; }
    public void setArmor(int armor) { m_armor += armor; }
    public void resetArmor() { m_armor = 0; }
    
    #endregion


    public void setVarHero()
    {
        if (m_role == Role.Arboriste)
        {
            gameManager.LifeArboriste = m_Pv;
        }
        else if (m_role == Role.Pretre)
        {
            gameManager.LifePretre = m_Pv;
        }
    }

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
        int firtAttack = 100;
        int secondAttack = 45;
        int thridAttack = 20;
        int fourthAttack = 10;
        int dmg = 5;
        int AOEDmg = 3;
        float diceRoll = Random.Range(0f, 100);

        if(fourthAttack >= diceRoll)
        {
            //diminue de moitier l'armur gagné pd 1 tour
        }
        else if (thridAttack >= diceRoll) //booste la force
        {
            dmg++;
            AOEDmg++;
            
        }
        else if(secondAttack >= diceRoll) //attaque tout les allier
        {

            foreach (hero hero in heroesToAttack)
            {
                hero.takeDamage(AOEDmg);
            }
        }
        else if (firtAttack >= diceRoll) //attauqe le plus faible
        {
            hero temp = heroesToAttack[0];

            foreach (hero champ in heroesToAttack)
            {
                if (champ.getPv() < temp.getPv())
                    temp = champ;
            }
            temp.takeDamage(dmg);
        }

    }


    public void squelettes(List<hero> heroesToAttack)
    {
        int firtAttack = 100;
        int secondAttack = 40;
        int dmg = 3;
        int armor = 2;
        float diceRoll = Random.Range(0f, 100);


        if (secondAttack >= diceRoll)
        {
            this.setArmor(armor);
        }
        else if (firtAttack >= diceRoll)
        {
            hero temp = heroesToAttack[0];

            foreach (hero champ in heroesToAttack)
            {
                if (champ.getPv() - dmg <= 0)
                {
                    temp.takeDamage(dmg);
                    return;
                }
            }
            heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)].takeDamage(dmg);
        }
    }

    public void main(List<hero> heroesToAttack)
    {
        int dmg = 5;
        int armor = 8;
        int firtAttack = 100;
        int secondAttack = 55;
        int thridAttack = 35;
        int fourthAttack = 10;
        float diceRoll = Random.Range(0f, 100);
        
        if(fourthAttack >= diceRoll)
        {
                // nerf : ajoute des cartes injouables dans la pioche
        }
        else if (thridAttack >= diceRoll)
        {
            //nerf draw card - 1

        }
        else if (secondAttack >= diceRoll)
        {
            this.setArmor(armor);

        }
        else if (firtAttack >= diceRoll)
        {
            hero temp = heroesToAttack[0];

            foreach (hero champ in heroesToAttack)
            {
                if (champ.getPv() < temp.getPv())
                    temp = champ;
            }
            temp.takeDamage(dmg);
        }
    }

    public void gargouilleAttack(List<hero> heroesToAttack)
    {
        int firtAttack = 100;
        int secondAttack = 60;
        int thridAttack = 25;
        int fourthAttack = 10;
        int dmgAOE = 6;
        int dmg = 8;
        float diceRoll = Random.Range(0f, 100);

        if (fourthAttack >= diceRoll) //debuff next round
        {

            hero temp = heroesToAttack[0];

            foreach (hero champ in heroesToAttack)
            {
                if (champ.getPv() < temp.getPv())
                    temp = champ;
            }

            //ajout du debuff p^rochain * 2
        }
        else if (thridAttack >= diceRoll)
        {
            //provocation
        }
        else if (secondAttack >= diceRoll)
        {
            foreach(hero champ in heroesToAttack)
                champ.takeDamage(dmgAOE);
        }
        else if (firtAttack >= diceRoll)
        {
            hero temp = heroesToAttack[0];

            foreach (hero champ in heroesToAttack)
            {
                if (champ.getPv() < temp.getPv())
                    temp = champ;
            }
            temp.takeDamage(dmg);
        }
    }

    public void hommeVers(List<hero> heroesToAttack)
    {
        int firtAttack = 100;
        int secondAttack = 65;
        int thridAttack = 30;
        int fourthAttack = 10;
        float diceRoll = Random.Range(0f, 100);
        int dmgLourd = 10;
        int dmg = 7;
        int heal = 3;

        if (fourthAttack >= diceRoll)
        {
            heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)].takeDamage(dmg);
            this.m_Pv += heal;
        }
        else if (thridAttack >= diceRoll)
        {
            heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)].takeDamage(dmgLourd);
        }
        else if (secondAttack >= diceRoll)
        {
            //nerf et on ne vois plus la description des cartes
        }
        else if (firtAttack >= diceRoll)
        {
            //réduit les cartes d amrmor /2
        }
    }

    public void demonAttack(List<hero> heroesToAttack)
    {
        int firtAttack = 100;
        int secondAttack = 45;
        int thridAttack = 12;
        float diceRoll = Random.Range(0f, 100);
        int dmg = 12;

        if (thridAttack >= diceRoll)
        {
            this.m_Pv = this.m_Pv * 15 / 100;
        }
        else if (secondAttack >= diceRoll)
        {
            hero temp = heroesToAttack[0];

            foreach (hero champ in heroesToAttack)
            {
                if (champ.getPv() > temp.getPv())
                    temp = champ;
            }
            temp.takeDamage(dmg);
        }
        else if (firtAttack >= diceRoll)
        {
            hero temp = heroesToAttack[0];

            foreach (hero champ in heroesToAttack)
            {
                if (champ.getPv() < temp.getPv())
                    temp = champ;
            }
            temp.takeDamage(dmg);
        }
    }


    public void dragonAttack(List<hero> heroesToAttack)
    {
        int firtAttack = 100;
        int secondAttack = 75;
        int thridAttack = 55;
        int fourthAttack = 25;
        int fithAttack = 10;
        int sixth = 5;
        float diceRoll = Random.Range(0f, 100);
        int dmg = 16;
        int dmgAOE = 10;
        int dmgLourd = 20;
        int armor = 12;

        if (sixth >= diceRoll)
        {
            //anti heal 
        }
        else if (fithAttack >= diceRoll)
        {
            dmg += 2;
            dmgAOE += 2;
            dmgLourd += 2;
        }
        else if (fourthAttack >= diceRoll)
        {
            this.setArmor(armor);
        }
        else if (thridAttack >= diceRoll)
        {
            foreach (hero hero in heroesToAttack)
            {
                hero.takeDamage(dmgAOE);
            }
        }
        else if (secondAttack >= diceRoll)
        {
            heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)].takeDamage(dmgLourd);
        }
        else if (firtAttack >= diceRoll)
        {
            heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)].takeDamage(dmg);
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
