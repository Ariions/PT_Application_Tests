#pragma warning disable IDE1006 // Naming Styles
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TerrainTests : TestTemplate
{
    //List of prefabs
    GameObject Prefab_TerrainBase => GenericTestMethods.GetPrefab("BaseTerrain");
    GameObject Prefab_TerrainTree => GenericTestMethods.GetPrefab("TerrainTree");
    GameObject Prefab_TerrainHight => GenericTestMethods.GetPrefab("TerrainHightMap");
    GameObject Prefab_TerrainHoles => GenericTestMethods.GetPrefab("TerrainHoles");
    GameObject Prefab_CollitionDetector => GenericTestMethods.GetPrefab("CollitionDetector");
    GameObject Prefab_MainCamera => GenericTestMethods.GetPrefab("Gameplay_Camera");


    [SetUp]
    public override void Setup()
    {
        base.Setup();
        Object.Instantiate(Prefab_MainCamera);
    }

    [Test]
    public void _01CreateTerrainGameObjectAPI()
    {
        GameObject terrain = (GameObject)Terrain.CreateTerrainGameObject(new TerrainData());
        terrain.name = "NewTarrain";
        Assert.NotNull(GameObject.Find("NewTarrain"));
    }

    [Test]
    public void _02CreateTerrainGameObjectDefaultValuesRight()
    {
        TerrainData data = new TerrainData();

        GameObject terrain = (GameObject)Terrain.CreateTerrainGameObject(data);
        GameObject terrainBaseSettings = (GameObject)Object.Instantiate(Prefab_TerrainBase);

        TerrainData tested = terrain.GetComponent<Terrain>().terrainData;
        TerrainData original = terrainBaseSettings.GetComponent<Terrain>().terrainData;


        Assert.IsTrue(tested.heightmapResolution == original.heightmapResolution);
        Assert.IsTrue(tested.size               == original.size);
        Assert.IsTrue(tested.treeInstanceCount == original.treeInstanceCount);
    }

    [Test]
    public void _03GetActiveTerrainAPI()
    {
        GameObject terrain = (GameObject)Object.Instantiate(Prefab_TerrainBase);
        Assert.IsTrue(Terrain.activeTerrain.gameObject == terrain);
    }

    [Test]
    public void _04GetActiveTerrainsAPI()
    {
        GameObject[] terrain = new GameObject[3];
        terrain[0] = (GameObject)Object.Instantiate(Prefab_TerrainBase);
        terrain[1] = (GameObject)Object.Instantiate(Prefab_TerrainTree);
        terrain[2] = (GameObject)Object.Instantiate(Prefab_TerrainHight);

        Assert.IsTrue(Terrain.activeTerrains.Length == 3);
    }

    [Test]
    public void _05AddTreeInstanceAPI()
    {
        GameObject GO = (GameObject)Object.Instantiate(Prefab_TerrainTree);
        Terrain terrain = GO.GetComponent<Terrain>();

        // clear all tree instances as this add tree instance adds trees to prefab
        terrain.terrainData.treeInstances = new TreeInstance[0];
        terrain.AddTreeInstance ( new TreeInstance() );
        Assert.IsTrue(terrain.terrainData.treeInstanceCount == 1);
    }

    [Test]
    public void _06SetGetNeighborAPI()
    {
        Terrain terrainMain      = Object.Instantiate(Prefab_TerrainBase).GetComponent<Terrain>();

        Terrain terrainLeft      = Object.Instantiate(Prefab_TerrainBase).GetComponent<Terrain>();
        Terrain terrainRight     = Object.Instantiate(Prefab_TerrainBase).GetComponent<Terrain>();
        Terrain terrainTop       = Object.Instantiate(Prefab_TerrainBase).GetComponent<Terrain>();
        Terrain terrainBottom    = Object.Instantiate(Prefab_TerrainBase).GetComponent<Terrain>();

        terrainMain.SetNeighbors(terrainLeft, terrainTop, terrainRight, terrainBottom);

        Assert.IsTrue(terrainMain.topNeighbor       == terrainTop);
        Assert.IsTrue(terrainMain.leftNeighbor      == terrainLeft);
        Assert.IsTrue(terrainMain.rightNeighbor     == terrainRight);
        Assert.IsTrue(terrainMain.bottomNeighbor    == terrainBottom);    
    }

    [Test]
    public void _07CheckForHoles()
    {
        Terrain terrain = Object.Instantiate(Prefab_TerrainHoles).GetComponent<Terrain>();
        Terrain terrainBase = Object.Instantiate(Prefab_TerrainBase).GetComponent<Terrain>();

        bool hasHoles = false;
        bool BaseHasHoles = false;

        foreach(bool ishole in terrain.terrainData.GetHoles(1,1,1,1))
        {
            hasHoles ^= ishole;
        }
        foreach (bool ishole in terrainBase.terrainData.GetHoles(1, 1, 1, 1))
        {
            BaseHasHoles ^= ishole;
        }

        Assert.IsTrue(hasHoles);
        Assert.IsTrue(!BaseHasHoles);
    }

    [UnityTest]
    public IEnumerator _08CollidingWithTerrainFlat()
    {
        GameObject terrain = Object.Instantiate(Prefab_TerrainBase, Vector3.zero, Quaternion.identity);
        GameObject Ball = Object.Instantiate(Prefab_CollitionDetector, new Vector3(12f, 1f, 12f), Quaternion.identity);

        yield return new WaitForSeconds(1f);
        yield return null;
        Assert.IsTrue(Ball.GetComponent<DetectColition>().collided);
    }

    [UnityTest]
    public IEnumerator _09CollidingWithTerrainHoles()
    {
        GameObject terrain = Object.Instantiate(Prefab_TerrainHoles, Vector3.zero, Quaternion.identity);
        GameObject Ball = Object.Instantiate(Prefab_CollitionDetector, new Vector3(500f, 1f, 500f), Quaternion.identity);

        yield return new WaitForSeconds(1f);
        yield return null;
        Assert.IsTrue(!Ball.GetComponent<DetectColition>().collided);
    }

    [UnityTest]
    public IEnumerator _10CollidingWithTerrainSlope()
    {
        GameObject terrain = Object.Instantiate(Prefab_TerrainHight, Vector3.zero, Quaternion.identity);
        GameObject Ball = Object.Instantiate(Prefab_CollitionDetector, new Vector3(500f, 70f, 500f), Quaternion.identity);

        yield return new WaitForSeconds(1f);
        yield return null;
        Assert.IsTrue(Ball.GetComponent<DetectColition>().collided);
    }

}
