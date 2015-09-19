using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace gk
{
	[AddComponentMenu("GK/TextMesh/gkTextMeshSize")]
	public class gkTextMeshSize : MonoBehaviour
	{
		public bool keepRatio = true;
		
		public float maxLength = 1.0f;
		
		public bool update;
		
		public bool editLive;
		
		private TextMesh m_rTextMesh;
		
		private Vector3 m_f3InitialLocalScale;
	
		public static void UpdateTextMeshSize(Component a_rComponent, string a_oCalibrationText)
		{
			if(a_rComponent == null)
			{
				return;
			}
			
			gkTextMeshSize rTextMeshSize = a_rComponent.GetComponent<gkTextMeshSize>();
			
			if(rTextMeshSize == null)
			{
				return;
			}
			
			rTextMeshSize._UpdateTextMeshSize(a_oCalibrationText);
		}
		
		public static void UpdateTextMeshSize(Component a_rComponent)
		{
			if(a_rComponent == null)
			{
				return;
			}
			
			gkTextMeshSize rTextMeshSize = a_rComponent.GetComponent<gkTextMeshSize>();
			
			if(rTextMeshSize == null)
			{
				return;
			}
			
			rTextMeshSize._UpdateTextMeshSize();
		}
		
		private void Awake()
		{
			m_rTextMesh = GetComponent<TextMesh>();
			m_f3InitialLocalScale = transform.localScale;
		}
		
		private void Start()
		{
			ApplyConstraint();
		}
		
		private void LateUpdate()
		{
			if(update || (Application.isEditor && editLive))
			{
				ApplyConstraint();
			}
		}
		
		private void _UpdateTextMeshSize(string a_oCalibrationText)
		{
			if(m_rTextMesh == null)
			{
				return;
			}
			
			string oCurrentTextSave = m_rTextMesh.text;
			
			m_rTextMesh.text = a_oCalibrationText;
			
			UpdateConstraint();
			
			m_rTextMesh.text = oCurrentTextSave;
		}
		
		private void _UpdateTextMeshSize()
		{
			if(m_rTextMesh == null)
			{
				return;
			}
			
			UpdateConstraint();
		}
		
		private void UpdateConstraint()
		{
			gkTextMeshCase rTextMeshCase = m_rTextMesh.GetComponent<gkTextMeshCase>();
			if(rTextMeshCase != null)
			{
				rTextMeshCase.ApplyConstraint();
			}
			
			ApplyConstraint();
		}
		
		private void ApplyConstraint()
		{
			float fCurrentLength = GetTextMeshLength();
			float fMaxLength = ComputeMaxLength();
			
			float fLengthScale = 1.0f;
			if(fCurrentLength > fMaxLength && fCurrentLength > 0.0f)
			{
				fLengthScale = fMaxLength/fCurrentLength;
			}
			
			Vector3 f3LocalScale = m_f3InitialLocalScale;
			f3LocalScale.x = m_f3InitialLocalScale.x * fLengthScale;
			if(keepRatio)
			{
				f3LocalScale.y = m_f3InitialLocalScale.y * fLengthScale;
			}
			transform.localScale = f3LocalScale;
		}
		
		private float ComputeMaxLength()
		{
			return maxLength;
		}
		
		private float GetTextMeshLength()
		{
			return GetTextMeshSize().x;
		}
		
		private Vector2 GetTextMeshSize()
		{
			Transform rParentTransformSave = transform.parent;
			bool bRendererEnableSave = GetComponent<Renderer>().enabled;
			bool bGameObjectActiveSave = gameObject.activeSelf;
			
			Vector3 f3LocalPositionSave = transform.localPosition;
			Quaternion oLocalRotationSave = transform.localRotation;
			Vector3 f3LocalScaleSave = transform.localScale;
			
			transform.parent = null;
			gameObject.SetActive(true);
			GetComponent<Renderer>().enabled = true;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			
			Vector2 f2Size = GetComponent<Renderer>().bounds.size;
			
			transform.parent = rParentTransformSave;
			gameObject.SetActive(bGameObjectActiveSave);
			GetComponent<Renderer>().enabled = bRendererEnableSave;
			transform.localPosition = f3LocalPositionSave;
			transform.localRotation = oLocalRotationSave;
			transform.localScale = f3LocalScaleSave;
			
			return f2Size;
		}
		
		private void OnDrawGizmos()
		{
			if(m_rTextMesh == null)
			{
				m_rTextMesh = GetComponent<TextMesh>();
				if(m_rTextMesh == null)
				{
					return;
				}
			}
			
			Vector2 f2TextMeshSize = GetTextMeshSize();
			float fHeight = f2TextMeshSize.y;
			float fWidth = maxLength;
			float fScaleX = transform.localScale.x;
			if(fScaleX != 0.0f)
			{
				fWidth /= fScaleX;
			}
			
			Vector2 f2Offset;
			switch(m_rTextMesh.anchor)
			{
				// Lower
				case TextAnchor.LowerCenter:
				{
					f2Offset = new Vector2(0.0f, 1.0f);	
				}
				break;
				
				case TextAnchor.LowerLeft:
				{
					f2Offset = new Vector2(1.0f, 1.0f);	
				}
				break;
				
				case TextAnchor.LowerRight:
				{
					f2Offset = new Vector2(-1.0f, 1.0f);	
				}
				break;
				
				// Middle
				case TextAnchor.MiddleCenter:
				default:
				{
					f2Offset = new Vector2(0.0f, 0.0f);	
				}
				break;
				
				case TextAnchor.MiddleLeft:
				{
					f2Offset = new Vector2(1.0f, 0.0f);	
				}
				break;
				
				case TextAnchor.MiddleRight:
				{
					f2Offset = new Vector2(-1.0f, 0.0f);	
				}
				break;
				
				// Upper
				case TextAnchor.UpperCenter:
				{
					f2Offset = new Vector2(0.0f, -1.0f);	
				}
				break;
				
				case TextAnchor.UpperLeft:
				{
					f2Offset = new Vector2(1.0f, -1.0f);	
				}
				break;
				
				case TextAnchor.UpperRight:
				{
					f2Offset = new Vector2(-1.0f, -1.0f);	
				}
				break;
			}
			Vector2 f2Center = new Vector2(f2Offset.x * 0.5f * fWidth, f2Offset.y * 0.5f * fHeight);
			
			gkRectangle oRectangle = new gkRectangle(f2Center.x, f2Center.y, fWidth, fHeight);
			
			oRectangle.DisplayLinesGizmos(transform, Color.red);
		}
	}
}