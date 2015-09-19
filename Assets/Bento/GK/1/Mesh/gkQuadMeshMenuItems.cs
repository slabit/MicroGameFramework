#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace gk
{
	public class gkQuadMeshMenuItems : Editor
	{
		private static string mc_oCreateMeshDefaultName = "QuadMesh";	
		
		[MenuItem("Assets/Create/GK/Mesh/QuadMesh")]
		[MenuItem("GK/Mesh/Create/QuadMesh")]
		private static void CreateQuadMesh()
		{
			Mesh oQuadMesh = gkEditorAssetMeshUtility.CreateMeshInSelectedProjectFolder(mc_oCreateMeshDefaultName);
			MakeIntoAQuadMesh(oQuadMesh);
		}
		
		[MenuItem("GK/Mesh/Update/QuadMesh")]
		private static void UpdateSelectedQuadMesh()
		{
			Mesh rQuadMesh = gkEditorSelectionUtility.GetSelected<Mesh>();
			if(rQuadMesh != null)
			{
				MakeIntoAQuadMesh(rQuadMesh);
			}
		}
		
		[MenuItem ("GK/Input/Update/QuadMesh", true)]
	    static bool ValidateUpdateSelectedQuadMesh() 
		{
	        // Return false if no mesh is selected
	       	return gkEditorSelectionUtility.GetSelected<Mesh>() != null;
	    }
		
		private static void MakeIntoAQuadMesh(Mesh a_rMesh)
		{
			gkQuadMeshUtility.MakeIntoAQuadMesh(a_rMesh, EMeshCreationAxis.Back, Vector2.one);
		}
	}
}
#endif