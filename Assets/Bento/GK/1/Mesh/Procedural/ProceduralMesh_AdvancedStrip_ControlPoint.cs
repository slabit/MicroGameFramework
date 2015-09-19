using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace gk
{
	[AddComponentMenu("GK/Mesh/ProceduralMesh_Strip_ControlPoint")]
	public class ProceduralMesh_AdvancedStrip_ControlPoint : MonoBehaviour
	{
		public enum Border
		{
			Top,
			Bottom
		}

		public Border border = Border.Top;

		public Color color = Color.white;
	}
}