using UnityEngine;

namespace gk
{
	public static class gkSegment3DUtility
	{
		public static float PointToSegmentDistance(Vector3 a_f3Point, Vector3 a_f3SegmentBegin, Vector3 a_f3SegmentEnd) 
		{
			// Return minimum distance between line segment vw and point a_f3Point
			float l2 = (a_f3SegmentEnd - a_f3SegmentBegin).sqrMagnitude;  // i.e. |w-v|^2 -  avoid a sqrt
			
			if (l2 == 0.0)
			{
				return Vector3.Distance(a_f3Point, a_f3SegmentBegin);   // v == w case
			}
			
			// Consider the line extending the segment, parameterized as v + t (w - v).
			// We find projection of point p onto the line. 
			// It falls where t = [(p-v) . (w-v)] / |w-v|^2
			float t = Vector3.Dot(a_f3Point - a_f3SegmentBegin, a_f3SegmentEnd - a_f3SegmentBegin) / l2;
			
			if (t < 0.0)
			{
				return Vector3.Distance(a_f3Point, a_f3SegmentBegin);       // Beyond the 'v' end of the segment
			}
			else if (t > 1.0)
			{
				return Vector3.Distance(a_f3Point, a_f3SegmentEnd);  // Beyond the 'w' end of the segment
			}
			
			Vector3 projection = a_f3SegmentBegin + t * (a_f3SegmentEnd - a_f3SegmentBegin);  // Projection falls on the segment
			
			return Vector3.Distance(a_f3Point, projection);
		}
	}
}