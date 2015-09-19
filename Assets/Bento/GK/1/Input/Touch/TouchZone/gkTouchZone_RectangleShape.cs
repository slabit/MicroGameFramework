using UnityEngine;
using System.Collections;
using System;

namespace gk
{
	[AddComponentMenu("GK/Input/Touch/TouchZone/gkTouchZone_RectangleShape")]
	public class gkTouchZone_RectangleShape : gkTouchZoneBase
	{
		public gkRectangleShape rectangleShape;
		
		public bool invertZone;

		public bool Project(Vector2 a_f2ScreenPoint, out Vector3 projectionInWorldSpace)
		{
			return rectangleShape.Project(GetCamera(), a_f2ScreenPoint, out projectionInWorldSpace);
		}


		protected override bool _ContainsScreenPoint(Vector2 a_f2ScreenPoint, Camera a_rCamera)
		{
			bool bInRectangle = rectangleShape.Contains(a_rCamera, a_f2ScreenPoint);
			if(invertZone)
			{
				return !bInRectangle;
			}
			else
			{
				return bInRectangle;
			}
		}
	}
}