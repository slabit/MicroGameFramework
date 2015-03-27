using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	public static class gkDelegateUtility 
	{
		public static bool HasFunctionAlreadyBeenAddedToDelegate(Action a_rAction, Action a_rFunction)
		{
			if(a_rAction != null)
			{
				foreach(Delegate rDelegate in a_rAction.GetInvocationList())
				{
					if(rDelegate == a_rFunction)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}