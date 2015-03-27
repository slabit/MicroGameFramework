using UnityEngine;
using System.Collections;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/Button/Renderer/gkButtonRenderer_VertexColor")]
	public class gkButtonRenderer_VertexColorIntensity : gkButtonRenderer
	{	
		public float intensityUp = 1.0f;
		
		public float intensityDown = 0.75f;
		
		private MeshFilter m_rButtonMeshFilter;
		
		private void Awake()
		{
			m_rButtonMeshFilter = GetComponent<MeshFilter>();
		}
		
		protected override void SetUp()
		{
			gkMeshColorUtility.SetVertexColorIntensity(m_rButtonMeshFilter.mesh, intensityUp);
		}
		
		protected override void SetDown()
		{
			gkMeshColorUtility.SetVertexColorIntensity(m_rButtonMeshFilter.mesh, intensityDown);
		}
	}
}