using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Transition/Alpha/gkAlphaTransition_SpriteRenderer")]
	public class gkAlphaTransition_SpriteRenderer : gkFloatInterpolation 
	{ 
		public List<SpriteRenderer> sprites = new List<SpriteRenderer>();
		
		public bool manageActivation;
		
		public static gkAlphaTransition_SpriteRenderer CreateTransition(SpriteRenderer a_oSpriteRenderer, GameObject a_rGameObject, bool a_bManageActivation = false)
		{
			gkAlphaTransition_SpriteRenderer rInterpolation = gkTransition.CreateTransition<gkAlphaTransition_SpriteRenderer>(a_rGameObject);
			
			rInterpolation.sprites.Add(a_oSpriteRenderer);
			rInterpolation.manageActivation = a_bManageActivation;
			
			return rInterpolation;
		}
		
		public static gkAlphaTransition_SpriteRenderer CreateTransition(List<SpriteRenderer> oSpriteRenderers, GameObject a_rGameObject, bool a_bManageActivation = false)
		{
			gkAlphaTransition_SpriteRenderer rInterpolation = gkFloatInterpolation.CreateTransition<gkAlphaTransition_SpriteRenderer>(a_rGameObject);
			
			rInterpolation.sprites.AddRange(oSpriteRenderers);
			rInterpolation.manageActivation = a_bManageActivation;
			
			return rInterpolation;
		}
		
		protected override void OnInterpolatedValueChange(float a_fInterpolatedValue)
		{
			if(manageActivation)
			{
				ActivateSpriteRendereres(a_fInterpolatedValue > 0.0f);
			}
			
			if(sprites != null)
			{
				foreach(SpriteRenderer rSpriteRenderer in sprites)
				{
					rSpriteRenderer.color = gkColorUtility.SetAlpha(rSpriteRenderer.color, a_fInterpolatedValue);
				}
			}
		}
		
		private void ActivateSpriteRendereres(bool a_bActivate)
		{
			foreach(SpriteRenderer rSpriteRenderer in sprites)
			{
				rSpriteRenderer.enabled = a_bActivate;
			}
		}
	}
}