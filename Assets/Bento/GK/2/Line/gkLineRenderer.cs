using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using System;
#endif

namespace gk
{
	[ExecuteInEditMode()]
	[AddComponentMenu("GK/Line/gkLineRenderer")]
	public class gkLineRenderer : MonoBehaviour
	{
		public gkLineBuilder lineBuilder;
		
		public bool closeLine;
		
		[HideInInspector]
		[SerializeField]
		private gkReference_OneOwner_Object m_oRefMeshInstance;
		
		private gkReference_OneOwner_Object m_rRefMeshInstance_Last;
		
		public Mesh MeshInstance
		{
			get
			{
				if(m_oRefMeshInstance == null || m_oRefMeshInstance.Owner != this)
				{
					m_oRefMeshInstance = ScriptableObject.CreateInstance<gkReference_OneOwner_Object>();
					
					Mesh rMesh = new Mesh();
					rMesh.MarkDynamic();
					
					m_oRefMeshInstance.Create(rMesh, this, true);
	#if UNITY_EDITOR
					EditorUtility.SetDirty(this);
	#endif				
					CheckForLostReferences(ref m_oRefMeshInstance, ref m_rRefMeshInstance_Last);
					
					UpdateMesh();
				}
				return m_oRefMeshInstance.Reference as Mesh;
			}
		}
		
		private void Awake()
		{	
			TotalUpdate();
			
			if(Application.isPlaying)
			{
				lineBuilder.onLineNeedToBeRebuilt += OnLineNeedToBeRebuilt;	
			}
		}
		
		private void OnDestroy()
		{		
			CheckForLostReferences();
			
			if(m_oRefMeshInstance != null)
			{
				DestroyImmediate(m_oRefMeshInstance);
			}
			
			lineBuilder.onLineNeedToBeRebuilt -= OnLineNeedToBeRebuilt;	
		}
		
		private void LateUpdate()
		{	
			if(Application.isPlaying == false)
			{
				TotalUpdate();
			}
		}
		
		private void OnLineNeedToBeRebuilt()
		{
			UpdateMesh();
		}
		
		private void TotalUpdate()
		{
			UpdateMesh();
			UpdateMeshRendererAndFilter();
			CheckForLostReferences();
		}
		
		private void CheckForLostReferences()
		{
			CheckForLostReferences(ref m_oRefMeshInstance, ref m_rRefMeshInstance_Last);
		}
		
		private void CheckForLostReferences(ref gkReference_OneOwner_Object a_rRef, ref gkReference_OneOwner_Object a_rRefLast)
		{
			if(a_rRefLast != a_rRef)
			{
				if(a_rRefLast != null && a_rRefLast.Owner == this)
				{
					DestroyImmediate(a_rRefLast);
				}
				a_rRefLast = a_rRef;
			}
		}
		
		private void UpdateMeshRendererAndFilter()
		{
			MeshFilter rMeshFilter = GetComponent<MeshFilter>();
			if(rMeshFilter == null)
			{
				rMeshFilter = gameObject.AddComponent<MeshFilter>();
			}
			
			Mesh rInstanceMesh = MeshInstance; 
			Mesh rMeshFilterMesh = rMeshFilter.sharedMesh;
			if(rMeshFilterMesh != rInstanceMesh)
			{
				rMeshFilter.sharedMesh = rInstanceMesh;
			}
			
			MeshRenderer rMeshRenderer = GetComponent<MeshRenderer>();	
			if(rMeshRenderer == null)
			{
				rMeshRenderer = gameObject.AddComponent<MeshRenderer>();
			}
		}
		
		private void UpdateMesh()
		{	
			Mesh rMeshInstance = MeshInstance;
			
			if(lineBuilder == null)
			{
				rMeshInstance.Clear();
			}
			else
			{
				lineBuilder.BuildMesh(rMeshInstance);
				int iVertexCount = rMeshInstance.vertexCount;
				if(iVertexCount > 1)
				{
					int iIndexCountTotal = (iVertexCount - 1) * 2;
					if(closeLine)
					{
						iIndexCountTotal += 2;
					}
					
					int[] oIndices = new int[iIndexCountTotal];
					int iCurrentIndexCount = 0;
					for(int i = 0; i < iVertexCount - 1; i++)
					{
						oIndices[iCurrentIndexCount] = i;
						oIndices[iCurrentIndexCount + 1] = i + 1;
						iCurrentIndexCount += 2;
					}
					if(closeLine)
					{
						oIndices[iCurrentIndexCount] =  iVertexCount - 1;
						oIndices[iCurrentIndexCount + 1] =  0;
						iCurrentIndexCount += 2;
					}
					rMeshInstance.SetIndices(oIndices, MeshTopology.Lines, 0);
				}
			}
			
			rMeshInstance.name = gameObject.name + " Line Mesh";
		}
	}
}