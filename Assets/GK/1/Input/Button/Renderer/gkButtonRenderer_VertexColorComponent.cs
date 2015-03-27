using UnityEngine;
using System.Collections;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/Button/Renderer/gkButtonRenderer_VertexColorComponentIntensity")]
	public class gkButtonRenderer_VertexColorComponent : gkButtonRenderer
	{	
		public Color colorUp = Color.white;
		
		public Color colorDown = Color.black;
		
		public bool dontControlAlpha;
		
		private gkVertexColor m_rButtonVertexColor;
		
		private void Awake()
		{
			m_rButtonVertexColor = GetComponent<gkVertexColor>();
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
				a_oColor.a = m_rButtonVertexColor.Alpha;
			}
			
			m_rButtonVertexColor.VertexColor =  a_oColor;
		}
	}
}