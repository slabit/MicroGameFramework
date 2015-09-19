using UnityEngine;
using System.Collections;

namespace gk
{
	public static class gkScreenVector2Utility
	{	
		public static Vector2 GetScreenVectorInPixel(Vector2 a_f2Value, EScreenScalarUnit a_eUnit, Camera a_rCamera)
		{
			return GetScreenVectorInPixel(a_f2Value, a_eUnit, a_rCamera.pixelRect);
		}
		
		public static Vector2 GetScreenVectorInPixel(Vector2 a_f2Value, EScreenScalarUnit a_eUnit, Rect a_oNormalizedViewportRectangle)
		{
			Vector2 oVector2InPixel = Vector2.zero;
			
			oVector2InPixel.x = gkScreenScalarUtility.GetScreenDistanceInPixel(a_f2Value.x, a_eUnit, EScreenScalarUseOrientation.Width, a_oNormalizedViewportRectangle);
			oVector2InPixel.y = gkScreenScalarUtility.GetScreenDistanceInPixel(a_f2Value.y, a_eUnit, EScreenScalarUseOrientation.Height, a_oNormalizedViewportRectangle);
			
			return oVector2InPixel;
		}
		
		public static Vector2 GetScreenPointInPixel(Vector2 a_f2Value, EScreenScalarUnit a_eUnit, EScreenDocking a_eScreenDocking, Camera a_rCamera)
		{
			return GetScreenPointInPixel(a_f2Value, a_eUnit, a_eScreenDocking, a_rCamera.pixelRect);
		}
		
		public static Vector2 GetScreenPointInPixel(Vector2 a_f2Value, EScreenScalarUnit a_eUnit, EScreenDocking a_eScreenDocking, Rect a_oNormalizedViewportRectangle)
		{
			Vector2 oVector2InPixel = Vector2.zero;
			
			// Compute the output vector2
			oVector2InPixel.x = gkScreenScalarUtility.GetScreenPointCoordinateInPixel(a_f2Value.x, a_eUnit, EScreenScalarUseOrientation.Width, a_eScreenDocking, a_oNormalizedViewportRectangle);
			oVector2InPixel.y = gkScreenScalarUtility.GetScreenPointCoordinateInPixel(a_f2Value.y, a_eUnit, EScreenScalarUseOrientation.Height, a_eScreenDocking, a_oNormalizedViewportRectangle);
			
			return oVector2InPixel;
		}
	}
}