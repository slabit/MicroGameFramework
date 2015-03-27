using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace gk
{
	[AddComponentMenu("GK/Mesh/ProceduralMesh_Strip")]
	public class ProceduralMesh_Strip : ProceduralMesh
	{
		public List<ProceduralMesh_Strip_Segment> segments = new List<ProceduralMesh_Strip_Segment>();

		public bool flipUV;

		public bool autoFillInEditMode;

		public void AutoFill()
		{
			segments.Clear();
			segments.AddRange(GetComponentsInChildren<ProceduralMesh_Strip_Segment>());
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

			if(segments.Count <= 0)
				return;

			List<Vector3> vertices = new List<Vector3>();
			List<Color> colors = new List<Color>();
			List<Vector2> uvs = new List<Vector2>();
			List<int> triangles = new List<int>();
			bool toggleUv = true;
			int currentVertexIndex = 0;
			List<int> lastSegmentVertexIndices = new List<int>();
			List<int> currentSegmentVertexIndices = new List<int>();
			foreach(ProceduralMesh_Strip_Segment segment in segments)
			{
				List<StripVertex> stripVertices = segment.StripVertices;

				Vector2 uv;
				if(toggleUv)
				{
					uv = Vector2.zero;
				}
				else
				{
					uv = Vector2.right;
				}


				currentSegmentVertexIndices.Clear();
				foreach(StripVertex stripVertex in stripVertices)
				{
					vertices.Add(GetPosition(segment.transform.TransformPoint(stripVertex.position)));
					colors.Add(stripVertex.color);
					uv.y = stripVertex.uvHeight;

					if(flipUV)
						uv.y = 1.0f - uv.y;
					
					uvs.Add(uv);

					currentSegmentVertexIndices.Add(currentVertexIndex);

					++currentVertexIndex;
				}

				toggleUv = !toggleUv;

				int lastCount = lastSegmentVertexIndices.Count;
				int currentCount = currentSegmentVertexIndices.Count;
				int minCount = Mathf.Min(lastCount, currentCount);
				if(minCount > 0)
				{
					int maxCount = Mathf.Max(lastCount, currentCount);
					for(int i = 0; i < minCount - 1; ++i)
					{
						ProceduralMeshUtility.AddQuad(triangles,
						                              lastSegmentVertexIndices[i],
						                              lastSegmentVertexIndices[i+1],
						                              currentSegmentVertexIndices[i+1],
						                              currentSegmentVertexIndices[i]);
					}
					if(lastCount > currentCount)
					{
						int currentVertexIndex_max = currentSegmentVertexIndices[minCount - 1];
						for(int i = minCount - 1; i < maxCount - 1; ++i)
						{
							ProceduralMeshUtility.AddTriangle(triangles,
							                              lastSegmentVertexIndices[i],
							                              lastSegmentVertexIndices[i+1],
							                              currentVertexIndex_max);
						}
					}
					else if(currentCount > lastCount)
					{
						int lastVertexIndex_max = lastSegmentVertexIndices[minCount - 1];
						for(int i = minCount - 1; i < maxCount - 1; ++i)
						{
							ProceduralMeshUtility.AddTriangle(triangles,
							                                  lastVertexIndex_max,
							                                  currentSegmentVertexIndices[i+1],
							                                  currentSegmentVertexIndices[i]);
						}
					}
				}

				List<int> swapSegmentVertexIndices;
				swapSegmentVertexIndices = lastSegmentVertexIndices;
				lastSegmentVertexIndices = currentSegmentVertexIndices;
				currentSegmentVertexIndices = swapSegmentVertexIndices;
			}

			mesh.vertices = vertices.ToArray();

			mesh.colors = colors.ToArray();
		
			mesh.uv = uvs.ToArray();

			mesh.triangles = triangles.ToArray();
		}

		protected override void OnAwake()
		{
			if(Application.isPlaying)
			{
				foreach(ProceduralMesh_Strip_Segment segment in segments)
				{
					if(segment != null)
						segment.onAnyPropertyChange += OnAnyPropertyChange;
				}
			}
		}
		
		protected override void OnAwakeEnd()
		{
			if(Application.isPlaying)
			{
				foreach(ProceduralMesh_Strip_Segment segment in segments)
				{
					if(segment != null)
						segment.onAnyPropertyChange -= OnAnyPropertyChange;
				}
			}
		}

		void OnAnyPropertyChange()
		{
			AskForMeshRebuild();
		}

		Vector3 GetPosition(Vector3 position)
		{
			return transform.InverseTransformPoint(position);
		}
	}
}