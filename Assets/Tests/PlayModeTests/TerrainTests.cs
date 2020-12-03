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
    GameObject Prefab_Terrain => GenericTestMethods.GetPrefab("Terrain");
    GameObject Prefab_MainCamera => GenericTestMethods.GetPrefab("Gameplay_Camera");

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        GameManager.InitializeTestingEnvironment(false, false, true, false, false);
        Object.Instantiate(Prefab_MainCamera);
    }

    tets

    [Test]
    public void _01TerrainPrefabExists()
    {
        Assert.NotNull(Prefab_Terrain);
    }
}
