using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.Arm;

public class hero : entityManager
{
    #region stat ennemies GD
    public int ChienfirtAttack = 100;
    public int ChiensecondAttack = 45;
    public int ChienthridAttack = 20;
    public int ChienfourthAttack = 10;
    public int Chiendmg = 5;
    public int ChienAOEDmg = 3;

    public int SquelettesfirtAttack = 100;
    public int SquelettessecondAttack = 40;
    public int Squelettesdmg = 3;
    public int Squelettesarmor = 2;

    public int Maindmg = 5;
    public int Mainarmor = 8;
    public int MainfirtAttack = 100;
    public int MainsecondAttack = 55;
    public int MainthridAttack = 35;
    public int MainfourthAttack = 10;
    public int MainnbCardDebuf = 5;

    public int GargouillefirtAttack = 100;
    public int GargouillesecondAttack = 60;
    public int GargouillethridAttack = 25;
    public int GargouillefourthAttack = 10;
    public int GargouilledmgAOE = 6;
    public int Gargouilledmg = 8;

    public int HommeVersfirtAttack = 100;
    public int HommeVerssecondAttack = 65;
    public int HommeVersthridAttack = 30;
    public int HommeVersfourthAttack = 10;
    public int HommeVersdmgLourd = 10;
    public int HommeVersdmg = 7;
    public int HommeVersheal = 3;

    public int DemonfirtAttack = 100;
    public int DemonsecondAttack = 45;
    public int DemonthridAttack = 12;
    public int Demondmg = 12;

    public int DragonfirtAttack = 100;
    public int DragonsecondAttack = 75;
    public int DragonthridAttack = 55;
    public int DragonfourthAttack = 25;
    public int DragonfithAttack = 10;
    public int Dragonsixth = 5;
    public int Dragondmg = 16;
    public int DragondmgAOE = 10;
    public int DragondmgLourd = 20;
    public int Dragonarmor = 12;
    #endregion
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

    public void addEffect(dataCard.CardEffect card)
    {
        MyEffects.Add(card);
    }
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
        
        var tempColor = m_spriteTypeAttack.color;
        tempColor.a = 1f;
        m_spriteTypeAttack.color = tempColor;
        m_spriteFocus.color = tempColor;

