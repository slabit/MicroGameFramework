using UnityEngine;
using System.Collections;

namespace gk
{
	[System.Serializable]
	public class gkScreenRectangle
	{
		public Rect rectangle;
		
		public EScreenScalarUnit unit;
		
		public EScreenDocking docking;
			
		public Rect GetLocalScreenRectangleInPixel(Camera a_rCamera)
		{
			return gkScreenRectangleUtility.GetLocalScreenRectangleInPixel(rectangle, unit, a_rCamera);
		}
		
		public Rect GetLocalScreenRectangleInPixel(Rect a_oNormalizeViewport)
		{
			return gkScreenRectangleUtility.GetLocalScreenRectangleInPixel(rectangle, unit, a_oNormalizeViewport);
		}
		
		public Rect GetScreenRectangleInPixel(Camera a_rCamera)
		{
			return gkScreenRectangleUtility.GetScreenRectangleInPixel(rectangle, unit, docking, a_rCamera);
		}
		
		public Rect GetScreenRectangleInPixel(Rect a_oNormalizeViewport)
		{
			return gkScreenRectangleUtility.GetScreenRectangleInPixel(rectangle, unit, docking, a_oNormalizeViewport);
		}
	}
}