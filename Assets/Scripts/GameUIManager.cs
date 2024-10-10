using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public GameDataScript gameData;
    public TMP_Text scoresText;
    public GameObject startMenu;
    public GameObject pauseMenu;
    public Button exitButton;
    private GameManager gameManager;

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }
        else
        {
            StartCoroutine(WaitForGameManager());
        }
    }
    private IEnumerator WaitForGameManager()
    {
        while (GameManager.Instance == null)
        {
            Debug.Log(GameManager.Instance);
            yield return null;
        }

        GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
    }

    void Start()
    {
        gameManager = GameManager.Instance;
        HandleGameStateChanged(gameManager.CurrentGameState);
        UpdateTopScoresUI();
    }

    void Update()
    {
    }

    private void HandleGameStateChanged(GameState newGameState)
    {
        switch (newGameState)
        {
            case GameState.MainMenu:
                Cursor.visible = true;
                startMenu.SetActive(true);
                pauseMenu.SetActive(false);
                exitButton.gameObject.SetActive(false);
                break;
            case GameState.Paused:
                Cursor.visible = true;
                startMenu.SetActive(false);
                pauseMenu.SetActive(true);
                exitButton.gameObject.SetActive(false);
                break;
            case GameState.GameOver:
                Cursor.visible = true;
                startMenu.SetActive(true);
                pauseMenu.SetActive(false);
                exitButton.gameObject.SetActive(true);

                if (gameData.points > gameData.topScores[4].score)
                {
                    gameData.UpdateScores();
                    UpdateTopScoresUI(true);
                }
                break;
            case GameState.Playing:
                Cursor.visible = false;
                startMenu.SetActive(false);
                pauseMenu.SetActive(false);
                exitButton.gameObject.SetActive(false);
                break;
            default:

                break;
        }
    }

    string OnOff(bool boolVal)
    {
        return boolVal ? "on" : "off";
    }

    private void drawGUI()
    {
        GUI.Label(new Rect(5, 4, Screen.width - 10, 100),
            string.Format(
                "<color=yellow><size=30>Level <b>{0}</b> Balls <b>{1}</b>" +
                " Score <b>{2}</b></size></color>", gameData.level, gameData.balls, gameData.points
            )
        );
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.UpperRight;
        GUI.Label(new Rect(5, 14, Screen.width - 10, 100),
            string.Format(
                "<color=yellow><size=20><color=white>Space</color>-pause {0}" +
                " <color=white>N</color>-new" +
                // " <color=white>J</color>-jump" +
                " <color=white>M</color>-music {1}" +
                " <color=white>S</color>-sound {2}" +
                " <color=white>Esc</color>-pause</size></color>",
                OnOff(Time.timeScale > 0), OnOff(!gameData.music),
                OnOff(!gameData.sound)
            ), style
        );
    }

    void OnGUI()
    {
        if (gameManager.CurrentGameState == GameState.Playing)  
        {
            drawGUI();
        }
    }

    void UpdateTopScoresUI(bool newRecord = false)
    {
        scoresText.text = "";
        if (newRecord) {
            scoresText.text += "New Highscore!!!\n";
        }
        scoresText.text += "Top Scores:\n";

        for (int i = 0; i < gameData.topScores.Length; i++)
        {
            var playerScore = gameData.topScores[i];

            if (playerScore.score <= 0)
            {
                continue;
            }

            scoresText.text += string.Format("{0}. {1} - {2} points\n", i + 1, playerScore.playerName, playerScore.score);
        }
    }

    public void StartNewGame()
    {
        if (gameData.resetOnStart)
            gameData.Load();
       
        gameData.Reset();
        gameManager.SetGameState(GameState.MainMenu);
        gameManager.SetGameState(GameState.Playing);
        gameData.playerName = playerNameInput.text;
        
    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
