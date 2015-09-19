using UnityEngine;
using System.Collections;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/Button/Renderer/gkButtonRenderer_SpriteColor")]
	public class gkButtonRenderer_SpriteColor : gkButtonRenderer
	{	
		public SpriteRenderer spriteRenderer;
		
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
				a_oColor.a = spriteRenderer.color.a;
			}
			
			spriteRenderer.color = a_oColor;
		}
	}
}