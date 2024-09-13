#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

using Gaskellgames.InputEventSystem;

namespace Gaskellgames.CameraController
{
    public class MenuItemUtilityCameraController : MenuItemUtility
    {
        #region Tools Menu
        
        #endregion
        
        //----------------------------------------------------------------------------------------------------
        
        #region GameObject Menu
        
        [MenuItem(CameraController_GameObjectMenu_Path + "/Camera Brain", false, CameraController_GameObjectMenu_Priority)]
        private static void Gaskellgames_GameobjectMenu_CameraBrain(MenuCommand menuCommand)
        {
            // create a custom gameObject, register in the undo system, parent and set position relative to context
            GameObject go = SetupMenuItemInContext(menuCommand, "CameraBrain");
            
            // add scripts & components
            go.AddComponent<Camera>();
            go.AddComponent<AudioListener>();
            go.AddComponent<CameraBrain>();
            
            // select newly created gameObject
            Selection.activeObject = go;
        }
        
        [MenuItem(CameraController_GameObjectMenu_Path + "/Camera Rig", false, CameraController_GameObjectMenu_Priority + 15)]
        private static void Gaskellgames_GameobjectMenu_CameraRig(MenuCommand menuCommand)
        {
            // create a custom gameObject, register in the undo system, parent and set position relative to context
            GameObject go = SetupMenuItemInContext(menuCommand, "CameraRig");
            
            // add scripts & components
            go.AddComponent<CameraRig>();
            
            // select newly created gameObject
            Selection.activeObject = go;
        }
        
        [MenuItem(CameraController_GameObjectMenu_Path + "/Camera Rig (Free Fly)", false, CameraController_GameObjectMenu_Priority + 15)]
        private static void Gaskellgames_GameobjectMenu_CameraRigFreeFly(MenuCommand menuCommand)
        {
            // create a custom gameObject, register in the undo system, parent and set position relative to context
            GMKInputController gmk = MenuItemUtilityInputEventSystem.AddPlayerInputController(menuCommand);
            GameObject go = gmk.gameObject;
            go.name = "CameraRig (Free Fly)";
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            GameObject context = (GameObject)menuCommand.context;
            if(context != null) { go.transform.SetParent(context.transform); }
            go.transform.localPosition = Vector3.zero;
            
            // add scripts & components
            CameraRig cr = go.AddComponent<CameraRig>();
            cr.GMKInputController = gmk;
            cr.IsFreeFlyCamera = true;
            UnityEditorInternal.ComponentUtility.MoveComponentUp(cr);
            UnityEditorInternal.ComponentUtility.MoveComponentUp(cr);
            
            // select newly created gameObject
            Selection.activeObject = go;
        }
        
        [MenuItem(CameraController_GameObjectMenu_Path + "/Camera Rig (Freelook)", false, CameraController_GameObjectMenu_Priority + 15)]
        private static void Gaskellgames_GameobjectMenu_CameraFreelookRig(MenuCommand menuCommand)
        {
            // add input action manager
            MenuItemUtilityInputEventSystem.AddInputActionsManager(menuCommand);
            
            // create a custom gameObject, register in the undo system, parent and set position relative to context
            GameObject go = SetupMenuItemInContext(menuCommand, "CameraRig (Freelook)");
            
            // add scripts & components
            go.AddComponent<CameraFreelookRig>();
            
            // select newly created gameObject
            Selection.activeObject = go;
        }
        
        [MenuItem(CameraController_GameObjectMenu_Path + "/Camera Track", false, CameraController_GameObjectMenu_Priority + 30)]
        private static void Gaskellgames_GameobjectMenu_CameraTrack(MenuCommand menuCommand)
        {
            // create a custom gameObject, register in the undo system, parent and set position relative to context
            GameObject go = SetupMenuItemInContext(menuCommand, "CameraTrack");
            
            // add scripts & components
            go.AddComponent<CameraTrack>();
            
            // select newly created gameObject
            Selection.activeObject = go;
        }
        
