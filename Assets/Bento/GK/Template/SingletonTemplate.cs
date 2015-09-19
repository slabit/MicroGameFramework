using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using gk;

[AddComponentMenu("TemplateFolder/SingletonTemplate")]
public class SingletonTemplate : MonoBehaviour
{
	static private SingletonTemplate instance;
	
	static public SingletonTemplate Instance
	{
		get
		{
			return instance;
		}
	}
	
	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
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
		if(instance == this)
		{
			instance = null;
		}
	}
}