using UnityEngine;
using System.Collections;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/Button/Renderer/gkButtonRenderer_TextColor")]
	public class gkButtonRenderer_TextColor : gkButtonRenderer
	{	
		public TextMesh textMesh;
		
		public Color colorUp = Color.white;
		
		public Color colorDown = Color.black;
		
		public bool dontControlAlpha;
		
		private void Start()
		{
			if(textMesh == null)
			{
				textMesh = GetComponent<TextMesh>();
			}
		}
		
		protected override void SetUp()
		{
			SetColor(colorUp);
		}
		
		protected override void SetDown()
		{
			SetColor(colorDown);
		}
		
		private void SetColor(Color a_oColor)
		{
			if(dontControlAlpha)
			{
				a_oColor.a = textMesh.color.a;
			}
			
			textMesh.color = a_oColor;
		}
	}
}