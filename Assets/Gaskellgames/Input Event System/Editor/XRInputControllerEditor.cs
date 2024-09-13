#if UNITY_EDITOR
using UnityEditor;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.InputEventSystem
{
    [CustomEditor(typeof(XRInputController))]
    public class XRInputControllerEditor : GGEditor
    {
        #region Serialized Properties / OnEnable

        SerializedProperty playerInput;
        SerializedProperty trackingType;
        SerializedProperty leftController;
        SerializedProperty rightController;
        SerializedProperty leftControllerInputs;
        SerializedProperty leftControllerTracking;
        SerializedProperty rightControllerInputs;
        SerializedProperty rightControllerTracking;

        private void OnEnable()
        {
            playerInput = serializedObject.FindProperty("playerInput");
            trackingType = serializedObject.FindProperty("trackingType");
            leftController = serializedObject.FindProperty("leftController");
            rightController = serializedObject.FindProperty("rightController");
            leftControllerInputs = serializedObject.FindProperty("leftControllerInputs");
            leftControllerTracking = serializedObject.FindProperty("leftControllerTracking");
            rightControllerInputs = serializedObject.FindProperty("rightControllerInputs");
            rightControllerTracking = serializedObject.FindProperty("rightControllerTracking");
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region OnInspectorGUI

        public override void OnInspectorGUI()
        {
            // get & update references
            XRInputController xrInputController = (XRInputController)target;
            serializedObject.Update();

            // draw banner if turned on in Gaskellgames settings
            EditorWindowUtility.DrawInspectorBanner("Assets/Gaskellgames/Input Event System/Editor/Icons/inspectorBanner_InputEventSystem.png");

            // custom inspector
            EditorGUILayout.PropertyField(playerInput);
            EditorGUILayout.PropertyField(trackingType);
            EditorGUILayout.PropertyField(leftController);
            EditorGUILayout.PropertyField(rightController);
            EditorGUILayout.PropertyField(leftControllerInputs);
            if (leftController.objectReferenceValue)
            {
                EditorGUILayout.PropertyField(leftControllerTracking);
            }
            EditorGUILayout.PropertyField(rightControllerInputs);
            if (rightController.objectReferenceValue)
            {
                EditorGUILayout.PropertyField(rightControllerTracking);
            }

            // apply reference changes
            serializedObject.ApplyModifiedProperties();
        }

        #endregion

    } // class end
}

#endif
