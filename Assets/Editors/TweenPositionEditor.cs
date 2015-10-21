﻿using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TweenPosition))]
public class TweenPositionEditor : UGUITweenerEditor
{
	public override void OnInspectorGUI ()
	{
		GUILayout.Space(6f);
		UGUIEditorTools.SetLabelWidth(120f);
		
		TweenPosition tw = target as TweenPosition;
		GUI.changed = false;
		
		Vector3 from = EditorGUILayout.Vector3Field("From", tw.from);
		Vector3 to = EditorGUILayout.Vector3Field("To", tw.to);
		
		if (GUI.changed)
		{
			UGUIEditorTools.RegisterUndo("Tween Change", tw);
			tw.from = from;
			tw.to = to;
			EditorUtility.SetDirty(tw);
		}
		
		DrawCommonProperties();
	}
}
