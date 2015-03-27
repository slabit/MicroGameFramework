using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Transition/Volume/gkVolumeTransition_AudioSource")]
	public class gkVolumeTransition_AudioSource : gkFloatInterpolation 
	{ 
		public float volumeBase = 1.0f;

		public List<AudioSource> audioSources = new List<AudioSource>();
		
		public bool manageActivation;

		float Volume
		{
			get
			{
				return volumeBase * interpolation.InterpolatedValue;
			}
		}

		public static gkVolumeTransition_AudioSource CreateTransition(AudioSource a_oAudioSource, GameObject a_rGameObject, bool a_bManageActivation = false)
		{
			gkVolumeTransition_AudioSource rInterpolation = gkTransition.CreateTransition<gkVolumeTransition_AudioSource>(a_rGameObject);
			
			rInterpolation.audioSources.Add(a_oAudioSource);
			rInterpolation.manageActivation = a_bManageActivation;
			
			return rInterpolation;
		}
		
		public static gkVolumeTransition_AudioSource CreateTransition(List<AudioSource> a_oAudioSources, GameObject a_rGameObject, bool a_bManageActivation = false)
		{
			gkVolumeTransition_AudioSource rInterpolation = gkFloatInterpolation.CreateTransition<gkVolumeTransition_AudioSource>(a_rGameObject);
			
			rInterpolation.audioSources.AddRange(a_oAudioSources);
			rInterpolation.manageActivation = a_bManageActivation;
			
			return rInterpolation;
		}

		protected override void OnPlayTransition()
		{
			if(manageActivation)
				ActivateAudioSources(true);
		}
		
		protected override void OnStopTransition()
		{
			if(manageActivation && TransitionPercent <= 0.0f)
				ActivateAudioSources(false);
		}

		protected override void OnInterpolatedValueChange(float a_fInterpolatedValue)
		{
			UpdateVolume();
		}
		
		private void ActivateAudioSources(bool a_bActivate)
		{
			foreach(AudioSource rAudioSource in audioSources)
			{
				rAudioSource.enabled = a_bActivate;
			}
		}

		void UpdateVolume()
		{
			float volume = Volume;
			
			foreach(AudioSource rAudioSource in audioSources)
			{
				rAudioSource.volume = volume;
			}
		}

		void LateUpdate()
		{
			UpdateVolume();
		}
	}
}