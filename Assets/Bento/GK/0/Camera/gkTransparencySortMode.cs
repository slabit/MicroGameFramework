using UnityEngine;
using System.Collections;

namespace gk
{
	[AddComponentMenu("GK/Camera/gkTransparencySortMode")]
	[ExecuteInEditMode()]
	public class gkTransparencySortMode : MonoBehaviour 
	{
		public TransparencySortMode transparencySortMode = TransparencySortMode.Orthographic;
		
		// Update is called once per frame
		void Update() 
		{
			if(GetComponent<Camera>() != null && GetComponent<Camera>().transparencySortMode != transparencySortMode)
			{
				GetComponent<Camera>().transparencySortMode = transparencySortMode;
			}
		}
	}
}