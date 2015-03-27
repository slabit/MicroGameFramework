#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;

namespace gk
{
	public static class gkEditorCreateAssetUtility 
	{
		public static ScriptableObjectType CreateScriptableObject<ScriptableObjectType>(string a_oAssetName, string a_oAssetFolderPath) where ScriptableObjectType : ScriptableObject
		{
			ScriptableObjectType oScriptableObject = ScriptableObject.CreateInstance<ScriptableObjectType>();
			oScriptableObject.name = a_oAssetName;
			
			SaveAsset(oScriptableObject, a_oAssetFolderPath);
			
			return oScriptableObject;
		}
			
		public static ScriptableObjectType CreateScriptableObjectInSelectedProjectFolder<ScriptableObjectType>(string a_oAssetName) where ScriptableObjectType : ScriptableObject
		{
			ScriptableObjectType oScriptableObject = ScriptableObject.CreateInstance<ScriptableObjectType>();
			oScriptableObject.name = a_oAssetName;
					
			SaveAssetInSelectedProjectFolder(oScriptableObject);
			
			return oScriptableObject;
		}
		
		public static string SaveAsset(UnityEngine.Object a_oObject, string a_oAssetFolderPath)
		{
			string oAssetPath = a_oAssetFolderPath + "/" + a_oObject.name + ".asset";
			AssetDatabase.CreateAsset(a_oObject, oAssetPath);
			
			return oAssetPath;
		}
		
		public static string SaveAssetInSelectedProjectFolder(UnityEngine.Object a_rObjectToSave)
		{
			string oAssetPath = GetCreationPathForNewAssetInSelectedProjectFolder(a_rObjectToSave.name);
			
			AssetDatabase.CreateAsset(a_rObjectToSave, oAssetPath);
			
			return oAssetPath;
		}
		
		public static string GetCreationPathForNewAssetInSelectedProjectFolder(string a_oAssetName)
		{
			string oAssetPath = gkEditorAssetPathUtility.GetLocalSelectedAssetFolderPath() + a_oAssetName + ".asset";
			oAssetPath = AssetDatabase.GenerateUniqueAssetPath(oAssetPath);
			
			return oAssetPath;
		}
	}
}
#endif