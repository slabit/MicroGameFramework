using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace gk
{
	[AddComponentMenu("GK/Input/Touch/gkTouchControllerScheduler")]
	public class gkTouchControllerScheduler : MonoBehaviour
	{
		private List<gkTouchButtonController> m_oTouchControllersSortedByPriorities = new List<gkTouchButtonController>();
		
		static private gkTouchControllerScheduler ms_oInstance;
		
		private bool m_bIsSortingDirty;
		
		static public bool TryToRegister(gkTouchButtonController a_rTouchController)
		{
			if(ms_oInstance == null || a_rTouchController == null)
			{
				return false;	
			}
			else
			{
				ms_oInstance.Register(a_rTouchController);
				return true;
			}
		}
		
		static public bool TryToUnregister(gkTouchButtonController a_rTouchController)
		{
			if(ms_oInstance == null || a_rTouchController == null)
			{
				return false;	
			}
			else
			{
				ms_oInstance.Unregister(a_rTouchController);
				return true;
			}
		}
		
		static public void OnUpdateSorting(gkTouchButtonController a_rTouchController)
		{
			if(ms_oInstance != null && a_rTouchController != null)
			{
				ms_oInstance._OnUpdateSorting(a_rTouchController);
			}
		}
		
		private void Awake()
		{
			if(ms_oInstance == null)
			{
				ms_oInstance = this;
			}
			else
			{
				Debug.LogWarning("A singleton can only be instantiated once!");
				Destroy(gameObject);
				return;
			}
		}
		
		private void Update()
		{
			if(m_bIsSortingDirty)
			{
				SortTouchControllers();
			}
			UpdateControllers();
		}
		
		private void Register(gkTouchButtonController a_rTouchController)
		{
			m_oTouchControllersSortedByPriorities.Add(a_rTouchController);
			SetSortingDirty();
		}
		
		private void Unregister(gkTouchButtonController a_rTouchController)
		{
			m_oTouchControllersSortedByPriorities.Remove(a_rTouchController);
		}
		
		private void SortTouchControllers()
		{
			m_oTouchControllersSortedByPriorities.Sort(CompareTouchController);
			m_bIsSortingDirty = false;
		}
		
		private void _OnUpdateSorting(gkTouchButtonController a_rTouchController)
		{
			SetSortingDirty();
		}
		
		private void SetSortingDirty()
		{
			// MAYBE_TODO_SEV :
			// If we ever want to optimize the sorting update, we can sort just the controllers of the dirty layer
			// instead of updating the sorting of all the controllers
			m_bIsSortingDirty = true;
		}
		
		private void UpdateControllers()
		{
			foreach(gkTouchButtonController rTouchController in m_oTouchControllersSortedByPriorities)
			{
				if(rTouchController.gameObject.activeInHierarchy)
				{	
					rTouchController.UpdateController();
				}
			}
		}
		
		private static int CompareTouchController(gkTouchButtonController a_rTouchControllerA, gkTouchButtonController a_rTouchControllerB)
		{
			int iCompareTouchSortingLayerID =  -(a_rTouchControllerA.TouchSortingLayerID).CompareTo(a_rTouchControllerB.TouchSortingLayerID);
			if(iCompareTouchSortingLayerID == 0)
			{
				return -(a_rTouchControllerA.TouchSortingOrder).CompareTo(a_rTouchControllerB.TouchSortingOrder);
			}
			else
			{
				return iCompareTouchSortingLayerID;
			}
		}
	}
}