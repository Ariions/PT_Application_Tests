using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//holds references to prefabs for standalone tests due to AssetDatabase.LoadAssetAtPath being an editor only function 
public class PrefabPaths : MonoBehaviour
{
    //Put all of the prefab assets in this list because loading them for tests is a huge pain, just search in Project Window for "t:prefab" and drag them all to this component
    public List<GameObject> allPrefabs;
}
