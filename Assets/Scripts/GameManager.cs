using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool effectsEnabled = true;
    public static bool startEnabled = true;
    public static bool updateEnabled = true;
    public static bool spawningEnabled = true;

    public static void InitializeTestingEnvironment(bool start, bool update, bool effects, bool asteroids, bool paused)
    {
        effectsEnabled = effects;
        startEnabled = start;
        updateEnabled = update;
        spawningEnabled = asteroids;
        IsPaused = paused;
    }

    public static GameManager instance;
    private static bool isPaused;

    public static bool IsPaused
    {
        get { return isPaused; }
        set
        {
            Time.timeScale = value ? 0.0f : 1.0f;
            isPaused = value;
        }         //timescale is 0 when paused, 1 when unpaused
    }

    void OnEnable()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        Time.timeScale = 1.0f;
        IsPaused = false;
    }


}
