using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace gk
{
	[AddComponentMenu("GK/Mesh/ProceduralMesh_PlaneGradient_2Colors")]
	public class ProceduralMesh_PlaneGradient_2Colors : ProceduralMesh
	{
		public gkRectangleShape rectangleShape;

		[SerializeField]
		Color colorTop = Color.red;

		[SerializeField]
		Color colorBottom = Color.green;

		public Color ColorTop
		{
			get
			{
				return colorTop;
			}
			
			set
			{
				if(value != colorTop)
				{
					colorTop = value;
					AskForMeshRebuild();
				}
			}
		}

		public Color ColorBottom
		{
			get
			{
				return colorBottom;
			}
			
			set
			{
				if(value != colorTop)
				{
					colorBottom = value;
					AskForMeshRebuild();
				}
			}
		}

		public void SetColors(Color colorTop, Color colorBottom)
		{
			this.colorTop = colorTop;
			this.colorBottom = colorBottom;

			AskForMeshRebuild();
		}

		protected override void OnBuildMesh(Mesh mesh)
		{
			if(rectangleShape == null)
				return;

			List<Vector3> vertices = rectangleShape.GetLocalVertices();

			mesh.vertices = vertices.ToArray();

			float uvScale = 1.0f;
			mesh.uv = new Vector2[]
			{
				new Vector2(0.0f, 0.0f),
				new Vector2(0.0f, uvScale),
				new Vector2(uvScale, uvScale),
				new Vector2(0.0f, 0.0f),
			};

			mesh.colors = new Color[]
			{
				colorBottom,
				colorTop,
				colorTop,
				colorBottom
			};

			List<int> triangles = new List<int>();
			ProceduralMeshUtility.AddQuad(triangles);
			mesh.triangles = triangles.ToArray();
		}
	}
}