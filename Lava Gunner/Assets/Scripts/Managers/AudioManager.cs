using UnityEngine.Audio;
using UnityEngine;

using System;
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public const string BGM = "bgm";

    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sound sound in sounds)
        {
            // create audio source in the audio manager for each sound object
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.audioClip;

            // init audio source params with sound object vals
            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;
            sound.audioSource.loop = sound.loop;
        }

        Play(BGM);
    }


    public void PlayBGM(int pitchLevel)
    {
        SetPitch(BGM, Mathf.Min(2, 1f + 0.1f * pitchLevel));
    }

    public void SetPitch(string name, float pitch)
    {
        Sound s = GetSound(name);
        s.pitch = pitch;
        s.audioSource.pitch = pitch;
    }

    // Update is called once per frame
    public void Play(string name)
    {
        Sound s = GetSound(name);
        s.audioSource.Play();
    }

    private Sound GetSound(String name)
    {
        return Array.Find<Sound>(sounds, sound => sound.name == name);
    }
}
