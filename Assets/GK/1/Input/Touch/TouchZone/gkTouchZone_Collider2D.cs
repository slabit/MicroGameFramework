using UnityEngine;
using System.Collections;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/Touch/TouchZone/gkTouchZone_Collider2D")]
	public class gkTouchZone_Collider2D : gkTouchZoneBase
	{
		public Collider2D collider2DComponent;
		
		public bool invertZone;
		
		protected override bool _ContainsScreenPoint(Vector2 a_f2ScreenPoint, Camera a_rCamera)
		{
			Vector2 f2TestPoint;
			if(gk2DUtility.ProjectScreenPointOn2DObjectInWorldSpace(collider2DComponent.transform, a_rCamera, a_f2ScreenPoint, out f2TestPoint))
			{
				bool bIn = collider2DComponent.OverlapPoint(f2TestPoint);
				if(invertZone)
				{
					return !bIn;
				}
				else
				{
					return bIn;
				}
			}
			else
			{
				return false;
			}
		}
	}
}