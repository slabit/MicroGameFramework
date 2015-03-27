using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace gk
{
	public enum ELineComponentFormat
	{
		None,
		Uniform,
		ByVertex
	}
	
	public abstract class gkLineBuilder : MonoBehaviour
	{
		public class LineVertexFormat
		{
			public ELineComponentFormat colorFormat;
			
			public ELineComponentFormat uvFormat;
		}
		
		public Action onLineNeedToBeRebuilt;
		
		protected LineVertexFormat m_eLineVertexFormat = new LineVertexFormat();
		
		public void AskForLineRebuild()
		{
			if(onLineNeedToBeRebuilt != null)
			{
				onLineNeedToBeRebuilt();
			}
		}
		
		public void BuildMesh(Mesh a_rMesh)
		{
			a_rMesh.Clear();
			
			Vector3[] rVertices;
			Color[] rColors;
			Vector2[] rUvs;
			GetLineComponents(out rVertices, out rColors, out rUvs);
			
			a_rMesh.vertices = rVertices;
			a_rMesh.colors = rColors;
			a_rMesh.uv = rUvs;
			
			a_rMesh.RecalculateBounds();
			a_rMesh.RecalculateNormals();
		}
		
		protected virtual void UpdateLineVertexFormat()
		{
		}
		
		protected virtual Vector3[] GetLineVertices()
		{
			return new Vector3[0];
		}
		
		protected virtual Color[] GetLineVerticesColors()
		{
			return new Color[0];
		}
		
		protected virtual Color GetLineVerticesColor()
		{
			return Color.white;
		}
		
		protected virtual Vector2[] GetLineVerticesUvs()
		{
			return new Vector2[0];
		}
		
		protected virtual Vector2 GetLineVerticesUv()
		{
			return Vector2.zero;
		}
		
		private void GetLineComponents(out Vector3[] a_rVertices, out Color[] a_rColors, out Vector2[] a_rUVs)
		{
			UpdateLineVertexFormat();
				
			a_rColors = null;
			a_rUVs = null;
			
			// Position
			a_rVertices = GetLineVertices();
			
			int iVertexCount = a_rVertices.Length;
			
			// Color
			switch(m_eLineVertexFormat.colorFormat)
			{
				case ELineComponentFormat.None:
				{
					a_rColors = null;
				}
				break;
				
				case ELineComponentFormat.Uniform: 
				{
					a_rColors = new Color[iVertexCount];
					Color oColor = GetLineVerticesColor();
					for(int i = 0; i < iVertexCount; i++)
					{
						a_rColors[i] = oColor;
					}
				}
				break;
				
				case ELineComponentFormat.ByVertex: 
				{
					a_rColors = GetLineVerticesColors();
				}
				break;
			}
			
			// UVs
			switch(m_eLineVertexFormat.uvFormat)
			{
				case ELineComponentFormat.None:
				{
					a_rUVs = null;
				}
				break;
				
				case ELineComponentFormat.Uniform: 
				{
					a_rUVs = new Vector2[iVertexCount];
					Vector2 f2UV = GetLineVerticesUv();
					for(int i = 0; i < iVertexCount; i++)
					{
						a_rUVs[i] = f2UV;
					}
				}
				break;
				
				case ELineComponentFormat.ByVertex: 
				{
					a_rUVs = GetLineVerticesUvs();
				}
				break;
			}
		}
	}
}