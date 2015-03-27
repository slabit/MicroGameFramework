using UnityEngine;
using System.Collections;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/Button/Renderer/gkButtonRenderer_Material")] 
	public class gkButtonRenderer_VertexColor : gkButtonRenderer
	{
		public gkVertexColor buttonVertexColor;
		
		public Color up;
		
		public Color down;
		
		protected override void SetUp()
		{
			buttonVertexColor.VertexColor = up;
		}
		
		protected override void SetDown()
		{
			buttonVertexColor.VertexColor = down;
		}
	}
}