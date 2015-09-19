using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using gk;

[AddComponentMenu("TemplateFolder/PersistentSingletonTemplate")]
public class PersistentSingletonTemplate : MonoBehaviour
{
	static private PersistentSingletonTemplate ms_oInstance;
	
	static public PersistentSingletonTemplate Instance
	{
		get
		{
			return ms_oInstance;
		}
	}
	
	private void Awake()
	{
		if(ms_oInstance == null)
		{
			ms_oInstance = this;
			DontDestroyOnLoad(ms_oInstance);
		}
		else
		{
			Destroy(gameObject);
			return;
		}
	}
}