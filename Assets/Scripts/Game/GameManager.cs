using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private float elapsedTime;
    private int score;
    private Difficulty difficulty = Difficulty.Easy;
    private ControlScheme controlScheme = ControlScheme.Joystick;

    public int Score => score;
    public float ElapsedTime => elapsedTime;
    public Difficulty CurrentDifficulty => difficulty;
    public ControlScheme CurrentControlScheme => controlScheme;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
    }

    public void AddScore(int amount)
    {
        score += amount;
        UIManager.Instance.UpdateScore(score);
    }
    
    public void SetDifficulty(Difficulty difficultySet)
    {
        difficulty = difficultySet;
    }
    
    public void SetControlScheme(ControlScheme scheme)
    {
        controlScheme = scheme;
        Debug.Log($"Схема управления: {controlScheme}");
    }
}

public enum ControlScheme
{
    Joystick,
    Buttons
}
