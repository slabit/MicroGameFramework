using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace gk
{
	[AddComponentMenu("GK/TextMesh/gkTextMeshHeight")]
	public class gkTextMeshHeight : MonoBehaviour
	{
		public bool keepRatio = true;
		
		public float maxHeight = 1.0f;
		
		private TextMesh m_rTextMesh;
		
		private Vector2 m_f2InitialLocalScale;
	
		private void Awake()
		{
			m_rTextMesh = GetComponent<TextMesh>();
			m_f2InitialLocalScale = transform.localScale;
		}
		
		private void LateUpdate()
		{
			ApplyConstraint();
		}
		
		private void ApplyConstraint()
		{
			float fCurrentLength = GetTextMeshHeight();
			float fMaxLength = ComputeMaxHeight();
				
			float fLengthScale = 1.0f;
			if(fCurrentLength > fMaxLength && fCurrentLength > 0.0f)
			{
				fLengthScale = fMaxLength/fCurrentLength;
			}
			
			Vector3 f3LocalScale = m_f2InitialLocalScale;
			f3LocalScale.y = m_f2InitialLocalScale.y * fLengthScale;
			if(keepRatio)
			{
				f3LocalScale.x = m_f2InitialLocalScale.x * fLengthScale;
			}
			transform.localScale = f3LocalScale;
		}
		
		private float ComputeMaxHeight()
		{
			return maxHeight;
		}
		
		private float GetTextMeshHeight()
		{
			return GetTextMeshSize().y;
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
			float fHeight = maxHeight;
			float fWidth = f2TextMeshSize.x;
			float fScaleY = transform.localScale.y;
			if(fScaleY != 0.0f)
			{
				fHeight /= fScaleY;
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