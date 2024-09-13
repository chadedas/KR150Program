using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gaskellgames.InputEventSystem
{
    /// <summary>
    /// Code created by Gaskellgames
    /// </summary>
    
    public class InputActionManager : GGMonoBehaviour
    {
        #region Variables

        [SerializeField, RequiredField]
        [Tooltip("Automatically enabled and disabled action assets")]
        private List<InputActionAsset> inputActionAssets;

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Game loop

#if UNITY_EDITOR
        
        private void Reset()
        {
            SetupInputActionManager();
        }
        
#endif

        private void OnEnable()
        {
            EnableAllInputActions();
        }

        private void OnDisable()
        {
            DisableAllInputActions();
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Private Functions

        private void EnableAllInputActions()
        {
            if (inputActionAssets != null)
            {
                foreach (InputActionAsset asset in inputActionAssets)
                {
                    if (asset != null)
                    {
                        asset.Enable();
                    }
                }
            }
        }

        private void DisableAllInputActions()
        {
            if (inputActionAssets != null)
            {
                foreach (InputActionAsset asset in inputActionAssets)
                {
                    if (asset != null)
                    {
                        asset.Disable();
                    }
                }
            }
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Public Functions

#if UNITY_EDITOR
        
        public void SetupInputActionManager()
        {
            inputActionAssets = new List<InputActionAsset>();
            AddInputActionToManager(AssetDatabase.LoadAssetAtPath<InputActionAsset>("Assets/Gaskellgames/Input Event System/Resources/InputActions/InputActionsGaskellgames.inputactions"));
        }
        
#endif
        
        public void AddInputActionToManager(InputActionAsset iaa)
        {
            if (!inputActionAssets.Contains(iaa))
            {
                inputActionAssets.Add(iaa);
            }
        }

        #endregion
        
    } // class end
}