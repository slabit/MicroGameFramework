using UnityEngine;
using System.Collections;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/Touch/TouchZone/gkTouchZone_TransformedRectangle")]
	public class gkTouchZone_TransformedRectangle : gkTouchZoneBase
	{
		public gkRectangle rectangle;
		
		public Color gizmoColor = Color.red;
		
		protected override bool _ContainsScreenPoint(Vector2 a_f2ScreenPoint, Camera a_rCamera)
		{
			return rectangle.Contains(transform, a_rCamera, a_f2ScreenPoint);
		}
		
		protected override void DisplayGizmos(Camera a_rCamera)
		{
			if(rectangle != null)
			{
				rectangle.DisplayLinesGizmos(transform, gizmoColor);
			}
		}
	}
}