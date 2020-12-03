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
    GameObject Prefab_MainCamera => GenericTestMethods.GetPrefab("Gameplay_Camera");

    GameObject listener;

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        GameManager.InitializeTestingEnvironment(false, false, true, false, false);
        Object.Instantiate(Prefab_MainCamera);
        InitializeAudioListener();
    }

    [Test]
    public void _01TerrainPrefabExists()
    {
        Assert.NotNull(Prefab_TerrainBase);
    }

    [Test]
    public void _02TerrainPrefabComponentTransform()
    {
        Assert.IsNotNull(Prefab_TerrainBase.GetComponent<Transform>());
    }

    [Test]
    public void _03TerrainPrefabComponentTerrain()
    {
        Assert.IsNotNull(Prefab_TerrainBase.GetComponent<Terrain>());
    }

    [Test]
    public void _04TerrainPrefabComponentTerrainCollider()
    {
        Assert.IsNotNull(Prefab_TerrainBase.GetComponent<TerrainCollider>());
    }

    [Test]
    public void _05CreateTerrainGameObjectAPI()
    {
        GameObject terrain = (GameObject)Terrain.CreateTerrainGameObject(new TerrainData());
        terrain.name = "NewTarrain";
        Assert.NotNull(GameObject.Find("NewTarrain"));
    }

    [Test]
    public void _06CreateTerrainGameObjectDefaultValuesRight()
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
    public void _07GetActiveTerrainAPI()
    {
        GameObject terrain = (GameObject)Object.Instantiate(Prefab_TerrainBase);
        Assert.IsTrue(Terrain.activeTerrain.gameObject == terrain);
    }

    [Test]
    public void _08GetActiveTerrainsAPI()
    {
        GameObject[] terrain = new GameObject[3];
        terrain[0] = (GameObject)Object.Instantiate(Prefab_TerrainBase);
        terrain[1] = (GameObject)Object.Instantiate(Prefab_TerrainTree);
        terrain[2] = (GameObject)Object.Instantiate(Prefab_TerrainHight);

        Debug.Log(Terrain.activeTerrains.Length);
        Assert.IsTrue(Terrain.activeTerrains.Length == 3);
    }

    [Test]
    public void _09AddTreeInstanceAPI()
    {
        GameObject GO = (GameObject)Object.Instantiate(Prefab_TerrainTree);
        Terrain terrain = GO.GetComponent<Terrain>();

        // clear all tree instances as this add tree instance adds trees to prefab
        terrain.terrainData.treeInstances = new TreeInstance[0];
        terrain.AddTreeInstance ( new TreeInstance() );
        Assert.IsTrue(terrain.terrainData.treeInstanceCount == 1);
    }

    [Test]
    public void _10SetGetNeighborAPI()
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
    public void _11GetNormalMapAPI()
    {
        Terrain terrain = Object.Instantiate(Prefab_TerrainHight).GetComponent<Terrain>();

        RenderTexture normalMap = terrain.normalmapTexture;

        /* for graphics tests
        Texture2D normalMapTexture = toTexture2D(normalMap);
        ImageConversion.EncodeToPNG(normalMapTexture);
        */

        Assert.NotNull(normalMap);

    }


    [TearDown]
    public virtual void Teardown()
    {
        GenericTestMethods.ClearScene();

        if (listener)
            Object.DestroyImmediate(listener);
    }

    private void InitializeAudioListener()
    {
        //Disables Audio Listener warning spam
        listener = new GameObject("TestListener");
        listener.AddComponent<AudioListener>();
        listener.AddComponent<TestAudioListener>();
        Object.DontDestroyOnLoad(listener);
    }

    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(512, 512, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }
}
