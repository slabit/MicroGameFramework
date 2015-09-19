using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace gk
{
	[AddComponentMenu("GK/Quality/DestroyOnLowEnd")]
	public class DestroyOnLowEnd : MonoBehaviour
	{	
		void Awake()
		{
			if(Quality.LowEnd)
			{
				Destroy(gameObject);
			}
		}
	}
}