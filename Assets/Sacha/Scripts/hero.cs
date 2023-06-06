using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hero : entityManager
{


    public hero(Role role, int maxPV, int Pv, int attack, int nerf, Deck deck, int mana, int venerate)
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
        m_venerate = venerate;
        int a = Random.Range(0, 1);
        if (m_role == Role.Arboriste)
            m_manaMax = 6;
        else
            m_manaMax = 4;
        if (a == 0) { multipleTarget = false; }
        else { multipleTarget = true; }
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
        gameManager.entityManager.heroList.Add(this);


    }
    public hero(Role role, int maxPV, int Pv, int attack, int nerf, Deck deck, int mana, int level, int experience, int experienceMax=4)
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
        m_experienceMax = 4+(level*2);
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
    public int getexperience() { return m_experience; }
    public int getexperienceMAX() { return m_experienceMax; }
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
    public int getVenerate() { return m_venerate; }
    public void setVenerate(int venerate) {m_venerate = venerate; }

    #endregion

    #region LEVEL UP
    public void levelUp()
    {
        m_level++;
        m_experience -= m_experienceMax;
        //m_experienceMax += 2;
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
        while (m_experience >= m_experienceMax)
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
                chienIA(heroesToAttack, fight);
                break;
            case Role.Squellettes:
                squelettes(heroesToAttack, fight);
                break;
           case Role.Gargouilles:
                gargouilleAttack(heroesToAttack, fight);
                break;
            case Role.HommesVers:
                hommeVers(heroesToAttack, fight);
                break;
            case Role.Demon:
                demonAttack(heroesToAttack, fight);
                break;
            case Role.Dragon:
                dragonAttack(heroesToAttack, fight);
                break;
            case Role.Mains:
                main(heroesToAttack, fight);
                break;
            default:
                return;
        }

        

    }

    #region IA
    public void chienIA(List<hero> heroesToAttack, bool fight)
    {
        if (this.isAlive == false)
            return;
        int firtAttack = 100;
        int secondAttack = 45;
        int thridAttack = 20;
        int fourthAttack = 10;
        int dmg = 5;
        int AOEDmg = 3;
        var tempColor = m_spriteTypeAttack.color;
        tempColor.a = 1f;
        m_spriteTypeAttack.color = tempColor;
        m_spriteFocus.color = tempColor;

        if (fight)
        {
            this.randomAttack = (int)Random.Range(0f, 100);
            if (fourthAttack >= randomAttack)
            {
                m_valueText.text = "";
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[4];
                this.randomHero = heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)];
                tempColor.a = 1f;
                m_spriteFocus.color = tempColor;
                if (randomHero.m_role == Role.Arboriste)
                {
                    this.m_spriteFocus.sprite = gameManager.FM.heroSprite;
                }
                else
                {
                    this.m_spriteFocus.sprite = gameManager.FM.heroSprite2;
                }
                Debug.Log("debuff armor");

            }
            else if (thridAttack >= randomAttack) //booste la force
            {
                Debug.Log("boostStat");
                m_valueText.text = "1";
                this.m_spriteFocus.sprite = null;
                tempColor.a = 0f;
                m_spriteFocus.color = tempColor;
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[3];


            }
            else if (secondAttack >= randomAttack) //attaque tout les allier
            {
                Debug.Log("aoe");
                m_valueText.text = AOEDmg.ToString();
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[2];
                this.m_spriteFocus.sprite = null;
                tempColor.a = 0f;
                m_spriteFocus.color = tempColor;
                m_IsAttacking = true;

            }
            else if (firtAttack >= randomAttack) //attauqe le plus faible
            {
                Debug.Log("attaque le plus faible");

                hero temp = heroesToAttack[0];

                foreach (hero champ in heroesToAttack)
                {
                    if (champ.getPv() < temp.getPv())
                        temp = champ;
                }
                randomHero = temp;
                m_valueText.text = dmg.ToString();
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[0];
                tempColor.a = 1f;
                m_spriteFocus.color = tempColor;
                if (randomHero.m_role == Role.Arboriste)
                {
                    this.m_spriteFocus.sprite = gameManager.FM.heroSprite;
                }
                else
                {
                    this.m_spriteFocus.sprite = gameManager.FM.heroSprite2;
                }
                m_IsAttacking = true;
            }
        }
        else
        {
            if (fourthAttack >= randomAttack)
            {
                randomHero.m_isDebufArmor = true;
                Debug.Log("debuff armor");

            }
            else if (thridAttack >= randomAttack) //booste la force
            {
                Debug.Log("boostStat");

                dmg++;
                AOEDmg++;

            }
            else if (secondAttack >= randomAttack) //attaque tout les allier
            {
                Debug.Log("aoe");

                foreach (hero hero in heroesToAttack)
                {
                    hero.takeDamage(AOEDmg);
                }
                m_IsAttacking = false;
            }
            else if (firtAttack >= randomAttack) //attauqe le plus faible
            {
                hero temp = heroesToAttack[0];
                m_IsAttacking = false;


                if (randomHero.getIsAlive() == true)
                    randomHero.takeDamage(dmg);
                else
                {
                    foreach (hero champ in heroesToAttack)
                    {
                        if (champ.getPv() - dmg <= 0)
                        {
                            temp.takeDamage(dmg);
                            return;
                        }
                    }
                }
            }
        }

        

    }


    public void squelettes(List<hero> heroesToAttack, bool fight)
    {
        if (this.isAlive == false)
            return;

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
                tempColor.a = 1f;
                m_spriteFocus.color = tempColor;
                if (randomHero.m_role == Role.Arboriste)
                {
                    this.m_spriteFocus.sprite = gameManager.FM.heroSprite;
                }
                else
                {
                    this.m_spriteFocus.sprite = gameManager.FM.heroSprite2;
                }
                m_IsAttacking = true;
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
                m_IsAttacking = false;
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

    public void main(List<hero> heroesToAttack, bool fight)
    {
        if (this.isAlive == false)
            return;
        int dmg = 5;
        int armor = 8;
        int firtAttack = 100;
        int secondAttack = 55;
        int thridAttack = 35;
        int fourthAttack = 10;
        int nbCardDebuf = 5;
        var tempColor = m_spriteTypeAttack.color;
        tempColor.a = 1f;
        m_spriteTypeAttack.color = tempColor;
        m_spriteFocus.color = tempColor;
        if (fight)
        {
            this.randomAttack = (int)Random.Range(0f, 100);
            if (fourthAttack >= randomAttack)
            {
                m_valueText.text = nbCardDebuf.ToString();
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[4];
                this.m_spriteFocus.sprite = null;
                tempColor.a = 0f;
                m_spriteFocus.color = tempColor;
                // nerf : ajoute des cartes injouables dans la pioche
            }
            else if (thridAttack >= randomAttack)
            {
                this.m_spriteFocus.sprite = null;
                tempColor.a = 0f;
                m_spriteFocus.color = tempColor;
                m_valueText.text = "1";
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[6];

            }
            else if (secondAttack >= randomAttack)
            {
                m_valueText.text = armor.ToString();
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[1];
                this.m_spriteFocus.sprite = null;
                tempColor.a = 0f;
                m_spriteFocus.color = tempColor;

            }
            else if (firtAttack >= randomAttack)
            {
                m_IsAttacking = true;
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
                tempColor.a = 1f;
                m_spriteFocus.color = tempColor;
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
            if (fourthAttack >= randomAttack)
            {
                throw new System.NotImplementedException("attaque pas encore implémenté (ajoute des cartes injouables dans la pioche)");
                // nerf : ajoute des cartes injouables dans la pioche
            }
            else if (thridAttack >= randomAttack)
            {
                gameManager.debuffDraw++;

            }
            else if (secondAttack >= randomAttack)
            {
                this.setArmor(armor);

            }
            else if (firtAttack >= randomAttack)
            {
                Debug.Log("dmg");
                m_IsAttacking = false;
                hero temp = heroesToAttack[0];

                foreach (hero champ in heroesToAttack)
                {
                    if (champ.getPv() - dmg <= 0)
                    {
                        temp.takeDamage(dmg);
                        return;
                    }
                }
                if (randomHero.getIsAlive() == true)
                    randomHero.takeDamage(dmg);
                else
                {
                    heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)].takeDamage(dmg);
                }
            }

        }
        
    }

    public void gargouilleAttack(List<hero> heroesToAttack, bool fight)
    {
        if (this.isAlive == false)
            return;
        int firtAttack = 100;
        int secondAttack = 60;
        int thridAttack = 25;
        int fourthAttack = 10;
        int dmgAOE = 6;
        int dmg = 8;

        var tempColor = m_spriteTypeAttack.color;
        tempColor.a = 1f;
        m_spriteTypeAttack.color = tempColor;
        m_spriteFocus.color = tempColor;
        if (fight)
        {
            randomAttack = (int)Random.Range(0f, 100);
            if (fourthAttack >= randomAttack) //debuff next round
            {

                hero temp = heroesToAttack[0];

                foreach (hero champ in heroesToAttack)
                {
                    if (champ.getPv() < temp.getPv())
                        temp = champ;
                }
                randomHero = temp;
                if (randomHero.m_role == Role.Arboriste)
                {
                    this.m_spriteFocus.sprite = gameManager.FM.heroSprite;
                }
                else
                {
                    this.m_spriteFocus.sprite = gameManager.FM.heroSprite2;
                }
                m_valueText.text = "2";
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[6];
                tempColor.a = 1f;
                m_spriteFocus.color = tempColor;
            }
            else if (thridAttack >= randomAttack)
            {
                m_valueText.text = "";
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[7];
                this.m_spriteFocus.sprite = null;
                tempColor.a = 0f;
                m_spriteFocus.color = tempColor;
            }
            else if (secondAttack >= randomAttack)
            {
                m_IsAttacking = true;
                Debug.Log("aoe");
                m_valueText.text = dmgAOE.ToString();
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[2];
                this.m_spriteFocus.sprite = null;
                tempColor.a = 0f;
                m_spriteFocus.color = tempColor;
            }
            else if (firtAttack >= randomAttack)
            {
                hero temp = heroesToAttack[0];
                m_IsAttacking = true;
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
                tempColor.a = 1f;
                m_spriteFocus.color = tempColor;
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
            if (fourthAttack >= randomAttack) //debuff next round
            {

                hero temp = heroesToAttack[0];

                foreach (hero champ in heroesToAttack)
                {
                    if (champ.getPv() < temp.getPv())
                        temp = champ;
                }

                temp.m_damageMultiplier = 2;
            }
            else if (thridAttack >= randomAttack)
            {
                this.isProvocation = true;
                gameManager.IsAnyProv = true;
            }
            else if (secondAttack >= randomAttack)
            {
                this.isProvocation = false;
                foreach (hero champ in heroesToAttack)
                    champ.takeDamage(dmgAOE);
            }
            else if (firtAttack >= randomAttack)
            {
                Debug.Log("dmg");
                m_IsAttacking = false;
                hero temp = heroesToAttack[0];

                foreach (hero champ in heroesToAttack)
                {
                    if (champ.getPv() - dmg <= 0)
                    {
                        temp.takeDamage(dmg);
                        return;
                    }
                }
                if (randomHero.getIsAlive() == true)
                    randomHero.takeDamage(dmg);
                else
                {
                    heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)].takeDamage(dmg);
                }
            }
        }


        
    }

    public void hommeVers(List<hero> heroesToAttack, bool fight)
    {
        if (this.isAlive == false)
            return;
        int firtAttack = 100;
        int secondAttack = 65;
        int thridAttack = 30;
        int fourthAttack = 10;
        
        int dmgLourd = 10;
        int dmg = 7;
        int heal = 3;
        var tempColor = m_spriteTypeAttack.color;
        tempColor.a = 1f;
        m_spriteTypeAttack.color = tempColor;
        m_spriteFocus.color = tempColor;

        if (fight)
        {
            randomAttack = (int)Random.Range(0f, 100);
            if (fourthAttack >= randomAttack)
            {
                m_IsAttacking = true;

                randomHero = heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)];
                m_valueText.text = dmg.ToString();
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[0];
                tempColor.a = 1f;
                m_spriteFocus.color = tempColor;
                if (randomHero.m_role == Role.Arboriste)
                {
                    this.m_spriteFocus.sprite = gameManager.FM.heroSprite;
                }
                else
                {
                    this.m_spriteFocus.sprite = gameManager.FM.heroSprite2;
                }
            }
            else if (thridAttack >= randomAttack)
            {
                m_IsAttacking = true;
                randomHero = heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)];
                m_valueText.text = dmgLourd.ToString();
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[0];
                tempColor.a = 1f;
                m_spriteFocus.color = tempColor;
                if (randomHero.m_role == Role.Arboriste)
                {
                    this.m_spriteFocus.sprite = gameManager.FM.heroSprite;
                }
                else
                {
                    this.m_spriteFocus.sprite = gameManager.FM.heroSprite2;
                }
            }
            else if (secondAttack >= randomAttack)
            {
                //nerf et on ne vois plus la description des cartes
                throw new System.NotImplementedException("attaque pas encore implémenté (ne plus voir la description des cartes)");
            }
            else if (firtAttack >= randomAttack)
            {
                
                randomHero = heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)];
               
                if (randomHero.m_role == Role.Arboriste)
                {
                    this.m_spriteFocus.sprite = gameManager.FM.heroSprite;
                }
                else
                {
                    this.m_spriteFocus.sprite = gameManager.FM.heroSprite2;
                }
                this.m_valueText.text = "";
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[5];
                tempColor.a = 1f;
                m_spriteFocus.color = tempColor;
            }
        }
        else
        {
            if (fourthAttack >= randomAttack)
            {
                heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)].takeDamage(dmg);
                this.m_Pv += heal;
            }
            else if (thridAttack >= randomAttack)
            {
                heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)].takeDamage(dmgLourd);
            }
            else if (secondAttack >= randomAttack)
            {
                //nerf et on ne vois plus la description des cartes
                throw new System.NotImplementedException("attaque pas encore implémenté (ne plus voir la description des cartes)");
            }
            else if (firtAttack >= randomAttack)
            {
                if(randomHero.getIsAlive() == true)
                    randomHero.m_isDebufArmor = true;
                else
                {
                    heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)].m_isDebufArmor = true;
                }
            }
        }

        
    }

    public void demonAttack(List<hero> heroesToAttack, bool fight)
    {

        if (this.isAlive == false)
            return;
        int firtAttack = 100;
        int secondAttack = 45;
        int thridAttack = 12;
        int dmg = 12;
        var tempColor = m_spriteTypeAttack.color;
        tempColor.a = 1f;
        m_spriteTypeAttack.color = tempColor;
        m_spriteFocus.color = tempColor;

        if (fight)
        {
            randomAttack = (int)Random.Range(0f, 100);
            if (thridAttack >= randomAttack)
            {
                int healing = this.m_Pv * 15 / 100;
                this.m_valueText.text = healing.ToString();
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[8];
            }
            else if (secondAttack >= randomAttack)
            {
                hero temp = heroesToAttack[0];
                m_IsAttacking = true;
                foreach (hero champ in heroesToAttack)
                {
                    if (champ.getPv() > temp.getPv())
                    {
                        temp = champ;

                    }
                }
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
                
            }
            else if (firtAttack >= randomAttack)
            {
                hero temp = heroesToAttack[0];
                m_IsAttacking = true;
                foreach (hero champ in heroesToAttack)
                {
                    if (champ.getPv() > temp.getPv())
                    {
                        temp = champ;

                    }
                }
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
            }

        }
        else
        {
            if (thridAttack >= randomAttack)
            {
                this.m_Pv = this.m_Pv * 15 / 100;
            }
            else if (secondAttack >= randomAttack)
            {
                hero temp = heroesToAttack[0];

                foreach (hero champ in heroesToAttack)
                {
                    if (champ.getPv() > temp.getPv())
                        temp = champ;
                }
                temp.takeDamage(dmg);
                m_IsAttacking = false;
            }
            else if (firtAttack >= randomAttack)
            {
                hero temp = heroesToAttack[0];

                foreach (hero champ in heroesToAttack)
                {
                    if (champ.getPv() < temp.getPv())
                        temp = champ;
                }
                temp.takeDamage(dmg);
                m_IsAttacking = false;
            }
        }

        
    }


    public void dragonAttack(List<hero> heroesToAttack, bool fight)
    {
        if (this.isAlive == false)
            return;
        int firtAttack = 100;
        int secondAttack = 75;
        int thridAttack = 55;
        int fourthAttack = 25;
        int fithAttack = 10;
        int sixth = 5;
        int dmg = 16;
        int dmgAOE = 10;
        int dmgLourd = 20;
        int armor = 12;
        var tempColor = m_spriteTypeAttack.color;
        tempColor.a = 1f;
        m_spriteTypeAttack.color = tempColor;
        m_spriteFocus.color = tempColor;

        if (fight)
        {
            randomAttack = (int)Random.Range(0f, 100);
            if (sixth >= randomAttack) //antiheal
            {
                randomHero = heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)];
                m_valueText.text = "1";
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[9];
                if (randomHero.m_role == Role.Arboriste)
                {
                    this.m_spriteFocus.sprite = gameManager.FM.heroSprite;
                }
                else
                {
                    this.m_spriteFocus.sprite = gameManager.FM.heroSprite2;
                }
            }
            else if (fithAttack >= randomAttack) //boost
            {
                Debug.Log("boostStat");
                m_valueText.text = "2";
                this.m_spriteFocus.sprite = null;
                tempColor.a = 0f;
                m_spriteFocus.color = tempColor;
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[3];
            }
            else if (fourthAttack >= randomAttack) //armor
            {
                m_valueText.text = armor.ToString();
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[1];
                this.m_spriteFocus.sprite = null;
                tempColor.a = 0f;
                m_spriteFocus.color = tempColor;
            }
            else if (thridAttack >= randomAttack) //aoe
            {
                Debug.Log("aoe");
                m_IsAttacking = true;
                m_valueText.text = dmgAOE.ToString();
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[2];
                this.m_spriteFocus.sprite = null;
                tempColor.a = 0f;
                m_spriteFocus.color = tempColor;
            }
            else if (secondAttack >= randomAttack) //lourd
            {
                m_IsAttacking = true;
                randomHero = heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)];
                m_valueText.text = dmgLourd.ToString();
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[0];
                tempColor.a = 1f;
                m_spriteFocus.color = tempColor;
                if (randomHero.m_role == Role.Arboriste)
                {
                    this.m_spriteFocus.sprite = gameManager.FM.heroSprite;
                }
                else
                {
                    this.m_spriteFocus.sprite = gameManager.FM.heroSprite2;
                }
            }
            else if (firtAttack >= randomAttack) //normal
            {
                m_IsAttacking = true;
                randomHero = heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)];
                m_valueText.text = dmg.ToString();
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[0];
                tempColor.a = 1f;
                m_spriteFocus.color = tempColor;
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
            if (sixth >= randomAttack) //antiheal
            {
                if(randomHero.isAlive) 
                    randomHero.isAntiHeal = true;
                else
                    heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)].isAntiHeal = true;
            }
            else if (fithAttack >= randomAttack) //boost
            {
                dmg += 2;
                dmgAOE += 2;
                dmgLourd += 2;
            }
            else if (fourthAttack >= randomAttack) //armor
            {
                this.setArmor(armor);
            }
            else if (thridAttack >= randomAttack) //aoe
            {
                foreach (hero hero in heroesToAttack)
                {
                    hero.takeDamage(dmgAOE);
                }
                m_IsAttacking = false;
            }
            else if (secondAttack >= randomAttack) //lourd
            {
                m_IsAttacking = false;
                if (randomHero.isAlive)
                    randomHero.takeDamage(dmgLourd);
                else
                    heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)].takeDamage(dmgLourd);
            }
            else if (firtAttack >= randomAttack) //normal
            {
                m_IsAttacking = false;
                if (randomHero.isAlive)
                    randomHero.takeDamage(dmg);
                else
                heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)].takeDamage(dmg);
            }
        }
        
  
    }

    #endregion

}

