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
using static UnityEngine.EventSystems.EventTrigger;

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
        DissolveController dissolveController = GetComponent<DissolveController>();
        if(m_dataCard.Deck == null)
        {
            dissolveController.outColor = new Color(35, 35, 35);
        }
        else if(m_dataCard.Deck.Role == dataDeck.DeckRole.Pretre)
        {
            if (m_dataCard.m_isUpsideDown)
            {
                dissolveController.outColor = new Color(95, 25, 10);
            }
            else
            {
                dissolveController.outColor = new Color(199, 148, 21);
            }
        }else if(m_dataCard.Deck.Role == dataDeck.DeckRole.Arboriste)
        {
            if (m_dataCard.m_isUpsideDown)
            {
                dissolveController.outColor = new Color(38, 128, 126);
            }
            else
            {
                dissolveController.outColor = new Color(149, 206, 146);
            }
        }

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
        if (gameManager.CardsInteractable)
        {
            transform.localScale = new Vector3(1, 1, 1);
            gameManager.deck.ReorderZCards();
            int costMana = 0;

            if(this.DataCard.m_isUpsideDown)
            {
                costMana = this.DataCard.BackCard.m_manaCost;
            }
            else
            {
                costMana = this.DataCard.m_manaCost;
            }
            
            if (transform.position.y >= gameManager.RangePourActiverCarte && gameManager.FM.mana >= costMana)   
            {
                print(costMana);
                gameManager.CarteUtilisee = this;
                gameManager.FM.Cardsend(this, indexHand); 
                Slot = this.gameObject.transform;
                FindObjectOfType<Deck>().CancelButton.gameObject.SetActive(true);
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
        if (hero != null)
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
        hero.m_slider.value = hero.m_Pv;
        hero.pvText.text = hero.getPv().ToString() + " / " + hero.getMaxPv().ToString();
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
        print("health apr�s : " + hero.getPv());

        if (GameManager.Instance.isAbsolution)
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
        StartCoroutine(GameManager.Instance.FM.UpdateLife(hero));

    }

    public void heal(hero hero, int value)
    {
        print("health avant : " + hero.getPv() + "value" + value);
        hero.setPv(hero.getPv() + value);

        if (hero.getPv() > hero.getMaxPv())
            hero.setPv(hero.getMaxPv());

        print("health apr�s : " + hero.getPv());
        if (GameManager.Instance.isAbsolution)
        {
            foreach (hero enemy in gameManager.FM.enemiesAtStartOfCombat)
            {
                if (enemy.getIsAlive())
                {
                    takeDamage(enemy, value);
                }
            }
        }
        if (hero != null)
        {
            StartCoroutine(UpdateLife(hero));
        StartCoroutine(GameManager.Instance.FM.UpdateLife(hero));
        }
        else
        {
            print("Hero is null, can't update life sadly");
        }
    }

    public void takeDamage(hero hero, int value)
    {
        print("dmg : " + value);
        print("pv avant : " + hero.getPv());
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
        print("pv apr�s : " + hero.getPv());
        hero.pvText.text = hero.getPv().ToString() + " / " + hero.getMaxPv().ToString();
        StartCoroutine(UpdateLife(hero));
        StartCoroutine(GameManager.Instance.FM.UpdateLife(hero));

        GameManager.Instance.FM.UpdateArmorValue(hero);
        if (hero.m_Pv <= 0)
        {
            if (GameManager.Instance.FM.isCanibalisme)
            {
                Venerate(3);
                if (GameManager.Instance.FM.perso2)
                    heal(GameManager.Instance.FM.heroes[1], 3);
                else
                    heal(GameManager.Instance.FM.heroes[0], 3);
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
        print("add Armor");

        hero.setArmor(value);
        GameManager.Instance.FM.UpdateArmorValue(hero);
        if (GameManager.Instance.isAbsolution)
        {
            foreach (hero enemy in GameManager.Instance.FM.enemiesAtStartOfCombat)
            {
                print("absolution");
                if (enemy.getIsAlive())
                {
                    print("dmg absolution");
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
        print(gameManager);
        print(GameManager.Instance);
        GameManager.Instance.deck.DrawCardTest(1);
    }

    public void AddCard(int value)
    {
        print(gameManager);
        print(GameManager.Instance);
        GameManager.Instance.deck.DrawCardTest(value);

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

    public void Venerate(int value)
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
                takeDamage(ennemy, value);
                StartCoroutine(UpdateLife(ennemy));
                if (ennemy.getPv() <= 0)
                {
                    ennemy.setIsAlive(false);
                }

                int pvHealed = (ally.getPv() + value);
                if (pvHealed > ally.getMaxPv())
                {
                    surplusToReturn = pvHealed - ally.getMaxPv();
                }
                heal(ally, value);

            }
        }
        else
        {
            takeDamage(enemy, value);
            StartCoroutine(UpdateLife(enemy));
            if (enemy.getPv() <= 0)
            {
                enemy.setIsAlive(false);
            }

            
            int pvHealed = (ally.getPv() + value);
            if (pvHealed > ally.getMaxPv())
            {
                surplusToReturn = pvHealed - ally.getMaxPv();
            }
            heal(ally, value);
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

    public void CultiverAme(hero ally)
    {
        AddArmor(ally, 3);
        Venerate(2);
        AddCard();
    }

    public void CultiverFlamme()
    {
        foreach (hero enemy in GameManager.Instance.FM.enemiesAtStartOfCombat)
        {
            if (enemy.getIsAlive())
            {
                takeDamage(enemy, 6);
            }
        }
        AddCard();
    }

    public void Conversion(hero hero)
    {
        while(hero.getArmor() > 0)
        {
            hero.setArmor(-1);
            hero.setPv(hero.getPv() + 1);

            if (hero.getPv() == hero.getMaxPv())
                break;
        }
        print(hero.getPv());
        GameManager.Instance.FM.UpdateArmorValue(hero);
        StartCoroutine(GameManager.Instance.FM.UpdateLife(hero));
    }

    public void Absolution()
    {
        GameManager.Instance.isAbsolution = true;
    }

    public void Benediction(hero enemy) // par ma main je te b�ni 
    {
        if (GameManager.Instance.FM.heroes[0].m_mana > 0)
        {
            takeDamage(enemy, GameManager.Instance.FM.heroes[0].m_mana);
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

    public void Tabernacle(hero ally) //appeler cette fonction � chaque fois que le joueur prend des degats
    {
        if (ally.m_tabernacleActive)
        {
            ally.m_nextArmor += ally.m_dmgTaken;
            ally.m_tabernacleActive = false;
        }
        else
        {
            ally.m_tabernacleActive = true;
        }

        /*AddArmor(ally, ally.m_dmgTaken);*/

    }

    public void Belial(hero pretre)
    {
        foreach (hero enemy in GameManager.Instance.FM.enemiesAtStartOfCombat)
        {
            if (enemy.getIsAlive())
            {
                takeDamage(enemy, 6);

                if (enemy.getPv() <= 0)
                {
                    enemy.setIsAlive(false);
                    heal(pretre, 3);
                    Venerate(4);
                }
            }
        }
    }
    public void VenererIdole(hero ally)
    {
        heal(ally, 3);
        Venerate(2);
    }
    public void Blaspheme(hero ally)
    {
        Steal(null, ally, 5, true);
    }
    public void AllumerCierges(hero pretre)
    {
        if (pretre.m_mana > 0)
        {
            for(int i = 0; i < pretre.m_mana; i++)
            {
                AddCard();
            }   
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

    public void MurDeRonces(hero hero)//ajout� le poison
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
                gameManager.FM.UpdatePoisonValue(enemies);
            }
        }
        AddArmor(hero, 6);

    }

    public void LaissePourMort()//ajout� le poison
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
                gameManager.FM.UpdatePoisonValue(enemies);

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
            enemy.m_armor = 0;
            GameManager.Instance.FM.UpdateArmorValue(enemy);
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
        AddCard();
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
        card.values = 5 * GameManager.Instance.FM.nbTransfo;
        
        foreach (hero enemies in GameManager.Instance.FM.enemiesAtStartOfCombat)
        {
            if (enemies.getIsAlive())
            {
                if (enemies.getIsAlive())
                {
                    enemies.addEffect(card);
                    gameManager.FM.UpdatePoisonValue(enemies);
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
                gameManager.FM.UpdatePoisonValue(enemy);
            }
        }
        
        AddArmor(ally, 7);
    }
    public void CommunionNature() 
    {
        GameManager.Instance.FM.venerations += 2;
        GameManager.Instance.manaMultiplier += 3;
    }
    public void Canibalisme() // voir si int�gr�
    {
        GameManager.Instance.FM.isCanibalisme = true;

    }
    public void SuivreEtoiles()
    {
        Venerate(2);
        foreach (hero hero in GameManager.Instance.FM.heroes)
        {
            if (hero.getMana() == hero.m_manaMax)
            {
                AddCard(2);
                return;
            }
        }
    }
    public void ProfanerCiel() // voir si int�gr�
    {
        GameManager.Instance.FM.isProf = true;
        foreach(hero hero in GameManager.Instance.FM.heroes)
        {
            hero.m_manaMax = 1;
            if(hero.m_mana > 1)
                hero.m_mana = 1;
            hero.stockText.text = hero.m_mana + " / " + hero.m_manaMax;
            
        }
    }
    public void DormirPresDeLautre(hero ally)
    {
        Venerate(4);
        heal(ally, 4);
        AddArmor(ally, 4);

    }
    public void ReveillerPourManger(hero ally, hero enemy) //uniquement arbo
    {
        takeDamage(enemy, 6);
        if (enemy.getPv() <= 0)
        {
            enemy.setIsAlive(false);
            heal(ally, 6);
            Venerate(6);
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
