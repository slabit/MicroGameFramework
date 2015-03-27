using UnityEngine;
using System.Collections;

namespace gk
{
	public class gkTouchUtility 
	{
		public static bool TryGetTouchByFingerId(int a_iFingerId, out Touch a_rTouch)
		{
			a_rTouch = default(Touch);
			
			// loop through the touches
			foreach(Touch rTouch in Input.touches)
			{
				if(rTouch.fingerId == a_iFingerId)
				{
					a_rTouch = rTouch;
					return true;
				}
			}
			
			return false;
		}
	}
}