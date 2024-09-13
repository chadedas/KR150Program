#if UNITY_EDITOR
using UnityEditor;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.InputEventSystem
{
    [CustomEditor(typeof(InputEvent))] [CanEditMultipleObjects]
    public class InputEventEditor : GGEditor
    {
        #region Serialized Properties / OnEnable
        
        private SerializedProperty userInput;
        private SerializedProperty OnPressed;
        private SerializedProperty OnHeld;
        private SerializedProperty OnReleased;

        private bool eventGroup;

        private void OnEnable()
        {
            userInput = serializedObject.FindProperty("userInput");
            OnPressed = serializedObject.FindProperty("OnPressed");
            OnHeld = serializedObject.FindProperty("OnHeld");
            OnReleased = serializedObject.FindProperty("OnReleased");
        }

        #endregion

        //----------------------------------------------------------------------------------------------------
        
        #region OnInspectorGUI

        public override void OnInspectorGUI()
        {
            // get & update references
            InputEvent inputEvent = (InputEvent)target;
            serializedObject.Update();

            // draw banner if turned on in Gaskellgames settings
            EditorWindowUtility.DrawInspectorBanner("Assets/Gaskellgames/Input Event System/Editor/Icons/inspectorBanner_InputEventSystem.png");
            
            // custom inspector
            EditorGUILayout.PropertyField(userInput);
            EditorGUILayout.Space();
            eventGroup = EditorGUILayout.BeginFoldoutHeaderGroup(eventGroup, "Events");
            if (eventGroup)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(OnPressed);
                EditorGUILayout.PropertyField(OnHeld);
                EditorGUILayout.PropertyField(OnReleased);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            // apply reference changes
            serializedObject.ApplyModifiedProperties();
        }

        #endregion
        
    } // class end
}

#endif