        if (fight)
        {
            this.randomAttack = (int)Random.Range(0f, 100);
            if (ChienfourthAttack >= randomAttack)
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
            else if (ChienthridAttack >= randomAttack) //booste la force
            {
                Debug.Log("boostStat");
                m_valueText.text = "1";
                this.m_spriteFocus.sprite = null;
                tempColor.a = 0f;
                m_spriteFocus.color = tempColor;
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[3];


            }
            else if (ChiensecondAttack >= randomAttack) //attaque tout les allier
            {
                Debug.Log("aoe");
                m_valueText.text = ChienAOEDmg.ToString();
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[2];
                this.m_spriteFocus.sprite = null;
                tempColor.a = 0f;
                m_spriteFocus.color = tempColor;
                m_IsAttacking = true;

            }
            else if (ChienfirtAttack >= randomAttack) //attauqe le plus faible
            {
                Debug.Log("attaque le plus faible");

                hero temp = heroesToAttack[0];

                foreach (hero champ in heroesToAttack)
                {
                    if (champ.getPv() < temp.getPv())
                        temp = champ;
                }
                randomHero = temp;
                m_valueText.text = Chiendmg.ToString();
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
            if (ChienfourthAttack >= randomAttack)
            {
                randomHero.m_isDebufArmor = true;
                Debug.Log("debuff armor");

            }
            else if (ChienthridAttack >= randomAttack) //booste la force
            {
                Debug.Log("boostStat");

                Chiendmg++;
                ChienAOEDmg++;

            }
            else if (ChiensecondAttack >= randomAttack) //attaque tout les allier
            {
                Debug.Log("aoe");

                foreach (hero hero in heroesToAttack)
                {
                    hero.takeDamage(ChienAOEDmg);
                }
                m_IsAttacking = false;
            }
            else if (ChienfirtAttack >= randomAttack) //attauqe le plus faible
            {
                hero temp = heroesToAttack[0];
                m_IsAttacking = false;


                if (randomHero.getIsAlive() == true)
                    randomHero.takeDamage(Chiendmg);
                else
                {
                    foreach (hero champ in heroesToAttack)
                    {
                        if (champ.getPv() - Chiendmg <= 0)
                        {
                            temp.takeDamage(Chiendmg);
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

        var tempColor = m_spriteTypeAttack.color;
        tempColor.a = 1f;
        m_spriteTypeAttack.color = tempColor;
        m_spriteFocus.color = tempColor;
        if (fight)
        {
            this.randomAttack = (int)Random.Range(0f, 100);
            if (SquelettessecondAttack >= randomAttack)
            {
                m_valueText.text = Squelettesarmor.ToString();
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[1];
                this.m_spriteFocus.sprite = null;
                tempColor.a = 0f;
                m_spriteFocus.color = tempColor;

            }
            else if (SquelettesfirtAttack >= randomAttack)
            {

                hero temp = heroesToAttack[0];

                foreach (hero champ in heroesToAttack)
                {
                    if (champ.getPv() - Squelettesdmg <= 0)
                    {
                        randomHero = temp;
                        m_valueText.text = Squelettesdmg.ToString();
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
                m_valueText.text = Squelettesdmg.ToString();
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
            if (SquelettessecondAttack >= randomAttack)
            {
                Debug.Log("armor");
                this.setArmor(Squelettesarmor);
            }
            else if (SquelettesfirtAttack >= randomAttack)
            {
                m_IsAttacking = false;
                Debug.Log("dmg");

                hero temp = heroesToAttack[0];

                foreach (hero champ in heroesToAttack)
                {
                    if (champ.getPv() - Squelettesdmg <= 0)
                    {
                        temp.takeDamage(Squelettesdmg);
                        return;
                    }
                }
                if(randomHero.getIsAlive() == true)
                    randomHero.takeDamage(Squelettesdmg);
                else
                {
                    heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)].takeDamage(Squelettesdmg);
                }
            }
        }


        
    }

    public void main(List<hero> heroesToAttack, bool fight)
    {
        if (this.isAlive == false)
            return;

        var tempColor = m_spriteTypeAttack.color;
        tempColor.a = 1f;
        m_spriteTypeAttack.color = tempColor;
        m_spriteFocus.color = tempColor;
        if (fight)
        {
            this.randomAttack = (int)Random.Range(0f, 100);
            if (MainfourthAttack >= randomAttack)
            {
                m_valueText.text = MainnbCardDebuf.ToString();
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[4];
                this.m_spriteFocus.sprite = null;
                tempColor.a = 0f;
                m_spriteFocus.color = tempColor;
                // nerf : ajoute des cartes injouables dans la pioche
            }
            else if (MainthridAttack >= randomAttack)
            {
                this.m_spriteFocus.sprite = null;
                tempColor.a = 0f;
                m_spriteFocus.color = tempColor;
                m_valueText.text = "1";
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[6];

            }
            else if (MainsecondAttack >= randomAttack)
            {
                m_valueText.text = Mainarmor.ToString();
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[1];
                this.m_spriteFocus.sprite = null;
                tempColor.a = 0f;
                m_spriteFocus.color = tempColor;

            }
            else if (MainfirtAttack >= randomAttack)
            {
                m_IsAttacking = true;
                hero temp = heroesToAttack[0];

                foreach (hero champ in heroesToAttack)
                {
                    if (champ.getPv() - Maindmg <= 0)
                    {
                        randomHero = temp;
                        m_valueText.text = Maindmg.ToString();
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
                m_valueText.text = Maindmg.ToString();
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
            if (MainfourthAttack >= randomAttack)
            {
                throw new System.NotImplementedException("attaque pas encore implémenté (ajoute des cartes injouables dans la pioche)");
                // nerf : ajoute des cartes injouables dans la pioche
            }
            else if (MainthridAttack >= randomAttack)
            {
                gameManager.debuffDraw++;

            }
            else if (MainsecondAttack >= randomAttack)
            {
                this.setArmor(Mainarmor);

            }
            else if (MainfirtAttack >= randomAttack)
            {
                Debug.Log("dmg");
                m_IsAttacking = false;
                hero temp = heroesToAttack[0];

                foreach (hero champ in heroesToAttack)
                {
                    if (champ.getPv() - Maindmg <= 0)
                    {
                        temp.takeDamage(Maindmg);
                        return;
                    }
                }
                if (randomHero.getIsAlive() == true)
                    randomHero.takeDamage(Maindmg);
                else
                {
                    heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)].takeDamage(Maindmg);
                }
            }

        }
        
    }

    public void gargouilleAttack(List<hero> heroesToAttack, bool fight)
    {
        if (this.isAlive == false)
            return;
        

        var tempColor = m_spriteTypeAttack.color;
        tempColor.a = 1f;
        m_spriteTypeAttack.color = tempColor;
        m_spriteFocus.color = tempColor;
        if (fight)
        {
            randomAttack = (int)Random.Range(0f, 100);
            if (GargouillefourthAttack >= randomAttack) //debuff next round
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
            else if (GargouillethridAttack >= randomAttack)
            {
                m_valueText.text = "";
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[7];
                this.m_spriteFocus.sprite = null;
                tempColor.a = 0f;
                m_spriteFocus.color = tempColor;
            }
            else if (GargouillesecondAttack >= randomAttack)
            {
                m_IsAttacking = true;
                Debug.Log("aoe");
                m_valueText.text = GargouilledmgAOE.ToString();
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[2];
                this.m_spriteFocus.sprite = null;
                tempColor.a = 0f;
                m_spriteFocus.color = tempColor;
            }
            else if (GargouillefirtAttack >= randomAttack)
            {
                hero temp = heroesToAttack[0];
                m_IsAttacking = true;
                foreach (hero champ in heroesToAttack)
                {
                    if (champ.getPv() - Gargouilledmg <= 0)
                    {
                        randomHero = temp;
                        m_valueText.text = Gargouilledmg.ToString();
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
                m_valueText.text = Gargouilledmg.ToString();
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
            if (GargouillefourthAttack >= randomAttack) //debuff next round
            {

                hero temp = heroesToAttack[0];

                foreach (hero champ in heroesToAttack)
                {
                    if (champ.getPv() < temp.getPv())
                        temp = champ;
                }

                temp.m_damageMultiplier = 2;
            }
            else if (GargouillethridAttack >= randomAttack)
            {
                this.isProvocation = true;
                gameManager.IsAnyProv = true;
            }
            else if (GargouillesecondAttack >= randomAttack)
            {
                this.isProvocation = false;
                foreach (hero champ in heroesToAttack)
                    champ.takeDamage(GargouilledmgAOE);
            }
            else if (GargouillefirtAttack >= randomAttack)
            {
                Debug.Log("dmg");
                m_IsAttacking = false;
                hero temp = heroesToAttack[0];

                foreach (hero champ in heroesToAttack)
                {
                    if (champ.getPv() - Gargouilledmg <= 0)
                    {
                        temp.takeDamage(Gargouilledmg);
                        return;
                    }
                }
                if (randomHero.getIsAlive() == true)
                    randomHero.takeDamage(Gargouilledmg);
                else
                {
                    heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)].takeDamage(Gargouilledmg);
                }
            }
        }


        
    }

    public void hommeVers(List<hero> heroesToAttack, bool fight)
    {
        if (this.isAlive == false)
            return;
        
        var tempColor = m_spriteTypeAttack.color;
        tempColor.a = 1f;
        m_spriteTypeAttack.color = tempColor;
        m_spriteFocus.color = tempColor;

        if (fight)
        {
            randomAttack = (int)Random.Range(0f, 100);
            if (HommeVersfourthAttack >= randomAttack)
            {
                m_IsAttacking = true;

                randomHero = heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)];
                m_valueText.text = HommeVersdmg.ToString();
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
            else if (HommeVersthridAttack >= randomAttack)
            {
                m_IsAttacking = true;
                randomHero = heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)];
                m_valueText.text = HommeVersdmgLourd.ToString();
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
            else if (HommeVerssecondAttack >= randomAttack)
            {
                //nerf et on ne vois plus la description des cartes
                throw new System.NotImplementedException("attaque pas encore implémenté (ne plus voir la description des cartes)");
            }
            else if (HommeVersfirtAttack >= randomAttack)
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
            if (HommeVersfourthAttack >= randomAttack)
            {
                heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)].takeDamage(HommeVersdmg);
                this.m_Pv += HommeVersheal;
            }
            else if (HommeVersthridAttack >= randomAttack)
            {
                heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)].takeDamage(HommeVersdmgLourd);
            }
            else if (HommeVerssecondAttack >= randomAttack)
            {
                //nerf et on ne vois plus la description des cartes
                throw new System.NotImplementedException("attaque pas encore implémenté (ne plus voir la description des cartes)");
            }
            else if (HommeVersfirtAttack >= randomAttack)
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

        var tempColor = m_spriteTypeAttack.color;
        tempColor.a = 1f;
        m_spriteTypeAttack.color = tempColor;
        m_spriteFocus.color = tempColor;

        if (fight)
        {
            randomAttack = (int)Random.Range(0f, 100);
            if (DemonthridAttack >= randomAttack)
            {
                int healing = this.m_Pv * 15 / 100;
                this.m_valueText.text = healing.ToString();
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[8];
            }
            else if (DemonsecondAttack >= randomAttack)
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
                m_valueText.text = Demondmg.ToString();
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
            else if (DemonfirtAttack >= randomAttack)
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
                m_valueText.text = Demondmg.ToString();
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
            if (DemonthridAttack >= randomAttack)
            {
                this.m_Pv = this.m_Pv * 15 / 100;
            }
            else if (DemonsecondAttack >= randomAttack)
            {
                hero temp = heroesToAttack[0];

                foreach (hero champ in heroesToAttack)
                {
                    if (champ.getPv() > temp.getPv())
                        temp = champ;
                }
                temp.takeDamage(Demondmg);
                m_IsAttacking = false;
            }
            else if (DemonfirtAttack >= randomAttack)
            {
                hero temp = heroesToAttack[0];

                foreach (hero champ in heroesToAttack)
                {
                    if (champ.getPv() < temp.getPv())
                        temp = champ;
                }
                temp.takeDamage(Demondmg);
                m_IsAttacking = false;
            }
        }

        
    }


    public void dragonAttack(List<hero> heroesToAttack, bool fight)
    {
        if (this.isAlive == false)
            return;
        
        var tempColor = m_spriteTypeAttack.color;
        tempColor.a = 1f;
        m_spriteTypeAttack.color = tempColor;
        m_spriteFocus.color = tempColor;

        if (fight)
        {
            randomAttack = (int)Random.Range(0f, 100);
            if (Dragonsixth >= randomAttack) //antiheal
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
            else if (DragonfithAttack >= randomAttack) //boost
            {
                Debug.Log("boostStat");
                m_valueText.text = "2";
                this.m_spriteFocus.sprite = null;
                tempColor.a = 0f;
                m_spriteFocus.color = tempColor;
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[3];
            }
            else if (DragonfourthAttack >= randomAttack) //armor
            {
                m_valueText.text = Dragonarmor.ToString();
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[1];
                this.m_spriteFocus.sprite = null;
                tempColor.a = 0f;
                m_spriteFocus.color = tempColor;
            }
            else if (DragonthridAttack >= randomAttack) //aoe
            {
                Debug.Log("aoe");
                m_IsAttacking = true;
                m_valueText.text = DragondmgAOE.ToString();
                this.m_spriteTypeAttack.sprite = gameManager.entityManager.m_spriteList[2];
                this.m_spriteFocus.sprite = null;
                tempColor.a = 0f;
                m_spriteFocus.color = tempColor;
            }
            else if (DragonsecondAttack >= randomAttack) //lourd
            {
                m_IsAttacking = true;
                randomHero = heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)];
                m_valueText.text = DragondmgLourd.ToString();
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
            else if (DragonfirtAttack >= randomAttack) //normal
            {
                m_IsAttacking = true;
                randomHero = heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)];
                m_valueText.text = Dragondmg.ToString();
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
            if (Dragonsixth >= randomAttack) //antiheal
            {
                if(randomHero.isAlive) 
                    randomHero.isAntiHeal = true;
                else
                    heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)].isAntiHeal = true;
            }
            else if (DragonfithAttack >= randomAttack) //boost
            {
                Dragondmg += 2;
                DragondmgAOE += 2;
                DragondmgLourd += 2;
            }
            else if (DragonfourthAttack >= randomAttack) //armor
            {
                this.setArmor(Dragonarmor);
            }
            else if (DragonthridAttack >= randomAttack) //aoe
            {
                foreach (hero hero in heroesToAttack)
                {
                    hero.takeDamage(DragondmgAOE);
                }
                m_IsAttacking = false;
            }
            else if (DragonsecondAttack >= randomAttack) //lourd
            {
                m_IsAttacking = false;
                if (randomHero.isAlive)
                    randomHero.takeDamage(DragondmgLourd);
                else
                    heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)].takeDamage(DragondmgLourd);
            }
            else if (DragonfirtAttack >= randomAttack) //normal
            {
                m_IsAttacking = false;
                if (randomHero.isAlive)
                    randomHero.takeDamage(Dragondmg);
                else
                heroesToAttack[(int)Random.Range(0f, heroesToAttack.Count)].takeDamage(Dragondmg);
            }
        }
        
  
    }

    #endregion

}

