using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace gk
{
	public class StripVertex
	{
		public Vector3 position;
		public Color color;
		public float uvHeight;

		public StripVertex(Vector3 position, Color color, float uvHeight)
		{
			this.position = position;
			this.color = color;
			this.uvHeight = uvHeight;
		}
	}

	[AddComponentMenu("GK/Mesh/ProceduralMesh_Strip_Segment")]
	public class ProceduralMesh_Strip_Segment : MonoBehaviour
	{
		public Action onAnyPropertyChange;

		[SerializeField]
		Color topColor = Color.red;

		[SerializeField]
		Color bottomColor = Color.green;

		[SerializeField]
		float top = 0.5f;

		[SerializeField]
		float bottom = -0.5f;

		[SerializeField]
		float topExtension = 0.5f;
		
		[SerializeField]
		float bottomExtension = 0.5f;

		[SerializeField]
		float scale = 1.0f;

		Color lastTopColor;
		Color lastBottomColor;

		bool initialized;

		List<StripVertex> stripVertices = new List<StripVertex>();

		public Color BottomColor
		{
			get
			{
				return bottomColor;
			}

			set
			{
				if(lastBottomColor != value || initialized == false)
				{
					lastBottomColor = value;
					lastTopColor = value;
					OnAnyPropertyChange();
				}
			}
		}
		
		public Color TopColor
		{
			get
			{
				return topColor;
			}

			set
			{
				if(lastTopColor != value || initialized == false)
				{
					topColor = value;
					lastTopColor = value;
					OnAnyPropertyChange();
				}
			}
		}

		public List<StripVertex> StripVertices
		{
			get
			{
				stripVertices.Clear();

				if(bottomExtension > 0)
				{
					stripVertices.Add(new StripVertex(Vector2.up * (bottom - bottomExtension) * scale, bottomColor, 0.0f));
					stripVertices.Add(new StripVertex(Vector2.up * bottom * scale, bottomColor, 0.0f));
				}
				else if(bottomExtension < 0)
				{
					stripVertices.Add(new StripVertex(Vector2.up * bottom * scale, bottomColor, 0.0f));
					stripVertices.Add(new StripVertex(Vector2.up * (bottom - bottomExtension) * scale, bottomColor, 0.0f));
				}
				else
				{
					stripVertices.Add(new StripVertex(Vector2.up * bottom * scale, bottomColor, 0.0f));
				}

				if(topExtension < 0)
				{
					stripVertices.Add(new StripVertex(Vector2.up * (top + topExtension) * scale, topColor, 1.0f));
					stripVertices.Add(new StripVertex(Vector2.up * top * scale, topColor, 1.0f));
				}
				else if(topExtension > 0)
				{
					stripVertices.Add(new StripVertex(Vector2.up * top * scale, topColor, 1.0f));
					stripVertices.Add(new StripVertex(Vector2.up * (top + topExtension) * scale, topColor, 1.0f));
				}
				else
				{
					stripVertices.Add(new StripVertex(Vector2.up * top * scale, topColor, 1.0f));
				}

				return stripVertices;
			}
		}

		void Awake()
		{
			if(initialized == false)
			{
				lastTopColor = topColor;
				lastBottomColor = bottomColor;
				initialized = true;
			}
		}

		void LateUpdate()
		{
			TopColor = topColor;
			BottomColor = bottomColor;
		}

		void OnAnyPropertyChange()
		{
			if(onAnyPropertyChange != null)
			{
				onAnyPropertyChange();
			}
		}
	}
}