using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundBGVolume : MonoBehaviour
{
    private AudioSource audioSource;
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }
    /// <summary>
    /// Lower background volume over time to amount given.
    /// </summary>
    /// <param name="duration">How fast to lower volume.</param>
    /// <param name="amount">The amount to lower to.</param>
    public void LowerVolume(float duration,float amount)
    {
        StartCoroutine(LowerVolumeOverTime(duration,amount));
    }
    public void RaiseVolume(float duration)
    {
        StartCoroutine(RaiseVolumeOverTime(duration));
    }

    private IEnumerator LowerVolumeOverTime(float duration,float amount)
    {
        float elapsedTime = 0;
        float startVolume = audioSource.volume;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, amount, elapsedTime / duration);
            yield return null;
        }

        audioSource.Pause();
    }
    private IEnumerator RaiseVolumeOverTime(float duration)
    {
        audioSource.UnPause();
        float elapsedTime = 0;
        float startVolume = audioSource.volume;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, GameManager.Instance.BGMVolume, elapsedTime / duration);
            yield return null;
        }

        audioSource.volume = GameManager.Instance.BGMVolume;
    }
}