        [MenuItem(CameraController_GameObjectMenu_Path + "/Camera Dolly", false, CameraController_GameObjectMenu_Priority + 30)]
        private static void Gaskellgames_GameobjectMenu_CameraDolly(MenuCommand menuCommand)
        {
            // create a custom gameObject, register in the undo system, parent and set position relative to context
            GameObject go = SetupMenuItemInContext(menuCommand, "CameraDolly");
            
            // add scripts & components
            go.AddComponent<CameraDolly>();
            
            // select newly created gameObject
            Selection.activeObject = go;
        }
        
        [MenuItem(CameraController_GameObjectMenu_Path + "/Camera Shaker", false, CameraController_GameObjectMenu_Priority + 45)]
        private static void Gaskellgames_GameobjectMenu_CameraShaker(MenuCommand menuCommand)
        {
            // create a custom gameObject, register in the undo system, parent and set position relative to context
            GameObject go = SetupMenuItemInContext(menuCommand, "CameraShaker");
            
            // add scripts & components
            go.AddComponent<CameraShaker>();
            
            // select newly created gameObject
            Selection.activeObject = go;
        }
        
        [MenuItem(CameraController_GameObjectMenu_Path + "/Camera Trigger Zone", false, CameraController_GameObjectMenu_Priority + 45)]
        private static void Gaskellgames_GameobjectMenu_CameraTriggerZone(MenuCommand menuCommand)
        {
            // create a custom gameObject, register in the undo system, parent and set position relative to context
            GameObject go = SetupMenuItemInContext(menuCommand, "CameraTriggerZone");
            
            // Create child gameObjects
            GameObject goChild1 = new GameObject("CameraRig");
            GameObject goChild2 = new GameObject("Ref: CamLookAt");
            GameObject goChild3 = new GameObject("CameraTriggerZone");
            goChild1.transform.SetParent(go.transform);
            goChild2.transform.SetParent(go.transform);
            goChild3.transform.SetParent(go.transform);
            
            // add scripts & components
            goChild1.transform.position = new Vector3(-3, 2, -3);
            CameraRig cr = goChild1.AddComponent<CameraRig>();
            cr.LookAt = goChild2.transform;
            goChild2.transform.position = Vector3.zero;
            goChild3.transform.position = Vector3.zero;
            goChild3.transform.localScale = new Vector3(3, 0.2f, 3);
            BoxCollider box = goChild3.AddComponent<BoxCollider>();
            box.isTrigger = true;
            CameraTriggerZone ctz = goChild3.AddComponent<CameraTriggerZone>();
            ctz.SetCameraRig(cr);
            
            // select newly created gameObject
            Selection.activeObject = go;
        }
        
        [MenuItem(CameraController_GameObjectMenu_Path + "/Camera Trigger Zone (Multi Target)", false, CameraController_GameObjectMenu_Priority + 45)]
        private static void Gaskellgames_GameobjectMenu_CameraTriggerZoneMultiTarget(MenuCommand menuCommand)
        {
            // create a custom gameObject, register in the undo system, parent and set position relative to context
            GameObject go = SetupMenuItemInContext(menuCommand, "CameraTriggerZone (Multi Target)");
            
            // create child gameObjects
            GameObject goChild1 = new GameObject("CameraRig");
            GameObject goChild2 = new GameObject("Ref: CamLookAt");
            GameObject goChild3 = new GameObject("CameraTriggerZone");
            goChild1.transform.SetParent(go.transform);
            goChild2.transform.SetParent(go.transform);
            goChild3.transform.SetParent(go.transform);
            
            // add scripts & components
            goChild1.transform.position = new Vector3(-3, 2, -3);
            CameraRig cr = goChild1.AddComponent<CameraRig>();
            cr.LookAt = goChild2.transform;
            goChild2.transform.position = Vector3.zero;
            goChild3.transform.position = Vector3.zero;
            goChild3.transform.localScale = new Vector3(3, 0.2f, 3);
            BoxCollider box = goChild3.AddComponent<BoxCollider>();
            box.isTrigger = true;
            CameraMultiTarget cmt = goChild3.AddComponent<CameraMultiTarget>();
            cmt.SetRefCamLookAt(goChild2.transform);
            CameraTriggerZone ctz = goChild3.AddComponent<CameraTriggerZone>();
            ctz.SetCameraRig(cr);
            
            // select newly created gameObject
            Selection.activeObject = go;
        }
        
        #endregion
        
    } // class end
}

#endif