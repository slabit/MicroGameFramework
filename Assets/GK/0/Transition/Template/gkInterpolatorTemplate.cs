using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[Serializable]
	public class gkInterpolatorTemplate : gkInterpolator
	{
		protected override bool UpdateInterpolatedValue()
		{
			return true;
		}
	}
}