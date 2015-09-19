using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace gk
{
	[ExecuteInEditMode()]
	[AddComponentMenu("GK/Mesh/gkVertexColor")]
	public class gkVertexColor : MonoBehaviour
	{
		public static string mc_oMeshInstancePrefix = " Instance";
		
		public Color vertexColor = Color.white;
		
		public float intensity = 1.0f;
		
		public Mesh sharedMesh;
		
		[HideInInspector]
		[SerializeField]
		private gkReference_OneOwner_Object m_oRefSavedVertexColor;
		
		[HideInInspector]
		[SerializeField]
		private gkReference_OneOwner_Object m_oRefSavedIntensity;
		
		[HideInInspector]
		[SerializeField]
		private gkReference_OneOwner_Object m_oRefMeshInstance;
		
		[HideInInspector]
		[SerializeField]
		private gkReference_OneOwner_Object m_oRefMeshResource;
		
		private gkReference_OneOwner_Object m_rRefMeshInstance_Last;
		
		private gkReference_OneOwner_Object m_rRefMeshResource_Last;
		
		private gkReference_OneOwner_Object m_rRefSavedVertexColor_Last;
		
		private gkReference_OneOwner_Object m_rRefSavedIntensity_Last;
		
		public float Alpha
		{
			get
			{
				return vertexColor.a;
			}
			
			set
			{
				vertexColor.a = value;
				UpdateColorIfNeeded();
			}
		}
		
		public float Intensity
		{
			set
			{
				intensity = value;
				UpdateColorIfNeeded();
			}
		}
		
		public Color VertexColor
		{
			get
			{
				return vertexColor;
			}
			
			set
			{
				vertexColor = value;
				UpdateColorIfNeeded();
			}
		}
		
		public Color RGB
		{
			get
			{
				return vertexColor;
			}
			
			set
			{
				vertexColor.r = value.r;
				vertexColor.g = value.g;
				vertexColor.b = value.b;
				UpdateColorIfNeeded();
			}
		}
		
		public Mesh SharedMesh
		{
			get
			{
				return sharedMesh;
			}
			
			set
			{
				sharedMesh = value;
				UpdateMeshResourceIfNeeded();
			}
		}
		
		public Mesh MeshInstance
		{
			get
			{
				if(m_oRefMeshInstance == null || m_oRefMeshInstance.Owner != this)
				{
					m_oRefMeshInstance = ScriptableObject.CreateInstance<gkReference_OneOwner_Object>();
					m_oRefMeshInstance.Create(new Mesh(), this, true);
	#if UNITY_EDITOR
					EditorUtility.SetDirty(this);
	#endif				
					CheckForLostReferences(ref m_oRefMeshInstance, ref m_rRefMeshInstance_Last);
					
					UpdateMeshResource();
				}
				return m_oRefMeshInstance.Reference as Mesh;
			}
			
			set
			{
				SharedMesh = value;
			}
		}
		
		public Mesh MeshResource
		{
			get
			{
				CheckForMeshResourceValididty();
				return m_oRefMeshResource.Reference as Mesh;
			}
			
			set
			{
				CheckForMeshResourceValididty();
				m_oRefMeshResource.Reference = value;
			}
		}
		
		private bool m_bSavedVertexColorInit;
		private Color SavedVertexColor
		{
			get
			{
				if(Application.isPlaying == false)
				{
					if(CheckSavedVertexColorValidity() == false)
					{
						UpdateColor();
					}
				}
				else if(m_bSavedVertexColorInit == false)
				{
					if(CheckSavedVertexColorValidity() == false)
					{
						UpdateColor();
					}
					m_bSavedVertexColorInit = true;
				}
				return (m_oRefSavedVertexColor.Reference as gkColorContainer).color;
			}
			
			set
			{
				CheckSavedVertexColorValidity();
				(m_oRefSavedVertexColor.Reference as gkColorContainer).color = value;
			}
		}
		
		private bool m_bSavedItensityInit;
		private float SavedIntensity
		{
			get
			{
				if(CheckSavedIntensityValidity() == false)
				{
					UpdateColor();
				}
				else if(m_bSavedItensityInit == false)
				{
					if(CheckSavedVertexColorValidity() == false)
					{
						UpdateColor();
					}
					m_bSavedItensityInit = true;
				}
				return (m_oRefSavedIntensity.Reference as gkFloatContainer).value;
			}
			
			set
			{
				CheckSavedIntensityValidity();
				(m_oRefSavedIntensity.Reference as gkFloatContainer).value = value;
			}
		}
		
		private bool CheckForMeshResourceValididty()
		{
			if(m_oRefMeshResource == null || m_oRefMeshResource.Owner != this)
			{
				m_oRefMeshResource = ScriptableObject.CreateInstance<gkReference_OneOwner_Object>();
				m_oRefMeshResource.Create(null, this, false);
	#if UNITY_EDITOR
				EditorUtility.SetDirty(this);
	#endif				
				CheckForLostReferences(ref m_oRefMeshResource, ref m_rRefMeshResource_Last);
				
				return false;
			}
			return true;
		}
		
		private bool CheckSavedVertexColorValidity()
		{
			if(m_oRefSavedVertexColor == null || m_oRefSavedVertexColor.Owner != this)
			{
				m_oRefSavedVertexColor = ScriptableObject.CreateInstance<gkReference_OneOwner_Object>();
				m_oRefSavedVertexColor.Create(ScriptableObject.CreateInstance<gkColorContainer>(), this, true);
	#if UNITY_EDITOR
				EditorUtility.SetDirty(this);
	#endif				
				CheckForLostReferences(ref m_oRefSavedVertexColor, ref m_rRefSavedVertexColor_Last);
				return false;
			}
			return true;
		}
		
		private bool CheckSavedIntensityValidity()
		{
			if(m_oRefSavedIntensity == null || m_oRefSavedIntensity.Owner != this)
			{
				m_oRefSavedIntensity = ScriptableObject.CreateInstance<gkReference_OneOwner_Object>();
				m_oRefSavedIntensity.Create(ScriptableObject.CreateInstance<gkFloatContainer>(), this, true);
	#if UNITY_EDITOR
				EditorUtility.SetDirty(this);
	#endif				
				CheckForLostReferences(ref m_oRefSavedIntensity, ref m_rRefSavedIntensity_Last);
				return false;
			}
			return true;
		}
		
		public void ResetInstance()
		{
			UpdateMeshResource();
		}
		
		private void Awake()
		{	
			if(Application.isPlaying == false)
			{
				if(sharedMesh == null)
				{
					InitializeMeshResource();
				}
			}
			if(UpdateMeshResourceIfNeeded() == false)
			{
				UpdateColorIfNeeded();
			}
			UpdateMeshRendererAndFilter();
		}
		
		private void OnDestroy()
		{		
			CheckForLostReferences();
			
			if(m_oRefMeshInstance != null)
			{
				DestroyImmediate(m_oRefMeshInstance);
			}
			
			if(m_oRefMeshResource != null)
			{
				DestroyImmediate(m_oRefMeshResource);
			}
			
			if(m_oRefSavedVertexColor != null)
			{
				DestroyImmediate(m_oRefSavedVertexColor);
			}
			
			if(m_oRefSavedIntensity != null)
			{
				DestroyImmediate(m_oRefSavedIntensity);
			}
		}
	
	#if UNITY_EDITOR 
		private void LateUpdate()
		{	
			if(Application.isPlaying == false)
			{
				if(UpdateMeshResourceIfNeeded() == false)
				{
					UpdateColorIfNeeded();
				}
				UpdateMeshRendererAndFilter();
				CheckForLostReferences();
			}
		}
	#endif
		
		private void CheckForLostReferences()
		{
			CheckForLostReferences(ref m_oRefMeshInstance, ref m_rRefMeshInstance_Last);
			CheckForLostReferences(ref m_oRefMeshResource, ref m_rRefMeshResource_Last);
			CheckForLostReferences(ref m_oRefSavedVertexColor, ref m_rRefSavedVertexColor_Last);
			CheckForLostReferences(ref m_oRefSavedIntensity, ref m_rRefSavedIntensity_Last);
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
		
		private void InitializeMeshResource()
		{
			MeshFilter rMeshFilter = GetComponent<MeshFilter>();
			if(rMeshFilter != null)
			{
				SharedMesh = rMeshFilter.sharedMesh;
			}
		}
		
		private void UpdateMeshRendererAndFilter()
		{
			MeshFilter rMeshFilter = GetComponent<MeshFilter>();
			if(rMeshFilter == null)
			{
				rMeshFilter = gameObject.AddComponent<MeshFilter>();
			}
			if(MeshResource == null)
			{
				rMeshFilter.sharedMesh = null;
			}
			else
			{	
				Mesh rInstanceMesh = MeshInstance; 
				Mesh rMeshFilterMesh = rMeshFilter.sharedMesh;
				if(rMeshFilterMesh != rInstanceMesh)
				{
					rMeshFilter.sharedMesh = rInstanceMesh;
				}
			}
			
			MeshRenderer rMeshRenderer = GetComponent<MeshRenderer>();	
			if(rMeshRenderer == null)
			{
				rMeshRenderer = gameObject.AddComponent<MeshRenderer>();
			}
		}
		
		private bool UpdateMeshResourceIfNeeded()
		{
			if(MeshResource != sharedMesh)
			{
				UpdateMeshResource();
				return true;
			}
			return false;
		}
		
		private void UpdateMeshResource()
		{	
			MeshResource = sharedMesh;
			
			Mesh rMeshInstance = MeshInstance;
			gkMeshCopyUtility.CopyMesh(MeshResource, ref rMeshInstance);
			
			string oName = rMeshInstance.name;
			oName = oName.Replace(mc_oMeshInstancePrefix, "");
			oName += mc_oMeshInstancePrefix;
			rMeshInstance.name = oName;
			
			UpdateColor();
			
			UpdateMeshRendererAndFilter();
		}
		
		private void UpdateColorIfNeeded()
		{	
			if(vertexColor != SavedVertexColor || intensity != SavedIntensity)
			{
				UpdateColor();
			}
		}
		
		private void UpdateColor()
		{	
			Color oVertexColorWithIntensity = vertexColor;
			oVertexColorWithIntensity.r *= intensity;
			oVertexColorWithIntensity.g *= intensity;
			oVertexColorWithIntensity.b *= intensity;
			
			gkMeshColorUtility.SetVertexColor(MeshInstance, oVertexColorWithIntensity);
			SavedVertexColor = vertexColor;
			SavedIntensity = intensity;
		}
	}
}