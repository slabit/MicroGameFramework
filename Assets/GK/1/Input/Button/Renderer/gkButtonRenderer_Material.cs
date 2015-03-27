using UnityEngine;
using System.Collections;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/Button/Renderer/gkButtonRenderer_Material")]
	public class gkButtonRenderer_Material : gkButtonRenderer
	{
		public Renderer buttonRenderer;
		
		public Material up;
		
		public Material down;
		
		protected override void SetUp()
		{
			buttonRenderer.material = up;
		}
		
		protected override void SetDown()
		{
			buttonRenderer.material = down;
		}
	}
}