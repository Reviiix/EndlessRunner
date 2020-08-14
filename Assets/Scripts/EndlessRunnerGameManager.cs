using System;
using System.Collections;
using Environment;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndlessRunnerGameManager : MonoBehaviour
{
    public static EndlessRunnerGameManager instance;
    private const bool ShowDebugMessages = true;
    private const int DamageFlashCycles = 10;
    private const float TimeBetweenDamageFlashes = 0.1f;
    public AudioManager audioManager;
    public PlayerManager player;
    public MenuManager menuManager;
    public EnvironmentManager environmentManager;
    public LaneManager laneManager;
    public ScoreAndTimeManager scoreAndTimeManager;
    public HighScoreManager highScoreManager = new HighScoreManager();
    public ShakeCamera cameraShake;

    private void Awake()
    {
        instance = this;
        Pause(true);
        menuManager.EnableIntroductionCanvas();
    }

    public void StartGame()
    {
        Pause(false);
        scoreAndTimeManager.StartTimer();
        player.enabled = true;
        audioManager.StartBackgroundLoop();
    }

    private void GameOver()
    {
        Pause(true);
        player.enabled = false;
        scoreAndTimeManager.StopTimer();
        SetLeaderBoards();
        menuManager.EnableGameOverCanvas();
        cameraShake.StopShake();
        audioManager.StopBackgroundLoop(0.1f);
    }

    public void EnemyCollision()
    {
        #if UNITY_EDITOR
        DisplayDebugMessage("Collided with enemy.");
        #endif

        if (!PlayerManager.canBeDamaged)
        {
            return;
        }
        
        cameraShake.Shake();
        
        if (PlayerManager.shieldActive)
        {
            player.TakeHit();
            audioManager.PlayShieldDepletedNoise();
            return;
        }
        audioManager.PlayCrashNoise();
        GameOver();
    }

    public void PickupCollision(int pointValue)
    {
        #if UNITY_EDITOR
        DisplayDebugMessage("Pickup collected.");
        #endif
        audioManager.PlayPickUpNoise();
        ScoreAndTimeManager.IncrementScore(pointValue);
        player.ActivateShield(true);
    }
    
    public static void Pause(bool state)
    {
        if (state)
        {
            Time.timeScale = 0;
            return;
        }
        Time.timeScale = 1;
    }

    public static void Restart()
    {
        Pause(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private static void SetLeaderBoards()
    {
        if (HighScoreManager.IsCurrentScoreHigherThanHighScore())
        {
            HighScoreManager.SetHighScore(ScoreAndTimeManager.score);
        }
    }
    
    public static IEnumerator FlashObject(Renderer renderer, Action callBack = null, int flashCycles = DamageFlashCycles, float timeBetweenFlashes = TimeBetweenDamageFlashes)
    {
        var state = false;
        for (var i = 0; i <= flashCycles; i++)
        {
            renderer.enabled = state;
            state = !state;
            yield return new WaitForSeconds(timeBetweenFlashes);
        }
        renderer.enabled = true;
        yield return new WaitForSeconds(timeBetweenFlashes);
        callBack?.Invoke();
    }
    
    public static IEnumerator Wait(float time, Action callBack)
    {
        yield return new WaitForSeconds(time);
        callBack();
    }
    
    public static void DisplayDebugMessage(string message)
    {
        #if UNITY_EDITOR
        if (ShowDebugMessages)
        {
            Debug.Log(message); 
        }
        #endif
    }
}
