using UnityEngine.Audio;
using System;
using UnityEngine;
using Unity.VisualScripting;


public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManager instance;

    public AudioMixer _mixer;

    private void Awake()
    {
        // Asegurar que solo haya una instancia de DataManager en el juego
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = _mixer.FindMatchingGroups("Master")[0];


            //s.source.outputAudioMixerGroup.audioMixer(s.mixer);
        }
    }


    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + " not found!");
            return;
        }
        s.source.Play();

    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + " not found!");
            return;
        }
        s.source.Stop();

    }

    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + " not found!");
            return;
        }
        s.source.Pause();

    }
    public void Unpause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + " not found!");
            return;
        }
        s.source.UnPause();

    }

    public void Mute()
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = 0.0f; // Establece el volumen de cada sonido a 0 para silenciarlo
        }
    }
    public void Unmute()
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume; // Restaura el volumen original de cada sonido
        }
    }
}
