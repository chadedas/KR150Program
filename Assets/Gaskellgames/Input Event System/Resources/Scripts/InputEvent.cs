using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.InputEventSystem
{
    public class InputEvent : GGMonoBehaviour
    {
        #region Variables

        [SerializeField]
        private InputActionProperty userInput;
        
        [LineSeparator, SerializeField, ReadOnly]
        private bool buttonHeld;
        
        [Space, SerializeField]
        private UnityEvent OnPressed;
        
        [Space, SerializeField]
        private UnityEvent OnHeld;
        
        [Space, SerializeField]
        private UnityEvent OnReleased;

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Game loop

        private void OnEnable()
        {
            userInput.action.performed += userInputCallbackPerformed;
            userInput.action.canceled += userInputCallbackCanceled;
            userInput.action.Enable();
        }

        private void OnDisable()
        {
            userInput.action.performed -= userInputCallbackPerformed;
            userInput.action.canceled -= userInputCallbackCanceled;
            userInput.action.Disable();
        }

        private void Update()
        {
            if (buttonHeld)
            {
                OnHeld.Invoke();
            }
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Input Action Callbacks

        private void userInputCallbackPerformed(InputAction.CallbackContext context)
        {
            OnPressed.Invoke();
            buttonHeld = true;
        }

        private void userInputCallbackCanceled(InputAction.CallbackContext context)
        {
            OnReleased.Invoke();
            buttonHeld = false;
        }

        #endregion

    } // class end
}
