using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	public static class gkPrefabBuilderUtility 
	{	
		public static ComponentType BuildPrefab<ComponentType>(ComponentType a_rPrefab, Component a_rParent) where ComponentType : Component
		{
			ComponentType oInstance = Component.Instantiate(a_rPrefab, a_rParent.transform.position, a_rParent.transform.rotation) as ComponentType;
			Transform rInstanceTransform = oInstance.transform;
			rInstanceTransform.parent = a_rParent.transform;
			rInstanceTransform.localScale = Vector3.one;
			
			return oInstance;
		}

		public static GameObject BuildPrefab(GameObject a_rPrefab, Component a_rParent)
		{
			GameObject oInstance = Component.Instantiate(a_rPrefab, a_rParent.transform.position, a_rParent.transform.rotation) as GameObject;
			Transform rInstanceTransform = oInstance.transform;
			rInstanceTransform.parent = a_rParent.transform;
			rInstanceTransform.localScale = Vector3.one;
			
			return oInstance;
		}
	}
}