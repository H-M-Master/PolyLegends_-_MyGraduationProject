using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BGMPlayer : MonoBehaviour
{
    public AudioClip bgmClip;
    public float fadeInDuration = 2f;
    public float fadeOutDuration = 1.5f;
    public float targetVolume = 0.5f;

    private AudioSource audioSource;
    private Coroutine fadeCoroutine;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = bgmClip;
        audioSource.loop = true;
        audioSource.volume = 0f;
        audioSource.playOnAwake = false;
        audioSource.Play();

        fadeCoroutine = StartCoroutine(FadeIn(fadeInDuration));
    }

    void OnDestroy()
    {
        // 场景卸载或物体销毁时自动淡出
        if (audioSource != null && audioSource.isPlaying)
        {
            StartCoroutine(FadeOutAndStop(fadeOutDuration));
        }
    }

    IEnumerator FadeIn(float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            audioSource.volume = Mathf.Lerp(0f, targetVolume, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = targetVolume;
    }

    IEnumerator FadeOutAndStop(float duration)
    {
        float startVolume = audioSource.volume;
        float time = 0f;

        while (time < duration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
    }
}
