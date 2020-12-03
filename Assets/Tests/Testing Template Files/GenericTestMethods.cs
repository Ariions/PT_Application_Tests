using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using UnityEngine;

public static class AssertExtensions
{
    public static void VectorsAreEqual(this NUnit.Framework.Assert assert, Vector3 expected, Vector3 actual, float delta, string message)
    {
        NUnit.Framework.Assert.AreEqual(expected.x, actual.x, delta, message);
        NUnit.Framework.Assert.AreEqual(expected.y, actual.y, delta, message);
        NUnit.Framework.Assert.AreEqual(expected.z, actual.z, delta, message);

    }
}

public static class GenericTestMethods
{
    //Fetch prefab by name
    public static GameObject GetPrefab(string name)
    {
        List<GameObject> prefabList = ((GameObject)Resources.Load("Tests/PrefabReferences", typeof(GameObject))).GetComponent<PrefabPaths>().allPrefabs;
        Assert.NotNull(prefabList, "Missing prefabs. List is null.");
        Assert.IsTrue(prefabList.Count > 0, "Missing prefabs. List is empty.");
        GameObject gameObject = prefabList.Single(i => i.name == name);
        Assert.NotNull(gameObject, "Failed to find " + name + " prefab.");
        return gameObject;
    }

    public static void ClearScene()
    {
        Transform[] objects = UnityEngine.Object.FindObjectsOfType<Transform>();
        foreach (Transform obj in objects)
        {
            if (obj != null)
                UnityEngine.Object.DestroyImmediate(obj.gameObject);
        }
    }

    public static IEnumerator SkipFrames(int minFrameCount, float minDuration)
    {
        int iterations = Mathf.Max(minFrameCount, (int)(minDuration / Time.unscaledTime));
        for (int i = 0; i < iterations; i++)
            yield return null;
    }

    public static IEnumerator SkipPhysicsFrames(int frames)
    {
        for (int i = 0; i < frames; i++)
            yield return new WaitForFixedUpdate();
    }
}

public static class TestConstants
{
    public const string OPENING_INTRO_SCENE_NAME = "Scenes/01_Opening_Intro";
    public const string MENU_SCENE_NAME = "Scenes/02_Menu";
    public const string GAMEPLAY_INTRO_SCENE_NAME = "Scenes/03_Gameplay_Intro";
    public const string GAMEPLAY_SCENE_NAME = "Scenes/04_Gameplay";
    public const string BOSS_INTRO_SCENE_NAME = "Scenes/05_Boss_Intro";
}
