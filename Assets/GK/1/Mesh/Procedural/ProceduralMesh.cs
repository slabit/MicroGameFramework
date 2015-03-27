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
	[AddComponentMenu("GK/Mesh/ProceduralMesh")]
	public abstract class ProceduralMesh : MonoBehaviour
	{
		[HideInInspector]
		[SerializeField]
		gkReference_OneOwner_Object refMeshInstance;
		
		gkReference_OneOwner_Object refMeshInstance_Last;

		bool runtimeDirty;

		bool awaken;

		public bool Awaken
		{
			get
			{
				return awaken;
			}
		}

		public Mesh MeshInstance
		{
			get
			{
				if(refMeshInstance == null || refMeshInstance.Owner != this)
				{
					refMeshInstance = ScriptableObject.CreateInstance<gkReference_OneOwner_Object>();

					Mesh mesh = new Mesh();
					mesh.MarkDynamic();
					
					refMeshInstance.Create(mesh, this, true);
#if UNITY_EDITOR
					EditorUtility.SetDirty(this);
#endif				
					CheckForLostReferences(ref refMeshInstance, ref refMeshInstance_Last);
					
					UpdateMesh();
				}
				return refMeshInstance.Reference as Mesh;
			}
		}

		public void AskForMeshRebuild()
		{
			OnMeshNeedToBeRebuilt();
		}

		protected virtual void OnBuildMesh(Mesh mesh)
		{
		}

		protected virtual void OnAwake()
		{
		}

		protected virtual void OnAwakeEnd()
		{
		}

		void Awake()
		{	
			if(awaken == false)
			{
				TotalUpdate();
				awaken = true;
				OnAwake();
			}
		}
		
		void OnDestroy()
		{		
			if(awaken)
			{
				OnAwakeEnd();
			}

			CheckForLostReferences();
			
			if(refMeshInstance != null)
			{
				DestroyImmediate(refMeshInstance);
			}
		}
		
		void LateUpdate()
		{	
			if(Application.isPlaying)
			{
				if(runtimeDirty)
				{
					UpdateMesh();
					runtimeDirty = false;
				}
			}
			else
			{
				TotalUpdate();
			}
		}

		void BuildMesh(Mesh mesh)
		{
			mesh.Clear();
			
			OnBuildMesh(mesh);
			
			mesh.RecalculateBounds();
			mesh.RecalculateNormals();
		}
		
		private void OnMeshNeedToBeRebuilt()
		{
			if(Application.isPlaying)
			{
				runtimeDirty = true;
			}
			else
			{
				UpdateMesh();
			}
		}
		
		private void TotalUpdate()
		{
			UpdateMesh();
			UpdateMeshRendererAndFilter();
			CheckForLostReferences();
		}
		
		private void CheckForLostReferences()
		{
			CheckForLostReferences(ref refMeshInstance, ref refMeshInstance_Last);
		}
		
		private void CheckForLostReferences(ref gkReference_OneOwner_Object refObject, ref gkReference_OneOwner_Object refObject_Last)
		{
			if(refObject_Last != refObject)
			{
				if(refObject_Last != null && refObject_Last.Owner == this)
				{
					DestroyImmediate(refObject_Last);
				}
				refObject_Last = refObject;
			}
		}
		
		private void UpdateMeshRendererAndFilter()
		{
			MeshFilter meshFilter = GetComponent<MeshFilter>();
			if(meshFilter == null)
			{
				meshFilter = gameObject.AddComponent<MeshFilter>();
			}
			
			Mesh instanceMesh = MeshInstance; 
			Mesh rMeshFilterMesh = meshFilter.sharedMesh;
			if(rMeshFilterMesh != instanceMesh)
			{
				meshFilter.sharedMesh = instanceMesh;
			}
			
			MeshRenderer meshRenderer = GetComponent<MeshRenderer>();	
			if(meshRenderer == null)
			{
				meshRenderer = gameObject.AddComponent<MeshRenderer>();
			}
		}
		
		private void UpdateMesh()
		{	
			Mesh meshInstance = MeshInstance;

			BuildMesh(meshInstance);
			
			meshInstance.name = gameObject.name + " Procedural Mesh";
		}
	}
}