using UnityEngine;
using System.Collections;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/Touch/TouchZone/gkTouchZone_ScreenRectangle")]
	public class gkTouchZone_ScreenRectangle : gkTouchZoneBase
	{
		public gkScreenRectangle screenRectangle;
		
		public Color gizmoColor = Color.red;
		
		protected override bool _ContainsScreenPoint(Vector2 a_f2ScreenPoint, Camera a_rCamera)
		{
			return GetScreenRectangle(a_rCamera).Contains(a_f2ScreenPoint);
		}
		
		protected override void DisplayGizmos(Camera a_rCamera)
		{
			gkGizmoUtility.DrawScreenRectangleGizmo(GetScreenRectangle(a_rCamera), gizmoColor, a_rCamera);
		}
				
		private Rect GetScreenRectangle(Camera a_rCamera)
		{
			if(screenRectangle == null)
			{
				return new Rect(0,0,0,0);
			}
			else
			{
				return screenRectangle.GetScreenRectangleInPixel(a_rCamera);
			}
		}
	}
}