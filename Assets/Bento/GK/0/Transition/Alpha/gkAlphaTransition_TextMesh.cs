using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Transition/Alpha/gkAlphaTransition_TextMesh")]
	public class gkAlphaTransition_TextMesh : gkFloatInterpolation 
	{ 
		public List<TextMesh> textMeshes = new List<TextMesh>();
		
		public bool manageActivation;
		
		public static gkAlphaTransition_TextMesh CreateTransition(TextMesh a_oTextMesh, GameObject a_rGameObject, bool a_bManageActivation = false)
		{
			gkAlphaTransition_TextMesh rInterpolation = gkTransition.CreateTransition<gkAlphaTransition_TextMesh>(a_rGameObject);
			
			rInterpolation.textMeshes.Add(a_oTextMesh);
			rInterpolation.manageActivation = a_bManageActivation;
			
			return rInterpolation;
		}
		
		public static gkAlphaTransition_TextMesh CreateTransition(List<TextMesh> a_oTextMeshes, GameObject a_rGameObject, bool a_bManageActivation = false)
		{
			gkAlphaTransition_TextMesh rInterpolation = gkFloatInterpolation.CreateTransition<gkAlphaTransition_TextMesh>(a_rGameObject);
			
			rInterpolation.textMeshes.AddRange(a_oTextMeshes);
			rInterpolation.manageActivation = a_bManageActivation;
			
			return rInterpolation;
		}
		
		protected override void OnInterpolatedValueChange(float a_fInterpolatedValue)
		{
			if(manageActivation)
			{
				ActivateTextMeshes(a_fInterpolatedValue > 0.0f);
			}
			
			if(textMeshes != null)
			{
				foreach(TextMesh rTextMesh in textMeshes)
				{
					rTextMesh.color = gkColorUtility.SetAlpha(rTextMesh.color, a_fInterpolatedValue);
				}
			}
		}
		
		private void ActivateTextMeshes(bool a_bActivate)
		{
			foreach(TextMesh rTextMesh in textMeshes)
			{
				rTextMesh.GetComponent<Renderer>().enabled = a_bActivate;
			}
		}
	}
}