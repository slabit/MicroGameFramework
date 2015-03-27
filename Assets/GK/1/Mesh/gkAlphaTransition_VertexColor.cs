using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[AddComponentMenu("GK/Transition/Alpha/gkAlphaTransition_VertexColor")]
	public class gkAlphaTransition_VertexColor : gkFloatInterpolation 
	{ 
		public List<gkVertexColor> vertexColors = new List<gkVertexColor>();
		
		public bool manageActivation;
		
		public static gkAlphaTransition_VertexColor CreateTransition(gkVertexColor a_rVertexColor, GameObject a_rGameObject, bool a_bManageActivation = false)
		{
			gkAlphaTransition_VertexColor rTransition = gkTransition.CreateTransition<gkAlphaTransition_VertexColor>(a_rGameObject);
			
			rTransition.vertexColors.Add(a_rVertexColor);
			rTransition.manageActivation = a_bManageActivation;
			
			return rTransition;
		}
		
		public static gkAlphaTransition_VertexColor CreateTransition(List<gkVertexColor> a_oVertexColors, GameObject a_rGameObject, bool a_bManageActivation = false)
		{
			gkAlphaTransition_VertexColor rTransition = gkTransition.CreateTransition<gkAlphaTransition_VertexColor>(a_rGameObject);
			
			rTransition.vertexColors.AddRange(a_oVertexColors);
			rTransition.manageActivation = a_bManageActivation;
			
			return rTransition;
		}
		
		protected override void OnInterpolatedValueChange(float a_fInterpolatedValue)
		{
			if(manageActivation)
			{
				ActivateRenderers(a_fInterpolatedValue > 0.0f);
			}
			
			foreach(gkVertexColor rVertexColor in vertexColors)
			{
				rVertexColor.VertexColor = gkColorUtility.SetAlpha(rVertexColor.VertexColor, a_fInterpolatedValue);
			}
		}
		
		private void ActivateRenderers(bool a_bActivate)
		{
			foreach(gkVertexColor rVertexColor in vertexColors)
			{
				rVertexColor.GetComponent<Renderer>().enabled = a_bActivate;
			}
		}
	}
}
