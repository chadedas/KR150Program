#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.CameraController
{
    [InitializeOnLoad]
    public class HierarchyIcon_CameraController
    {
        #region Variables

        private static readonly Texture2D icon_CameraBrain;
        private static readonly Texture2D icon_CameraRig;
        private static readonly Texture2D icon_CameraFreelookRig;
        private static readonly Texture2D icon_CameraSwitcher;
        private static readonly Texture2D icon_CameraShaker;
        private static readonly Texture2D icon_CameraTriggerZone;
        private static readonly Texture2D icon_CameraTarget;
        private static readonly Texture2D icon_CameraMultiTarget;
        private static readonly Texture2D icon_CameraDolly;
        private static readonly Texture2D icon_CameraTrack;
        private static readonly Texture2D icon_ShadowTransform;

        private static readonly string filePath = "Assets/Gaskellgames/Camera Controller/Editor/Icons/";

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Editor Loop

        static HierarchyIcon_CameraController()
        {
            icon_CameraBrain = AssetDatabase.LoadAssetAtPath(filePath + "Icon_CameraBrain.png", typeof(Texture2D)) as Texture2D;
            EditorApplication.hierarchyWindowItemOnGUI += DrawHierarchyIcon_CameraBrain;
            
            icon_CameraRig = AssetDatabase.LoadAssetAtPath(filePath + "Icon_CameraRig.png", typeof(Texture2D)) as Texture2D;
            EditorApplication.hierarchyWindowItemOnGUI += DrawHierarchyIcon_CameraRig;
            
            icon_CameraFreelookRig = AssetDatabase.LoadAssetAtPath(filePath + "Icon_CameraFreelookRig.png", typeof(Texture2D)) as Texture2D;
            EditorApplication.hierarchyWindowItemOnGUI += DrawHierarchyIcon_CameraFreelookRig;
            
            icon_CameraSwitcher = AssetDatabase.LoadAssetAtPath(filePath + "Icon_CameraSwitcher.png", typeof(Texture2D)) as Texture2D;
            EditorApplication.hierarchyWindowItemOnGUI += DrawHierarchyIcon_CameraSwitcher;
            
            icon_CameraShaker = AssetDatabase.LoadAssetAtPath(filePath + "Icon_CameraShaker.png", typeof(Texture2D)) as Texture2D;
            EditorApplication.hierarchyWindowItemOnGUI += DrawHierarchyIcon_CameraShaker;
            
            icon_CameraTriggerZone = AssetDatabase.LoadAssetAtPath(filePath + "Icon_CameraTriggerZone.png", typeof(Texture2D)) as Texture2D;
            EditorApplication.hierarchyWindowItemOnGUI += DrawHierarchyIcon_CameraTriggerZone;
            
            icon_CameraTarget = AssetDatabase.LoadAssetAtPath(filePath + "Icon_CameraTarget.png", typeof(Texture2D)) as Texture2D;
            EditorApplication.hierarchyWindowItemOnGUI += DrawHierarchyIcon_CameraTarget;
            
            icon_CameraMultiTarget = AssetDatabase.LoadAssetAtPath(filePath + "Icon_CameraMultiTarget.png", typeof(Texture2D)) as Texture2D;
            EditorApplication.hierarchyWindowItemOnGUI += DrawHierarchyIcon_CameraMultiTarget;
            
            icon_CameraDolly = AssetDatabase.LoadAssetAtPath(filePath + "Icon_CameraDolly.png", typeof(Texture2D)) as Texture2D;
            EditorApplication.hierarchyWindowItemOnGUI += DrawHierarchyIcon_CameraDolly;
            
            icon_CameraTrack = AssetDatabase.LoadAssetAtPath(filePath + "Icon_CameraTrack.png", typeof(Texture2D)) as Texture2D;
            EditorApplication.hierarchyWindowItemOnGUI += DrawHierarchyIcon_CameraTrack;
            
            icon_ShadowTransform = AssetDatabase.LoadAssetAtPath(filePath + "Icon_ShadowTransform.png", typeof(Texture2D)) as Texture2D;
            EditorApplication.hierarchyWindowItemOnGUI += DrawHierarchyIcon_ShadowTransform;
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Private Functions

        private static void DrawHierarchyIcon_CameraBrain(int instanceID, Rect position)
        {
            HierarchyUtility.DrawHierarchyIcon<CameraBrain>(instanceID, position, icon_CameraBrain);
        }

        private static void DrawHierarchyIcon_CameraRig(int instanceID, Rect position)
        {
            int offset = HierarchyUtility.CheckForHierarchyIconOffset<InputEventSystem.GMKInputController>(instanceID);
            HierarchyUtility.DrawHierarchyIcon<CameraRig>(instanceID, position, icon_CameraRig, offset);
        }

        private static void DrawHierarchyIcon_CameraFreelookRig(int instanceID, Rect position)
        {
            HierarchyUtility.DrawHierarchyIcon<CameraFreelookRig>(instanceID, position, icon_CameraFreelookRig);
        }

        private static void DrawHierarchyIcon_CameraSwitcher(int instanceID, Rect position)
        {
            int offset = HierarchyUtility.CheckForHierarchyIconOffset<CameraBrain>(instanceID);
            HierarchyUtility.DrawHierarchyIcon<CameraSwitcher>(instanceID, position, icon_CameraSwitcher, offset);
        }

        private static void DrawHierarchyIcon_CameraShaker(int instanceID, Rect position)
        {
            HierarchyUtility.DrawHierarchyIcon<CameraShaker>(instanceID, position, icon_CameraShaker);
        }

        private static void DrawHierarchyIcon_CameraTriggerZone(int instanceID, Rect position)
        {
            int offset = HierarchyUtility.CheckForHierarchyIconOffset<CameraMultiTarget>(instanceID);
            HierarchyUtility.DrawHierarchyIcon<CameraTriggerZone>(instanceID, position, icon_CameraTriggerZone, offset);
        }

        private static void DrawHierarchyIcon_CameraTarget(int instanceID, Rect position)
        {
            HierarchyUtility.DrawHierarchyIcon<CameraTarget>(instanceID, position, icon_CameraTarget);
        }

        private static void DrawHierarchyIcon_CameraMultiTarget(int instanceID, Rect position)
        {
            HierarchyUtility.DrawHierarchyIcon<CameraMultiTarget>(instanceID, position, icon_CameraMultiTarget);
        }

        private static void DrawHierarchyIcon_CameraDolly(int instanceID, Rect position)
        {
            HierarchyUtility.DrawHierarchyIcon<CameraDolly>(instanceID, position, icon_CameraDolly);
        }

        private static void DrawHierarchyIcon_CameraTrack(int instanceID, Rect position)
        {
            HierarchyUtility.DrawHierarchyIcon<CameraTrack>(instanceID, position, icon_CameraTrack);
        }

        private static void DrawHierarchyIcon_ShadowTransform(int instanceID, Rect position)
        {
            HierarchyUtility.DrawHierarchyIcon<ShadowTransform>(instanceID, position, icon_ShadowTransform);
        }

        #endregion
        
    } // class end
}

#endif