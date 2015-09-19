using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace gk
{
	[ExecuteInEditMode()]
	[AddComponentMenu("GK/TextMesh/gkTextMeshColor")]
	public class gkTextMeshColor : MonoBehaviour 
	{
		[SerializeField]
		[HideInInspector]
		private TextMesh m_rTextMesh;
		
		public string text = "Hello World";
		
		public Color color = Color.white;
		
		[SerializeField]
		[HideInInspector]
		private Color m_fLastColor;
		
		[SerializeField]
		[HideInInspector]
		private string m_oLastText;
		
		public Color Color
		{
			get
			{
				return color;
			}
			
			set
			{
				if(color != value)
				{
					color = value;
					ApplyColor();
				}
			}
		}
		
		public void SetText(string a_oText)
		{
			text = a_oText;
			ApplyColor();
		}
		
		public void ApplyColor()
		{
			GetTextMeshIfNeeded();
			if(m_rTextMesh != null)
			{
				m_rTextMesh.text = gkTextMeshUtility.ComputeHtmlColoredText(text, color);
				m_fLastColor = color;
				m_oLastText = text;
			}
		}
		
		public TextMesh TextMesh
		{
			get
			{
				return m_rTextMesh;
			}
		}
		
		private void Update()
		{
			if(color != m_fLastColor || text != m_oLastText)
			{
				ApplyColor();
			}
		}
		
		private void GetTextMeshIfNeeded()
		{
			if(m_rTextMesh == null)
			{
				m_rTextMesh = GetComponent<TextMesh>();
			}
		}
	}
}