using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Transition/Alpha/gkAlphaTransition_TextMeshColor")]
	public class gkAlphaTransition_TextMeshColor : gkFloatInterpolation 
	{ 
		public List<gkTextMeshColor> textMeshColors = new List<gkTextMeshColor>();
		
		public bool manageActivation;
		
		public static gkAlphaTransition_TextMeshColor CreateTransition(gkTextMeshColor a_rTextMeshColor, GameObject a_rGameObject, bool a_bManageActivation = false)
		{
			gkAlphaTransition_TextMeshColor rTransition = gkTransition.CreateTransition<gkAlphaTransition_TextMeshColor>(a_rGameObject);
			
			rTransition.textMeshColors.Add(a_rTextMeshColor);
			rTransition.manageActivation = a_bManageActivation;
			
			return rTransition;
		}
		
		public static gkAlphaTransition_TextMeshColor CreateTransition(List<gkTextMeshColor> a_oTextMeshColors, GameObject a_rGameObject, bool a_bManageActivation = false)
		{
			gkAlphaTransition_TextMeshColor rTransition = gkTransition.CreateTransition<gkAlphaTransition_TextMeshColor>(a_rGameObject);
			
			rTransition.textMeshColors.AddRange(a_oTextMeshColors);
			rTransition.manageActivation = a_bManageActivation;
			
			return rTransition;
		}
		
		protected override void OnInterpolatedValueChange(float a_fInterpolatedValue)
		{
			if(manageActivation)
			{
				ActivateRenderers(a_fInterpolatedValue > 0.0f);
			}
			
			foreach(gkTextMeshColor rTextMeshColor in textMeshColors)
			{
				rTextMeshColor.Color = gkColorUtility.SetAlpha(rTextMeshColor.Color, a_fInterpolatedValue);
			}
		}
		
		private void ActivateRenderers(bool a_bActivate)
		{
			foreach(gkTextMeshColor rTextMeshColor in textMeshColors)
			{
				rTextMeshColor.GetComponent<Renderer>().enabled = a_bActivate;
			}
		}
	}
}