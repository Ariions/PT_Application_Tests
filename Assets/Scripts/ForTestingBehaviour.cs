using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForTestingBehaviour : MonoBehaviour
{
    public GameObject Prefab_TerrainBase;
    public GameObject Prefab_TerrainTree;
    public GameObject Prefab_TerrainHoles;
    public GameObject Prefab_TerrainHight;
    public TerrainData te;


    // Start is called before the first frame update
    void Start()
    {
        Terrain terrain = Object.Instantiate(Prefab_TerrainHight).GetComponent<Terrain>();

    
        Debug.Log(terrain.terrainData.GetHeight(5, 5));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
