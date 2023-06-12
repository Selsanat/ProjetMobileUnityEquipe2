using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class entityManager : MonoBehaviour
{

    public enum Role 
    {
        Enemy,
        Mage,
        Pretre,
        Arboriste,
        Debuffer,
        ChienEnemy,
        Squellettes,
        Mains,
        Gargouilles,
        HommesVers,
        Demon,
        Dragon

/*      Paladin,
        Demoniste,
        Guerrier,
        Tank
*/
    }

    [SerializeField] public Role m_role;
    [SerializeField] public int m_maxPv;
    [SerializeField] public int m_Pv;
    [SerializeField] public int m_attack;
    [SerializeField] public int m_speed;
    [SerializeField] public int m_buff;
    [SerializeField] public int m_nerf;
    [SerializeField] public int m_armor;
    [SerializeField] public int m_experience;
    [SerializeField] public int m_level;
    [SerializeField] public bool m_IsAttacking = false;
    [SerializeField] public int m_experienceMax = 4;
    [SerializeField] public bool m_isDebufArmor = false;
    [SerializeField] public bool m_tabernacleActive = false;
    [SerializeField] public bool isAlive = true;
    [SerializeField] public bool isAntiHeal = false;
    [SerializeField] public bool isProvocation = false;
    [SerializeField] public int m_damageMultiplier = 1;
    [SerializeField] public int m_mana;
    [SerializeField] public int m_nextArmor = 0;
    [SerializeField] public int m_manaMax;
    [SerializeField] public int m_dmgTaken = 0;
    [SerializeField] public bool isFull;
    [SerializeField] public int randomAttack;
    [SerializeField] public hero randomHero;
    [SerializeField] protected int m_venerate;
    [SerializeField] public Sprite m_sprite;
    [SerializeField] public Image m_spriteTypeAttack;
    [SerializeField] public Image m_spriteFocus;
    [SerializeField] public List<Sprite> m_spriteList;
    [SerializeField] public TextMeshProUGUI m_valueText;
    [SerializeField] public Slider m_slider;
    [SerializeField] public Transform m_buffs;
    [SerializeField] public TextMeshProUGUI stockText;
    [SerializeField] public Image Armor;
    [SerializeField] public TextMeshProUGUI ArmorText;
    [SerializeField] protected Deck m_deck;
    [SerializeField] public List<hero> heroList;
    public int m_total_poison;
    [SerializeField] protected bool multipleTarget;
    [SerializeField] protected GameManager gameManager;
    [SerializeField] public List<dataCard.CardEffect> MyEffects = new List<dataCard.CardEffect>();

    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    public List<hero> getListHero() { return heroList; }






}

