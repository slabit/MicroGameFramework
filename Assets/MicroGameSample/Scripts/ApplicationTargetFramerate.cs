using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace OneButtonPong
{
	[AddComponentMenu("OneButtonPong/ApplicationTargetFramerate")]
	public class ApplicationTargetFramerate : MonoBehaviour
	{
		public int targetFramerate = 1000;

		void Awake()
		{
			Application.targetFrameRate = targetFramerate;
		}
	}
}