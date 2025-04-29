using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] songs;

    public AudioSource audioSource;

    private void Start()
    {
        PlayRandomSong();
        StartCoroutine(WaitForFinish());
    }

    IEnumerator WaitForFinish()
    {
        yield return new WaitForSecondsRealtime(audioSource.clip.length);
        AudioFinished();
    }

    void AudioFinished()
    {
        PlayRandomSong();
        StartCoroutine(WaitForFinish());
    }

    void PlayRandomSong()
    {
        audioSource.clip = songs[Random.Range(0, songs.Length)];
        audioSource.Play();
    }
}
