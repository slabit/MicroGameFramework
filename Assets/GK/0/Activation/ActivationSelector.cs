using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[ExecuteInEditMode()]
	[AddComponentMenu("GK/ActivationSelector")]
	public class ActivationSelector : MonoBehaviour
	{
		[SerializeField]
		private int selectedIndex;

		public bool animateIndex;
		public float animatedIndex;

		public List<ActivationGroup> activationGroups = new List<ActivationGroup>();

		public int SelectedIndex
		{
			get
			{
				return selectedIndex;
			}

			set
			{
				int clampedValue = ClampIndex(value);
				if(clampedValue != selectedIndex)
				{
					selectedIndex = value;
					UpdateActivation();
				}
			}
		}

		void Awake()
		{
			selectedIndex = ClampIndex(selectedIndex);
			UpdateActivation();
		}

		void LateUpdate()
		{
			if(Application.isPlaying == false)
			{
				if(animateIndex)
				{
					SelectedIndex = Mathf.FloorToInt(animatedIndex);
				}
				else
				{
					selectedIndex = ClampIndex(selectedIndex);
					UpdateActivation();
				}
			}
			else
			{
				if(animateIndex)
				{
					SelectedIndex = Mathf.FloorToInt(animatedIndex);
				}
			}
		}

		int ClampIndex(int index)
		{
			return Mathf.Clamp(index, 0, activationGroups.Count - 1);
		}

		void UpdateActivation()
		{
			for(int i = 0; i < activationGroups.Count; ++i)
			{
				activationGroups[i].Activate(i == SelectedIndex);
			}
		}
	}
}