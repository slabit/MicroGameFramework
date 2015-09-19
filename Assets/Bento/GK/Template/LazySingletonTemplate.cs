using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using gk;

[AddComponentMenu("TemplateFolder/LazySingletonTemplate")]
public class LazySingletonTemplate : MonoBehaviour
{
	static private LazySingletonTemplate ms_oInstance;
	
	public static LazySingletonTemplate Instance
	{
		get
		{
			if(ms_oInstance == null)
			{
				CreateInstance();
			}
			
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
	
	private static void CreateInstance()
	{
		GameObject rInstanceGameObject = new GameObject(typeof(LazySingletonTemplate).Name);
		rInstanceGameObject.AddComponent<LazySingletonTemplate>();
	}
}