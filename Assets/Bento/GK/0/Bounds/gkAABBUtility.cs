using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	public static class gkAABBUtility 
	{
		public static Bounds ComputeAABB(Vector3[] a_rVertices, int[] a_oIndices)
		{
			Vector3 f3AABBMin;
			Vector3 f3AABBMax;
			ComputeAABB(a_rVertices, out f3AABBMin, out f3AABBMax, a_oIndices);
			
			Vector3 f3Size = f3AABBMax - f3AABBMin;
			Vector3 f3Center = f3AABBMin + f3Size * 0.5f;
			
			return new Bounds(f3Center, f3Size);
		}
		
		public static void ComputeAABB(Vector3[] a_rVertices, out Vector3 a_f3AABBMin, out Vector3 a_f3AABBMax, int[] a_oIndices)
		{
			a_f3AABBMin = Vector3.one * float.PositiveInfinity;
			a_f3AABBMax = Vector3.one * float.NegativeInfinity;
			
			foreach(int iVertexIndex in a_oIndices)
			{
				Vector3 f3Vertex = a_rVertices[iVertexIndex];
					
				a_f3AABBMin.x = Mathf.Min(f3Vertex.x, a_f3AABBMin.x);
				a_f3AABBMin.y = Mathf.Min(f3Vertex.y, a_f3AABBMin.y);
				a_f3AABBMin.z = Mathf.Min(f3Vertex.z, a_f3AABBMin.z);
				
				a_f3AABBMax.x = Mathf.Max(f3Vertex.x, a_f3AABBMax.x);
				a_f3AABBMax.y = Mathf.Max(f3Vertex.y, a_f3AABBMax.y);
				a_f3AABBMax.z = Mathf.Max(f3Vertex.z, a_f3AABBMax.z);
			}
		}
	}
}