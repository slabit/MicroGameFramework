using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	[Serializable]
	public class ActivationGroupSelection
	{
		public ActivationSelector selector;
		public int indexToSelect;

		int savedIndex;

		public void SaveCurrentIndex()
		{
			savedIndex = selector.SelectedIndex;
		}

		public void RestoreSavedIndex()
		{
			selector.SelectedIndex = savedIndex;
		}

		public void Select()
		{
			selector.SelectedIndex = indexToSelect;
		}

		public ActivationGroupSelection(int indexToSelect)
		{
			this.indexToSelect = indexToSelect;
		}

		public ActivationGroupSelection()
		{
			this.indexToSelect = 0;
		}
	}
}