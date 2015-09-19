using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace gk
{
	[Serializable]
	public class gkSpriteContainer
	{
		public Sprite sprite;
		
		public Texture2D texture;
		
		public static gkSpriteContainer Create(Texture2D a_rTexture)
		{
			return Create(a_rTexture, a_rTexture.name);
		}
		
		public static gkSpriteContainer Create(Texture2D a_rTexture, string a_oName)
		{
			gkSpriteContainer oThumbnail = new gkSpriteContainer();
			if(a_rTexture == null)
			{
				a_rTexture = new Texture2D(1,1, TextureFormat.ARGB32, false);
				a_rTexture.SetPixel(0,0, new Color(1,1,1,1));
				a_rTexture.Apply();
			}
			oThumbnail.texture = a_rTexture;
			oThumbnail.texture.name = a_oName;
			
			oThumbnail.sprite = Sprite.Create(a_rTexture, new Rect(0, 0, a_rTexture.width, a_rTexture.height), Vector2.one * 0.5f);
			oThumbnail.sprite.name = a_oName;
			
			return oThumbnail;
		}
		
		public static void Destroy(gkSpriteContainer a_rThumbnail)
		{
			UnityEngine.Object.Destroy(a_rThumbnail.sprite);
			UnityEngine.Object.Destroy(a_rThumbnail.texture);
		}
	}
}