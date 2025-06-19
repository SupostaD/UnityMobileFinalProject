using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    private string playerName = "DefaultName";
    private float elapsedTime;
    private int score;
    private Difficulty difficulty = Difficulty.Easy;
    private ControlScheme controlScheme = ControlScheme.Joystick;

    public int Score => score;
    public float ElapsedTime => elapsedTime;
    public Difficulty CurrentDifficulty => difficulty;
    public ControlScheme CurrentControlScheme => controlScheme;
    public string PlayerName => playerName;


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
    }

    public void SetPlayerName(string name)
    {
        if (!string.IsNullOrWhiteSpace(name))
            playerName = name;
    }

    public void SetScore(int newScore)
    {
        score = newScore;
    }
}

public enum ControlScheme
{
    Joystick,
    Buttons
}
