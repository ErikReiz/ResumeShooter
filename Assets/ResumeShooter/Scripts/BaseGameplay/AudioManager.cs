using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	[System.Serializable]
	public struct Audio
	{
		[SerializeField] public AudioClip Clip;
		[SerializeField] public float Volume;
	}

	#region FIELDS
	GameObject audioObject;
	#endregion

	private void Start()
	{
		audioObject = GameObject.FindGameObjectWithTag("AudioPoint");
	}

	public void PlaySound(Audio sound)
	{
		if (!sound.Clip) { return; }

		GameObject audio = new GameObject(sound.ToString());
		if(audioObject)
			audio.transform.parent = audioObject.transform;

		AudioSource audioSource = audio.AddComponent<AudioSource>();
		audioSource.volume = sound.Volume;

		audioSource.PlayOneShot(sound.Clip);
		DestroySoundWhenEnded(audioSource);
	}

	private IEnumerator DestroySoundWhenEnded(AudioSource source)
	{
		yield return new WaitUntil(() => source.isPlaying);

		Destroy(source.gameObject);
	}
}
