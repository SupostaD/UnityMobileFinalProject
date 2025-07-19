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
    private SaveData pendingLoadData;
    
    public event Action<ControlScheme> OnControlSchemeChanged;

    public int Score => score;
    public float ElapsedTime => elapsedTime;
    public Difficulty CurrentDifficulty => difficulty;
    public ControlScheme CurrentControlScheme => controlScheme;
    public string PlayerName => playerName;
    public SaveData PendingLoadData => pendingLoadData;
    
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
    
    public void SetControlScheme(ControlScheme scheme)
    {
        controlScheme = scheme;
        OnControlSchemeChanged?.Invoke(controlScheme);
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

    public void SetPlayerName(string name)
    {
        if (!string.IsNullOrWhiteSpace(name))
            playerName = name;
    }

    public void SetScore(int newScore)
    {
        score = newScore;
    }

    public void SetElapsedTime(float newElapsedTime)
    {
        elapsedTime = newElapsedTime;
    }
    
    public void SetPendingLoadData(SaveData data)
    {
        pendingLoadData = data;
    }
}

public enum ControlScheme
{
    Joystick,
    Buttons
}
