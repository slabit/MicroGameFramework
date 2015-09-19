using UnityEngine;
using System.Collections;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/Button/Renderer/gkButtonRenderer_SpriteTexture")]
	public class gkButtonRenderer_SpriteTexture : gkButtonRenderer
	{	
		public SpriteRenderer spriteRenderer;
		
		public Sprite spriteUp;
		
		public Sprite spriteDown;
		
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
		
		private void SetSprite(Sprite a_rSprite)
		{
			spriteRenderer.sprite = a_rSprite;
		}
	}
}