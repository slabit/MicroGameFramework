#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEditor;

namespace gk
{
	public static class gkEditorAssetPathUtility 
	{	
		public static string GetLocalSelectedAssetFolderPath()
		{
			return GetLocalAssetFolderPath(Selection.activeObject);
		}
		
		public static string GetGlobalSelectedAssetFolderPath()
		{
			return GetGlobalAssetFolderPath(Selection.activeObject);
		}
		
		public static string GetLocalAssetFolderPath(UnityEngine.Object a_rAsset, bool lastSeparator = true)
		{
			return GlobalToLocalAssetPath(GetGlobalAssetFolderPath(a_rAsset, lastSeparator));
		}
		
		public static string GetGlobalAssetFolderPath(UnityEngine.Object a_rAsset, bool lastSeparator = true)
		{
			string oAssetPath = AssetDatabase.GetAssetPath(a_rAsset);
			string oAssetFolderPath = "";
			if (oAssetPath.Length > 0)
			{
				oAssetFolderPath = Application.dataPath + "/" + oAssetPath.Substring(7);
				oAssetFolderPath = oAssetFolderPath.Replace('\\', '/');
				if ((File.GetAttributes(oAssetFolderPath) & FileAttributes.Directory) != FileAttributes.Directory)
				{
					for (int i = oAssetFolderPath.Length - 1; i > 0; --i)
					{
						if (oAssetFolderPath[i] == '/')
						{
							oAssetFolderPath = oAssetFolderPath.Substring(0, i);
							break;
						}
					}
				}
				if(lastSeparator)
					oAssetFolderPath += "/";
			}
			else
			{
				oAssetFolderPath = Application.dataPath + "/";
			}
			
			return oAssetFolderPath;
		}
		
		public static string LocalToResourcesAssetPath(string a_rLocalPath)
		{
			string[] oSplits = a_rLocalPath.Split(new string[]{"Resources/"}, StringSplitOptions.RemoveEmptyEntries);
			if(oSplits.Length > 1)
			{
				return oSplits[1];
			}
			else
			{
				return "";
			}
		}
		
		public static string LocalToGlobalAssetPath(string a_rLocalPath)
		{
			return Application.dataPath.Replace("Assets", "") + a_rLocalPath;
		}
		
		public static string GlobalToLocalAssetPath(string a_rGlobalPath)
		{
			return a_rGlobalPath.Replace(Application.dataPath.Replace("Assets", ""), "");
		}
		
		public static string GetGlobalAssetPath(UnityEngine.Object a_rAsset)
		{
			return LocalToGlobalAssetPath(GetLocalAssetPath(a_rAsset));
		}
		
		public static string GetLocalAssetPath(UnityEngine.Object a_rAsset)
		{
			return AssetDatabase.GetAssetPath(a_rAsset);
		}
	}
}
#endif