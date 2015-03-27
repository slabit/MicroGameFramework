using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace OneButtonPong
{
	[AddComponentMenu("OneButtonPong/SoundManager")]
	public class SoundManager : MonoBehaviour
	{
		public AudioSource soundAudioSource;

		public void PlaySound(AudioClip clip)
		{
			soundAudioSource.PlayOneShot(clip);
		}

		public void PlaySound(AudioClip clip, AudioSource audioSource)
		{
			audioSource.PlayOneShot(clip);
		}

		public void PlaySound(AudioClip clip, AudioSource audioSource, float volumeScale)
		{
			audioSource.PlayOneShot(clip, volumeScale);
		}
	}
}