using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace AnimationClipFixer
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(AnimationClipVisualisation))]
	public class AnimationClipVisualisationInspector : Editor 
	{
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI();

			if(targets.Length > 1)
				return;

			AnimationClipVisualisation visualisation = target as AnimationClipVisualisation;

			bool enabledSave = GUI.enabled;
			GUI.enabled = visualisation.clip != null;
			{
				if(GUILayout.Button("Save and replace clip"))
				{
					AnimationClipFixerUtility.SaveAnimationClipVisualisation(visualisation);
				}
			}
			GUI.enabled = enabledSave;
		}
	}
}
