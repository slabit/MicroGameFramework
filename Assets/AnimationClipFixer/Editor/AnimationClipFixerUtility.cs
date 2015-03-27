using UnityEngine;
using System.Collections.Generic;

using UnityEditor;
using System;

namespace AnimationClipFixer
{
	public static class AnimationClipFixerUtility
	{
		public static AnimationClipVisualisation CreateAnimationClipVisualisation(AnimationClip clip)
		{
			if(clip == null)
				return null;

			GameObject visualisationGameObject = new GameObject(clip.name);

			AnimationClipVisualisation visualisation = visualisationGameObject.AddComponent<AnimationClipVisualisation>();
			visualisation.clip = clip;

			EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(clip);
			foreach(EditorCurveBinding binding in bindings)
			{
				CreateAnimationClipVisualisationProperty(visualisation.transform, 
				                                         binding.path, binding.propertyName, 
				                                         binding.type, AnimationUtility.GetEditorCurve(clip, binding));
			}

			return visualisation;
		}

		public static AnimationClipVisualisationProperty CreateAnimationClipVisualisationProperty(Transform root, 
		                                                                                          string propertyPath, string propertyName,
		                                                                                          Type propertyType, AnimationCurve curve)
		{
			string[] pathGameObjectNames = propertyPath.Split(new char[]{'/'});
			Transform pathGameObjectParent = root;
			foreach(string pathGameObjectName in pathGameObjectNames)
			{
				Transform pathGameObjectTransform = pathGameObjectParent.FindChild(pathGameObjectName);

				// If the path game object doesn't exist create it
				if(pathGameObjectTransform == null)
				{
					GameObject pathGameObject = new GameObject(pathGameObjectName);
					pathGameObjectTransform = pathGameObject.transform;
					pathGameObjectTransform.parent = pathGameObjectParent;
				}
					
				pathGameObjectParent = pathGameObjectTransform;
			}

			// The last path game object is the property
			AnimationClipVisualisationProperty property = pathGameObjectParent.gameObject.AddComponent<AnimationClipVisualisationProperty>();
			property.propertyName = propertyName;
			property.curve = curve;
			property.propertyType.name = propertyType.FullName;
			property.propertyType.assemblyName = propertyType.Assembly.GetName().Name;

			return property;
		}

		public static AnimationClip SaveAnimationClipVisualisation(AnimationClipVisualisation visualisation)
		{
			return SaveAnimationClipVisualisation(visualisation, visualisation.clip);
		}

		public static AnimationClip SaveAnimationClipVisualisation(AnimationClipVisualisation visualisation, AnimationClip clip)
		{
			if(clip == null)
				return null;

			clip.ClearCurves();

			AnimationClipVisualisationProperty[] properties = visualisation.GetComponentsInChildren<AnimationClipVisualisationProperty>();
			foreach(AnimationClipVisualisationProperty property in properties)
			{
				EditorCurveBinding binding = new EditorCurveBinding();

				binding.propertyName = property.propertyName;
				binding.path = ComputePath(visualisation, property);
				binding.type = System.Type.GetType(property.propertyType.name + ", " + property.propertyType.assemblyName);

				AnimationUtility.SetEditorCurve(clip, binding, property.curve);
			}

			EditorUtility.SetDirty(clip);

			//Debug.Log("Clip " + clip + " successfully saved.");

			return clip;
		}

		public static string ComputePath(Component rootComponent, Component component)
		{
			if(rootComponent == null)
				return "";
			
			if(component == null)
				return "";
			
			Transform root = rootComponent.transform;
			
			Transform transform = component.transform;
			
			Transform searchedTransform = transform;
			string path = "";
			while(searchedTransform != null)
			{
				if(searchedTransform == root)
				{
					return path;
				}
				
				if(path != "")
				{
					path = "/" + path;
				}
				path = searchedTransform.name + path;
				
				searchedTransform = searchedTransform.parent;
			}
			
			return "";
		}
	}
}
