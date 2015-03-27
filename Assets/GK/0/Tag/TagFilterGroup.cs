using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace gk
{
	[AddComponentMenu("GK/Layer/TagFilterGroup")]
	public class TagFilterGroup : MonoBehaviour 
	{
		[SerializeField]
		List<TagFilter> tagFilters = new List<TagFilter>();

		[SerializeField]
		int selectedFilterIndex;

		public int SelectedFilterIndex
		{
			get
			{
				return selectedFilterIndex;
			}

			set
			{
				selectedFilterIndex = value;
			}
		}

		public bool ContainsTag(GameObject taggedGameObject)
		{
			return tagFilters[SelectedFilterIndex].ContainsTag(taggedGameObject);
		}
	}
}
