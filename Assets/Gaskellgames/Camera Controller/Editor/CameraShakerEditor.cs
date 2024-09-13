#if UNITY_EDITOR
using UnityEditor;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.CameraController
{
    [CustomEditor(typeof(CameraShaker))] [CanEditMultipleObjects]
    public class CameraShakerEditor : GGEditor
    {
        #region Serialized Properties / OnEnable

        SerializedProperty gizmosOnSelected;
        SerializedProperty gizmoColor;
        SerializedProperty intensity;
        SerializedProperty range;

        private void OnEnable()
        {
            gizmosOnSelected = serializedObject.FindProperty("gizmosOnSelected");
            gizmoColor = serializedObject.FindProperty("gizmoColor");
            intensity = serializedObject.FindProperty("intensity");
            range = serializedObject.FindProperty("range");
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region OnInspectorGUI

        public override void OnInspectorGUI()
        {
            // get & update references
            CameraShaker cameraShaker = (CameraShaker)target;
            serializedObject.Update();

            // draw banner if turned on in Gaskellgames settings
            EditorWindowUtility.DrawInspectorBanner("Assets/Gaskellgames/Camera Controller/Editor/Icons/inspectorBanner_CameraController.png");

            // custom inspector
            EditorGUILayout.PropertyField(gizmosOnSelected);
            EditorGUILayout.PropertyField(gizmoColor);
            EditorGUILayout.PropertyField(intensity);
            EditorGUILayout.PropertyField(range);

            // apply reference changes
            serializedObject.ApplyModifiedProperties();
        }

        #endregion
        
    } // class end
}

#endif
