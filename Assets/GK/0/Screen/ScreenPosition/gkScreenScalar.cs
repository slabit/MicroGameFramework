using UnityEngine;
using System.Collections;

namespace gk
{
	[System.Serializable]
	public class gkScreenScalar
	{
		public float scalar;
		
		public EScreenScalarUnit unit;
		
		public EScreenDocking docking;
		
		public float GetScreenPointCoordinateInPixel(EScreenScalarUseOrientation a_eOrientation, Camera a_rCamera)
		{
			return gkScreenScalarUtility.GetScreenPointCoordinateInPixel(scalar, unit, a_eOrientation, docking, a_rCamera.pixelRect);
		}
		
		public float GetScreenPointCoordinateInPixel(EScreenScalarUseOrientation a_eOrientation, Rect a_oNormalizedViewportRectangle)
		{
			return gkScreenScalarUtility.GetScreenPointCoordinateInPixel(scalar, unit, a_eOrientation, docking, a_oNormalizedViewportRectangle);
		}
		
		public float GetScreenDistanceInPixel(EScreenScalarUseOrientation a_eOrientation, Camera a_rCamera)
		{
			return gkScreenScalarUtility.GetScreenDistanceInPixel(scalar, unit, a_eOrientation, a_rCamera.pixelRect);
		}
		
		public float GetScreenDistanceInPixel(EScreenScalarUseOrientation a_eOrientation, Rect a_oNormalizedViewportRectangle)
		{
			return gkScreenScalarUtility.GetScreenDistanceInPixel(scalar, unit, a_eOrientation, a_oNormalizedViewportRectangle);
		}
	}
}