using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using gk;

[CustomEditor(typeof(gkInputManager))]
[CanEditMultipleObjects]
public class gkInputManagerInspector : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		if(GUILayout.Button("Auto Fill"))
		{
			foreach(Object rTarget in targets)
			{
				gkInputManager rComponent = rTarget as gkInputManager;
				if(rComponent != null)
				{
					rComponent.EditorOnly_AutoFill();
				}
			}
		}
	}
}