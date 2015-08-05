using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// Tools for the editor
/// </summary>

public static class UGUIEditorTools
{
    /// <summary>
    /// Unity 4.3 changed the way LookLikeControls works.
    /// </summary>

    static public void SetLabelWidth(float width)
    {
        EditorGUIUtility.labelWidth = width;
    }

    /// <summary>
    /// Draw a distinctly different looking header label
    /// </summary>

    static public bool DrawHeader(string text) { return DrawHeader(text, text, false, false); }

    /// <summary>
    /// Draw a distinctly different looking header label
    /// </summary>

    static public bool DrawHeader(string text, string key) { return DrawHeader(text, key, false, false); }

    /// <summary>
    /// Draw a distinctly different looking header label
    /// </summary>

    static public bool DrawHeader(string text, bool detailed) { return DrawHeader(text, text, detailed, !detailed); }

    /// <summary>
    /// Draw a distinctly different looking header label
    /// </summary>

    static public bool DrawHeader(string text, string key, bool forceOn, bool minimalistic)
    {
        bool state = EditorPrefs.GetBool(key, true);

        if (!minimalistic) GUILayout.Space(3f);
        if (!forceOn && !state) GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
        GUILayout.BeginHorizontal();
        GUI.changed = false;

        if (minimalistic)
        {
            if (state) text = "\u25BC" + (char)0x200a + text;
            else text = "\u25BA" + (char)0x200a + text;

            GUILayout.BeginHorizontal();
            GUI.contentColor = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.7f) : new Color(0f, 0f, 0f, 0.7f);
            if (!GUILayout.Toggle(true, text, "PreToolbar2", GUILayout.MinWidth(20f))) state = !state;
            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();
        }
        else
        {
            text = "<b><size=11>" + text + "</size></b>";
            if (state) text = "\u25BC " + text;
            else text = "\u25BA " + text;
            if (!GUILayout.Toggle(true, text, "dragtab", GUILayout.MinWidth(20f))) state = !state;
        }

        if (GUI.changed) EditorPrefs.SetBool(key, state);

