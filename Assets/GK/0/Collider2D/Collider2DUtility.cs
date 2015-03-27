#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace gk
{
	public static class Collider2DUtility
	{
		public static void MakeMeshColliderConvexInChildren(GameObject root, bool convex)
		{
			if(root == null)
				return;
			
			foreach(MeshCollider meshCollider in root.GetComponentsInChildren<MeshCollider>())
			{
				meshCollider.convex = convex;
			}
		}

		public static void MakeMeshColliderConvex(GameObject root, bool convex)
		{
			if(root == null)
				return;
			
			MeshCollider meshCollider = root.GetComponentInChildren<MeshCollider>();
			
			if(meshCollider == null)
				return;

			meshCollider.convex = convex;
		}

		public static void CreateMeshCollider2DForMeshesInChildren(GameObject root)
		{
			if(root == null)
				return;
			
			foreach(MeshFilter meshFilter in root.GetComponentsInChildren<MeshFilter>())
			{
				CreateMeshCollider2DForMesh(meshFilter);
			}
		}
		
		public static MeshCollider CreateMeshCollider2DForMesh(GameObject gameObject)
		{
			return CreateMeshCollider2DForMesh(gameObject.GetComponent<MeshFilter>());
		}
		
		public static MeshCollider CreateMeshCollider2DForMesh(MeshFilter meshFilter)
		{
			if(meshFilter == null)
				return null;
			
			Mesh mesh = meshFilter.sharedMesh;

			Mesh colliderMesh = new Mesh();
			ComputeBorderMesh(mesh.vertices, mesh.triangles, ref colliderMesh);

			// Destroy 2d physic components
			Collider2D[] colliders2D = meshFilter.GetComponents<Collider2D>();
			foreach(Collider2D collider2D in colliders2D)
			{
				Component.DestroyImmediate(collider2D);
			}

			Rigidbody2D[] rigidbodies2D = meshFilter.GetComponents<Rigidbody2D>();
			foreach(Rigidbody2D rigidbody2D in rigidbodies2D)
			{
				Component.DestroyImmediate(rigidbody2D);
			}

			MeshCollider collider = gkComponentBuilderUtility.GetOrAddComponent<MeshCollider>(meshFilter.gameObject);
			
			if(collider == null)
				return null;

			if(collider.sharedMesh != null && EditorUtility.IsPersistent(collider.sharedMesh) == false)
			{
				Component.DestroyImmediate(collider.sharedMesh);
			}

			collider.sharedMesh = colliderMesh;
			
			return collider;
		}
		
		public static void ComputeBorderMesh(Vector3[] vertices, int[] triangles, ref Mesh outputBorderMesh, float extrusionDepth = 1.0f)
		{
			int triangleCount = triangles.Length * 6;
			int vertexCount = vertices.Length * 2;

			int[] outputTriangles = new int[triangleCount];
			Vector3[] outputVertices = new Vector3[vertexCount];

			// Duplicate vertices
			int startDuplicateIndex = vertices.Length;
			Vector3 halfExtrudeOffset = Vector3.forward * extrusionDepth * 0.5f;
			for(int i = 0; i < vertices.Length; ++i)
			{
				Vector3 vertex = vertices[i];
				outputVertices[i] = vertex + halfExtrudeOffset;
				outputVertices[i + startDuplicateIndex] = vertex - halfExtrudeOffset;
			}

			// Border triangles
			int backFirstIndex = startDuplicateIndex;
			int currentOutputTriangleIndex = 0;
			for(int i = 0; i < triangles.Length; i+=3)
			{
				int AFront = triangles[i];
				int BFront = triangles[i + 1];
				int CFront = triangles[i + 2];

				int ABack = AFront + backFirstIndex;
				int BBack = BFront + backFirstIndex;
				int CBack = CFront + backFirstIndex;

				MakeQuad(AFront, ABack, BBack, BFront, outputTriangles, ref currentOutputTriangleIndex);
				MakeQuad(BFront, BBack, CBack, CFront, outputTriangles, ref currentOutputTriangleIndex);
				MakeQuad(CFront, CBack, ABack, AFront, outputTriangles, ref currentOutputTriangleIndex);
			}
			
			outputBorderMesh.vertices = outputVertices;
			outputBorderMesh.triangles = outputTriangles;
		}

		static void MakeQuad(int A, int B, int C, int D, int[] outputTriangles, ref int currentIndex)
		{
			MakeTriangle(A, B, D, outputTriangles, ref currentIndex);
			MakeTriangle(D, B, C, outputTriangles, ref currentIndex);
		}

		static void MakeTriangle(int A, int B, int C, int[] outputTriangles, ref int currentIndex)
		{
			outputTriangles[currentIndex] = A;
			++currentIndex;
			outputTriangles[currentIndex] = B;
			++currentIndex;
			outputTriangles[currentIndex] = C;
			++currentIndex;
		}

		public static void CreatePolygonCollider2DForMeshesInChildren(GameObject root)
		{
			if(root == null)
				return;

			foreach(MeshFilter meshFilter in root.GetComponentsInChildren<MeshFilter>())
			{
				CreatePolygonCollider2DForMesh(meshFilter);
			}
		}

		public static PolygonCollider2D CreatePolygonCollider2DForMesh(GameObject gameObject)
		{
			return CreatePolygonCollider2DForMesh(gameObject.GetComponent<MeshFilter>());
		}

		public static PolygonCollider2D CreatePolygonCollider2DForMesh(MeshFilter meshFilter)
		{
			if(meshFilter == null)
				return null;

			Mesh mesh = meshFilter.sharedMesh;

			List<Vector2[]> colliderPaths = Compute2DColliderPaths(mesh.vertices, mesh.triangles);

			if(colliderPaths == null || colliderPaths.Count <= 0)
				return null;

			PolygonCollider2D collider = gkComponentBuilderUtility.GetOrAddComponent<PolygonCollider2D>(meshFilter.gameObject);

			if(collider == null)
				return null;

			collider.pathCount = 0;
			collider.pathCount = colliderPaths.Count;

			int colliderPathIndex = 0;
			foreach(Vector2[] colliderPath in colliderPaths)
			{
				collider.SetPath(colliderPathIndex, colliderPath);

				++colliderPathIndex;
			}

			return collider;
		}

		public static List<Vector2[]> Compute2DColliderPaths(Vector3[] vertices, int[] triangles)
		{
			List<Vector2[]> paths = new List<Vector2[]>();
			for(int i = 0; i < triangles.Length; i+=3)
			{
				paths.Add(new Vector2[3]
				{
					vertices[triangles[i]],
					vertices[triangles[i + 1]],
					vertices[triangles[i + 2]]
				});
			}

			return paths;
		}
	}
}
#endif