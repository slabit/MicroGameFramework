#if UseGKExtensions
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[Serializable]
	public class gkInAppPurchaseProductID
	{	
		public string unityProductID;
		
		public string iosProductID;
		
		public string androidProductID;

		public bool consumable;
	}
}
#endif