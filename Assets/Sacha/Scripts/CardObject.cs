using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using NaughtyAttributes;
//using Unity.VisualScripting.Dependencies.Sqlite;
using DG.Tweening.Core.Easing;
using Unity.VisualScripting;
using TMPro;
using System;

[ExecuteInEditMode]
public class CardObject : MonoBehaviour
{
    [Header("DATACARD")]
    [SerializeField]private dataCard m_dataCard;
    public dataCard DataCard { get => m_dataCard;}

    [Header("             Statistics")]
    [SerializeField] float RatioGrowHoverCard;
    public GameManager gameManager;
    public Vector3 PosBeforeDrag;
    public Transform Slot;
    public Vector2 BaseColliderDimensions;
    public float TempsClick;
    public Renderer rendeureur;
    public int indexHand;
    public Canvas canvas;
    public TMP_Text Description;
    public TMP_Text Name;
    public TMP_Text Mana;




    public List<hero> heroToAttack; //always Start Null
    public bool stayInHand = false;

    Transform M_t;

    //public bool MenuCarde = false;

    public void RemettreCardSlot()
    {
        Transform trans = gameManager.deck.GetTransformSlotFromCard(this);
        transform.position = trans.position;
        transform.rotation = trans.rotation;
    }

    void Awake()
    {
        Mana.text = m_dataCard.m_manaCost.ToString();
        Mana.color = Color.black;
        Mana.color = new Color(Mana.color.r, Mana.color.g, Mana.color.b, 0.75f);
        M_t = this.transform;
        gameManager = GameManager.Instance;
        TempsClick = gameManager.TempsPourClickCardInspect;
        BaseColliderDimensions = this.GetComponent<BoxCollider2D>().size;
        rendeureur = GetComponent<Renderer>();
        canvas = transform.GetChild(0).GetComponent<Canvas>();
        Description = canvas.transform.GetChild(0).GetComponent<TMP_Text>();
        Description.text = DataCard.Description;
        Name = canvas.transform.GetChild(1).GetComponent<TMP_Text>();
        Name.text = DataCard.Name;
    }
    void OnMouseDown()
    {
        if (gameManager.CardsInteractable  && !gameManager.HasCardInHand)
        {
            PosBeforeDrag = transform.position;
            TempsClick = Time.time;
        }
    }
/*    void OnMouseUpAsButton()
    {
    }*/
    void OnMouseOver()
    {
        if (gameManager.CardsInteractable  && !gameManager.HasCardInHand)
        {
            if (Input.GetMouseButton(0))
            {

                transform.localScale = new Vector3(RatioGrowHoverCard, RatioGrowHoverCard, RatioGrowHoverCard);
                transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0,0);
                Transform trans = gameManager.deck.GetTransformSlotFromCard(this);
                transform.position = new Vector3(trans.position.x, trans.position.y + 1, trans.position.z);
                this.GetComponent<BoxCollider2D>().size = BaseColliderDimensions;
                gameManager.deck.ReorderZCards();
                
               rendeureur.sortingOrder = 10;
                canvas.sortingOrder = 10;
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
                RemettreCardSlot();
                gameManager.deck.ReorderZCards();
            }
        }
        
    }
    void OnMouseDrag()
    {
        if (gameManager.CardsInteractable)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            print(this.isActiveAndEnabled);
            gameManager.HasCardInHand = true;
            
        }
    }
    void OnMouseExit()
    {
        if (gameManager.CardsInteractable  && !gameManager.HasCardInHand)
        {
            transform.localScale = new Vector3(1, 1, 1);
            RemettreCardSlot();
            gameManager.deck.ReorderZCards();

        }

    }

    void SelectedCard(bool Side1, bool Side2)
    {
        gameManager.CardsInteractable  = false;

        gameManager.deck.DeplaceCardUtiliseToPlace();
        transform.localScale = new Vector3(2,2, 2);
        rendeureur.sortingOrder = 10;
        canvas.sortingOrder = 10;
        HideHandExceptThis();
    }

    [Button]
    void OnMouseUp()
    {
        if (gameManager.CardsInteractable )
        {
            transform.localScale = new Vector3(1, 1, 1);
            gameManager.deck.ReorderZCards();

            
            if (transform.position.y >= gameManager.RangePourActiverCarte && gameManager.FM.mana >= DataCard.m_manaCost)   
            {
                print(DataCard.m_manaCost);
                gameManager.CarteUtilisee = this;
                gameManager.FM.Cardsend(this, indexHand);

                Slot = this.gameObject.transform;
                FindObjectOfType<Deck>().CancelButton.gameObject.SetActive(true);
                FindObjectOfType<Deck>().PlayButton.gameObject.SetActive(true);
                SelectedCard(DataCard.TargetAllies, DataCard.TargetEnnemies);

                
            }
            else 
            {


                if (Time.time - TempsClick < gameManager.TempsPourClickCardInspect && transform.position.y < gameManager.RangePourActiverCarte)
                {
                    gameManager.CardsInteractable = false;
                    //print(gameManager.InspectUI.Image.sprite);
                    print(this.GetComponent<SpriteRenderer>().sprite);
                    gameManager.InspectUI.Image.sprite = this.GetComponent<SpriteRenderer>().sprite;
                    gameManager.InspectUI.UI.SetActive(true);
                    gameManager.InspectUI.Name.text = this.DataCard.Name;
                    gameManager.InspectUI.description.text = this.DataCard.Description;
                    RemettreCardSlot();

                }
                transform.position = PosBeforeDrag;
                transform.position = M_t.position;
                transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

            }
        }
        gameManager.HasCardInHand = false;

    }

    void HideHandExceptThis()
    {
        print("hey");
        foreach(CardObject Carte in gameManager.Hand)
        {
            Carte.gameObject.SetActive(false);
        }
        this.gameObject.SetActive(true);
    }
    void ShowHand()
    {
        foreach (CardObject Carte in gameManager.Hand)
        {
            Carte.gameObject.SetActive(true);
        }
    }
    Sprite SetSprite()
    {
        SpriteRenderer Sr = GetComponent<SpriteRenderer>();
        Sr.sprite = m_dataCard.CardSprite;
        return Sr.sprite;
    }

    [SerializeField][Button]
    private void OnValidate()
    {
       //SetSprite();
    }

    private void Update()
    {
        //print(gameManager.HasCardInHand);
    }

    IEnumerator TransposeAtoB(GameObject objetABouger, Vector3 position)
    {
        for (int i =0; i <100; i++)
        {
            gameManager.deck.TransposeAtoB(objetABouger, position);
            yield return null;
        }
        
    }

    public bool getIsDeleteOnTurn()
    {
        return this.DataCard.m_isDeleteOnTurn;
    }
    public void setIsDeletOnTurn()
    {
        this.DataCard.m_isDeleteOnTurn = true;
    }

    #region CARD EFFECTS
    public void heal(hero hero)
    {
        int currentPv = hero.getPv() + this.DataCard.m_value;
        int currentMaxPv = hero.getMaxPv();
        hero.setPv(currentPv);
        if (currentPv < currentMaxPv)
            hero.setPv(currentMaxPv);
        if (gameManager.isAbsolution)
        {
            foreach (hero enemy in gameManager.FM.enemiesAtStartOfCombat)
            {
                if (enemy.getIsAlive())
                {
                    takeDamage(enemy, this.DataCard.m_value);
                }
            }

        }

    }

    public void heal(hero hero, int value)
    {
        hero.setPv(hero.getPv() + value);
        if (hero.getPv() < hero.getMaxPv())
            hero.setPv(hero.getMaxPv());
        if (gameManager.isAbsolution)
        {
            foreach (hero enemy in gameManager.FM.enemiesAtStartOfCombat)
            {
                if (enemy.getIsAlive())
                {
                    takeDamage(enemy, value);
                }
            }

        }
    }

    public void takeDamage(hero hero)
    {
        if (hero.m_isDebufArmor)
        {
            hero.m_armor /= 2;
            hero.m_isDebufArmor = false;
        }
        this.DataCard.m_value -= hero.m_armor;
        if (this.DataCard.m_value >= 0)
            hero.m_armor = 0;
        else
            this.DataCard.m_value = 0;

        hero.m_Pv -= this.DataCard.m_value * hero.m_damageMultiplier;
        hero.m_slider.value = hero.m_Pv;
        Debug.Log("Pv apres: " + hero.m_Pv + " " + hero.m_role);
        if (hero.m_Pv <= 0)
        {
            hero.isAlive = false;
        }

        hero.setVarHero();
    }

    public void takeDamage(hero hero, int value)
    {
            if (hero.m_isDebufArmor)
            {
                hero.m_armor /= 2;
                hero.m_isDebufArmor = false;
            }
            value -= hero.m_armor;
            if (value >= 0)
                hero.m_armor = 0;
            else
                value = 0;

            hero.m_Pv -= value * hero.m_damageMultiplier;
            hero.m_slider.value = hero.m_Pv;
            Debug.Log("Pv apres: " + hero.m_Pv + " " + hero.m_role);
            if (hero.m_Pv <= 0)
            {
                if (GameManager.Instance.FM.isCanibalisme)
                {
                    Venerate(hero, 3);
                    heal(hero, 3);
                }
                if (GameManager.Instance.FM.isProf)
                {
                    foreach (hero champ in gameManager.FM.heroes)
                    {
                        champ.m_manaMax = 1;
                        if (champ.m_mana > 1)
                        {
                            champ.m_mana = 1;
                            champ.stockText.text = champ.m_mana.ToString() + " / " + champ.m_manaMax;
                        }
                    }
                }
                hero.isAlive = false;
            }

            hero.setVarHero();
    }

    public void AddArmor(hero hero)
    {
        hero.setArmor(this.DataCard.m_value);
        if (gameManager.isAbsolution)
        {
            foreach (hero enemy in gameManager.FM.enemiesAtStartOfCombat)
            {
                if (enemy.getIsAlive())
                {
                    takeDamage(enemy, this.DataCard.m_value);
                }
            }

        }
    }
    public void AddArmor(hero hero, int value)
    {
        hero.setArmor(value);
        if (gameManager.isAbsolution)
        {
            foreach (hero enemy in gameManager.FM.enemiesAtStartOfCombat)
            {
                if (enemy.getIsAlive())
                {
                    takeDamage(enemy, value);
                }
            }

        }
    }

    public void AddMana(int value)
    {
        gameManager.FM.mana += value;
    }

    public void AddCard()
    {
        gameManager.deck.DrawCard();
    }

    public void AddCard(int value)
    {
        gameManager.deck.DrawCard(value);
    }

    public void KeepCardInHand(CardObject cardToKeep)
    {
        cardToKeep.stayInHand = true;
    }

    public void ChangeCardMana(CardObject card)
    {
        card.DataCard.m_manaCost += this.DataCard.m_value;
    }

    public void ChangeCardDamage(CardObject card)
    {
        card.DataCard.m_value += this.DataCard.m_value;
    }

    public void Venerate(hero hero, int value)
    {
        gameManager.FM.stock += value;

        gameManager.FM.stockText.text = gameManager.FM.stock.ToString();
    }

    public int Steal(hero enemy, hero ally, int value)
    {
        int surplusToReturn = 0;
        enemy.setPv(enemy.getPv() - value);
        if (enemy.getPv() <= 0)
        {
            enemy.setIsAlive(false);
        }

        ally.setPv(ally.getPv() + value);
        if (ally.getPv() > ally.getMaxPv())
        {
            surplusToReturn = ally.getPv() - ally.getMaxPv();
            ally.setPv(ally.getMaxPv());
        }
        return surplusToReturn;
    }

    public void HabemusDominum(hero enemy)
    {
        //detecter l'attaque d'un enemy
        takeDamage(enemy, 6);
    }

    public void DiabolusEst(hero enemy)
    {
        //detecter l'attaque d'un enemy

        takeDamage(enemy, 12);
    }

    public void CultiverAme(dataCard data)
    {
        throw new NotImplementedException();
    }

    public void CultiverFlamme()
    {
        throw new NotImplementedException();
    }

    public void Conversion(hero hero)
    {
        while (hero.getArmor() > 0)
        {
            if (hero.getPv() == hero.getMaxPv())
                break;
            hero.setArmor(hero.getArmor() - 1);
            hero.setPv(hero.getPv() + 1);
        }
    }

    public void Absolution()
    {
        gameManager.isAbsolution = true;
    }

    public void Benediction(hero ally, hero enemy)
    {
        if (ally.getVenerate() > 0)
        {
            enemy.takeDamage(ally.getVenerate());
            AddCard();
        }
    }

    public void Apotasie()
    {
        foreach (hero enemy in gameManager.FM.enemiesAtStartOfCombat)
        {
            if (enemy.getIsAlive())
            {
                takeDamage(enemy, 4);
            }
        }
        gameManager.FM.isApo = true;
    }

    public void Tabernacle(hero ally) //appeler cette fonction à chaque fois que le joueur prend des degats
    {
        AddArmor(ally, ally.m_dmgTaken);

    }

    public void Belial(hero pretre)
    {
        foreach (hero enemy in gameManager.FM.enemiesAtStartOfCombat)
        {
            if (enemy.getIsAlive())
            {
                enemy.setPv(enemy.getPv() - 6);
                if (enemy.getPv() <= 0)
                {
                    enemy.setIsAlive(false);
                    heal(pretre, 3);
                    Venerate(pretre, 4);
                }
            }
        }
    }
    public void VenererIdole(hero ally)
    {
        heal(ally, 3);
        Venerate(ally, 2);
    }
    public void Blaspheme(hero ally)
    {
        foreach (hero enemy in gameManager.FM.enemiesAtStartOfCombat)
        {
            if (enemy.getIsAlive())
            {
                Steal(enemy, ally, 5);
            }
        }

    }
    public void AllumerCierges(hero pretre)
    {
        if (pretre.getVenerate() > 0)
        {
            AddCard(pretre.getVenerate());
        }

    }
    public void IncendierCloatre()
    {
        hero enemyWithMostPV = null;
        foreach (hero enemy in gameManager.FM.enemiesAtStartOfCombat)
        {
            if (enemy.getIsAlive())
            {
                if ((enemyWithMostPV == null) || (enemy.getPv() > enemyWithMostPV.getPv()))
                {
                    enemyWithMostPV = enemy;
                }
            }

        }
        takeDamage(enemyWithMostPV, 15);
    }
    public void AccueillirNecessiteux(hero hero)
    {
        int nb = 0;
        foreach (hero enemies in gameManager.FM.enemiesAtStartOfCombat)
        {
            if (enemies.getIsAlive() && enemies.m_IsAttacking)
            {
                nb++;
            }
        }
        AddArmor(hero, nb);
    }
    public void MassacrerInfideles(hero ally, hero enemy)
    {
        int surplus = Steal(enemy, ally, 7);
        takeDamage(enemy, surplus * 3);

    }
    public void MoxLion(hero ally)
    {
        if (gameManager.FM.mana > 0)
        {
            AddArmor(ally, gameManager.FM.mana * 2);
            gameManager.FM.mana = 0;
            gameManager.isManaMultiplier = true;
        }

    }

    public void MoxAraignee(hero ally, hero enemy)
    {
        if (gameManager.FM.mana > 0)
        {
            AddArmor(ally, gameManager.FM.mana * 2);
            takeDamage(enemy, gameManager.FM.mana * 2);
            gameManager.FM.mana = 0;
            gameManager.isManaMultiplier = true;

        }
    }

    public void MurDeRonces(hero hero)//ajouté le poison
    {
        int nb = 0;
        dataCard.CardEffect card = new dataCard.CardEffect();
        card.nbTour = 2;
        card.effects = dataCard.CardType.Poison;
        card.values = 2;
        foreach (hero enemies in GameObject.FindObjectOfType<GameManager>().FM.enemiesAtStartOfCombat)
        {
            if (enemies.getIsAlive() && enemies.m_IsAttacking)
            {
                nb++;
                enemies.MyEffects.Add(card);
            }
        }
        AddArmor(hero, nb * 6);

    }

    public void LaissePourMort()//ajouté le poison
    {
        dataCard.CardEffect card = new dataCard.CardEffect();
        card.nbTour = 2;
        card.effects = dataCard.CardType.Poison;
        card.values = 6;
        foreach (hero enemies in gameManager.FM.enemiesAtStartOfCombat)
        {
            if (enemies.getIsAlive() && !enemies.m_IsAttacking)
            {
                enemies.addEffect(card);

            }
        }

    }

    public void Cataplasme(hero hero)
    {
        heal(hero, 2);
        gameManager.debuffDraw = 0;
        hero.m_isDebufArmor = false;
        hero.m_damageMultiplier = 1;
        hero.isAntiHeal = false;
    }

    public void Belladone(hero enemy)
    {
        if (enemy.getArmor() > 0)
        {
            enemy.setArmor(0);
        }
        takeDamage(enemy, 8);

    }

    public void SurgissementVitalique(hero arboriste)
    {
        if (arboriste.getVenerate() > 0)
        {
            AddMana(arboriste.getVenerate());
            arboriste.setVenerate(0);
            AddCard();
        }
    }

    public void RepandreMort()
    {
        dataCard.CardEffect card = new dataCard.CardEffect();
        card.nbTour = 2;
        card.effects = dataCard.CardType.Poison;
        card.values = 5;
        for (int i = 0; i < gameManager.FM.nbTransfo; i++)
        {
            foreach (hero enemies in gameManager.FM.enemiesAtStartOfCombat)
            {
                if (enemies.getIsAlive())
                {
                    enemies.addEffect(card);
                }
            }
        }



    }

    public void ArmureEcorse(hero ally)
    {
        AddArmor(ally, 7);
        ally.m_nextArmor = 7;
    }

    public void MaleusHerbeticae(hero ally)//poison
    {
        dataCard.CardEffect card = new dataCard.CardEffect();
        card.nbTour = 2;
        card.effects = dataCard.CardType.Poison;
        card.values = 4;
        foreach (hero enemy in gameManager.FM.enemiesAtStartOfCombat)
        {
            if (enemy.getIsAlive())
            {
                enemy.MyEffects.Add(card);
            }
        }
        AddArmor(ally, 7);
    }

    public void CommunionNature(hero hero)
    {
        Venerate(hero, 3);

    }

    public void Canibalisme()
    {
        gameManager.FM.isCanibalisme = true;

    }
    public void SuivreEtoiles(hero ally)
    {
        Venerate(ally, 2);
        foreach (hero hero in gameManager.FM.heroes)
        {
            if (hero.getMana() == hero.m_manaMax)
            {
                AddCard(2);
                return;

            }
        }



    }
    public void ProfanerCiel()
    {
        gameManager.FM.isProf = true;

    }
    public void DormirPresDeLautre(hero ally, hero arboriste)
    {
        Venerate(arboriste, 4);
        heal(ally, 4);
        AddArmor(ally, 4);

    }
    public void ReveillerPourManger(hero enemy, hero ally)
    {
        enemy.setPv(enemy.getPv() - 6);
        if (enemy.getPv() <= 0)
        {
            enemy.setIsAlive(false);
            heal(ally, 6);
            Venerate(ally, 6);
        }
    }

    public static void DamageEffect(hero h, int value)
    {
        h.setPv(h.getPv() - value);
    }

    public static void HealEffect(hero h, int value)
    {
        h.setPv(h.getPv() + value);
    }

    #endregion

    public void draw(hero hero, int amout)
    {
        hero.GetDeck();
    }
}
