using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	public static class gkRectUtility
	{
		public static Rect ClipRectangle(Rect a_oRectangleToClip, Rect a_oClippingRect)
		{
			Rect oClippedRectangle = a_oRectangleToClip;
			
			// X
			if(a_oRectangleToClip.xMin < a_oClippingRect.xMin)
			{
				oClippedRectangle.xMin = a_oClippingRect.xMin;
			}
			
			if(a_oRectangleToClip.xMax > a_oClippingRect.xMax)
			{
				oClippedRectangle.xMax = a_oClippingRect.xMax;
			}
			
			// Y
			if(a_oRectangleToClip.yMin < a_oClippingRect.yMin)
			{
				oClippedRectangle.yMin = a_oClippingRect.yMin;
			}
			
			if(a_oRectangleToClip.yMax > a_oClippingRect.yMax)
			{
				oClippedRectangle.yMax = a_oClippingRect.yMax;
			}
			
			return oClippedRectangle;
		}
		
		public static Vector2 FitContentSizeIntoContainer(Vector2 a_f2ContentSize, Vector2 a_f2ContainerSize, bool a_bIncreaseSizeToFillContainer)
		{
			Vector2 f2FitSize;
			Vector2 f2FitScale;
			FitContentSizeIntoContainer(a_f2ContentSize, a_f2ContainerSize, a_bIncreaseSizeToFillContainer, out f2FitSize, out f2FitScale);
	
			return f2FitSize; 	
		}
		
		public static void FitContentSizeIntoContainer(Vector2 a_f2ContentSize, Vector2 a_f2ContainerSize, bool a_bIncreaseSizeToFillContainer, out Vector2 a_f2FitSize, out Vector2 a_f2FitScale)
		{
			// Scale to fit
			a_f2FitScale.x = a_f2ContainerSize.x / a_f2ContentSize.x;
			a_f2FitScale.y = a_f2ContainerSize.y / a_f2ContentSize.y;
			
			// Keep the ratio
			float fScaleToFit = Mathf.Min(a_f2FitScale.x, a_f2FitScale.y);
			
			if(a_bIncreaseSizeToFillContainer == false)
			{
				fScaleToFit = Mathf.Clamp(fScaleToFit, -1.0f, 1.0f);
			}
			
			a_f2FitScale.x = fScaleToFit;
			a_f2FitScale.y = fScaleToFit;
			
			// Compute fit size
			if(float.IsInfinity(fScaleToFit))
			{
				a_f2FitSize = a_f2ContainerSize;
			}
			else
			{
				a_f2FitSize.x = a_f2FitScale.x * a_f2ContentSize.x;
				a_f2FitSize.y = a_f2FitScale.y * a_f2ContentSize.y;
			}
		}
		
		public static bool AreRectOverlapping(Rect a_oRectA, Rect a_oRectB, float a_fTolerance = 0.0f)
		{
			return a_oRectA.xMin < a_oRectB.xMax + a_fTolerance && a_oRectA.xMax > a_oRectB.xMin - a_fTolerance
				&& a_oRectA.yMin < a_oRectB.yMax + a_fTolerance && a_oRectA.yMax > a_oRectB.yMin - a_fTolerance;
		}
		
		public static Rect ComputeBounds(Rect a_oRectA, Rect a_oRectB)
		{
			Rect oRectBoundsAB = new Rect();
			
			Vector2 f2Min;
			f2Min.x = Mathf.Min(a_oRectA.xMin, a_oRectB.xMin);
			f2Min.y = Mathf.Min(a_oRectA.yMin, a_oRectB.yMin);
			
			Vector2 f2Max;
			f2Max.x = Mathf.Max(a_oRectA.xMax, a_oRectB.xMax);
			f2Max.y = Mathf.Max(a_oRectA.yMax, a_oRectB.yMax);
			
			oRectBoundsAB.x = f2Min.x;
			oRectBoundsAB.y = f2Min.y;
			oRectBoundsAB.width = f2Max.x - f2Min.x;
			oRectBoundsAB.height = f2Max.y - f2Min.y;
			
			return oRectBoundsAB;
		}
	}
}
