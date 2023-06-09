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
using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine.Rendering.Universal;
using static UnityEngine.Rendering.DebugUI;

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
                
               rendeureur.sortingOrder = 1000;
                canvas.sortingOrder = 1000;
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
                    gameManager.InspectUI.UI.SetActive(true);
                    gameManager.CardsInteractable = false;
                    if (m_dataCard.m_isUpsideDown)
                    {
                        
                        gameManager.InspectUI.Name.text = this.DataCard.BackCard.Name;
                        gameManager.InspectUI.description.text = this.DataCard.BackCard.Description;
                    }
                    else
                    {
                        
                        gameManager.InspectUI.Name.text = this.DataCard.Name;
                        gameManager.InspectUI.description.text = this.DataCard.Description;
                    }
                    
                    gameManager.InspectUI.Image.sprite = this.GetComponent<SpriteRenderer>().sprite;
                    gameManager.InspectUI.AutreUI.SetActive(false);
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

    public IEnumerator UpdateLife(hero hero)
    {
        float TempsTransition = 5f;
        float timeElapsed = 0;
        while (timeElapsed < TempsTransition)
        {
            hero.m_slider.value = Mathf.Lerp(hero.m_slider.value, hero.m_Pv, Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        hero.m_slider.value = hero.m_Pv;
    }
    #region CARD EFFECTS
    public void heal(hero hero)
    {

        print("health avant : " + hero.getPv() + "value" + this.DataCard.m_value);
        int currentPv = hero.getPv() + this.DataCard.m_value;
        int currentMaxPv = hero.getMaxPv();
        hero.setPv(currentPv);
        if (currentPv < currentMaxPv)
            hero.setPv(currentMaxPv);
        print("health après : " + hero.getPv());

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
        StartCoroutine(UpdateLife(hero));

    }

    public void heal(hero hero, int value)
    {

        hero.setPv(hero.getPv() + value);

        if (hero.getPv() > hero.getMaxPv())
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
        StartCoroutine(UpdateLife(hero));
    }

    public void takeDamage(hero hero, int value)
    {
        GameObject Placeholder = new GameObject();
        Placeholder.transform.position = Camera.main.ScreenToWorldPoint(hero.m_slider.transform.position);
        GameManager.Instance.FM.DamageNumber(Placeholder, value);
        Destroy(Placeholder);
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
        StartCoroutine(UpdateLife(hero));
        GameManager.Instance.FM.UpdateArmorValue(hero);
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
        GameManager.Instance.FM.UpdateArmorValue(hero);

        if (GameManager.Instance.isAbsolution)
        {
            foreach (hero enemy in GameManager.Instance.FM.enemiesAtStartOfCombat)
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
        GameManager.Instance.FM.UpdateArmorValue(hero);
        if (GameManager.Instance.isAbsolution)
        {
            foreach (hero enemy in GameManager.Instance.FM.enemiesAtStartOfCombat)
            {
                if (enemy.getIsAlive())
                {
                    takeDamage(enemy, value);
                }
            }

        }
        GameManager.Instance.FM.UpdateArmorValue(hero);
    }

    public void AddMana(int value)
    {
        GameManager.Instance.FM.mana += value;
    }

    public void AddCard()
    {
        StartCoroutine(GameManager.Instance.deck.DrawCardCoroutine());
    }

    public void AddCard(int value)
    {
        StartCoroutine(GameManager.Instance.deck.DrawCardCoroutine(value));
        GameManager.Instance.deck.rearangecardslots();
        GameManager.Instance.deck.RestoreCardPosition(false);
        GameManager.Instance.deck.ReorderZCards();
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
        GameManager.Instance.FM.stock += value;

        GameManager.Instance.FM.stockText.text = GameManager.Instance.FM.stock.ToString();
    }

    public int Steal(hero enemy, hero ally, int value, bool isBlas)
    {
        int surplusToReturn = 0;
        if(isBlas)
        {
            foreach (hero ennemy in GameManager.Instance.FM.enemiesAtStartOfCombat)
            {
                ennemy.setPv(ennemy.getPv() - value);
                if (ennemy.getPv() <= 0)
                {
                    ennemy.setIsAlive(false);
                }

                ally.setPv(ally.getPv() + value);
                if (ally.getPv() > ally.getMaxPv())
                {
                    surplusToReturn = ally.getPv() - ally.getMaxPv();
                    ally.setPv(ally.getMaxPv());
                }
            }
        }
        else
        {
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
        }
        
        
        UpdateLife(ally);
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

    public void CultiverAme(dataCard data,hero ally)
    {
        ally.setArmor(ally.getArmor() + 3);
        AddCard();
        Venerate(ally, 2);
    }

    public void CultiverFlamme()
    {
        foreach (hero enemy in GameManager.Instance.FM.enemiesAtStartOfCombat)
        {
            if (enemy.getIsAlive())
            {
                enemy.setPv(enemy.getPv() - 6);
            }
        }
        AddCard();
    }

    public void Conversion(hero hero)
    {
        for(int i = 0; i == hero.getArmor(); i++)
        {
            hero.setArmor(hero.getArmor() - 1);
            hero.setPv(hero.getPv() + 1);

            if (hero.getPv() == hero.getMaxPv())
                break;
        }
        GameManager.Instance.FM.UpdateArmorValue(hero);
    }

    public void Absolution()
    {
        GameManager.Instance.isAbsolution = true;
    }

    public void Benediction(hero ally, hero enemy)
    {
        if (ally.getVenerate() > 0)
        {
            takeDamage(enemy, ally.getVenerate());
            AddCard();
        }
    }

    public void Apotasie()
    {
        foreach (hero enemy in GameManager.Instance.FM.enemiesAtStartOfCombat)
        {
            if (enemy.getIsAlive())
            {
                takeDamage(enemy, 4);
            }
        }
        GameManager.Instance.FM.isApo = true;
    }

    public void Tabernacle(hero ally) //appeler cette fonction à chaque fois que le joueur prend des degats
    {
        AddArmor(ally, ally.m_dmgTaken);

    }

    public void Belial(hero pretre)
    {
        foreach (hero enemy in GameManager.Instance.FM.enemiesAtStartOfCombat)
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
        Steal(null, ally, 5, true);
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
        foreach (hero enemy in GameManager.Instance.FM.enemiesAtStartOfCombat)
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
        foreach (hero enemies in GameManager.Instance.FM.enemiesAtStartOfCombat)
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
        int surplus = Steal(enemy, ally, 7, false);
        takeDamage(enemy, surplus * 3);

    }
    public void MoxLion(hero ally) // marche
    {
        if (GameManager.Instance.FM.mana > 0)
        {
            AddArmor(ally, GameManager.Instance.FM.mana * 2);
            GameManager.Instance.manaMultiplier += GameManager.Instance.FM.mana;
            GameManager.Instance.FM.mana = 0;
            GameManager.Instance.FM.manaText.text = GameManager.Instance.FM.mana.ToString();
        }

    }

    public void MoxAraignee(hero ally, hero enemy)
    {
        if (GameManager.Instance.FM.mana > 0)
        {
            AddArmor(ally, GameManager.Instance.FM.mana * 2);
            takeDamage(enemy, GameManager.Instance.FM.mana * 2);
            GameManager.Instance.manaMultiplier += GameManager.Instance.FM.mana;
            GameManager.Instance.FM.mana *= 2;
            GameManager.Instance.FM.manaText.text = GameManager.Instance.FM.mana.ToString();


        }
    }

    public void MurDeRonces(hero hero)//ajouté le poison
    {
        dataCard.CardEffect card = new dataCard.CardEffect();
        card.nbTour = 2;
        card.effects = dataCard.CardType.Poison;
        card.values = 2;
        foreach (hero enemies in GameManager.Instance.FM.enemiesAtStartOfCombat)
        {
            if (enemies.getIsAlive() && enemies.m_IsAttacking)
            {
                enemies.MyEffects.Add(card);
            }
        }
        AddArmor(hero, 6);

    }

    public void LaissePourMort()//ajouté le poison
    {
        dataCard.CardEffect card = new dataCard.CardEffect();
        card.nbTour = 2;
        card.effects = dataCard.CardType.Poison;
        card.values = 6;
        foreach (hero enemies in GameManager.Instance.FM.enemiesAtStartOfCombat)
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
        GameManager.Instance.debuffDraw = 0;
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

    public void SurgissementVitalique()
    {
        int newMana = GameManager.Instance.FM.mana;

        foreach(hero hero in GameManager.Instance.FM.heroes)
        {
            if (hero.getIsAlive())
            {
                newMana += hero.m_mana;
                hero.m_mana = 0;
                hero.stockText.text = hero.m_mana.ToString();
                
            }
        }
        GameManager.Instance.FM.mana = newMana + GameManager.Instance.FM.stock;
        GameManager.Instance.FM.stock = 0;
        GameManager.Instance.FM.manaText.text = GameManager.Instance.FM.mana.ToString();
        GameManager.Instance.FM.stockText.text = GameManager.Instance.FM.stock.ToString();
    }

    public void RepandreMort()
    {
        dataCard.CardEffect card = new dataCard.CardEffect();
        card.nbTour = 2;
        card.effects = dataCard.CardType.Poison;
        card.values = 5;
        for (int i = 0; i < GameManager.Instance.FM.nbTransfo; i++)
        {
            foreach (hero enemies in GameManager.Instance.FM.enemiesAtStartOfCombat)
            {
                if (enemies.getIsAlive())
                {
                    enemies.addEffect(card);
                }
            }
        }



    }

    public void ArmureEcorse(hero ally) // voir si next armor est mis
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
        foreach (hero enemy in GameManager.Instance.FM.enemiesAtStartOfCombat)
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
        GameManager.Instance.manaMultiplier += 3;
    }

    public void Canibalisme() // voir si intégré
    {
        GameManager.Instance.FM.isCanibalisme = true;

    }
    public void SuivreEtoiles(hero ally)
    {
        Venerate(ally, 2);
        foreach (hero hero in GameManager.Instance.FM.heroes)
        {
            if (hero.getMana() == hero.m_manaMax)
            {
                AddCard();
                return;

            }
        }



    }
    public void ProfanerCiel() // voir si intégré
    {
        GameManager.Instance.FM.isProf = true;

    }
    public void DormirPresDeLautre(hero ally, hero arboriste)
    {
        Venerate(arboriste, 4);
        heal(ally, 4);
        AddArmor(ally, 4);

    }
    public void ReveillerPourManger(hero enemy, hero ally) //uniquement arbo
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


}
