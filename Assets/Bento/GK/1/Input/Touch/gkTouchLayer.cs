using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace gk
{
	[AddComponentMenu("GK/Input/Touch/gkTouchLayer")]
	public class gkTouchLayer : MonoBehaviour
	{
		private Dictionary<string, int> m_oTouchLayerIDByName = new Dictionary<string, int>();
		
		private List<string> m_oTouchLayerNames = new List<string>();
		
		static private gkTouchLayer ms_oInstance;
		
		static public string TouchLayerIDToName(int a_iTouchLayerID)
		{
			return Instance._TouchLayerIDToName(a_iTouchLayerID);
		}
		
		static public int TouchLayerNameToID(string a_oTouchLayerName)
		{
			return Instance._TouchLayerNameToID(a_oTouchLayerName);
		}
		
		static public void AddLayerOnTop(string a_oTouchLayerName)
		{
			Instance._AddLayerOnTop(a_oTouchLayerName);
		}
		
		static public void RemoveLayer(string a_oTouchLayerName)
		{
			Instance._RemoveLayer(a_oTouchLayerName);
		}
		
		private string _TouchLayerIDToName(int a_iTouchLayerID)
		{
			if(a_iTouchLayerID < 0 || a_iTouchLayerID >= m_oTouchLayerNames.Count)
			{
				return "";
			}
			else
			{
				return m_oTouchLayerNames[a_iTouchLayerID];
			}
		}
		
		private int _TouchLayerNameToID(string a_oTouchLayerName)
		{
			int iLayerID;
			if(m_oTouchLayerIDByName.TryGetValue(a_oTouchLayerName, out iLayerID))
			{	
				return iLayerID;
			}
			else
			{
				return -1;
			}
		}
		
		private void _AddLayerOnTop(string a_oTouchLayerName)
		{
			RemoveLayer(a_oTouchLayerName);
			
			m_oTouchLayerNames.Add(a_oTouchLayerName);
			m_oTouchLayerIDByName.Add(a_oTouchLayerName, m_oTouchLayerNames.Count - 1);
		}
		
		private void _RemoveLayer(string a_oTouchLayerName)
		{
			int iLayerID = TouchLayerNameToID(a_oTouchLayerName);
			if(iLayerID != -1)
			{
				m_oTouchLayerIDByName.Remove(a_oTouchLayerName);
				m_oTouchLayerNames.RemoveAt(iLayerID);
			}
		}
		
		private static gkTouchLayer Instance
		{
			get
			{
				if(ms_oInstance == null)
				{
					CreateInstance();
				}
				
				return ms_oInstance;
			}
		}
		
		private void Awake()
		{
			if(ms_oInstance == null)
			{
				ms_oInstance = this;
			}
			else
			{
				Debug.LogWarning("A singleton can only be instantiated once!");
				Destroy(gameObject);
				return;
			}
		}
		
		private static void CreateInstance()
		{
			GameObject rInstanceGameObject = new GameObject(typeof(gkTouchLayer).Name);
			rInstanceGameObject.AddComponent<gkTouchLayer>();
		}
	}
}