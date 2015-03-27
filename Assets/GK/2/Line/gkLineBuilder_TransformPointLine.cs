using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace gk
{
	[AddComponentMenu("GK/Rectangle/gkLineBuilder_TransformPointLine")]
	public class gkLineBuilder_TransformPointLine : gkLineBuilder
	{
		[Serializable]
		public class TransformPoint
		{
			public Transform transform;
			public Color color = Color.white;
			
			public Vector3 LocalPosition
			{
				get
				{
					if(transform == null)	
					{
						return Vector3.zero;	
					}
					else
					{
						return transform.localPosition;
					}
				}
			}
			
			public Vector3 Position
			{
				get
				{
					if(transform == null)	
					{
						return Vector3.zero;	
					}
					else
					{
						return transform.position;
					}
				}
			}
		}
		
		public List<TransformPoint> points;
		
		public bool displayGizmos;
		
		public Color gizmoColor = Color.red;
		
		public float gizmoPointRadius = 0.1f;
		
		protected override Vector3[] GetLineVertices()
		{
			Vector3[] oVertices = new Vector3[points.Count];
			
			int iVertexIndex = 0;
			foreach(TransformPoint rPoint in points)
			{
				oVertices[iVertexIndex] = rPoint.LocalPosition;
				iVertexIndex++;
			}
			
			return oVertices;
		}
		
		protected override Color[] GetLineVerticesColors()
		{
			Color[] oColors = new Color[points.Count];
			
			int iColorIndex = 0;
			foreach(TransformPoint rPoint in points)
			{
				oColors[iColorIndex] = rPoint.color;
				iColorIndex++;
			}
			
			return oColors;
		}
		
		protected override Vector2 GetLineVerticesUv()
		{
			return Vector2.zero;
		}
		
		protected override void UpdateLineVertexFormat()
		{
			m_eLineVertexFormat.colorFormat = ELineComponentFormat.ByVertex;
			m_eLineVertexFormat.uvFormat = ELineComponentFormat.Uniform;
		}
		
		private void OnDrawGizmos()
		{
			if(displayGizmos)
			{
				Color rGizmoColorSave = Gizmos.color;
				Gizmos.color = gizmoColor;
				foreach(TransformPoint rPoint in points)
				{
					Gizmos.DrawSphere(rPoint.Position, gizmoPointRadius);
				}
				Gizmos.color = rGizmoColorSave;
			}
		}
	}
}