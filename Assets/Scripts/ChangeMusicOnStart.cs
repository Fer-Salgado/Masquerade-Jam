using UnityEngine;

public class ChangeMusicOnStart : MonoBehaviour
{
    public AudioClip musicaDelNivel;

    void Start()
    {
        // Buscamos al "viajero" que vino del menú y le damos la nueva canción
        if (MusicManager.instance != null)
        {
            MusicManager.instance.PlayBackgroundMusic(true, musicaDelNivel);
        }
    }
}