#pragma warning disable IDE1006 // Naming Styles
//#define GRAPHICS_TESTS_ENABLED

using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class GraphicsTests
{

#if GRAPHICS_TESTS_ENABLED && UNITY_EDITOR
    int referenceImageHeight = 128;
    int referenceImageWidth = 128;
    float colorThreshold = 0.05f;

    string referenceDirectory = "Assets/Tests/Playmode Tests/GraphicsTests/";
    Texture2D referenceTexture;
    
    //GameObject cube;

    [SetUp]
    public void Setup()
    {
        referenceTexture = (Texture2D)AssetDatabase.LoadAssetAtPath( referenceDirectory + "BasicTest/_01ReferenceImage.png", typeof(Texture2D));
        //cube = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Cube.prefab", typeof(GameObject));
    }
    
    [TearDown]
    public void Teardown()
    {
        GenericTestMethods.ClearScene();
    }

    public bool CompareImages(string imageFolder, Camera camera)
    {
        bool comparisonErrors = false;

        RenderTexture renderTexture = new RenderTexture(referenceImageHeight, referenceImageWidth, 24);
        camera.targetTexture = renderTexture;
        camera.Render();
        RenderTexture.active = renderTexture;

        if (referenceTexture == null)               //Generate new reference image if it does not already exist
        {
            camera.Render();
            if (Screen.width != referenceImageHeight || Screen.height != referenceImageWidth)
                Debug.LogError("Incorrect resolution, set scene view resolution to fixed " + referenceImageHeight + "x" + referenceImageWidth);

            Texture2D newReferenceTexture = new Texture2D(referenceImageHeight, referenceImageWidth, TextureFormat.RGB24, false);
            newReferenceTexture.ReadPixels(new Rect(0, 0, referenceImageHeight, referenceImageWidth), 0, 0);
            byte[] newReferenceImageBytes = newReferenceTexture.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/../" + referenceDirectory + imageFolder + "/_01ReferenceImage.png", newReferenceImageBytes);

            Debug.LogError("New reference " + imageFolder + " texture generated, reimport images and rerun test for results");
            return false;
        }

        Texture2D newTexture = new Texture2D(referenceImageHeight, referenceImageWidth, TextureFormat.RGB24, false);
        newTexture.ReadPixels(new Rect(0, 0, referenceImageHeight, referenceImageWidth), 0, 0);
        byte[] newImageBytes = newTexture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/../" + referenceDirectory + imageFolder + "/_02ComparisonImage.png", newImageBytes);

        Color[] newImagePixels = newTexture.GetPixels();
        Color[] referenceImagePixels = referenceTexture.GetPixels();
        Color[] diffPixels = new Color[newImagePixels.Length];
        
        Assert.IsTrue(newImagePixels.Length == referenceImagePixels.Length); //IMAGE SIZE DOES NOT MATCH UP

        bool identical = true;
        for (int i = 0; i < newImagePixels.Length; i++)
        {
            if (newImagePixels[i] != referenceImagePixels[i])
            {
                //diffPixels[i] = (newImagePixels[i] + referenceImagePixels[i]) / 2.0f;
                //diffPixels[i] = newImagePixels[i];
                if (Mathf.Abs(newImagePixels[i].r - referenceImagePixels[i].r) > colorThreshold || Mathf.Abs(newImagePixels[i].g - referenceImagePixels[i].g) > colorThreshold || Mathf.Abs(newImagePixels[i].b - referenceImagePixels[i].b) > colorThreshold)
                {
                    diffPixels[i] = new Color(1.0f, 0.41f, 0.705f, 1.0f);
                    identical = false;
                }
            }
            else
            {
                diffPixels[i] = newImagePixels[i];
            }
        }

        if (identical)
            for(int i = 0; i < diffPixels.Length; i++)
                diffPixels[i] = new Color(0.2f, 1.0f, 0.2f, 1.0f);

        Texture2D outputTexture = new Texture2D(referenceImageHeight, referenceImageWidth, TextureFormat.RGB24, false);
        outputTexture.SetPixels(diffPixels);
        outputTexture.Apply();
        byte[] diffImageBytes = outputTexture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/../" + referenceDirectory + imageFolder + "/_03OutputImage.png", diffImageBytes);

        comparisonErrors = !identical;
        if (comparisonErrors)
            Debug.LogError(imageFolder + " images do not match. Make sure reference images were made with the same project revision and graphics settings.");
        AssetDatabase.Refresh();

        return comparisonErrors;
    }

    // A Test behaves as an ordinary method
    [UnityTest]
    public IEnumerator _01BasicGraphicsTest()
    {
        //graphic test initialization
        SceneManager.LoadScene("Scenes/04_Gameplay");
        GameManager.InitializeTestingEnvironment(true, false, false, false, true);
        GameManager.updateEnabled = false;
        yield return null;
        yield return new WaitForSeconds(0.1f);
        GameObject.Find("Camera");
        //graphic test initialization

        Assert.IsFalse(CompareImages("BasicTest", GameObject.Find("Camera").GetComponent<Camera>()));
    }
#endif
}