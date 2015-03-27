using UnityEngine;
using System.Collections;
using System;

namespace gk
{
	[AddComponentMenu("GK/Rectangle/gkLineBuilder_RectangleShape")]
	public class gkLineBuilder_RectangleShape : gkLineBuilder
	{
		public gkRectangleShape rectangleShape;
		
		public Color lineColor = Color.cyan;
		
		public Color LineColor
		{
			get
			{
				return lineColor;
			}
			
			set
			{
				if(value != lineColor)
				{
					lineColor = value;
					AskForLineRebuild();
				}
			}
		}
		
		protected override Vector3[] GetLineVertices()
		{
			if(rectangleShape == null)
			{
				return new Vector3[0];
			}
			else
			{
				return rectangleShape.GetLocalVertices().ToArray();
			}
		}
		
		protected override Color GetLineVerticesColor()
		{
			return lineColor;
		}
		
		protected override Vector2 GetLineVerticesUv()
		{
			return Vector2.zero;
		}
		
		protected override void UpdateLineVertexFormat()
		{
			m_eLineVertexFormat.colorFormat = ELineComponentFormat.Uniform;
			m_eLineVertexFormat.uvFormat = ELineComponentFormat.Uniform;
		}
	}
}