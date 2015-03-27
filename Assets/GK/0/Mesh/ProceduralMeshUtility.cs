using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	public static class ProceduralMeshUtility
	{	
		public static void AddQuad(List<int> triangles, int offset = 0)
		{
			AddQuad(triangles, 0, 1, 2, 3, offset);
		}

		public static void AddQuad(List<int> triangles, int a, int b, int c, int d, int offset = 0)
		{
			AddTriangle(triangles, a, b, d, offset);
			AddTriangle(triangles, d, b, c, offset);
		}

		public static void AddTriangle(List<int> triangles, int a, int b, int c, int offset = 0)
		{
			a += offset;
			b += offset;
			c += offset;

			triangles.Add(a);
			triangles.Add(b);
			triangles.Add(c);
		}
	}
}