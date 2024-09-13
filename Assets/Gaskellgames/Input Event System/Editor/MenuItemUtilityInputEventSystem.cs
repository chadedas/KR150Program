#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.InputEventSystem
{
    public class MenuItemUtilityInputEventSystem : MenuItemUtility
    {
        #region Tools Menu
        
        #endregion
        
        //----------------------------------------------------------------------------------------------------

        #region GameObject Menu
        
        [MenuItem(InputEventSystem_GameObjectMenu_Path + "/Input Actions Manager", true, InputEventSystem_GameObjectMenu_Priority)]
        private static bool Gaskellgames_GameobjectMenu_InputActionsManagerValidate()
        {
            return !(GameObject.FindObjectOfType<InputActionManager>());
        }
        
        [MenuItem(InputEventSystem_GameObjectMenu_Path + "/Input Actions Manager", false, InputEventSystem_GameObjectMenu_Priority)]
        private static void Gaskellgames_GameobjectMenu_InputActionsManager(MenuCommand menuCommand)
        {
            // create a custom gameObject, register in the undo system, parent and set position relative to context
            GameObject go = SetupMenuItemInContext(menuCommand, "InputActionsManager");
            
            // Add scripts & components
            InputActionManager inputActionManager = go.AddComponent<InputActionManager>();
            inputActionManager.SetupInputActionManager();
            
            // select newly created gameObject
            Selection.activeObject = go;
        }
        
        [MenuItem(InputEventSystem_GameObjectMenu_Path + "/Input Event", false, InputEventSystem_GameObjectMenu_Priority + 15)]
        private static void Gaskellgames_GameobjectMenu_InputEvent(MenuCommand menuCommand)
        {
            // create a custom gameObject, register in the undo system, parent and set position relative to context
            GameObject go = SetupMenuItemInContext(menuCommand, "InputEvent");
            
            // Add scripts & components
            go.AddComponent<InputEvent>();
            
            // select newly created gameObject
            Selection.activeObject = go;
        }
        
        [MenuItem(InputEventSystem_GameObjectMenu_Path + "/Input Controller (Gamepad, Mouse, Keyboard)", false, InputEventSystem_GameObjectMenu_Priority + 30)]
        private static GMKInputController Gaskellgames_GameobjectMenu_GMKInputController(MenuCommand menuCommand)
        {
            // add input action manager
            AddInputActionsManager(menuCommand);
            
            // create a custom gameObject, register in the undo system, parent and set position relative to context
            GameObject go = SetupMenuItemInContext(menuCommand, "InputController (GMK)");
            
            // Add scripts & components
            GMKInputController inputController = go.AddComponent<GMKInputController>();
            PlayerInput playerInput = go.AddComponent<PlayerInput>();
            inputController.PlayerInput = playerInput;
            playerInput.actions = AssetDatabase.LoadAssetAtPath<InputActionAsset>("Assets/Gaskellgames/Input Event System/Resources/InputActions/InputActionsGaskellgames.inputactions");

            // select newly created gameObject
            Selection.activeObject = go;
            
            return inputController;
        }
        
        [MenuItem(InputEventSystem_GameObjectMenu_Path + "/Input Controller (Extended Reality)", false, InputEventSystem_GameObjectMenu_Priority + 30)]
        private static void Gaskellgames_GameobjectMenu_XRInputController(MenuCommand menuCommand)
        {
            // add input action manager
            AddInputActionsManager(menuCommand);
            
            // create a custom gameObject, register in the undo system, parent and set position relative to context
            GameObject go = SetupMenuItemInContext(menuCommand, "InputController (XR)");
            
            // Add scripts & components
            XRInputController inputController = go.AddComponent<XRInputController>();
            PlayerInput playerInput = go.AddComponent<PlayerInput>();
            inputController.PlayerInput = playerInput;
            playerInput.actions = AssetDatabase.LoadAssetAtPath<InputActionAsset>("Assets/Gaskellgames/Input Event System/Resources/InputActions/InputActionsGaskellgames.inputactions");
            
            // select newly created gameObject
            Selection.activeObject = go;
        }
        
        #endregion
        
        //----------------------------------------------------------------------------------------------------

        #region Public Functions

        public static GMKInputController AddPlayerInputController(MenuCommand menuCommand)
        {
            return Gaskellgames_GameobjectMenu_GMKInputController(menuCommand);
        }

        public static void AddInputActionsManager(MenuCommand menuCommand)
        {
            if (Gaskellgames_GameobjectMenu_InputActionsManagerValidate())
            {
                Gaskellgames_GameobjectMenu_InputActionsManager(menuCommand);
            }
        }

        #endregion
        
    } // class end
}

#endif