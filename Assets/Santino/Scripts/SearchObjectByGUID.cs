using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;

[ExecuteInEditMode]

public class SearchObjectByGUID : MonoBehaviour
{

    public List<Object> obj;

    public string guid;

    [Button]
    void search()
    {
        Debug.Log(AssetDatabase.GUIDToAssetPath(guid));
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
