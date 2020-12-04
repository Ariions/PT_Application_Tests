using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Graphics;
using UnityEngine.SceneManagement;

public class GraphicsTests
{
    public const string universalPackagePath = "Assets/ReferenceImages/";

    [UnityTest, Category("TerrainTest")]
    [PrebuildSetup("SetupGraphicsTestCases")]
    [UseGraphicsTestCases(universalPackagePath)]
    

    public IEnumerator Runtime(GraphicsTestCase testCase)
    {
        SceneManager.LoadScene(testCase.ScenePath);

        // Always wait one frame for scene load
        yield return null;
        
        var cameras = GameObject.FindGameObjectsWithTag("MainCamera").Select(x => x.GetComponent<Camera>());
        var settings = Object.FindObjectOfType<GlobalGraphicsTestSettings>();
        Assert.IsNotNull(settings, "Invalid test scene, couldn't find GlobalGraphicsTestSettings");

        Scene scene = SceneManager.GetActiveScene();

        ImageAssert.AreEqual(testCase.ReferenceImage, cameras.Where(x => x != null), settings.ImageComparisonSettings);
    }

#if UNITY_EDITOR
    [TearDown]
    public void DumpImagesInEditor()
    {
        UnityEditor.TestTools.Graphics.ResultsUtility.ExtractImagesFromTestProperties(TestContext.CurrentContext.Test);
    }
#endif
}
