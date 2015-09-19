using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gk
{
	[AddComponentMenu("TayasuiUI/LayerUtility")]
	public static class gkLayerUtility
	{
		// Set Layer Recursively
		public static void SetSpriteLayerRecursively(string a_oLayerName, Component a_rComponent)
		{
			foreach(SpriteRenderer rSpriteRenderer in a_rComponent.GetComponentsInChildren<SpriteRenderer>(true))
			{
				rSpriteRenderer.sortingLayerName = a_oLayerName;
			}
		}
			
		// Set Layer Recursively
		public static void SetLayerRecursively(string a_oLayerName, Component a_rComponent)
		{
			int iLayer = NameToLayerSafe(a_oLayerName);
			// Set all the content view game objects layer to the culling layer
			foreach(Transform rTransform in a_rComponent.GetComponentsInChildren<Transform>(true))
			{
				rTransform.gameObject.layer = iLayer;
			}
		}
		
		// Layer to culling mask
		public static int LayerNameToCullingMask(string a_oLayerName)
		{	
			int iCullingLayer = NameToLayerSafe(a_oLayerName);
			return 1 << iCullingLayer;
		}
		
		// Layer to culling mask
		public static int RemoveLayerNameFromCullingMask(string a_oLayerName, int a_iCullingMask)
		{	
			int iCullingLayerToRemove = LayerNameToCullingMask(a_oLayerName);
			return a_iCullingMask &= ~iCullingLayerToRemove;
		}
		
		// Name to layer
		// Return 0 if the layer name doesn't exist
		public static int NameToLayerSafe(string a_oLayerName)
		{	
			int iCullingLayer = LayerMask.NameToLayer(a_oLayerName);
			if(iCullingLayer < 0)
			{
				iCullingLayer = 0;
			}
			return iCullingLayer;
		}
	}
}