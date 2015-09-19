using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace gk
{
	[AddComponentMenu("GK/Render/gkRenderQueueOverride_Material")]
	public class gkRenderQueueOverride_Material : MonoBehaviour 
	{
		public int renderQueue = 3000;
		
		public Material material;
		
		public void Update()
		{
			if(material.renderQueue != renderQueue)
			{
				material.renderQueue = renderQueue;
			}
		}
	}
}