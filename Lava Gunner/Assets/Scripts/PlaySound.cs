using UnityEngine;

public class PlaySound : MonoBehaviour
{
    private AudioSource source;

	private void Start()
	{
		source = GetComponent<AudioSource>();
	}

	public void Play(AudioClip soundClip)
	{
		source.PlayOneShot(soundClip);
	}
}
