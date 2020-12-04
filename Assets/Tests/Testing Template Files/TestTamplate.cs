#pragma warning disable IDE1006 // Naming Styles
using UnityEngine;
using NUnit.Framework;

public abstract class TestTemplate
{
    GameObject listener;

    [SetUp]
    public virtual void Setup()
    {
        Time.timeScale = 1.0f; //reset timescale
        AudioListener.volume = 0.0f;    //mute audio in tests


        //Disables Audio Listener warning spam
        InitializeAudioListener();
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