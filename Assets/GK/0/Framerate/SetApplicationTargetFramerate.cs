using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/SetApplicationTargetFramerate")]
	public class SetApplicationTargetFramerate : MonoBehaviour
	{
		public int targetFramerate = 120;

		void Awake()
		{
			Application.targetFrameRate = targetFramerate;
		}
	}
}