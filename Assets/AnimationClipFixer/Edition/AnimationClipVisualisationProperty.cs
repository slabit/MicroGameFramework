using UnityEngine;
using System.Collections.Generic;

namespace AnimationClipFixer
{
	public class AnimationClipVisualisationProperty : MonoBehaviour
	{
		[System.Serializable]
		public class PropertyType
		{
			public string name;
			public string assemblyName;
		}

		public PropertyType propertyType = new PropertyType();
		public string propertyName;
		public AnimationCurve curve;
	}
}
