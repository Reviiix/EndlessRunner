using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip backgroundClip;
    public AudioSource backgroundMusicSource;
    
    public AudioClip buttonClick;
    public AudioClip laneChange;
    public AudioClip pickupCollected;
    public AudioClip shieldsGone;
    public AudioClip crash;
    public AudioClip invalidLaneNoise;

    private const float FadeTime = 0.001f;
    private const float backgroundVolume = 0.1f;

    public void StartBackgroundLoop(float fadeTime = FadeTime)
    {
        backgroundMusicSource.clip = backgroundClip;
        StartCoroutine(FadeIn(backgroundMusicSource, fadeTime, () =>
        {
            backgroundMusicSource.loop = true;
            backgroundMusicSource.Play();
        }));
    }
    
    public void StopBackgroundLoop(float fadeTime = FadeTime)
    {
        StartCoroutine(FadeOut(backgroundMusicSource, fadeTime, () =>
        {
            backgroundMusicSource.clip = null;
            backgroundMusicSource.loop = false;
            backgroundMusicSource.Stop();
        }));
    }

    public void PlayButtonClickNoise()
    {
        PlaySound(buttonClick);
    }
    
    public void PlayLaneChangeNoise()
    {
        PlaySound(laneChange);
    }
    
    public void PlayPickUpNoise()
    {
        PlaySound(pickupCollected);
    }
    
    public void PlayShieldDepletedNoise()
    {
        PlaySound(shieldsGone);
    }
    
    public void PlayCrashNoise()
    {
        PlaySound(crash);
    }
    
    public void PlayInvalidLaneNoise()
    {
        PlaySound(invalidLaneNoise);
    }
    
    private static IEnumerator FadeOut(AudioSource source, float speed, Action callBack)
    {
        while (source.volume > 0)
        {
            source.volume -= speed;
            yield return null;
        }
        callBack();
    }
    
    private static IEnumerator FadeIn(AudioSource source, float speed, Action callBack)
    {
        while (source.volume < backgroundVolume)
        {
            source.volume += speed;
            yield return null;
        }
        callBack();
    }
    
    private void PlaySound(AudioClip clip)
    {
        var audioSourceGameObject = ObjectPooling.ReturnObjectFromPool(3, transform.position, Quaternion.identity);
        var audioSource = audioSourceGameObject.GetComponent<AudioSource>();
        audioSource.clip = clip;
      
        audioSource.Play();
      
        StartCoroutine(EndlessRunnerGameManager.Wait(clip.length, () =>
        {
            audioSource.clip = null;
            audioSourceGameObject.SetActive(false);
        }));
    }
}
