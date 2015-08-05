using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TweenAlpha))]
public class TweenAlphaEditor : UGUITweenerEditor
{
    public override void OnInspectorGUI()
    {
        GUILayout.Space(6f);
        UGUIEditorTools.SetLabelWidth(120f);

        TweenAlpha tw = target as TweenAlpha;
        GUI.changed = false;

        float from = EditorGUILayout.Slider("From", tw.from, 0f, 1f);
        float to = EditorGUILayout.Slider("To", tw.to, 0f, 1f);

        if (GUI.changed)
        {
            tw.from = from;
            tw.to = to;
            EditorUtility.SetDirty(tw);
        }

        DrawCommonProperties();
    }
}
