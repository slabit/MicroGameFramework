using UnityEngine;
using System.Collections;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/Button/Renderer/gkButtonRenderer_TextMeshColor")]
	public class gkButtonRenderer_TextMeshColor : gkButtonRenderer
	{
		public gkTextMeshColor textMeshColor;
		
		public Color up;
		
		public Color down;
		
		public bool dontControlAlpha;
		
		protected override void SetUp()
		{
			SetColor(up);
		}
		
		protected override void SetDown()
		{
			SetColor(down);
		}
		
		private void SetColor(Color a_oColor)
		{
			if(dontControlAlpha)
			{
				a_oColor.a = textMeshColor.Color.a;
			}
			
			textMeshColor.Color =  a_oColor;
		}
	}
}