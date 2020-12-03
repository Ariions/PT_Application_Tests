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
    public void _02CreateTerrainGameObjectAPICheck()
    {
        GameObject spaceship = (GameObject)Terrain.CreateTerrainGameObject(new TerrainData());
        spaceship.name = "NewTarrain";
        Assert.NotNull(GameObject.Find("NewTarrain"));
    }

    [Test]
    public void _03TerrainPrefabComponentTransform()
    {
        Assert.IsNotNull(Prefab_TerrainBase.GetComponent<Transform>());
    }

    [Test]
    public void _04TerrainPrefabComponentTerrain()
    {
        Assert.IsNotNull(Prefab_TerrainBase.GetComponent<Terrain>());
    }

    [Test]
    public void _04TerrainPrefabComponentTerrainCollider()
    {
        Assert.IsNotNull(Prefab_TerrainBase.GetComponent<TerrainCollider>());
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
}
