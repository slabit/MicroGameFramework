using UnityEngine;
using System.Collections;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/Button/Renderer/gkButtonRenderer_Texture")]
	public class gkButtonRenderer_Texture : gkButtonRenderer
	{	
		public Renderer spriteRenderer;
		
		public Texture spriteUp;
		
		public Texture spriteDown;
		
		private void Start()
		{
			if(spriteRenderer == null)
			{
				spriteRenderer = GetComponent<SpriteRenderer>();
			}
		}
		
		protected override void SetUp()
		{
			SetSprite(spriteUp);
		}
		
		protected override void SetDown()
		{
			SetSprite(spriteDown);
		}
		
		private void SetSprite(Texture a_rSprite)
		{
			spriteRenderer.material.mainTexture = a_rSprite;
		}
	}
}