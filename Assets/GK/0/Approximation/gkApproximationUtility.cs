using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	public static class gkApproximationUtility 
	{
		public static bool Approximately(float a_fA, float a_fB, float a_fEpsilon)
		{
			return (a_fA > a_fB - a_fEpsilon) && (a_fA < a_fB + a_fEpsilon);
		}
	}
}