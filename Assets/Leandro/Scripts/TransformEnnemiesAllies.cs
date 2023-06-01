using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformEnnemiesAllies : MonoBehaviour
{
    [SerializeField] Transform SpawnMobs;
    [SerializeField] Transform SpawnAllies;
    [SerializeField] int NumberOfMobs;
    [SerializeField] int NumberOfAllies;
    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent red cube at the transforms position
        Gizmos.DrawCube(transform.position, new Vector3(5, 5, 5));
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        for (int i = 1; i < NumberOfAllies; i++)
        { 
        }

        Gizmos.color = new Color(1, 0, 0, 0.5f);
        for (int i = 1; i < NumberOfMobs; i++)
        { 
        }
    }
}
