using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public struct PlayerScore
{
    public string playerName;
    public int score;
}

[CreateAssetMenu(fileName = "GameData", menuName = "Game Data", order = 51)]
public class GameDataScript : ScriptableObject
{
    public bool resetOnStart;
    public int level = 1;
    public int balls = 6;
    public int points = 0;
    public bool music = true;
    public bool sound = true;
    public int pointsToBall = 0;
    public string playerName = "Player";

    public PlayerScore[] topScores = new PlayerScore[5];

    public void Reset()
    {
        level = 1;
        balls = 6;
        points = 0;
        pointsToBall = 0;
    }

    public void Save()
    {
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.SetInt("balls", balls);
        PlayerPrefs.SetInt("points", points);
        PlayerPrefs.SetInt("pointsToBall", pointsToBall);
        PlayerPrefs.SetInt("music", music ? 1 : 0);
        PlayerPrefs.SetInt("sound", sound ? 1 : 0);

        for (int i = 0; i < topScores.Length; i++)
        {
            PlayerPrefs.SetString("playerName" + i, topScores[i].playerName);
            PlayerPrefs.SetInt("playerScore" + i, topScores[i].score);
        }
    }

    public void Load()
    {
        level = PlayerPrefs.GetInt("level", 1);
        balls = PlayerPrefs.GetInt("balls", 6);
        points = PlayerPrefs.GetInt("points", 0);
        pointsToBall = PlayerPrefs.GetInt("pointsToBall", 0);
        music = PlayerPrefs.GetInt("music", 1) == 1;
        sound = PlayerPrefs.GetInt("sound", 1) == 1;

        for (int i = 0; i < topScores.Length; i++)
        {
            topScores[i].playerName = PlayerPrefs.GetString("playerName" + i, "Player");
            topScores[i].score = PlayerPrefs.GetInt("playerScore" + i, 0);
        }
    }

    public void UpdateScores()
    {
        List<(string pName, int scr)> topScoresList = new List<(string, int)>();

        for (int i = 0; i < topScores.Length; i++)
        {
            topScoresList.Add((topScores[i].playerName, topScores[i].score));
        }
        topScoresList.Add((playerName == "" ? "Player" : playerName, points));
        topScoresList = topScoresList.OrderByDescending(x => x.scr).ToList();

        for (int i = 0; i < topScores.Length; i++)
        {
            if (i < topScoresList.Count)
            {
                topScores[i].playerName = topScoresList[i].pName;
                topScores[i].score = topScoresList[i].scr;
            }
            else
            {
                topScores[i].playerName = "";
                topScores[i].score = 0;
            }
        }

        Save();
    }
}

