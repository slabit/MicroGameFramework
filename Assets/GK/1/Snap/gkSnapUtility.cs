using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	public enum gkESnapType
	{
		None,
		ClampByMinBounds,
		ClampByMaxBounds,
		SnapRangeMax,
		SnapRangeMin
	}
	
	public enum gkESnapMode
	{
		BiDirectionnal,
		IfAbsInferior
	}
	
	public class gkSnapAnchor
	{
		private float m_fMin = 0.0f;
		
		private float m_fMax = 0.0f;
		
		private float m_fCenter = 0.0f;
		
		private float m_fSize = 0.0f;
			
		public float Max
		{
			get
			{
				return m_fMax;
			}
		}
		
		public float Min
		{
			get
			{
				return m_fMin;
			}
		}
		
		public float Center
		{
			get
			{
				return m_fCenter;
			}
		}
		
		public float Size
		{
			get
			{
				return m_fSize;
			}
		}
		
		public void Offset(float a_fOffset)
		{
			m_fCenter += a_fOffset;
			m_fMin += a_fOffset;
			m_fMax += a_fOffset;
		}
		
		public void Reduce(float a_fReduction)
		{
			m_fMin += a_fReduction;
			m_fMax -= a_fReduction;
		}
		
		public void CopyFrom(gkSnapAnchor a_rSnapAnchorToCopy)
		{
			m_fCenter = a_rSnapAnchorToCopy.m_fCenter;
			m_fSize = a_rSnapAnchorToCopy.m_fSize;
			m_fMin = a_rSnapAnchorToCopy.m_fMin;
			m_fMax = a_rSnapAnchorToCopy.m_fMax;
		}
		
		public void Set_Center(float a_fCenter, float a_fSize = 0.0f)
		{
			m_fCenter = a_fCenter;
			m_fSize = a_fSize;
			
			float fHalfSize = a_fSize * 0.5f;
			m_fMin = a_fCenter - fHalfSize;
			m_fMax = a_fCenter + fHalfSize;
		}
		
		public void Set_MinMax(float a_fMin, float a_fMax)
		{
			m_fMin = a_fMin;
			m_fMax = a_fMax;
			
			m_fSize = (a_fMax - a_fMin);
			m_fCenter = a_fMin + m_fSize * 0.5f;
		}
		
		public void Set_UnorderedRangeBounds(float a_fRangeBoundA, float a_fRangeBoundB)
		{
			if(a_fRangeBoundA <= a_fRangeBoundB)
			{
				Set_MinMax(a_fRangeBoundA, a_fRangeBoundB);
			}
			else
			{
				Set_MinMax(a_fRangeBoundB, a_fRangeBoundA);	
			}
		}
		
		public static gkSnapAnchor CreateSnapAnchor_Center(float a_fCenter, float a_fSize = 0.0f)
		{
			gkSnapAnchor rSnapAnchor = new gkSnapAnchor();
			rSnapAnchor.Set_Center(a_fCenter, a_fSize);
			
			return rSnapAnchor;
		}
		
		public static gkSnapAnchor CreateSnapAnchor_UnorderedRangeBounds(float a_fRangeBoundA, float a_fRangeBoundB)
		{
			gkSnapAnchor rSnapAnchor = new gkSnapAnchor();
			rSnapAnchor.Set_UnorderedRangeBounds(a_fRangeBoundA, a_fRangeBoundB);
			
			return rSnapAnchor;
		}
		
		public static gkSnapAnchor CreateSnapAnchor_MinMax(float a_fMin, float a_fMax)
		{
			gkSnapAnchor rSnapAnchor = new gkSnapAnchor();
			rSnapAnchor.Set_MinMax(a_fMin, a_fMax);
			
			return rSnapAnchor;
		}
		
		public static List<float> ConvertAnchorListToFloatList(List<gkSnapAnchor> a_rAnchors)
		{
			List<float> oFloatList = new List<float>();
			foreach(gkSnapAnchor rSnapAnchor in a_rAnchors)
			{
				oFloatList.Add(rSnapAnchor.Min);
				oFloatList.Add(rSnapAnchor.Max);
			}
			
			return oFloatList;
		}
		
		public gkSnapAnchor()
		{
		}
		
		public gkSnapAnchor(gkSnapAnchor a_rSnapAnchorToCopy)
		{
			CopyFrom(a_rSnapAnchorToCopy);
		}
	}
	
	public class gkSnapResult
	{
		public float snapMovement = 0.0f;
		public gkESnapType snapType = gkESnapType.None;
		public int anchorIndex = -1;
		
		public bool SnappedOrClamp
		{
			get
			{
				return snapType != gkESnapType.None;
			}
		}
		
		public bool SnappedOnAnchor
		{
			get
			{
				return snapType == gkESnapType.SnapRangeMax || snapType == gkESnapType.SnapRangeMin;
			}
		}
		
		public bool ClampByBounds
		{
			get
			{
				return snapType == gkESnapType.ClampByMaxBounds || snapType == gkESnapType.ClampByMinBounds;
			}
		}
		
		public static void Copy(gkSnapResult a_rSnapResultFrom, gkSnapResult a_rSnapResultTo)
		{
			a_rSnapResultTo.snapMovement = a_rSnapResultFrom.snapMovement;
			a_rSnapResultTo.snapType = a_rSnapResultFrom.snapType;
			a_rSnapResultTo.anchorIndex = a_rSnapResultFrom.anchorIndex;
		}
	}
	
	public static class gkSnapUtility
	{
		public static Vector2 SnapPoint(Vector2 a_f2PointToSnap, float fSnapGridStep = 1.0f)
		{
			a_f2PointToSnap.x = Snap(a_f2PointToSnap.x, fSnapGridStep);
			a_f2PointToSnap.y = Snap(a_f2PointToSnap.y, fSnapGridStep);
			
			return a_f2PointToSnap;
		}
		
		public static bool SnapPoints(Vector2 a_f2PointToSnap, List<Vector2> a_oPointsToSnapOn, float a_fSnapSquareDistance, out Vector2 a_f2SnapMovement, out int a_iSnapIndex)
		{
			a_f2SnapMovement = Vector2.zero;
			a_iSnapIndex = 0;
			
			// Loop through points to snap on
			Vector2 f2SnapMovementMin = Vector2.zero;
			float fSnapSquareDistanceMin = float.PositiveInfinity;
			bool bSnap = false;
			int iSnapPointIndex = 0;
			foreach(Vector2 f2PointToSnapOn in a_oPointsToSnapOn)
			{
				Vector2 f2SnapMovement = f2PointToSnapOn - a_f2PointToSnap;
				float fSnapSquareDistance = f2SnapMovement.sqrMagnitude;
				if(fSnapSquareDistanceMin > fSnapSquareDistance)
				{
					fSnapSquareDistanceMin = fSnapSquareDistance;
					f2SnapMovementMin = f2SnapMovement;
					bSnap = true;
					a_iSnapIndex = iSnapPointIndex;
				}
				iSnapPointIndex++;
			}
			
			if(bSnap)
			{
				if(fSnapSquareDistanceMin <= a_fSnapSquareDistance)
				{
					a_f2SnapMovement = f2SnapMovementMin;
					return true;
				}
			}
			
			return false;
		}
		
		public static bool SnapRectangles(Rect a_oRectangleToSnap, List<Rect> a_oRectanglesToSnapOn, float a_fSnapDistance, out Vector2 a_f2SnapMovement)
		{	
			// Snap corners
			if(SnapRectangleCorners(a_oRectangleToSnap, a_oRectanglesToSnapOn, a_fSnapDistance * a_fSnapDistance, out a_f2SnapMovement))
			{
				return true;
			}
			else if(SnapRectangleEdges(a_oRectangleToSnap, a_oRectanglesToSnapOn, a_fSnapDistance, out a_f2SnapMovement))
			{
				return true;
			}
			
			return false;
		}
		
		public static bool SnapRectangleEdges(Rect a_oRectangleToSnap, List<Rect> a_oRectanglesToSnapOn, float a_fSnapDistance, out Vector2 a_f2SnapMovement)
		{
			a_f2SnapMovement = Vector2.zero;
			bool bSnap = false;
	
			// Vertical Snap
			
			// Get the vertical snap anchors
			List<gkSnapAnchor> oVerticalSnapAnchors = new List<gkSnapAnchor>();
			foreach(Rect oOtherTangramRect in a_oRectanglesToSnapOn)
			{
				if(gkSegment1DUtility.SegmentIntersectsSegment_MinMax(a_oRectangleToSnap.yMin - a_fSnapDistance, a_oRectangleToSnap.yMax + a_fSnapDistance, oOtherTangramRect.yMin, oOtherTangramRect.yMax))
				{
					gkSnapAnchor oSnapAnchor = gkSnapAnchor.CreateSnapAnchor_MinMax(oOtherTangramRect.xMin, oOtherTangramRect.xMax);
					oVerticalSnapAnchors.Add(oSnapAnchor);
				}
			}
			
			// Snap
			gkSnapResult oVerticalSnapResult;
			if(gkSnapUtility.SnapRange(a_oRectangleToSnap.xMin, a_oRectangleToSnap.xMax, a_fSnapDistance, oVerticalSnapAnchors, float.NegativeInfinity, float.PositiveInfinity, out oVerticalSnapResult))
			{
				a_f2SnapMovement.x = oVerticalSnapResult.snapMovement;
				bSnap = true;
			}
			
			// Horizontal Snap
			
			// Get the vertical snap anchors
			List<gkSnapAnchor> oHorizontalSnapAnchors = new List<gkSnapAnchor>();
			foreach(Rect oOtherTangramRect in a_oRectanglesToSnapOn)
			{
				if(gkSegment1DUtility.SegmentIntersectsSegment_MinMax(a_oRectangleToSnap.xMin - a_fSnapDistance, a_oRectangleToSnap.xMax + a_fSnapDistance, oOtherTangramRect.xMin, oOtherTangramRect.xMax))
				{
					gkSnapAnchor oSnapAnchor = gkSnapAnchor.CreateSnapAnchor_MinMax(oOtherTangramRect.yMin, oOtherTangramRect.yMax);
					oHorizontalSnapAnchors.Add(oSnapAnchor);
				}
			}
			
			// Snap
			gkSnapResult oHorizontalSnapResult;
			if(gkSnapUtility.SnapRange(a_oRectangleToSnap.yMin, a_oRectangleToSnap.yMax, a_fSnapDistance, oHorizontalSnapAnchors, float.NegativeInfinity, float.PositiveInfinity, out oHorizontalSnapResult))
			{
				a_f2SnapMovement.y = oHorizontalSnapResult.snapMovement;
				bSnap = true;
			}
			
			return bSnap;
		}
		
		public static bool SnapRectangleCorners(Rect a_oRectangleToSnap, List<Rect> a_rRectanglesToSnapOn, float a_fSnapSquareDistance, out Vector2 a_f2SnapMovement)
		{
			a_f2SnapMovement = Vector2.zero;
			bool bSnap = false;
			
			Vector2[] oCorners = new Vector2[]
			{
				new Vector2(a_oRectangleToSnap.xMin, a_oRectangleToSnap.yMin),
				new Vector2(a_oRectangleToSnap.xMin, a_oRectangleToSnap.yMax),
				new Vector2(a_oRectangleToSnap.xMax, a_oRectangleToSnap.yMax),
				new Vector2(a_oRectangleToSnap.xMax, a_oRectangleToSnap.yMin)
			};
			
			
			float fMinimumSnapMovementSquareLength = float.PositiveInfinity;
			foreach(Rect oRectangleToSnapOn in a_rRectanglesToSnapOn)
			{
				foreach(Vector2 f2Corner in oCorners)
				{
					float fSnapMovementSquareLength;
					Vector2 f2SnapMovement;
					if(SnapPointToRectangleCorner(f2Corner, oRectangleToSnapOn, a_fSnapSquareDistance, out f2SnapMovement, out fSnapMovementSquareLength))
					{
						bSnap = true;
						if(fMinimumSnapMovementSquareLength > fSnapMovementSquareLength)
						{
							a_f2SnapMovement = f2SnapMovement;
						}
					}
				}
			}
			
			return bSnap;
		}
		
		public static bool SnapPointToRectangleCorner(Vector2 a_f2PointToSnap, Rect a_oRectangleToSnapOn, float a_fSnapSquareDistance, out Vector2 a_f2SnapMovement, out float a_fSnapMovementSquareLength)
		{
			a_f2SnapMovement = Vector2.zero;
			a_fSnapMovementSquareLength = 0.0f;
			
			// Divide the space in 9 with the rectangle edges
			
			// Left
			if(a_f2PointToSnap.x <= a_oRectangleToSnapOn.xMin)
			{
				// Bottom Left
				if(a_f2PointToSnap.y <= a_oRectangleToSnapOn.yMin)
				{
					// We can only snap on the bottom left corner
					a_f2SnapMovement = new Vector2(a_oRectangleToSnapOn.xMin, a_oRectangleToSnapOn.yMin) - a_f2PointToSnap;
				}
				// Top Left
				else if(a_f2PointToSnap.y >= a_oRectangleToSnapOn.yMax)
				{
					// We can only snap on the top left corner
					a_f2SnapMovement = new Vector2(a_oRectangleToSnapOn.xMin, a_oRectangleToSnapOn.yMax) - a_f2PointToSnap;
				}
				// Middle Left
				else
				{
					// We can either snap on the bottom left corner or the top left corner
					
					// Select the nearest one
					
					// Bottom is the nearest
					if(Mathf.Abs(a_oRectangleToSnapOn.yMin - a_f2PointToSnap.y) < Mathf.Abs(a_oRectangleToSnapOn.yMax - a_f2PointToSnap.y))
					{
						a_f2SnapMovement = new Vector2(a_oRectangleToSnapOn.xMin, a_oRectangleToSnapOn.yMin) - a_f2PointToSnap;
					}
					else
					// Top is the nearest
					{
						a_f2SnapMovement = new Vector2(a_oRectangleToSnapOn.xMin, a_oRectangleToSnapOn.yMax) - a_f2PointToSnap;
					}
				}
				
				a_fSnapMovementSquareLength = a_f2SnapMovement.sqrMagnitude;
			}
			// Right
			else if(a_f2PointToSnap.x >= a_oRectangleToSnapOn.xMax)
			{
				// Bottom Right
				if(a_f2PointToSnap.y <= a_oRectangleToSnapOn.yMin)
				{
					// We can only snap on the bottom right corner
					a_f2SnapMovement = new Vector2(a_oRectangleToSnapOn.xMax, a_oRectangleToSnapOn.yMin) - a_f2PointToSnap;
				}
				// Top Right
				else if(a_f2PointToSnap.y >= a_oRectangleToSnapOn.yMax)
				{
					// We can only snap on the top right corner
					a_f2SnapMovement = new Vector2(a_oRectangleToSnapOn.xMax, a_oRectangleToSnapOn.yMax) - a_f2PointToSnap;
				}
				// Middle Right
				else
				{
					// We can either snap on the bottom left corner or the top left corner
					
					// Select the nearest one
					
					// Bottom is the nearest
					if(Mathf.Abs(a_oRectangleToSnapOn.yMin - a_f2PointToSnap.y) < Mathf.Abs(a_oRectangleToSnapOn.yMax - a_f2PointToSnap.y))
					{
						a_f2SnapMovement = new Vector2(a_oRectangleToSnapOn.xMax, a_oRectangleToSnapOn.yMin) - a_f2PointToSnap;
					}
					else
					// Top is the nearest
					{
						a_f2SnapMovement = new Vector2(a_oRectangleToSnapOn.xMax, a_oRectangleToSnapOn.yMax) - a_f2PointToSnap;
					}
				}
				
				a_fSnapMovementSquareLength = a_f2SnapMovement.sqrMagnitude;
			}
			// Center
			else
			{
				// Bottom Center
				if(a_f2PointToSnap.y <= a_oRectangleToSnapOn.yMin)
				{
					// We can either snap on the bottom left corner or the bottom right corner
					
					// Select the nearest one
					
					// Left is the nearest
					if(Mathf.Abs(a_oRectangleToSnapOn.xMin - a_f2PointToSnap.x) < Mathf.Abs(a_oRectangleToSnapOn.xMax - a_f2PointToSnap.x))
					{
						a_f2SnapMovement = new Vector2(a_oRectangleToSnapOn.xMin, a_oRectangleToSnapOn.yMin) - a_f2PointToSnap;
					}
					else
					// Right is the nearest
					{
						a_f2SnapMovement = new Vector2(a_oRectangleToSnapOn.xMax, a_oRectangleToSnapOn.yMin) - a_f2PointToSnap;
					}
					
					a_fSnapMovementSquareLength = a_f2SnapMovement.sqrMagnitude;
				}
				// Top Center
				else if(a_f2PointToSnap.y >= a_oRectangleToSnapOn.yMax)
				{
					// We can either snap on the top left corner or the top right corner
					
					// Select the nearest one
					
					// Left is the nearest
					if(Mathf.Abs(a_oRectangleToSnapOn.xMin - a_f2PointToSnap.x) < Mathf.Abs(a_oRectangleToSnapOn.xMax - a_f2PointToSnap.x))
					{
						a_f2SnapMovement = new Vector2(a_oRectangleToSnapOn.xMin, a_oRectangleToSnapOn.yMax) - a_f2PointToSnap;
					}
					else
					// Right is the nearest
					{
						a_f2SnapMovement = new Vector2(a_oRectangleToSnapOn.xMax, a_oRectangleToSnapOn.yMax) - a_f2PointToSnap;
					}
					
					a_fSnapMovementSquareLength = a_f2SnapMovement.sqrMagnitude;
				}
				// Middle Right
				else
				{
					// We can either snap on any of the corners
					Vector2[] oCorners = new Vector2[]
					{
						new Vector2(a_oRectangleToSnapOn.xMin, a_oRectangleToSnapOn.yMin),
						new Vector2(a_oRectangleToSnapOn.xMin, a_oRectangleToSnapOn.yMax),
						new Vector2(a_oRectangleToSnapOn.xMax, a_oRectangleToSnapOn.yMax),
						new Vector2(a_oRectangleToSnapOn.xMax, a_oRectangleToSnapOn.yMin)
					};
					
					a_fSnapMovementSquareLength = float.PositiveInfinity;
					foreach(Vector2 f2Corner in oCorners)
					{
						Vector2 f2DistanceToCorner = f2Corner - a_f2PointToSnap;
						float fSquareDistanceToCorner = f2DistanceToCorner.sqrMagnitude;
						if(a_fSnapMovementSquareLength > fSquareDistanceToCorner)
						{
							a_fSnapMovementSquareLength = fSquareDistanceToCorner;
							a_f2SnapMovement = f2DistanceToCorner;
						}
					}
				}
			}
			
			if(a_fSnapMovementSquareLength <= a_fSnapSquareDistance)
			{
				return true;
			}
			
			return false;
		}
		
		public static bool SnapRange(float a_fSnapRangeMin, float a_fSnapRangeMax, List<gkSnapAnchor> a_rSnapAnchors, float a_fBoundMin, float a_fBoundMax, out gkSnapResult a_rSnapResult)
		{
			return SnapRange(a_fSnapRangeMin, a_fSnapRangeMax, float.PositiveInfinity, a_rSnapAnchors, a_fBoundMin, a_fBoundMax, out a_rSnapResult);
		}
		
		public static bool SnapRange(float a_fSnapRangeMin, float a_fSnapRangeMax, float a_fSnapDistance, List<gkSnapAnchor> a_rSnapAnchors, float a_fBoundMin, float a_fBoundMax, out gkSnapResult a_rSnapResult)
		{
			gkSnapResult oSnapResult_Min;
			gkSnapResult oSnapResult_Max;
			return SnapRange(a_fSnapRangeMin, a_fSnapRangeMax, a_fSnapDistance, a_rSnapAnchors, a_fBoundMin, a_fBoundMax, out a_rSnapResult, out oSnapResult_Min, out oSnapResult_Max);
		}
		
		public static bool SnapRange(float a_fSnapRangeMin, float a_fSnapRangeMax, float a_fSnapDistance, List<gkSnapAnchor> a_rSnapAnchors, float a_fBoundMin, float a_fBoundMax,
			out gkSnapResult a_rSnapResult, out gkSnapResult a_rSnapResult_Min, out gkSnapResult a_rSnapResult_Max)
		{
			a_rSnapResult = new gkSnapResult();
			a_rSnapResult_Min = new gkSnapResult();
			a_rSnapResult_Max = new gkSnapResult();
			
			// Compute the range size
			float fRangeSize = a_fSnapRangeMax - a_fSnapRangeMin;
			
			// Create the list of valid top values
			// On wich we can snap the bottom of the snap range
			int iStartTopIndex = 0;
			List<float> oTopValues = GetValidSnapValues(a_rSnapAnchors, a_fBoundMin, a_fBoundMax - fRangeSize, true, out iStartTopIndex);
			
			// Create the list of valid bottom values
			// On wich we can snap the top of the snap range
			int iStartBottomIndex = 0;
			List<float> oBottomValues = GetValidSnapValues(a_rSnapAnchors, a_fBoundMin + fRangeSize, a_fBoundMax, false, out iStartBottomIndex);
			
			float fSnappedValue;
			float fMinimumSnapMovement = float.PositiveInfinity;
			
			// Try to snap the bottom to the tops
			int iSnappedTopValueIndex;
			if(Snap(a_fSnapRangeMin, a_fSnapDistance, oTopValues, out fSnappedValue, out iSnappedTopValueIndex))
			{	
				float fSnapMovement = fSnappedValue - a_fSnapRangeMin;
					
				a_rSnapResult_Min.snapMovement = fSnapMovement;
				a_rSnapResult_Min.snapType = gkESnapType.SnapRangeMin;
				a_rSnapResult_Min.anchorIndex = iSnappedTopValueIndex + iStartTopIndex;
				
				fMinimumSnapMovement = fSnapMovement;
				
				gkSnapResult.Copy(a_rSnapResult_Min, a_rSnapResult);
			}
			
			// Try to snap the top to the bottoms
			int iSnappedBottomValueIndex;
			if(Snap(a_fSnapRangeMax, a_fSnapDistance, oBottomValues, out fSnappedValue, out iSnappedBottomValueIndex))
			{
				float fSnapMovement = fSnappedValue - a_fSnapRangeMax;
			
				a_rSnapResult_Max.snapMovement = fSnapMovement; 
				a_rSnapResult_Max.snapType = gkESnapType.SnapRangeMax;
				a_rSnapResult_Max.anchorIndex = iSnappedBottomValueIndex + iStartBottomIndex;
				
				if(Mathf.Abs(fSnapMovement) < Mathf.Abs(fMinimumSnapMovement))
				{
					fMinimumSnapMovement = fSnapMovement;
					
					gkSnapResult.Copy(a_rSnapResult_Max, a_rSnapResult);
				}
			}
			
			if(a_rSnapResult.SnappedOnAnchor)
			{
				a_rSnapResult.snapMovement = fMinimumSnapMovement;
			}
			else
			{
				// Clamp to interval
				if(a_fSnapRangeMin < a_fBoundMin)
				{
					a_rSnapResult.snapMovement = a_fBoundMin - a_fSnapRangeMin;
					a_rSnapResult.snapType = gkESnapType.ClampByMinBounds;
				}
				else if(a_fSnapRangeMax > a_fBoundMax)
				{
					a_rSnapResult.snapMovement = a_fBoundMax - a_fSnapRangeMax;
					a_rSnapResult.snapType = gkESnapType.ClampByMaxBounds; 
				}
			}
			
			
			return a_rSnapResult.SnappedOrClamp;
		}
		
		public static List<float> GetValidSnapValues(List<float> a_rSnapValues, float a_fMin, float a_fMax)
		{
			List<float> oValidSnapValues = new List<float>();
			foreach(float fSnapValue in a_rSnapValues)
			{
				if(fSnapValue >= a_fMin && fSnapValue <= a_fMax)
				{
					oValidSnapValues.Add(fSnapValue);
				}
			}
			
			return oValidSnapValues;
		}
		
		public static List<float> GetValidSnapValues(List<gkSnapAnchor> a_rSnapAnchors, float a_fMin, float a_fMax, bool a_bMax, out int a_iStartIndex)
		{
			List<float> oValues = new List<float>();
			a_iStartIndex = 0;
			bool bIndexHasBeenStarted = false;
			foreach(gkSnapAnchor rSnapAnchor in a_rSnapAnchors)
			{
				float fValue;
				
				if(a_bMax)
				{
					fValue = rSnapAnchor.Max;
				}
				else
				{
					fValue = rSnapAnchor.Min;
				}
				
				if(fValue >= a_fMin && fValue <= a_fMax)
				{
					oValues.Add(fValue);
					bIndexHasBeenStarted = true;
				}
				else
				{
					if(bIndexHasBeenStarted == false)
					{
						a_iStartIndex++;
					}
				}
			}
			
			return oValues;
		}
		
		public static float Snap(float a_fValueToSnap, float a_fSnapStep)
		{
			if(a_fSnapStep == 0.0f)
			{
				return a_fValueToSnap;
			}
			else
			{
				int iSnapStepIndex;
				iSnapStepIndex = Mathf.RoundToInt(a_fValueToSnap/a_fSnapStep);
				return a_fSnapStep * iSnapStepIndex;
			}
		}
		
		public static bool Snap(float a_fValueToSnap, float a_fSnapDistance, List<float> a_rSnapValues, out float a_fSnappedValue, gkESnapMode a_eSnapMode = gkESnapMode.BiDirectionnal, float a_fEqualityPrecision = 0.01f)
		{
			int iSnapIndex;
			return Snap(a_fValueToSnap, a_fSnapDistance, a_rSnapValues, out a_fSnappedValue, out iSnapIndex, a_eSnapMode, a_fEqualityPrecision);
		}
		
		public static bool Snap(float a_fValueToSnap, float a_fSnapDistance, List<gkSnapAnchor> a_rSnapAnchors, out float a_fSnappedValue, gkESnapMode a_eSnapMode = gkESnapMode.BiDirectionnal, float a_fEqualityPrecision = 0.01f)
		{
			int iSnapIndex;
			return Snap(a_fValueToSnap, a_fSnapDistance, a_rSnapAnchors, out a_fSnappedValue, out iSnapIndex, a_eSnapMode, a_fEqualityPrecision);
		}
		
		public static bool Snap(float a_fValueToSnap, float a_fSnapDistance, List<gkSnapAnchor> a_rSnapAnchors, out float a_fSnappedValue, out int a_iSnapValueIndex, gkESnapMode a_eSnapMode = gkESnapMode.BiDirectionnal, float a_fEqualityPrecision = 0.01f)
		{
			return Snap(a_fValueToSnap, a_fSnapDistance, gkSnapAnchor.ConvertAnchorListToFloatList(a_rSnapAnchors), out a_fSnappedValue, out a_iSnapValueIndex, a_eSnapMode, a_fEqualityPrecision);
		}
		
		public static bool Snap(float a_fValueToSnap, float a_fSnapDistance, List<float> a_rSnapValues, out float a_fSnappedValue, out int a_iSnapValueIndex, gkESnapMode a_eSnapMode = gkESnapMode.BiDirectionnal, float a_fEqualityPrecision = 0.01f)
		{
			a_rSnapValues.Sort();
			a_iSnapValueIndex = -1;
			a_fSnappedValue = a_fValueToSnap;
			if(a_rSnapValues.Count <= 0)
			{
				return false;
			}
			else
			{
				float fNearestValue;
				float fFirstValue = a_rSnapValues[0];
				int iSnapValuesCount = a_rSnapValues.Count;
				if(a_fValueToSnap <= fFirstValue || iSnapValuesCount <= 1)
				{
					fNearestValue = fFirstValue;
					a_iSnapValueIndex = 0;
				}
				else
				{
					float fValueDirectlyBefore = fFirstValue;
					float fValueDirectlyAfter = fFirstValue;
					int iValueDirectlyAfterIndex = 0;
					int iValueDirectlyBeforeIndex = 0;
					for(int i = 1; i < iSnapValuesCount; i++)
					{
						float fCurrentValue = a_rSnapValues[i];
						if(a_fValueToSnap <= fCurrentValue)
						{
							fValueDirectlyAfter = fCurrentValue;
							iValueDirectlyAfterIndex = i;
							break;
						}
						else
						{
							fValueDirectlyBefore = fCurrentValue;
							fValueDirectlyAfter = fCurrentValue;
							iValueDirectlyBeforeIndex = i;
							iValueDirectlyAfterIndex = i;
						}
					}
					
					switch(a_eSnapMode)
					{
						default:
						case gkESnapMode.BiDirectionnal:
						{
							// Compute the nearest value between the one directly before and the one directly after
							if(a_fValueToSnap - fValueDirectlyBefore <= fValueDirectlyAfter - a_fValueToSnap)
							{
								fNearestValue = fValueDirectlyBefore;
								a_iSnapValueIndex = iValueDirectlyBeforeIndex;
							}
							else
							{
								fNearestValue = fValueDirectlyAfter;
								a_iSnapValueIndex = iValueDirectlyAfterIndex;
							}
						}
						break;
						
						case gkESnapMode.IfAbsInferior:
						{
							bool bEqualToAfter = Mathf.Abs(a_fValueToSnap - fValueDirectlyAfter) <= a_fEqualityPrecision;
							bool bEqualToBefore = Mathf.Abs(a_fValueToSnap - fValueDirectlyBefore) <= a_fEqualityPrecision;
						
							// Compute the nearest value between the one directly before and the one directly after
							if(bEqualToBefore ||
							(a_fValueToSnap >= 0.0f && bEqualToAfter == false))
							{
								fNearestValue = fValueDirectlyBefore;
								a_iSnapValueIndex = iValueDirectlyBeforeIndex;
							}
							else
							{
								fNearestValue = fValueDirectlyAfter;
								a_iSnapValueIndex = iValueDirectlyAfterIndex;
							}
						
							if(Mathf.Sign(a_fValueToSnap) * Mathf.Sign(fNearestValue) < 0.0f
								|| 
								Mathf.Abs(a_fValueToSnap) < Mathf.Abs(fNearestValue))
							{
								return false;
							}
						}
						break;
					}
				}
				
				// Can we snap to the nearest value?
				if(Mathf.Abs(fNearestValue - a_fValueToSnap) <= a_fSnapDistance)
				{
					a_fSnappedValue = fNearestValue;
					return true;
				}
				else
				{
					return false;
				}
			}
		}
		
		public static bool Snap(float a_fValueToSnap, float a_fSnapDistance, float a_fSnapValue, out float a_fSnappedValue)
		{
			a_fSnappedValue = 0.0f;
			if(Mathf.Abs(a_fValueToSnap - a_fSnapValue) <= a_fSnapDistance)
			{
				a_fSnappedValue = a_fSnapValue;
				return true;
			}
			return false;
		}
	}
}