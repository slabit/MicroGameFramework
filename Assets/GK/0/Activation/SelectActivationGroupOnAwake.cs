using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/SelectActivationGroupOnAwake")]
	public class SelectActivationGroupOnAwake : MonoBehaviour
	{
		public gk.ActivationGroupSelection selection = new gk.ActivationGroupSelection(0);

		void Awake()
		{
			selection.Select();
		}
	}
}