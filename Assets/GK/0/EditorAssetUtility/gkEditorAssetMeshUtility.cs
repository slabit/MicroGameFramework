#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;

namespace gk
{
	public static class gkEditorAssetMeshUtility 
	{	
		public static Mesh CreateMeshInSelectedProjectFolder(string a_oMeshName)
		{
			Mesh oMesh = new Mesh();
			oMesh.name = a_oMeshName;
			
			SaveMeshInSelectedProjectFolder(oMesh);
			
			return oMesh;
		}
		
		public static string SaveMeshInSelectedProjectFolder(Mesh a_rMeshToSave)
		{
			string oAssetPath = GetCreationPathForNewMeshInSelectedProjectFolder(a_rMeshToSave.name);
			
			AssetDatabase.CreateAsset(a_rMeshToSave, oAssetPath);
			
			return oAssetPath;
		}
		
		public static string GetCreationPathForNewMeshInSelectedProjectFolder(string a_oMeshName)
		{
			string oAssetPath = gkEditorAssetPathUtility.GetLocalSelectedAssetFolderPath() + a_oMeshName + ".asset";
			oAssetPath = AssetDatabase.GenerateUniqueAssetPath(oAssetPath);
			
			return oAssetPath;
		}
	}
}
#endif