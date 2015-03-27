using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace gk
{
	[System.Serializable]
	public class TagFilter
	{
		public List<string> tags;

		public bool exclude;

		public bool ContainsTag(GameObject taggedGameObject)
		{
			foreach(string tag in tags)
			{
				if(taggedGameObject.CompareTag(tag))
					return !exclude;
			}

			return exclude;
		}
	}
}
