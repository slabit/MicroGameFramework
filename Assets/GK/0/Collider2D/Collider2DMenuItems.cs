#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace gk
{
	public static class Collider2DMenuItems
	{
		[MenuItem("GK/Collider 2D/Check Convex In Children Mesh Colliders")]
		public static void DoCheckConvexInChildrenMeshColliders()
		{
			foreach(GameObject gameObject in Selection.gameObjects)
			{
				Collider2DUtility.MakeMeshColliderConvexInChildren(gameObject, true);
			}
		}

		[MenuItem("GK/Collider 2D/Uncheck Convex In Children Mesh Colliders")]
		public static void DoUncheckConvexInChildrenMeshColliders()
		{
			foreach(GameObject gameObject in Selection.gameObjects)
			{
				Collider2DUtility.MakeMeshColliderConvexInChildren(gameObject, false);
			}
		}

		[MenuItem("GK/Collider 2D/Create Mesh Collider 2D For Mesh")]
		public static void DoCreateMeshCollider2DForMesh()
		{
			foreach(GameObject gameObject in Selection.gameObjects)
			{
				Collider2DUtility.CreateMeshCollider2DForMesh(gameObject);
			}
		}
		
		[MenuItem("GK/Collider 2D/Create Mesh Collider 2D For Meshes")]
		public static void DoCreateMeshCollider2DForMeshesInChildren()
		{
			foreach(GameObject gameObject in Selection.gameObjects)
			{
				Collider2DUtility.CreateMeshCollider2DForMeshesInChildren(gameObject);
			}
		}

		[MenuItem("GK/Collider 2D/Create Collider 2D For Mesh")]
		public static void DoCreateCollider2DForMesh()
		{
			foreach(GameObject gameObject in Selection.gameObjects)
			{
				Collider2DUtility.CreatePolygonCollider2DForMesh(gameObject);
			}
		}

		[MenuItem("GK/Collider 2D/Create Collider 2D For Meshes")]
		public static void DoCreateCollider2DForMeshesInChildren()
		{
			foreach(GameObject gameObject in Selection.gameObjects)
			{
				Collider2DUtility.CreatePolygonCollider2DForMeshesInChildren(gameObject);
			}
		}
	}
}
#endif