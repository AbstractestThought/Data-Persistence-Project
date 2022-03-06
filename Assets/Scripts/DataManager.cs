using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public MainManager mainManager;

    public TextMeshProUGUI FirstPlace;
    public TextMeshProUGUI SecondPlace;
    public TextMeshProUGUI ThirdPlace;

    public int score = 0;
    public int FirstPlaceScore = 0;
    public int SecondPlaceScore = 0;
    public int ThirdPlaceScore = 0;

    public string FirstPlaceName;
    public string SecondPlaceName;
    public string ThirdPlaceName;
    public string HighScoreText;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadScore(0);
    }


    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    

    class SaveData
    {
        public int FirstPlaceScore;
        public int SecondPlaceScore;
        public int ThirdPlaceScore;

        public string FirstPlaceName;
        public string SecondPlaceName;
        public string ThirdPlaceName;
    }

    public void SaveScore(string username)
    {
        SaveData data = new SaveData();

        if (score > FirstPlaceScore)
        {
            ThirdPlaceScore = SecondPlaceScore;
            SecondPlaceScore = FirstPlaceScore;
            FirstPlaceScore = score;

            ThirdPlaceName = SecondPlaceName;
            SecondPlaceName = FirstPlaceName;
            FirstPlaceName = username;

            HighScoreText = $"{FirstPlaceName} has a highscore of: {FirstPlaceScore}";
            mainManager.HighScoreText.text = HighScoreText;
        }
        else if (score > SecondPlaceScore)
        {
            ThirdPlaceScore = SecondPlaceScore;
            SecondPlaceScore = score;

            ThirdPlaceName = SecondPlaceName;
            SecondPlaceName = username;
        }
        else if (score > ThirdPlaceScore)
        {
            ThirdPlaceScore = score;

            ThirdPlaceName = username;
        }

        data.FirstPlaceScore = FirstPlaceScore;
        data.SecondPlaceScore = SecondPlaceScore;
        data.ThirdPlaceScore = ThirdPlaceScore;

        data.FirstPlaceName = FirstPlaceName;
        data.SecondPlaceName = SecondPlaceName;
        data.ThirdPlaceName = ThirdPlaceName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/leaderboard.json", json);
    }

    public void LoadScore(int sceneNum)
    {
        string path = Application.persistentDataPath + "/leaderboard.json";

        if (File.Exists(path))
        {
            string jsonText = File.ReadAllText(path);
            SaveData scoreData = JsonUtility.FromJson<SaveData>(jsonText);

            FirstPlaceScore = scoreData.FirstPlaceScore;
            SecondPlaceScore = scoreData.SecondPlaceScore;
            ThirdPlaceScore = scoreData.ThirdPlaceScore;

            FirstPlaceName = scoreData.FirstPlaceName;
            SecondPlaceName = scoreData.SecondPlaceName;
            ThirdPlaceName = scoreData.ThirdPlaceName;
        }
        
        if (sceneNum == 0)
        {
            FirstPlace.text = $"1. {FirstPlaceName}: {FirstPlaceScore}";
            SecondPlace.text = $"2. {SecondPlaceName}: {SecondPlaceScore}";
            ThirdPlace.text = $"3. {ThirdPlaceName}: {ThirdPlaceScore}";
        }
        else
        {
            HighScoreText = $"{FirstPlaceName} has a highscore of: {FirstPlaceScore}";
            mainManager.HighScoreText.text = HighScoreText;
        }


    }
}
