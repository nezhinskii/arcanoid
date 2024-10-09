using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState CurrentGameState { get; private set; }

    public GameState PreviousGameState { get; private set;  }

    public event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            SetGameState(GameState.MainMenu);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetGameState(GameState newGameState)
    {
        PreviousGameState = CurrentGameState;
        CurrentGameState = newGameState;
        OnGameStateChanged?.Invoke(newGameState);
        Debug.Log("Game State changed to: " + newGameState);
    }
}
