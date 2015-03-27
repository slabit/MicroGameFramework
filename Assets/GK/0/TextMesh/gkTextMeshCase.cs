using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace gk
{
	[AddComponentMenu("GK/TextMesh/gkTextMeshCase")]
	public class gkTextMeshCase : MonoBehaviour
	{
		public enum ETextCase
		{
			Free,
			UpperCase,
			LowerCase
		}
		
		public ETextCase textCase;
		
		private TextMesh m_rTextMesh;
		
		public void ApplyConstraint()
		{
			switch(textCase)
			{
			case ETextCase.UpperCase:
			{ 
				m_rTextMesh.text = m_rTextMesh.text.ToUpper();
			}
				break;
				
			case ETextCase.LowerCase:
			{ 
				m_rTextMesh.text = m_rTextMesh.text.ToLower();
			}
				break;
			}
		}
		
		private void Awake()
		{
			m_rTextMesh = GetComponent<TextMesh>();
		}
		
		private void Start()
		{
			ApplyConstraint();
		}
		
		private void LateUpdate()
		{
			ApplyConstraint();
		}
	}
}