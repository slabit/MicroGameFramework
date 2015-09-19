using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[ExecuteInEditMode()]
	[AddComponentMenu("GK/ActivationSelector_LinkIndex")]
	public class ActivationSelector_LinkIndex : MonoBehaviour
	{
		public ActivationSelector master;

		public ActivationSelector slave;

		void LateUpdate()
		{
			if(slave == null || master == null)
				return;

			slave.SelectedIndex = master.SelectedIndex;
		}
	}
}