#pragma warning disable IDE1006 // Naming Styles
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PrefabTests : TestTemplate
{
    //List of prefabs
    GameObject Prefab_TerrainBase => GenericTestMethods.GetPrefab("BaseTerrain");
    GameObject Prefab_TerrainTree => GenericTestMethods.GetPrefab("TerrainTree");
    GameObject Prefab_TerrainHight => GenericTestMethods.GetPrefab("TerrainHightMap");
    GameObject Prefab_MainCamera => GenericTestMethods.GetPrefab("Gameplay_Camera");

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        Object.Instantiate(Prefab_MainCamera);
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

}
