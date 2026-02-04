using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    private AudioSource audioSource;
    public AudioClip backgroundMusic;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (backgroundMusic != null)
        {
            PlayBackgroundMusic(true, backgroundMusic);
        }
    }

    public void PlayBackgroundMusic(bool resetSong, AudioClip audioClip = null)
    {
        if (audioSource == null)
        {
            Debug.LogError("¡Falta el AudioSource en el MusicManager!");
            return;
        }

        // Si pasamos un clip nuevo
        if (audioClip != null)
        {
            // Evitamos reiniciar la canción si ya es la que está sonando
            if (audioSource.clip == audioClip && audioSource.isPlaying && !resetSong)
            {
                return;
            }

            Debug.Log("Reproduciendo: " + audioClip.name);
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        // Si no pasamos clip, pero ya hay uno cargado en el AudioSource
        else if (audioSource.clip != null)
        {
            if (resetSong) audioSource.Stop();
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Se intentó tocar música pero no hay Clip asignado.");
        }
    }
}