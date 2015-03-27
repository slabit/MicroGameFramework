using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Transition/Alpha/gkAlphaTransition_RendererMaterial")]
	public class gkAlphaTransition_RendererMaterial : gkFloatInterpolation 
	{ 
		public List<Renderer> renderers = new List<Renderer>();
		
		public bool sameMaterialForAllRenderers;
		
		public bool manageActivation;
		
		private Material m_oSharedMaterialDuplicate;
		
		public static gkAlphaTransition_RendererMaterial CreateTransition(Renderer a_oRenderer, bool a_bSameMaterialForAllRenderers, GameObject a_rGameObject, bool a_bManageActivation = false)
		{
			gkAlphaTransition_RendererMaterial rInterpolation = gkTransition.CreateTransition<gkAlphaTransition_RendererMaterial>(a_rGameObject);
			
			rInterpolation.renderers.Add(a_oRenderer);
			rInterpolation.sameMaterialForAllRenderers = a_bSameMaterialForAllRenderers;
			rInterpolation.manageActivation = a_bManageActivation;
			
			return rInterpolation;
		}
		
		public static gkAlphaTransition_RendererMaterial CreateTransition(List<Renderer> a_oRenderers, bool a_bSameMaterialForAllRenderers, GameObject a_rGameObject, bool a_bManageActivation = false)
		{
			gkAlphaTransition_RendererMaterial rInterpolation = gkFloatInterpolation.CreateTransition<gkAlphaTransition_RendererMaterial>(a_rGameObject);
			
			rInterpolation.renderers.AddRange(a_oRenderers);
			rInterpolation.sameMaterialForAllRenderers = a_bSameMaterialForAllRenderers;
			rInterpolation.manageActivation = a_bManageActivation;
			
			return rInterpolation;
		}
		
		protected override void BeforeInitializeTransition()
		{
			if(sameMaterialForAllRenderers)
			{
				if(renderers != null && renderers.Count > 0)
				{
					m_oSharedMaterialDuplicate = renderers[0].material;
					
					foreach(Renderer rRenderer in renderers)
					{
						rRenderer.sharedMaterial = m_oSharedMaterialDuplicate;
					}
				}
			}
		}
		
		protected override void OnInterpolatedValueChange(float a_fInterpolatedValue)
		{
			if(manageActivation)
			{
				ActivateRenderers(a_fInterpolatedValue > 0.0f);
			}
			
			if(sameMaterialForAllRenderers && m_oSharedMaterialDuplicate != null)
			{
				gkColorUtility.SetAlpha(m_oSharedMaterialDuplicate, a_fInterpolatedValue);
			}
			else if(renderers != null)
			{
				foreach(Renderer rRenderer in renderers)
				{
					gkColorUtility.SetAlpha(rRenderer, a_fInterpolatedValue);
				}
			}
		}
		
		private void ActivateRenderers(bool a_bActivate)
		{
			foreach(Renderer rRenderer in renderers)
			{
				rRenderer.enabled = a_bActivate;
			}
		}
	}
}