using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.UI;

namespace gk
{
	[ExecuteInEditMode()]
	[AddComponentMenu("GK/UI/CanvasRendererAlpha")]
	public class CanvasRendererAlpha : MonoBehaviour
	{
		public float alpha = 1.0f;
		
		CanvasRenderer canvasRenderer;
		
		void Awake()
		{
			if(Application.isPlaying)
			{
				canvasRenderer = GetComponent<CanvasRenderer>();
				
				if(canvasRenderer == null)
					Destroy(this);
			}
		}
		
		void LateUpdate()
		{
			if(Application.isPlaying == false)
			{
				canvasRenderer = GetComponent<CanvasRenderer>();
				if(canvasRenderer == null)
					return;
			}

			canvasRenderer.SetAlpha(alpha);
		}
	}
}