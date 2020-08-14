using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreAndTimeManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public static int score = 0;
    private const string ScorePrefix = MenuManager.ScorePrefix;
    private const int ScoreIncrement = 1;
    private static float _scoreMultiplier = 1;
    
    public TMP_Text timeText;
    private static bool _trackTime = false;
    private Coroutine _timeTracker;
    private const string TimePrefix = MenuManager.TimePrefix;
    private static DateTime _startTime;
    public static TimeSpan finalTime;

    private void Awake()
    {
        InitialiseVariables();
    }

    private static void InitialiseVariables()
    {
        score = 0;
    }

    public static void IncrementScore(int amount)
    {
        score += amount;
    }

    public void StartTimer()
    {
        _trackTime = true;
        _startTime = DateTime.Now;
        _timeTracker = StartCoroutine(TrackTime(_startTime));
    }
    
    public void StopTimer()
    {
        _trackTime = false;
        OnTimerStop(TrackTimeFrom(_startTime));
    }
    
    private IEnumerator TrackTime(DateTime startingTime)
    {
        while (_trackTime)
        {
            if (_trackTime == false)
            {
                OnTimerStop(TrackTimeFrom(startingTime));
                yield return null;
            }
            UpdateScore();
            UpdateTime(startingTime);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    private void OnTimerStop(TimeSpan endTime)
    {
        if (_timeTracker != null)
        {
            StopCoroutine(_timeTracker);
        }
        finalTime = endTime;
    }

    private void UpdateTime(DateTime startingTime)
    {
        timeText.text = TimePrefix + TrackTimeFrom(startingTime).ToString(@"m\:ss\:ff");
    }
    
    private void UpdateScore()
    {
        score += ScoreIncrement;
        UpdateScoreMultiplier();
        scoreText.text = ScorePrefix + score;
    }

    private static void UpdateScoreMultiplier()
    {
        _scoreMultiplier *= (_scoreMultiplier / 10);
    }
        
    private static TimeSpan TrackTimeFrom(DateTime timePlayedSoFar)
    {
        return(DateTime.Now - timePlayedSoFar);
    }
}

public class HighScoreManager
{
    //There are much better, safer, more optimal ways to do store data; This is just for a prototype feature demonstration;
    public const string HighScoreId = "HIGHSCORE: ";
    
    public static int ReturnHighScore()
    {
        if (PlayerPrefs.HasKey(HighScoreId))
        {
            #if UNITY_EDITOR
            EndlessRunnerGameManager.DisplayDebugMessage("No previous high score saved.");
            #endif
            return PlayerPrefs.GetInt(HighScoreId);
        }
        #if UNITY_EDITOR
        EndlessRunnerGameManager.DisplayDebugMessage("High score retrieval successful.");
        #endif
        return ScoreAndTimeManager.score;
    }
    
    public static void SetHighScore(int newScore)
    {
        PlayerPrefs.SetInt(HighScoreId, newScore);
        
        #if UNITY_EDITOR
        EndlessRunnerGameManager.DisplayDebugMessage("New high score set: " + newScore);
        #endif
    }

    public static bool IsCurrentScoreHigherThanHighScore()
    {
        if (!PlayerPrefs.HasKey(HighScoreId)) return true;

        return ScoreAndTimeManager.score >= PlayerPrefs.GetInt(HighScoreId);
    }
}
