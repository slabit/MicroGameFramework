using UnityEngine;
using System.Collections;
using System;

namespace gk
{
	[AddComponentMenu("GK/Polygon/gkPolygonShape")]
	public class gkPolygonShape : MonoBehaviour
	{
		public gkPolygon2DWithTransform polygon = new gkPolygon2DWithTransform(EPolygonPrimitive.Square);
		
		public Color gizmoColor = Color.red;
		
		private void OnDrawGizmos()
		{
			if(polygon != null)
			{
				polygon.DisplayLinesGizmos(gizmoColor);
			}
		}
	}
}