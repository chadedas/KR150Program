#if UNITY_EDITOR
using UnityEditor;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.InputEventSystem
{
    [CustomEditor(typeof(GMKInputController))]
    public class GMKInputControllerEditor : GGEditor
    {
        #region Serialized Properties / OnEnable

        SerializedProperty legacyInputSystem;
        SerializedProperty legacyInputs;
        SerializedProperty playerInput;
        SerializedProperty inputs;

        bool LegacyGroup = false;

        private void OnEnable()
        {
            legacyInputSystem = serializedObject.FindProperty("legacyInputSystem");
            legacyInputs = serializedObject.FindProperty("legacyInputs");
            playerInput = serializedObject.FindProperty("playerInput");
            inputs = serializedObject.FindProperty("inputs");
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region OnInspectorGUI

        public override void OnInspectorGUI()
        {
            // get & update references
            GMKInputController gmkInputController = (GMKInputController)target;
            serializedObject.Update();

            // draw banner if turned on in Gaskellgames settings
            EditorWindowUtility.DrawInspectorBanner("Assets/Gaskellgames/Input Event System/Editor/Icons/inspectorBanner_InputEventSystem.png");

            // custom inspector
            EditorGUILayout.PropertyField(legacyInputSystem);
            LegacyGroup = legacyInputSystem.boolValue;
            if (LegacyGroup)
            {
                EditorGUILayout.PropertyField(legacyInputs);
            }
            else
            {
                EditorGUILayout.PropertyField(playerInput);
            }
            EditorGUILayout.PropertyField(inputs);

            // apply reference changes
            serializedObject.ApplyModifiedProperties();
        }

        #endregion

    } // class end
}

#endif
