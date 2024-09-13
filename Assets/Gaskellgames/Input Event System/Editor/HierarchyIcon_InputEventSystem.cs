#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.InputEventSystem
{
    [InitializeOnLoad]
    public class HierarchyIcon_InputEventSystem
    {
        #region Variables

        private static readonly Texture2D icon_InputActionManager;
        private static readonly Texture2D icon_GMKInputController;
        private static readonly Texture2D icon_XRInputController;
        private static readonly Texture2D icon_InputEvent;

        #endregion
        
        //----------------------------------------------------------------------------------------------------

        #region Editor Loop

        static HierarchyIcon_InputEventSystem()
        {
            icon_InputActionManager = AssetDatabase.LoadAssetAtPath("Assets/Gaskellgames/Input Event System/Editor/Icons/Icon_InputActionManager.png", typeof(Texture2D)) as Texture2D;
            EditorApplication.hierarchyWindowItemOnGUI += DrawHierarchyIcon_InputActionManager;
            
            icon_GMKInputController = AssetDatabase.LoadAssetAtPath("Assets/Gaskellgames/Input Event System/Editor/Icons/Icon_GMKInputController.png", typeof(Texture2D)) as Texture2D;
            EditorApplication.hierarchyWindowItemOnGUI += DrawHierarchyIcon_GMKInputController;
            
            icon_XRInputController = AssetDatabase.LoadAssetAtPath("Assets/Gaskellgames/Input Event System/Editor/Icons/Icon_XRInputController.png", typeof(Texture2D)) as Texture2D;
            EditorApplication.hierarchyWindowItemOnGUI += DrawHierarchyIcon_XRInputController;
            
            icon_InputEvent = AssetDatabase.LoadAssetAtPath("Assets/Gaskellgames/Input Event System/Editor/Icons/Icon_InputEvent.png", typeof(Texture2D)) as Texture2D;
            EditorApplication.hierarchyWindowItemOnGUI += DrawHierarchyIcon_InputEvent;
        }

        #endregion
        
        //----------------------------------------------------------------------------------------------------

        #region Private Functions

        private static void DrawHierarchyIcon_InputActionManager(int instanceID, Rect position)
        {
            HierarchyUtility.DrawHierarchyIcon<InputActionManager>(instanceID, position, icon_InputActionManager);
        }

        private static void DrawHierarchyIcon_GMKInputController(int instanceID, Rect position)
        {
            HierarchyUtility.DrawHierarchyIcon<GMKInputController>(instanceID, position, icon_GMKInputController);
        }

        private static void DrawHierarchyIcon_XRInputController(int instanceID, Rect position)
        {
            HierarchyUtility.DrawHierarchyIcon<XRInputController>(instanceID, position, icon_XRInputController);
        }

        private static void DrawHierarchyIcon_InputEvent(int instanceID, Rect position)
        {
            HierarchyUtility.DrawHierarchyIcon<InputEvent>(instanceID, position, icon_InputEvent);
        }

        #endregion
        
    } // class end
}

#endif