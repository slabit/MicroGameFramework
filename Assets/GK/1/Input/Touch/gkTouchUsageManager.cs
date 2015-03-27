using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using gk;
//namespace gk
//{
	[AddComponentMenu("GK/Input/Touch/gkTouchUsageManager")]
	public class gkTouchUsageManager : MonoBehaviour
	{
		private gkMultiValueDictionary<int, gkTouchButtonController> m_oUsedTouches = new gkMultiValueDictionary<int, gkTouchButtonController>();
		
		private gkMultiValueDictionary<int, gkTouchButtonController> m_oUsedExclusivelyTouches = new gkMultiValueDictionary<int, gkTouchButtonController>();
		
		static private gkTouchUsageManager ms_oInstance;
		
		static public bool IsTouchUsedExclusively(int a_iFingerId)
		{
			if(ms_oInstance != null)
			{
				if(ms_oInstance.m_oUsedExclusivelyTouches.GetValues(a_iFingerId) != null)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}
		
		static public void StartUseTouch(int a_iFingerId, gkTouchButtonController a_rCallerComponent, bool a_bExclusively)
		{
			if(ms_oInstance != null)
			{
				ms_oInstance.m_oUsedTouches.Add(a_iFingerId, a_rCallerComponent);
				if(a_bExclusively)
				{
					ms_oInstance.m_oUsedExclusivelyTouches.Add(a_iFingerId, a_rCallerComponent);
				}
			}
		}
		
		static public void StopUseTouch(int a_iFingerId, gkTouchButtonController a_rCallerComponent)
		{
			if(ms_oInstance != null)
			{
				ms_oInstance.m_oUsedTouches.Remove(a_iFingerId, a_rCallerComponent);
				ms_oInstance.m_oUsedExclusivelyTouches.Remove(a_iFingerId, a_rCallerComponent);
			}
		}
	
		static public void CancelOtherUsedTouches(int a_iFingerId, gkTouchButtonController a_rCallerComponent, bool a_bOnlyThoseWithLesserPriority = true)
		{
			if(ms_oInstance != null)
			{
				HashSet<gkTouchButtonController> oTouchButtonControllers;
				if(ms_oInstance.m_oUsedTouches.TryGetValue(a_iFingerId, out oTouchButtonControllers))
				{
					// Copy the hash set because removal can occur
					gkTouchButtonController[] oTouchButtonControllersArray = new gkTouchButtonController[oTouchButtonControllers.Count];
					oTouchButtonControllers.CopyTo(oTouchButtonControllersArray);
					
					foreach(gkTouchButtonController rButtonController in oTouchButtonControllersArray)
					{
						if( a_rCallerComponent != rButtonController
						   && (a_bOnlyThoseWithLesserPriority == false || rButtonController.TouchSortingOrder > a_rCallerComponent.TouchSortingOrder))
						{
							rButtonController.CancelTouch();
						}
					}
				}
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
	}
//}