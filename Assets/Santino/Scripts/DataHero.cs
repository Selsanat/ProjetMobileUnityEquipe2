using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DataHero : ScriptableObject
{
    public entityManager.Role m_role;
    public dataDeck deck;
    
    public Sprite m_sprite;



    hero hero;


    public hero SetEnemy()
    {
        return hero = new hero(m_role, 20, 20, 0, 0, null, 0);
    }


}
