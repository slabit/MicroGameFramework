using UnityEngine;

namespace gk
{
	public static class gkSegment1DUtility
	{
		public static void SegmentMinMaxToCenter(float a_fMin, float a_fMax, out float a_fCenter, out float a_fWidth)
		{
			a_fWidth = a_fMax - a_fMin;
			a_fCenter = a_fMin + a_fWidth * 0.5f;
		}
		
		public static void SegmentCenterToMinMax(float a_fCenter, float a_fWidth, out float a_fMin, out float a_fMax)
		{
			float fHalfWidth = a_fWidth * 0.5f;
			
			a_fMin = a_fCenter - fHalfWidth;
			a_fMax = a_fCenter + fHalfWidth;
		}
			
		public static bool SegmentIntersectsSegment_MinMax(float a_fSegmentA_Begin, float a_fSegmentA_End, float a_fSegmentB_Begin, float a_fSegmentB_End)
		{
			return a_fSegmentA_End >= a_fSegmentB_Begin && a_fSegmentA_Begin <= a_fSegmentB_End;
		}
		
		public static bool SegmentIntersectsSegment_MinMax(float a_fSegmentA_Begin, float a_fSegmentA_End, float a_fSegmentB_Begin, float a_fSegmentB_End, float a_fTolerance)
		{
			return a_fSegmentA_End >= (a_fSegmentB_Begin + a_fTolerance) && a_fSegmentA_Begin <= (a_fSegmentB_End - a_fTolerance);
		}
		
		public static bool SegmentContainsSegment_MinMax(float a_fSegmentContainer_Begin, float a_fSegmentContainer_End, float a_fSegmentTested_Begin, float a_fSegmentTested_End)
		{
			// if totally included
			return a_fSegmentTested_Begin >= a_fSegmentContainer_Begin && a_fSegmentTested_End <= a_fSegmentContainer_End;
		}
		
		public static bool SegmentContainsSegment_MinMax(float a_fSegmentContainer_Begin, float a_fSegmentContainer_End, float a_fSegmentTested_Begin, float a_fSegmentTested_End, float a_fTolerance)
		{
			// if totally included
			return a_fSegmentTested_Begin >= (a_fSegmentContainer_Begin - a_fTolerance) && a_fSegmentTested_End <= (a_fSegmentContainer_End + a_fTolerance);
		}
		
		public static bool SegmentContainsPoint_MinMax(float a_fSegmentContainer_Begin, float a_fSegmentContainer_End, float a_fPoint)
		{
			// if totally included
			return a_fPoint >= a_fSegmentContainer_Begin && a_fPoint <= a_fSegmentContainer_End;
		}
		
		public static void ClampSegmentInSegment_MinMax(float a_fSegmentToClamp_Begin, float a_fSegmentToClamp_End, float a_fSegmentBounds_Begin, float a_fSegmentBounds_End,
			out float a_fSegmentResult_Begin, out float a_fSegmentResult_End)
		{
			a_fSegmentResult_Begin = Mathf.Clamp(a_fSegmentToClamp_Begin, a_fSegmentBounds_Begin, a_fSegmentBounds_End);
			a_fSegmentResult_End = Mathf.Clamp(a_fSegmentToClamp_End, a_fSegmentBounds_Begin, a_fSegmentBounds_End);
		}
		
		public static void ClampSegmentInSegment_Center(float a_fSegmentToClamp_Center, float a_fSegmentToClamp_Width, float a_fSegmentBounds_Center, float a_fSegmentBounds_Width,
			out float a_fSegmentResult_Center, out float a_fSegmentResult_Width)
		{
			float fSegmentToClamp_Min;
			float fSegmentToClamp_Max;
			SegmentCenterToMinMax(a_fSegmentToClamp_Center, a_fSegmentToClamp_Width, out fSegmentToClamp_Min, out fSegmentToClamp_Max);
			
			float fSegmentBounds_Min;
			float fSegmentBounds_Max;
			SegmentCenterToMinMax(a_fSegmentBounds_Center, a_fSegmentBounds_Width, out fSegmentBounds_Min, out fSegmentBounds_Max);
			
			float fSegmentResult_Min;
			float fSegmentResult_Max;
			ClampSegmentInSegment_MinMax(fSegmentToClamp_Min, fSegmentToClamp_Max, fSegmentBounds_Min, fSegmentBounds_Max, out fSegmentResult_Min, out fSegmentResult_Max);
			
			SegmentMinMaxToCenter(fSegmentResult_Min, fSegmentResult_Max, out a_fSegmentResult_Center, out a_fSegmentResult_Width);
		}
	}
}