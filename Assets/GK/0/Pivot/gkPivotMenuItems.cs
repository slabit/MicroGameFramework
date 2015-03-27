#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace gk
{
	public class gkPivotMenuItems : Editor
	{
		[MenuItem("GK/Add Pivot")]
		private static void DoAddPivot()
		{
			GameObject[] rSelectedGameObjects = Selection.gameObjects;
			if(rSelectedGameObjects.Length > 0)
			{
				List<GameObject> oCreatedGameObjects = new List<GameObject>();
				foreach(GameObject rSelectedGameObject in rSelectedGameObjects)
				{
					oCreatedGameObjects.Add(DoAddPivot(rSelectedGameObject));
				}
				Selection.objects = oCreatedGameObjects.ToArray();
			}
		}
		
		// Add pivot
		private static GameObject DoAddPivot(GameObject a_rGameObject)
		{
			GameObject rPivot = new GameObject(a_rGameObject.name + "_Pivot");
			
			Transform rPivotTransform = rPivot.transform;
			Transform rSelectedTransform = a_rGameObject.transform;
			
			rPivotTransform.parent = rSelectedTransform.parent;
			rPivotTransform.localPosition = rSelectedTransform.localPosition;
			rPivotTransform.localRotation = rSelectedTransform.localRotation;
			rPivotTransform.localScale = rSelectedTransform.localScale;
			
			//rSelectedTransform.parent = rPivotTransform;
	
			Undo.RegisterCreatedObjectUndo(rPivot, "Add Pivot");
			Undo.SetTransformParent(rSelectedTransform, rPivotTransform, "Add Pivot");
	
			return rPivot;
		}
		
		[MenuItem("GK/Fix Pivot")]
		private static void DoFixPivot()
		{
			GameObject rSelected = Selection.activeGameObject;
			if(rSelected != null)
			{
				Transform rSelectedTransform = rSelected.transform;
				Transform rPivotTransform = rSelectedTransform.parent;
				
				if(rPivotTransform != null)
				{
					rSelectedTransform.parent = rPivotTransform.parent;
					
					rPivotTransform.parent = rSelectedTransform.parent;
					rPivotTransform.localPosition = rSelectedTransform.localPosition;
					rPivotTransform.localRotation = rSelectedTransform.localRotation;
					rPivotTransform.localScale = rSelectedTransform.localScale;
					
					rSelectedTransform.parent = rPivotTransform;
					
					Selection.activeGameObject = rSelected;
				}
			}
		}
	}
}
#endif