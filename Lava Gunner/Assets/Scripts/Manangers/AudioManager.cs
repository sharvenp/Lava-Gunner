using UnityEngine.Audio;
using UnityEngine;

using System;
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
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

        Play("bgm");
    }

    // Update is called once per frame
    public void Play(string name)
    {
        Debug.Log("test");
        Sound s = Array.Find<Sound>(sounds, sound => sound.name == name);
        Debug.Log(s.name);
        s.audioSource.Play();
    }
}
