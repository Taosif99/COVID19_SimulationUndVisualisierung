using System;
using UnityEngine;
using EditorObjects;


/// <summary>
/// Struct to see prefabs with Name in the Inspector
/// </summary>
[Serializable]
public struct NamedPrefab
{
    public PrefabName prefabName;
    public GameObject prefab;
}

