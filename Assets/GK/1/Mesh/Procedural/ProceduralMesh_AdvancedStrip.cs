using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace gk
{
	[AddComponentMenu("GK/Mesh/ProceduralMesh_AdvancedStrip")]
	public class ProceduralMesh_AdvancedStrip : ProceduralMesh
	{
		public List<ProceduralMesh_AdvancedStrip_ControlPoint> top = new List<ProceduralMesh_AdvancedStrip_ControlPoint>();

		public List<ProceduralMesh_AdvancedStrip_ControlPoint> bottom = new List<ProceduralMesh_AdvancedStrip_ControlPoint>();

		public bool autoFillInEditMode;

		public void AutoFill()
		{
			top.Clear();
			bottom.Clear();
			foreach(ProceduralMesh_AdvancedStrip_ControlPoint controlPoint in GetComponentsInChildren<ProceduralMesh_AdvancedStrip_ControlPoint>())
			{
				switch(controlPoint.border)
				{
				case ProceduralMesh_AdvancedStrip_ControlPoint.Border.Top:
					top.Add(controlPoint);
					break;

				case ProceduralMesh_AdvancedStrip_ControlPoint.Border.Bottom:
					bottom.Add(controlPoint);
					break;
				}
			}
		}

		protected override void OnBuildMesh(Mesh mesh)
		{
			if(Application.isPlaying == false)
			{
				if(autoFillInEditMode)
				{
					AutoFill();
				}
			}

			List<ProceduralMesh_AdvancedStrip_ControlPoint> min = null;
			List<ProceduralMesh_AdvancedStrip_ControlPoint> max = null;

			if(top.Count >= bottom.Count)
			{
				max = top;
				min = bottom;
			}
			else
			{
				min = bottom;
				max = top;
			}

			int maxCount = max.Count;
			int minCount = min.Count;

			if(maxCount < 2)
				return;

			if(minCount < 1)
				return;

			List<Vector3> vertices = new List<Vector3>();
			List<Color> colors = new List<Color>();
			foreach(ProceduralMesh_AdvancedStrip_ControlPoint controlPoint in min)
			{
				vertices.Add(GetPosition(controlPoint));
				colors.Add(controlPoint.color);
			}
			foreach(ProceduralMesh_AdvancedStrip_ControlPoint controlPoint in max)
			{
				vertices.Add(GetPosition(controlPoint));
				colors.Add(controlPoint.color);
			}

			mesh.vertices = vertices.ToArray();

			mesh.colors = colors.ToArray();

			mesh.uv = new Vector2[vertices.Count];

			List<int> triangles = new List<int>();
			for(int i = 0; i < minCount - 1; ++i)
			{
				ProceduralMeshUtility.AddQuad(triangles, i, minCount + i, minCount + i + 1, i + 1);
			}
			for(int i = minCount; i < maxCount; ++i)
			{
				ProceduralMeshUtility.AddTriangle(triangles, minCount - 1, minCount + i - 1, minCount + i);
			}

			mesh.triangles = triangles.ToArray();
		}

		Vector3 GetPosition(ProceduralMesh_AdvancedStrip_ControlPoint controlPoint)
		{
			return transform.InverseTransformPoint(controlPoint.transform.position);
		}
	}
}