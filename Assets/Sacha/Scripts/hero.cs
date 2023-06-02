using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        m_level = 0;
        m_experience = 0;
        int a = Random.Range(0, 1);
        if (m_role == Role.Arboriste)
            m_manaMax = 6;
        else
            m_manaMax = 4;
        if (a == 0) { multipleTarget = false; }
        else { multipleTarget = true; }
        if(gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }   
        gameManager.entityManager.heroList.Add(this);


    }
    public hero(Role role, int maxPV, int Pv, int attack, int nerf, Deck deck, int mana, int level, int experience)
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
        m_level = level;
        m_experience = experience;
        if(m_role == Role.Arboriste)
            m_manaMax = 6;
        else
            m_manaMax = 4;

        int a = Random.Range(0, 1);
        if (a == 0) { multipleTarget = false; }
        else { multipleTarget = true; }
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
        gameManager.entityManager.heroList.Add(this);


    }


    public hero(Role role, int maxPv, int Pv, Sprite sprite)
    {
        m_role = role;
        m_maxPv = maxPv;
        m_Pv = Pv;
        m_sprite = sprite;
        m_buff = 0;
        m_nerf = 0;
        isAlive = true;
        m_armor = 0;
        m_level = 0;
        m_experience = 0;
        if (m_role == Role.Arboriste)
            m_manaMax = 6;
        else
            m_manaMax = 4;
        if (gameManager == null)
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
    public void healMidLife() { this.m_Pv = this.m_maxPv / 2; }
    public int getArmor() { return m_armor; }
    public void setArmor(int armor) { m_armor += armor; }
    public void resetArmor() { m_armor = 0; }
    public bool getIsProvocation() { return isProvocation; }

    #endregion

    #region LEVEL UP
    public void levelUp()
    {
        m_level++;
        m_experience = 0;
        if (this.m_role == Role.Arboriste)
        {
            gameManager.expArboriste = m_experience;
            gameManager.levelArboriste = m_level;
        }
        else if (this.m_role == Role.Pretre)
        {
            gameManager.expPretre = m_experience;
            gameManager.levelPretre = m_level;
        }

        /*m_maxPv = m_maxPv + 10;
        m_Pv = m_maxPv;*/

        if (m_level == 1)
        {
            //Accueillir les nécessiteux
            //Armure d'écorse
        }
        else if (m_level == 2)
        {
            //Suivre les étoiles
            //"Par ma main, soit béni !"
        }
        else if (m_level == 3)
        {
            //Au pied des tabernacles
            //Dormir prêt de l'autre
        }
        else if (m_level == 4)
        {
            //Surgissement Vitalique
            //Cultiver son âme
        }
        else if (m_level == 5)
        {
            //Communion avec la nature
            //Allumer les cierges 
        }
        else if (m_level == 6)
        {
            //conversion
            //Application de cataplasme
        }
    }

    public void gainExperience(int experience)
    {
        m_experience += experience;
        if(this.m_role == Role.Arboriste)
        {
            gameManager.expArboriste = m_experience;
        }
        else if(this.m_role == Role.Pretre)
        {
            gameManager.expPretre = m_experience;
        }
        if (m_experience >= m_experienceMax)
        {
            levelUp();
        }
    }
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

    public void EnemyAttack(List<hero> heroesToAttack, bool fight)
    {
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);

        switch (this.m_role)
        {
            case Role.ChienEnemy:
                chienIA(heroesToAttack);
                break;
            case Role.Squellettes:
                squelettes(heroesToAttack, fight);
                break;
           case Role.Gargouilles:
                gargouilleAttack(heroesToAttack);
                break;
            case Role.HommesVers:
                hommeVers(heroesToAttack);
                break;
            case Role.Demon:
                demonAttack(heroesToAttack);
                break;
            case Role.Dragon:
                dragonAttack(heroesToAttack);
                break;
            case Role.Mains:
                main(heroesToAttack);
                break;
            default:
                return;
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
            heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)].m_isDebufArmor = true;
            Debug.Log("debuff armor");

        }
        else if (thridAttack >= diceRoll) //booste la force
        {
            Debug.Log("boostStat");

            dmg++;
            AOEDmg++;
            
        }
        else if(secondAttack >= diceRoll) //attaque tout les allier
        {
            Debug.Log("aoe");

            foreach (hero hero in heroesToAttack)
            {
                hero.takeDamage(AOEDmg);
            }
        }
        else if (firtAttack >= diceRoll) //attauqe le plus faible
        {
            Debug.Log("attaque le plus faible");

            hero temp = heroesToAttack[0];

            foreach (hero champ in heroesToAttack)
            {
                if (champ.getPv() < temp.getPv())
                    temp = champ;
            }
            temp.takeDamage(dmg);
        }

    }


    public void squelettes(List<hero> heroesToAttack, bool fight)
    {

        int firtAttack = 100;
        int secondAttack = 40;
        int dmg = 3;
        int armor = 2;
        var tempColor = m_spriteTypeAttack.color;
        tempColor.a = 1f;
        m_spriteTypeAttack.color = tempColor;
        m_spriteFocus.color = tempColor;
        if (fight)
        {
            this.randomAttack = (int)Random.Range(0f, 100);
            if (secondAttack >= randomAttack)
            {
                m_valueText.text = armor.ToString();
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[1];
                this.m_spriteFocus.sprite = null;
                tempColor.a = 0f;
                m_spriteFocus.color = tempColor;

            }
            else if (firtAttack >= randomAttack)
            {

                hero temp = heroesToAttack[0];

                foreach (hero champ in heroesToAttack)
                {
                    if (champ.getPv() - dmg <= 0)
                    {
                        randomHero = temp;
                        m_valueText.text = dmg.ToString();
                        this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[0];
                        if (randomHero.m_role == Role.Arboriste)
                        {
                            this.m_spriteFocus.sprite = gameManager.FM.heroSprite;
                        }
                        else
                        {
                            this.m_spriteFocus.sprite = gameManager.FM.heroSprite2;
                        }
                        
                        return;
                    }
                }
                randomHero = heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)];
                m_valueText.text = dmg.ToString();
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[0];
                if (randomHero.m_role == Role.Arboriste)
                {
                    this.m_spriteFocus.sprite = gameManager.FM.heroSprite;
                }
                else
                {
                    this.m_spriteFocus.sprite = gameManager.FM.heroSprite2;
                }
            }

        }
        else
        {
            if (secondAttack >= randomAttack)
            {
                Debug.Log("armor");
                this.setArmor(armor);
            }
            else if (firtAttack >= randomAttack)
            {
                Debug.Log("dmg");

                hero temp = heroesToAttack[0];

                foreach (hero champ in heroesToAttack)
                {
                    if (champ.getPv() - dmg <= 0)
                    {
                        temp.takeDamage(dmg);
                        return;
                    }
                }
                if(randomHero.getIsAlive() == true)
                    randomHero.takeDamage(dmg);
                else
                {
                    heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)].takeDamage(dmg);
                }
            }
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
            throw new System.NotImplementedException("attaque pas encore implémenté (ajoute des cartes injouables dans la pioche)");
            // nerf : ajoute des cartes injouables dans la pioche
        }
        else if (thridAttack >= diceRoll)
        {
            gameManager.debuffDraw++;

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

            temp.m_damageMultiplier = 2;
        }
        else if (thridAttack >= diceRoll)
        {
            this.isProvocation = true;
            gameManager.IsAnyProv = true;
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
            throw new System.NotImplementedException("attaque pas encore implémenté (ne plus voir la description des cartes)");
        }
        else if (firtAttack >= diceRoll)
        {
            heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)].m_isDebufArmor = true;
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
            heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)].isAntiHeal = true;
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

