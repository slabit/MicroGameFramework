#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;

namespace gk
{
	public static class gkEditorAssetPrefabUtility 
	{
		public static ComponentType CreatePrefab<ComponentType>(string a_oPrefabName) where ComponentType : Component
		{
			return CreatePrefab<ComponentType>(a_oPrefabName, gkEditorAssetPathUtility.GetLocalSelectedAssetFolderPath());
		}
		
		public static ComponentType CreatePrefab<ComponentType>(string a_oPrefabName, string a_oAssetPath) where ComponentType : Component
		{
			a_oAssetPath = AssetDatabase.GenerateUniqueAssetPath(a_oAssetPath + "/" + a_oPrefabName + ".prefab");
			
			ComponentType oComponent = gkComponentBuilderUtility.BuildComponent<ComponentType>();
			
			ComponentType oComponentPrefab = PrefabUtility.CreatePrefab(a_oAssetPath, oComponent.gameObject).GetComponent<ComponentType>();
			
			GameObject.DestroyImmediate(oComponent.gameObject);
			
			return oComponentPrefab;
		}
		
		public static string GetCreationPathForNewPrefabInSelectedProjectFolder(string a_rPrefabName)
		{
			string oAssetPath = gkEditorAssetPathUtility.GetLocalSelectedAssetFolderPath() + a_rPrefabName + ".prefab";
			oAssetPath = AssetDatabase.GenerateUniqueAssetPath(oAssetPath);
			
			return oAssetPath;
		}
		
		public static void PingPrefabInProjectView(GameObject a_rGameObject)
		{
			GameObject rGameObjectToPing = a_rGameObject;
			if(a_rGameObject.transform.parent != null && a_rGameObject.transform.parent.parent != null)
			{
				rGameObjectToPing = PrefabUtility.FindPrefabRoot(a_rGameObject);
			}
			
			EditorGUIUtility.PingObject(rGameObjectToPing);
		}
	}
}
#endif