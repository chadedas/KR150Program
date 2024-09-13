#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.CameraController
{
    [CustomEditor(typeof(ShadowTransform))] [CanEditMultipleObjects]
    public class ShadowTransformEditor : Editor
    {
        #region Serialized Properties / OnEnable

        private SerializedProperty updateInEditor;
        private SerializedProperty follow;
        private SerializedProperty followX;
        private SerializedProperty followY;
        private SerializedProperty followZ;
        private SerializedProperty lookAt;
        private SerializedProperty rotateWith;
        private SerializedProperty updateType;
        private SerializedProperty rotateWithX;
        private SerializedProperty rotateWithY;
        private SerializedProperty rotateWithZ;
        private SerializedProperty turnSpeed;

        private bool FoldoutGroup;

        private void OnEnable()
        {
            updateInEditor = serializedObject.FindProperty("updateInEditor");
            follow = serializedObject.FindProperty("follow");
            followX = serializedObject.FindProperty("followX");
            followY = serializedObject.FindProperty("followY");
            followZ = serializedObject.FindProperty("followZ");
            lookAt = serializedObject.FindProperty("lookAt");
            rotateWith = serializedObject.FindProperty("rotateWith");
            updateType = serializedObject.FindProperty("updateType");
            rotateWithX = serializedObject.FindProperty("rotateWithX");
            rotateWithY = serializedObject.FindProperty("rotateWithY");
            rotateWithZ = serializedObject.FindProperty("rotateWithZ");
            turnSpeed = serializedObject.FindProperty("turnSpeed");
        }

        #endregion
        
        //----------------------------------------------------------------------------------------------------

        #region OnInspectorGUI

        public override void OnInspectorGUI()
        {
            // get & update references
            ShadowTransform shadowTransform = (ShadowTransform)target;
            serializedObject.Update();

            /*
            // draw default inspector
            base.OnInspectorGUI();
            */

            // draw banner if turned on in Gaskellgames settings
            EditorWindowUtility.DrawInspectorBanner("Assets/Gaskellgames/Camera Controller/Editor/Icons/inspectorBanner_CameraController.png");

            // custom inspector
            EditorGUILayout.PropertyField(updateInEditor);
            EditorGUILayout.PropertyField(follow);
            EditorGUILayout.PropertyField(lookAt);
            EditorGUILayout.PropertyField(rotateWith);
            EditorGUILayout.PropertyField(updateType);
            FoldoutGroup = EditorGUILayout.Foldout(FoldoutGroup, "Constraints");
            if (FoldoutGroup)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUI.indentLevel++;
                EditorGUILayout.PrefixLabel("Freeze Position");
                EditorGUI.indentLevel--;
                shadowTransform.FollowX = EditorGUILayout.ToggleLeft("X", shadowTransform.FollowX, GUILayout.Width(28), GUILayout.ExpandWidth(false));
                shadowTransform.FollowY = EditorGUILayout.ToggleLeft("Y", shadowTransform.FollowY, GUILayout.Width(28), GUILayout.ExpandWidth(false));
                shadowTransform.FollowZ = EditorGUILayout.ToggleLeft("Z", shadowTransform.FollowZ, GUILayout.Width(28), GUILayout.ExpandWidth(false));
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                EditorGUI.indentLevel++;
                EditorGUILayout.PrefixLabel("Freeze Rotation");
                EditorGUI.indentLevel--;
                shadowTransform.RotateWithX = EditorGUILayout.ToggleLeft("X", shadowTransform.RotateWithX, GUILayout.Width(28), GUILayout.ExpandWidth(false));
                shadowTransform.RotateWithY = EditorGUILayout.ToggleLeft("Y", shadowTransform.RotateWithY, GUILayout.Width(28), GUILayout.ExpandWidth(false));
                shadowTransform.RotateWithZ = EditorGUILayout.ToggleLeft("Z", shadowTransform.RotateWithZ, GUILayout.Width(28), GUILayout.ExpandWidth(false));
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.PropertyField(turnSpeed);
            if (shadowTransform.LookAt && shadowTransform.RotateWith)
            {
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Transform rotation is being driven by LookAt. RotateWith will be ignored", MessageType.Info);
            }

            // apply reference changes
            serializedObject.ApplyModifiedProperties();
        }

        #endregion
        
    } // class end
}

#endif
