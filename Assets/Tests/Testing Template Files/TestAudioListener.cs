using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Disables Audio Listener warning spam in tests
public class TestAudioListener : MonoBehaviour
{
    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectsOfType<AudioListener>().Length > 1)
            DestroyImmediate(gameObject);
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (FindObjectsOfType<AudioListener>().Length > 1)
            if (this != null && gameObject != null)
                DestroyImmediate(gameObject);
    }
}
