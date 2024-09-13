#if UNITY_EDITOR
using UnityEditor;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.InputEventSystem
{
    [CustomEditor(typeof(InputActionManager))]
    public class InputActionManagerEditor : GGEditor
    {
        #region Serialized Properties / OnEnable

        SerializedProperty inputActionAssets;

        private void OnEnable()
        {
            inputActionAssets = serializedObject.FindProperty("inputActionAssets");
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region OnInspectorGUI

        public override void OnInspectorGUI()
        {
            // get & update references
            InputActionManager inputActionManager = (InputActionManager)target;
            serializedObject.Update();

            // draw banner if turned on in Gaskellgames settings
            EditorWindowUtility.DrawInspectorBanner("Assets/Gaskellgames/Input Event System/Editor/Icons/inspectorBanner_InputEventSystem.png");

            // custom inspector
            EditorGUILayout.PropertyField(inputActionAssets);

            // apply reference changes
            serializedObject.ApplyModifiedProperties();
        }

        #endregion

    } // class end
}

#endif
