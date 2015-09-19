using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace gk
{
	public class gkReference_OneOwner_Object : ScriptableObject
	{
		[HideInInspector]
		[SerializeField]
		private bool m_bOwnResource;
		
		[HideInInspector]
		[SerializeField]
		private Object m_rReference;

		//Hack_Sev in order to work with additive scene without the annoying cross reference issue pop up each save
		// don't serialize the owner which force mesh update event if not needed (Scene reload for example or script compilation)
		//[HideInInspector]
		//[SerializeField]
		private Object m_rOwner; 
		
		public Object Reference
		{
			get
			{
				return m_rReference;
			}
			
			set
			{
				m_rReference = value;
			}
		}
		
		public Object Owner
		{
			get
			{
				return m_rOwner;
			}
		}
		
		public void Create(Object a_rReference, Object a_rOwner, bool a_bOwnResource)
		{
			m_rReference = a_rReference;
			m_rOwner = a_rOwner;
			m_bOwnResource = a_bOwnResource;
		}
		
		private void OnDestroy()
		{
			if(m_bOwnResource)
			{
				if(m_rReference != null)
				{
					DestroyImmediate(m_rReference);
				}
			}
		}
	}
}
