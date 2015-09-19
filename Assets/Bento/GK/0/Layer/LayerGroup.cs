using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace gk
{
	[AddComponentMenu("GK/Layer/LayerGroup")]
	public class LayerGroup : MonoBehaviour 
	{
		public GameObject[] gameObjects;

		List<int> initialLayers = new List<int>();

		int layer = -1;

		public string LayerName
		{
			get
			{
				return LayerMask.LayerToName(Layer);
			}

			set
			{
				Layer = LayerMask.NameToLayer(value);
			}
		}

		public int Layer
		{
			get
			{
				return layer;
			}

			set
			{
				if(layer != value)
				{
					layer = value;

					for(int i = 0; i < gameObjects.Length; ++i)
					{
						GameObject layerGroupGameObject = gameObjects[i];
						if(layerGroupGameObject == null)
							continue;
						
						layerGroupGameObject.layer = layer;
					}
				}
			}
		}

		public void ResetInitialLayers()
		{
			int gameObjectCount = gameObjects.Length;
			for(int i = 0; i < gameObjectCount; ++i)
			{
				GameObject layerGroupGameObject = gameObjects[i];
				if(layerGroupGameObject == null)
					continue;

				layerGroupGameObject.layer = initialLayers[i];
			}

			layer = -1;
		}

		void Awake()
		{
			for(int i = 0; i < gameObjects.Length; ++i)
			{
				GameObject layerGroupGameObject = gameObjects[i];
				if(layerGroupGameObject == null)
					continue;

				initialLayers.Add(layerGroupGameObject.layer);
			}
		}
	}
}
