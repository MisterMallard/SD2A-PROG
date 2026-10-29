using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
	private static AudioManager instance;

	// READ ONLY (Projectile)
	public static AudioManager Instance
	{
		get { return instance; }
	}

	private void Awake()
	{
		instance = this;
	}

	public void PlaySFX(AudioClip clip, float volume = 1f)
	{
		StartCoroutine(PlaySFXCoroutine(clip, volume));
	}

	IEnumerator PlaySFXCoroutine(AudioClip clip, float volume = 1f)
	{
		AudioSource audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.clip = clip;
		audioSource.volume = volume;
		audioSource.Play();

		yield return new WaitForSeconds(clip.length * 2);

		Destroy(audioSource );
	}
}
