#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace gk
{
	public class gkButtonMenuItems : Editor
	{
		private const float mc_fButtonUnityWorldSize = 1.0f;
		
		private static string mc_oCreateMeshDefaultName = "ButtonMesh";	
		
		[MenuItem("Assets/Create/GK/Input/ButtonMesh")]
		[MenuItem("GK/Input/Create/ButtonMesh")]
		private static void CreateButtonMesh()
		{
			Mesh oButtonMesh = gkEditorAssetMeshUtility.CreateMeshInSelectedProjectFolder(mc_oCreateMeshDefaultName);
			MakeIntoAButtonMesh(oButtonMesh);
		}
		
		[MenuItem("GK/Input/Update/ButtonMesh")]
		private static void UpdateSelectedButtonMesh()
		{
			Mesh rButtonMesh = gkEditorSelectionUtility.GetSelected<Mesh>();
			if(rButtonMesh != null)
			{
				MakeIntoAButtonMesh(rButtonMesh);
			}
		}
		
		[MenuItem ("GK/Input/Update/ButtonMesh", true)]
	    static bool ValidateUpdateSelectedButtonMesh()
		{
	       	return gkEditorSelectionUtility.GetSelected<Mesh>() != null;
	    }
		
		private static void MakeIntoAButtonMesh(Mesh a_rMesh)
		{
			gkQuadMeshUtility.MakeIntoAQuadMesh(a_rMesh, EMeshCreationAxis.Back, Vector2.one * mc_fButtonUnityWorldSize);
		}
	}
}
#endif