        if (!minimalistic) GUILayout.Space(2f);
        GUILayout.EndHorizontal();
        GUI.backgroundColor = Color.white;
        if (!forceOn && !state) GUILayout.Space(3f);
        return state;
    }

    /// <summary>
    /// Begin drawing the content area.
    /// </summary>

    static public void BeginContents() { BeginContents(false); }

    static bool mEndHorizontal = false;

    /// <summary>
    /// Begin drawing the content area.
    /// </summary>

    static public void BeginContents(bool minimalistic)
    {
        if (!minimalistic)
        {
            mEndHorizontal = true;
            GUILayout.BeginHorizontal();
            EditorGUILayout.BeginHorizontal("AS TextArea", GUILayout.MinHeight(10f));
        }
        else
        {
            mEndHorizontal = false;
            EditorGUILayout.BeginHorizontal(GUILayout.MinHeight(10f));
            GUILayout.Space(10f);
        }
        GUILayout.BeginVertical();
        GUILayout.Space(2f);
    }

    /// <summary>
    /// End drawing the content area.
    /// </summary>

    static public void EndContents()
    {
        GUILayout.Space(3f);
        GUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        if (mEndHorizontal)
        {
            GUILayout.Space(3f);
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(3f);
    }

    /// <summary>
    /// Draw a list of fields for the specified list of delegates.
    /// </summary>

    static public void DrawEvents(string text, Object undoObject, List<EventDelegate> list)
    {
        DrawEvents(text, undoObject, list, null, null, false);
    }

    /// <summary>
    /// Draw a list of fields for the specified list of delegates.
    /// </summary>

    static public void DrawEvents(string text, Object undoObject, List<EventDelegate> list, bool minimalistic)
    {
        DrawEvents(text, undoObject, list, null, null, minimalistic);
    }

    /// <summary>
    /// Draw a list of fields for the specified list of delegates.
    /// </summary>

    static public void DrawEvents(string text, Object undoObject, List<EventDelegate> list, string noTarget, string notValid, bool minimalistic)
    {
        if (!UGUIEditorTools.DrawHeader(text, text, false, minimalistic)) return;

        if (!minimalistic)
        {
            UGUIEditorTools.BeginContents(minimalistic);
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();

            EventDelegateEditor.Field(undoObject, list, notValid, notValid, minimalistic);

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            UGUIEditorTools.EndContents();
        }
        else EventDelegateEditor.Field(undoObject, list, notValid, notValid, minimalistic);
    }

    /// <summary>
    /// Helper function that draws a serialized property.
    /// </summary>

    static public SerializedProperty DrawProperty(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
    {
        return DrawProperty(null, serializedObject, property, false, options);
    }

    /// <summary>
    /// Helper function that draws a serialized property.
    /// </summary>

    static public SerializedProperty DrawProperty(string label, SerializedObject serializedObject, string property, params GUILayoutOption[] options)
    {
        return DrawProperty(label, serializedObject, property, false, options);
    }

    /// <summary>
    /// Helper function that draws a serialized property.
    /// </summary>

    static public SerializedProperty DrawPaddedProperty(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
    {
        return DrawProperty(null, serializedObject, property, true, options);
    }

    /// <summary>
    /// Helper function that draws a serialized property.
    /// </summary>

    static public SerializedProperty DrawPaddedProperty(string label, SerializedObject serializedObject, string property, params GUILayoutOption[] options)
    {
        return DrawProperty(label, serializedObject, property, true, options);
    }

    /// <summary>
    /// Helper function that draws a serialized property.
    /// </summary>

    static public SerializedProperty DrawProperty(string label, SerializedObject serializedObject, string property, bool padding, params GUILayoutOption[] options)
    {
        SerializedProperty sp = serializedObject.FindProperty(property);

        if (sp != null)
        {
            // if (false) padding = false;

            if (padding) EditorGUILayout.BeginHorizontal();

            if (label != null) EditorGUILayout.PropertyField(sp, new GUIContent(label), options);
            else EditorGUILayout.PropertyField(sp, options);

            if (padding)
            {
                UGUIEditorTools.DrawPadding();
                EditorGUILayout.EndHorizontal();
            }
        }
        return sp;
    }

    /// <summary>
    /// Helper function that draws a serialized property.
    /// </summary>

    static public void DrawProperty(string label, SerializedProperty sp, params GUILayoutOption[] options)
    {
        DrawProperty(label, sp, true, options);
    }

    /// <summary>
    /// Helper function that draws a serialized property.
    /// </summary>

    static public void DrawProperty(string label, SerializedProperty sp, bool padding, params GUILayoutOption[] options)
    {
        if (sp != null)
        {
            if (padding) EditorGUILayout.BeginHorizontal();

            if (label != null) EditorGUILayout.PropertyField(sp, new GUIContent(label), options);
            else EditorGUILayout.PropertyField(sp, options);

            if (padding)
            {
                UGUIEditorTools.DrawPadding();
                EditorGUILayout.EndHorizontal();
            }
        }
    }

    /// <summary>
    /// Helper function that draws a compact Vector4.
    /// </summary>

    static public void DrawBorderProperty(string name, SerializedObject serializedObject, string field)
    {
        if (serializedObject.FindProperty(field) != null)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(name, GUILayout.Width(75f));

                UGUIEditorTools.SetLabelWidth(50f);
                GUILayout.BeginVertical();
                UGUIEditorTools.DrawProperty("Left", serializedObject, field + ".x", GUILayout.MinWidth(80f));
                UGUIEditorTools.DrawProperty("Bottom", serializedObject, field + ".y", GUILayout.MinWidth(80f));
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                UGUIEditorTools.DrawProperty("Right", serializedObject, field + ".z", GUILayout.MinWidth(80f));
                UGUIEditorTools.DrawProperty("Top", serializedObject, field + ".w", GUILayout.MinWidth(80f));
                GUILayout.EndVertical();

                UGUIEditorTools.SetLabelWidth(80f);
            }
            GUILayout.EndHorizontal();
        }
    }

    /// <summary>
    /// Helper function that draws a compact Rect.
    /// </summary>

    static public void DrawRectProperty(string name, SerializedObject serializedObject, string field)
    {
        DrawRectProperty(name, serializedObject, field, 56f, 18f);
    }

    /// <summary>
    /// Helper function that draws a compact Rect.
    /// </summary>

    static public void DrawRectProperty(string name, SerializedObject serializedObject, string field, float labelWidth, float spacing)
    {
        if (serializedObject.FindProperty(field) != null)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(name, GUILayout.Width(labelWidth));

                UGUIEditorTools.SetLabelWidth(20f);
                GUILayout.BeginVertical();
                UGUIEditorTools.DrawProperty("X", serializedObject, field + ".x", GUILayout.MinWidth(50f));
                UGUIEditorTools.DrawProperty("Y", serializedObject, field + ".y", GUILayout.MinWidth(50f));
                GUILayout.EndVertical();

                UGUIEditorTools.SetLabelWidth(50f);
                GUILayout.BeginVertical();
                UGUIEditorTools.DrawProperty("Width", serializedObject, field + ".width", GUILayout.MinWidth(80f));
                UGUIEditorTools.DrawProperty("Height", serializedObject, field + ".height", GUILayout.MinWidth(80f));
                GUILayout.EndVertical();

                UGUIEditorTools.SetLabelWidth(80f);
                if (spacing != 0f) GUILayout.Space(spacing);
            }
            GUILayout.EndHorizontal();
        }
    }

    /// <summary>
    /// Create an undo point for the specified objects.
    /// </summary>

    static public void RegisterUndo(string name, params Object[] objects)
    {
        if (objects != null && objects.Length > 0)
        {
            UnityEditor.Undo.RecordObjects(objects, name);

            foreach (Object obj in objects)
            {
                if (obj == null) continue;
                EditorUtility.SetDirty(obj);
            }
        }
    }


    static public void DrawPadding()
    {
        if (!false)
            GUILayout.Space(18f);
    }
}