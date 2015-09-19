using UnityEngine;
using System.Collections;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/Touch/TouchZone/gkTouchZone_TransformedPolygon")]
	public class gkTouchZone_TransformedPolygon : gkTouchZoneBase
	{
		public gkPolygon2DWithTransform polygon = new gkPolygon2DWithTransform(EPolygonPrimitive.Square);
		
		public Color gizmoColor = Color.red;
		
		protected override bool _ContainsScreenPoint(Vector2 a_f2ScreenPoint, Camera a_rCamera)
		{
			return polygon.ContainsAsConvex(a_f2ScreenPoint, a_rCamera);
		}
		
		protected override void DisplayGizmos(Camera a_rCamera)
		{
			if(polygon != null)
			{
				polygon.DisplayLinesGizmos(gizmoColor);
			}
		}
	}
}