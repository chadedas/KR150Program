#if UNITY_EDITOR
using UnityEditor;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.CameraController
{
    [CustomEditor(typeof(CameraRig))] [CanEditMultipleObjects]
    public class CameraRigEditor : GGEditor
    {
        #region Serialized Properties / OnEnable

        SerializedProperty freelookRig;
        SerializedProperty registerCamera;
        SerializedProperty cameraShake;
        SerializedProperty freeFlyCamera;
        SerializedProperty gmkInputController;
        SerializedProperty moveSpeed;
        SerializedProperty boostSpeed;
        SerializedProperty xSensitivity;
        SerializedProperty ySensitivity;
        SerializedProperty freeFlyActive;
        SerializedProperty follow;
        SerializedProperty lookAt;
        SerializedProperty turnSpeed;
        SerializedProperty followOffset;
        SerializedProperty shakeSmoothing;
        SerializedProperty positionMagnitude;
        SerializedProperty rotationMagnitude;
        SerializedProperty lens;

        private void OnEnable()
        {
            freelookRig = serializedObject.FindProperty("freelookRig");
            registerCamera = serializedObject.FindProperty("registerCamera");
            cameraShake = serializedObject.FindProperty("cameraShake");
            freeFlyCamera = serializedObject.FindProperty("freeFlyCamera");
            gmkInputController = serializedObject.FindProperty("gmkInputController");
            moveSpeed = serializedObject.FindProperty("moveSpeed");
            boostSpeed = serializedObject.FindProperty("boostSpeed");
            xSensitivity = serializedObject.FindProperty("xSensitivity");
            ySensitivity = serializedObject.FindProperty("ySensitivity");
            freeFlyActive = serializedObject.FindProperty("freeFlyActive");
            follow = serializedObject.FindProperty("follow");
            lookAt = serializedObject.FindProperty("lookAt");
            turnSpeed = serializedObject.FindProperty("turnSpeed");
            followOffset = serializedObject.FindProperty("followOffset");
            shakeSmoothing = serializedObject.FindProperty("shakeSmoothing");
            positionMagnitude = serializedObject.FindProperty("positionMagnitude");
            rotationMagnitude = serializedObject.FindProperty("rotationMagnitude");
            lens = serializedObject.FindProperty("lens");
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region OnInspectorGUI

        public override void OnInspectorGUI()
        {
            // get & update references
            CameraRig cameraRig = (CameraRig)target;
            serializedObject.Update();

            // draw banner if turned on in Gaskellgames settings
            EditorWindowUtility.DrawInspectorBanner("Assets/Gaskellgames/Camera Controller/Editor/Icons/inspectorBanner_CameraController.png");

            // custom inspector
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(registerCamera);
            EditorGUILayout.PropertyField(freeFlyCamera);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(cameraShake);
            if (freeFlyCamera.boolValue)
            {
                EditorGUILayout.PropertyField(gmkInputController);
                EditorGUILayout.PropertyField(moveSpeed);
                EditorGUILayout.PropertyField(boostSpeed);
                EditorGUILayout.PropertyField(xSensitivity);
                EditorGUILayout.PropertyField(ySensitivity);
                EditorGUILayout.PropertyField(freeFlyActive);
            }
            else
            {
                EditorGUILayout.PropertyField(freelookRig);
                EditorGUILayout.PropertyField(follow);
                EditorGUILayout.PropertyField(lookAt);
                EditorGUILayout.PropertyField(turnSpeed);
                EditorGUILayout.PropertyField(followOffset);
            }
            
            if (cameraShake.boolValue)
            {
                EditorGUILayout.PropertyField(shakeSmoothing);
                EditorGUILayout.PropertyField(positionMagnitude);
                EditorGUILayout.PropertyField(rotationMagnitude);
            }
            EditorGUILayout.PropertyField(lens);

            if(!freeFlyCamera.boolValue)
            {
                if (freelookRig.objectReferenceValue != null)
                {
                    if (lookAt.objectReferenceValue != null)
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.HelpBox("Follow, TurnSpeed & FollowOffset are being driven by the CameraFreelookRig", MessageType.Info);
                        EditorGUILayout.HelpBox("LookAt is being driven by the CameraRig", MessageType.Info);
                    }
                    else
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.HelpBox( "Follow, LookAt, TurnSpeed & FollowOffset are being driven by the CameraFreelookRig", MessageType.Info);
                    }
                }
            }

            // apply reference changes
            serializedObject.ApplyModifiedProperties();
        }

        #endregion
        
    } // class end
}

#endif
