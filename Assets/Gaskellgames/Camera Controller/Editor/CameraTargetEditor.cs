#if UNITY_EDITOR
using UnityEditor;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.CameraController
{
    [CustomEditor(typeof(CameraTarget))] [CanEditMultipleObjects]
    public class CameraTargetEditor : GGEditor
    {
        #region Serialized Properties / OnEnable

        SerializedProperty targetType;
        SerializedProperty autoFindMultiTarget;
        SerializedProperty multiTarget;
        
        SerializedProperty cameraBrain;
        SerializedProperty triggerTag;
        SerializedProperty revertOnExit;
        SerializedProperty OnEnterTag;
        SerializedProperty OnExitTag;

        private bool EventsGroup;

        private void OnEnable()
        {
            targetType = serializedObject.FindProperty("targetType");
            autoFindMultiTarget = serializedObject.FindProperty("autoFindMultiTarget");
            multiTarget = serializedObject.FindProperty("multiTarget");
            
            cameraBrain = serializedObject.FindProperty("cameraBrain");
            triggerTag = serializedObject.FindProperty("triggerTag");
            revertOnExit = serializedObject.FindProperty("revertOnExit");
            OnEnterTag = serializedObject.FindProperty("OnEnterTag");
            OnExitTag = serializedObject.FindProperty("OnExitTag");
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region OnInspectorGUI

        public override void OnInspectorGUI()
        {
            // get & update references
            CameraTarget cameraTarget = (CameraTarget)target;
            serializedObject.Update();

            // draw banner if turned on in Gaskellgames settings
            EditorWindowUtility.DrawInspectorBanner("Assets/Gaskellgames/Camera Controller/Editor/Icons/inspectorBanner_CameraController.png");

            // custom inspector
            EditorGUILayout.PropertyField(targetType);
            if (cameraTarget.GetTargetType() == "OnEnable")
            {
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(autoFindMultiTarget);
                if (!autoFindMultiTarget.boolValue)
                {
                    EditorGUILayout.PropertyField(multiTarget);
                }
                else
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.HelpBox("autoFindMultiTarget should only be used when there is one multiTarget in the scene", MessageType.Info);
                }
            }
            else if (cameraTarget.GetTargetType() == "OnTrigger")
            {
                EditorGUILayout.PropertyField(cameraBrain);
                EditorGUILayout.PropertyField(triggerTag);
                EditorGUILayout.PropertyField(revertOnExit);
            
                EventsGroup = EditorGUILayout.BeginFoldoutHeaderGroup(EventsGroup, "Events");
                if (EventsGroup)
                {
                    EditorGUILayout.PropertyField(OnEnterTag);
                    EditorGUILayout.PropertyField(OnExitTag);
                }
            }

            // apply reference changes
            serializedObject.ApplyModifiedProperties();
        }

        #endregion
        
    } // class end
}

#endif
