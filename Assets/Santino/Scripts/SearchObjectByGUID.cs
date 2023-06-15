using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;

[ExecuteInEditMode]

public class SearchObjectByGUID : MonoBehaviour
{
    public string guid;

    [Button]
    void search()
    {
        Debug.Log(AssetDatabase.GUIDToAssetPath(guid));
    }
}
