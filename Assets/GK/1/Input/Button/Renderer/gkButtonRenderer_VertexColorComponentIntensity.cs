using UnityEngine;
using System.Collections;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/Button/Renderer/gkButtonRenderer_VertexColorComponentIntensity")]
	public class gkButtonRenderer_VertexColorComponentIntensity : gkButtonRenderer
	{	
		public float intensityUp = 1.0f;
		
		public float intensityDown = 0.75f;
		
		private gkVertexColor m_rButtonVertexColor;
		
		private void Awake()
		{
			m_rButtonVertexColor = GetComponent<gkVertexColor>();
		}
		
		protected override void SetUp()
		{
			m_rButtonVertexColor.Intensity =  intensityUp;
		}
		
		protected override void SetDown()
		{
			m_rButtonVertexColor.Intensity =  intensityDown;
		}
	}
}