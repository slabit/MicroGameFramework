using UnityEngine;
using System.Collections;

namespace gk
{
	public class gkTouch 
	{
		private static bool m_bDeactivateMultiTouch = false;
		
		public static bool MultiTouchEnabled
		{
			get
			{
				return Input.multiTouchEnabled && (m_bDeactivateMultiTouch == false);
			}
		}
	}
}