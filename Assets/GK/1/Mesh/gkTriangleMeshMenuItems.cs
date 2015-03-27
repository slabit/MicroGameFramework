#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace gk
{
	public class gkTriangleMeshMenuItems : Editor
	{
		private static string mc_oCreateMeshDefaultName = "TriangleMesh";
		
		[MenuItem("Assets/Create/GK/Mesh/TriangleMesh_Isoceles")]
		[MenuItem("GK/Mesh/Create/TriangleMesh_Isoceles")]
		private static void CreateTriangleMesh_Isoceles()
		{
			Mesh oTriangleMesh = gkEditorAssetMeshUtility.CreateMeshInSelectedProjectFolder(mc_oCreateMeshDefaultName + "_Isoceles");
			MakeIntoATriangleMesh(oTriangleMesh, ETriangleType.Isoceles);
		}
		
		[MenuItem("GK/Mesh/Update/TriangleMesh_Isoceles")]
		private static void UpdateSelectedTriangleMesh_Isoceles()
		{
			Mesh rTriangleMesh = gkEditorSelectionUtility.GetSelected<Mesh>();
			if(rTriangleMesh != null)
			{
				MakeIntoATriangleMesh(rTriangleMesh, ETriangleType.Isoceles);
			}
		}
		
		[MenuItem("Assets/Create/GK/Mesh/TriangleMesh_Equilateral")]
		[MenuItem("GK/Mesh/Create/TriangleMesh_Equilateral")]
		private static void CreateTriangleMesh_Equilateral()
		{
			Mesh oTriangleMesh = gkEditorAssetMeshUtility.CreateMeshInSelectedProjectFolder(mc_oCreateMeshDefaultName + "_Equilateral");
			MakeIntoATriangleMesh(oTriangleMesh, ETriangleType.Equilateral);
		}
		
		[MenuItem("GK/Mesh/Update/TriangleMesh_Equilateral")]
		private static void UpdateSelectedTriangleMesh_Equilateral()
		{
			Mesh rTriangleMesh = gkEditorSelectionUtility.GetSelected<Mesh>();
			if(rTriangleMesh != null)
			{
				MakeIntoATriangleMesh(rTriangleMesh, ETriangleType.Equilateral);
			}
		}
		
		[MenuItem ("GK/Input/Update/TriangleMesh_Isoceles", true)]
		[MenuItem ("GK/Input/Update/TriangleMesh_Equilateral", true)]
	    static bool ValidateUpdateSelectedTriangleMesh() 
		{
	        // Return false if no mesh is selected
	       	return gkEditorSelectionUtility.GetSelected<Mesh>() != null;
	    }
		
		private static void MakeIntoATriangleMesh(Mesh a_rMesh, ETriangleType a_eTriangleType)
		{
			gkTriangleMeshUtility.MakeIntoATriangleMesh(a_eTriangleType, a_rMesh, EMeshCreationAxis.Back, Vector2.one);
		}
	}
}
#endif