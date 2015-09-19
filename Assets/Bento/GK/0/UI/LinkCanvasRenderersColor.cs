using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.UI;

namespace gk
{
	[AddComponentMenu("GK/UI/LinkCanvasRenderersColor")]
	public class LinkCanvasRenderersColor : MonoBehaviour
	{
		public CanvasRenderer canvasFrom;

		public CanvasRenderer canvasTo;

		void LateUpdate()
		{
			UpdateLink();
		}

		void UpdateLink()
		{
			canvasTo.SetColor(canvasFrom.GetColor());
		}
	}
}