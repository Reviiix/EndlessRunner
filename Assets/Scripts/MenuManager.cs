using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Introduction")]
    public Canvas introductionCanvas;
    public Button startButton;
    [Header("Main")]
    public Canvas mainCanvas;
    public Button pauseButton;
    [Header("Pause")]
    public Canvas pauseCanvas;
    public Button restartButtonPauseMenu;
    public Button resumeButton;
    public Canvas restartConfirmationCanvas;
    public Button cancelRestartPause;
    public Button confirmRestartButton;
    [Header("Game Over")]
    public Canvas gameOverCanvas;
    private CanvasGroup _gameOverCanvasGroup;
    public Button restartButtonGameOverMenu;
    public TMP_Text finalTime;
    public TMP_Text finalScore;
    public TMP_Text highScore;
    public const string ScorePrefix = "SCORE: ";
    public const string TimePrefix = "TIME: ";
    private const string RichTextHighLightStringStart = "<color=blue><b><u>";
    private const string RichTextHighLightStringEnd = "</u></b></color>";
    private const string HighScorePrefix = HighScoreManager.HighScoreId;
    private const float FadeSpeed = 0.005f;


    private void Awake()
    {
        InitialiseVariables();
        
        InitialiseButtons();
    }

    private void InitialiseVariables()
    {
        _gameOverCanvasGroup = gameOverCanvas.GetComponent<CanvasGroup>();
    }

    private void InitialiseButtons()
    {
        startButton.onClick.AddListener(StartGame);
        restartButtonGameOverMenu.onClick.AddListener(RestartButton);
        restartButtonPauseMenu.onClick.AddListener(RestartConfirmation);
        cancelRestartPause.onClick.AddListener(EnablePauseCanvas);
        confirmRestartButton.onClick.AddListener(RestartButton);
        resumeButton.onClick.AddListener(Resume);
        pauseButton.onClick.AddListener(Pause);

        AddButtonClickNoise();
    }

    private void AddButtonClickNoise()
    {
        startButton.onClick.AddListener(PlayButtonClick);
        restartButtonGameOverMenu.onClick.AddListener(PlayButtonClick);
        restartButtonPauseMenu.onClick.AddListener(PlayButtonClick);
        cancelRestartPause.onClick.AddListener(PlayButtonClick);
        confirmRestartButton.onClick.AddListener(PlayButtonClick);
        resumeButton.onClick.AddListener(PlayButtonClick);
        pauseButton.onClick.AddListener(PlayButtonClick);
    }

    private static void PlayButtonClick()
    {
        EndlessRunnerGameManager.instance.audioManager.PlayButtonClickNoise();
    }

    private void StartGame()
    {
        EndlessRunnerGameManager.instance.StartGame();
        EnableMainCanvas();
    }

    private static void RestartButton()
    {
        EndlessRunnerGameManager.Restart();
    }

    public void EnableIntroductionCanvas()
    {
        HideAllCanvases();
        introductionCanvas.enabled = true;
    }

    private void EnableMainCanvas()
    {
        HideAllCanvases();
        mainCanvas.enabled = true;
    }
    
    private void EnablePauseCanvas()
    {
        HideAllCanvases();
        pauseCanvas.enabled = true;
    }
    
    private void RestartConfirmation()
    {
        HideAllCanvases();
        restartConfirmationCanvas.enabled = true;
    }
    
    public void EnableGameOverCanvas()
    {
        StartCoroutine(Fade(_gameOverCanvasGroup, FadeSpeed));
        HideAllCanvases();
        finalTime.text = TimePrefix + ScoreAndTimeManager.finalTime.ToString(@"m\:ss\:ff");
        if (HighScoreManager.IsCurrentScoreHigherThanHighScore())
        {
            highScore.text = HighScorePrefix + RichTextHighLightStringStart + HighScoreManager.ReturnHighScore() + RichTextHighLightStringEnd;
            finalScore.text = ScorePrefix + RichTextHighLightStringStart + ScoreAndTimeManager.score + RichTextHighLightStringEnd;
            gameOverCanvas.enabled = true;
            return;
        }
        highScore.text = HighScorePrefix + HighScoreManager.ReturnHighScore();
        finalScore.text = ScorePrefix + ScoreAndTimeManager.score;
        gameOverCanvas.enabled = true;
    }

    private void HideAllCanvases()
    {
        SetAllCanvases(false);
    }

    private void SetAllCanvases(bool state)
    {
        introductionCanvas.enabled = state;
        mainCanvas.enabled = state;
        pauseCanvas.enabled = state;
        gameOverCanvas.enabled = state;
        restartConfirmationCanvas.enabled = state;
    }

    private void Pause()
    {
        EndlessRunnerGameManager.Pause(true);
        EnablePauseCanvas();
    }

    private void Resume()
    {
        EndlessRunnerGameManager.Pause(false);
        EnableMainCanvas();
    }

    private static IEnumerator Fade(CanvasGroup canvas, float speed)
    {
        while (canvas.alpha < 1)
        {
            canvas.alpha += speed;
            yield return null;
        }
    }
}
