using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using gk;

[AddComponentMenu("TemplateFolder/SingletonTemplate")]
public class SingletonTemplate : MonoBehaviour
{
	static private SingletonTemplate ms_oInstance;
	
	static public SingletonTemplate Instance
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
		}
		else
		{
			Debug.LogWarning("A singleton can only be instantiated once!");
			Destroy(gameObject);
			return;
		}
	}
	
	private void OnDestroy()
	{
		if(ms_oInstance == this)
		{
			ms_oInstance = null;
		}
	}
}