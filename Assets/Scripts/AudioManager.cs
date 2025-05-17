using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private void Start()
    {
        PlayMusicForCurrentScene();
    }

    private void PlayMusicForCurrentScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Menu")
        {
            PlayMusic("MenuMusic");
        }
        else if (sceneName == "FirstScene") 
        {
            PlayMusic("GameMusic");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        if (scene.name == "Menu")
        {
            PlayMusic("MenuMusic");
        }
        else if (scene.name == "FirstScene") 
        {
            PlayMusic("GameMusic");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public void PlayMusic(string name)
    {
        Sound sound = System.Array.Find(musicSounds, s => s.name == name);
        if (sound == null)
        {
           Debug.Log("Sound not found: " + name);
        }
        else
        {
            musicSource.clip = sound.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound sound = System.Array.Find(sfxSounds, s => s.name == name);
        if (sound == null)
        {
            Debug.Log("Sound not found: " + name);
        }
        else
        {
            sfxSource.PlayOneShot(sound.clip);
        }
    }
    public void StopMusic()
    {
        musicSource.Stop();
    }
}
