using UnityEngine;
using System.Collections;

namespace gk
{
	[System.Serializable]
	public class gkScreenVector2
	{
		public Vector2 vector;
		
		public EScreenScalarUnit unit;
		
		public EScreenDocking docking;
		
		public Vector2 GetScreenVectorInPixel(Camera a_rCamera)
		{
			return gkScreenVector2Utility.GetScreenVectorInPixel(vector, unit, a_rCamera);
		}
		
		public Vector2 GetScreenVectorInPixel(Rect a_oNormalizedViewportRectangle)
		{
			return gkScreenVector2Utility.GetScreenVectorInPixel(vector, unit, a_oNormalizedViewportRectangle);
		}
		
		public Vector2 GetScreenPointInPixel(Camera a_rCamera)
		{
			return gkScreenVector2Utility.GetScreenPointInPixel(vector, unit, docking, a_rCamera);
		}
		
		public Vector2 GetScreenPointInPixel(Rect a_oNormalizedViewportRectangle)
		{
			return gkScreenVector2Utility.GetScreenPointInPixel(vector, unit, docking, a_oNormalizedViewportRectangle);
		}
	}
}