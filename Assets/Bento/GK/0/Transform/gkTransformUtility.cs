using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	public static class gkTransformUtility 
	{
		public static void Attach(Component a_rChild, Component a_rParent)
		{
			Attach(a_rChild.transform, a_rParent.transform);
		}
		
		public static void Attach(Transform a_rChild, Transform a_rParent)
		{
			a_rChild.parent = a_rParent;
			a_rChild.localPosition = Vector3.zero;
			a_rChild.localRotation = Quaternion.identity;
			a_rChild.localScale = Vector3.one;
		}
		
		public static void DetachChildrenFromParent(Transform a_rParent)
		{
			a_rParent.DetachChildren();
		}
		
		public static void DetachChildrenFromParentToGreatParent(Transform a_rParent)
		{
			DetachChildrenFromParent(a_rParent, a_rParent.parent);
		}
		
		public static void DetachChildrenFromParent(Transform a_rParent, Transform a_rNewParent)
		{
			foreach(Transform rChild in a_rParent)
			{
				rChild.parent = a_rNewParent;
			}
		}
	}
}