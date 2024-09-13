#if UNITY_EDITOR
using UnityEditor;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.CameraController
{
    [CustomEditor(typeof(CameraSwitcher))] [CanEditMultipleObjects]
    public class CameraSwitcherEditor : GGEditor
    {
        #region Serialized Properties / OnEnable

        SerializedProperty cameraBrain;
        SerializedProperty useRegisteredList;
        SerializedProperty switchCamera;
        SerializedProperty customCameraRigsList;

        bool CustomGroup = false;

        private void OnEnable()
        {
            cameraBrain = serializedObject.FindProperty("cameraBrain");
            useRegisteredList = serializedObject.FindProperty("useRegisteredList");
            switchCamera = serializedObject.FindProperty("switchCamera");
            customCameraRigsList = serializedObject.FindProperty("customCameraRigsList");
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region OnInspectorGUI

        public override void OnInspectorGUI()
        {
            // get & update references
            CameraSwitcher cameraSwitcher = (CameraSwitcher)target;
            serializedObject.Update();

            // draw banner if turned on in Gaskellgames settings
            EditorWindowUtility.DrawInspectorBanner("Assets/Gaskellgames/Camera Controller/Editor/Icons/inspectorBanner_CameraController.png");

            // custom inspector
            EditorGUILayout.PropertyField(cameraBrain);
            EditorGUILayout.PropertyField(useRegisteredList);
            EditorGUILayout.PropertyField(switchCamera);

            CustomGroup = useRegisteredList.boolValue;
            if (!CustomGroup)
            {
                EditorGUILayout.PropertyField(customCameraRigsList);
            }

            // apply reference changes
            serializedObject.ApplyModifiedProperties();
        }

        #endregion
        
    } // class end
}

#endif
