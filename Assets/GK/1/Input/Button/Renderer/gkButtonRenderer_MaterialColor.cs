using UnityEngine;
using System.Collections;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/Button/Renderer/gkButtonRenderer_MaterialColor")]
	public class gkButtonRenderer_MaterialColor : gkButtonRenderer
	{	
		public Renderer spriteRenderer;
		
		public Color colorUp = Color.white;
		
		public Color colorDown = Color.black;
		
		public bool dontControlAlpha;
		
		private void Start()
		{
			if(spriteRenderer == null)
			{
				spriteRenderer = GetComponent<SpriteRenderer>();
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
				a_oColor.a = spriteRenderer.material.color.a;
			}
			
			spriteRenderer.material.color = a_oColor;
		}
	}
}