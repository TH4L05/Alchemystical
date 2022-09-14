using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Alchemystical
{
    public class GameInput : MonoBehaviour
    {
        #region Events

        public static Action<bool> SpeedlinkJoystickDeviceChanged;
        public static Action SetupFinished;
        public static Action ToggleBrewMode;
        public static Action ToggleQuests;
        public static Action ToggleMenu;
        public static Action ToggleBook;
        public static Action ToggleConversations;
        public static Action UnsupprotedJostickVersionActive;
        public static Action<Vector2> MixInputValuesChanged;

        #endregion

        #region PrivateFields

        private Controls controls;
        private List<InputAction> gameBaseInputActions;
        private List<InputAction> brewInputActions;
        private List<InputAction> uiInputActions;
        private List<InputAction> uiOptionsInputActions;
        private bool speedLinkJoystickConnected = false;
        private int connectedJoystickVersion = -1;
        private bool destroy;

        #endregion

        #region PublicFields

        public bool BaseActionsAreActive;
        public bool BrewActionsAreActive;
        public bool UIActionsAreActive;
        public bool UIOptionsActionsAreActive;

        public bool UIInputPaused { get; set; }
        public bool UIoptionsNavigatLeftActive { get; set; }
        public bool UIoptionsNavigatRightActive { get; set; }
        public bool UIoptionsNavigatBackActive { get; set; }

        public static bool SpeedLinkPhantomHawkJoystickConnected;

        #endregion

        #region UnityFunctions

        private void Awake()
        {
            controls = new Controls();

            gameBaseInputActions = new List<InputAction>();
            brewInputActions = new List<InputAction>();
            //uiInputActions = new List<InputAction>();
            uiOptionsInputActions = new List<InputAction>();

            InputSystem.onDeviceChange += OnDeviceChange;
        }

        private void Start()
        {
            SetBaseInputActions();
            SetBrewInputActions();
            SetUIInputActions();
            SetUIoptionsInputActions();

            JoystickChanged(false, -1, null, InputDeviceChange.UsageChanged);
            if (Joystick.all.Count < 1)
            {
                SetupFinished?.Invoke();
                return;
            }

            if (Joystick.current.description.product == "PS3/PC Gamepad")
            {
                JoystickChanged(true, 0, null, InputDeviceChange.HardReset);
                Debug.Log("A Speedlink PhantomHawk Joystick is Connected");
            }
            //else if (Joystick.current.description.product == "Generic   USB  Joystick  ")
            else
            {
                UnsupprotedJostickVersionActive?.Invoke();
                JoystickChanged(false, -1, null, InputDeviceChange.UsageChanged);
                //Debug.Log("Another Version of Speedlink PhantomHawk Joystick is Connected");
                Debug.Log("Unsupported Version of Speedlink PhantomHawk Joystick is Connected");
            }

            //Debug.Log(Joystick.current.description.product + " / " + Joystick.current.description.version);
            SetupFinished?.Invoke();
        }

        private void OnDestroy()
        {
            destroy = true;
            Cursor.visible = true;
            InputSystem.onDeviceChange -= OnDeviceChange;
        }

        private void OnDeviceChange(InputDevice inputDevice, InputDeviceChange inputDeviceChange)
        {
            if (destroy) return;

            switch (inputDeviceChange)
            {
                case InputDeviceChange.Added:

                    if (inputDevice.description.product == "PS3/PC Gamepad")
                    {
                        JoystickChanged(true, 0, inputDevice, inputDeviceChange);
                    }
                    //else if (inputDevice.description.product == "Generic   USB  Joystick  ")
                    else
                    {
                        UnsupprotedJostickVersionActive?.Invoke();
                        JoystickChanged(false, -1, null, InputDeviceChange.UsageChanged);
                        //Debug.Log("Another Version of Speedlink PhantomHawk Joystick is Connected");
                        Debug.Log("Unsupported Version of Speedlink PhantomHawk Joystick is Connected");
                    }
                    break;

                case InputDeviceChange.Removed:

                    if (inputDevice.description.product == "PS3/PC Gamepad")
                    {
                        JoystickChanged(false, -1, inputDevice, inputDeviceChange);

                    }
                    //else if (inputDevice.description.product == "Generic   USB  Joystick  ")
                    //{
                    //}
                    break;

                case InputDeviceChange.Disconnected:

                    if (inputDevice.description.product == "PS3/PC Gamepad")
                    {
                        JoystickChanged(false, -1, inputDevice, inputDeviceChange);

                    }
                    //else if (inputDevice.description.product == "Generic   USB  Joystick  ")
                    //{
                    //}
                    break;

                case InputDeviceChange.Reconnected:

                    if (inputDevice.description.product == "PS3/PC Gamepad")
                    {
                        JoystickChanged(true, 0, inputDevice, inputDeviceChange);
                    }
                    //else if (inputDevice.description.product == "Generic   USB  Joystick  ")
                    else
                    {
                        UnsupprotedJostickVersionActive?.Invoke();
                        JoystickChanged(false, -1, null, InputDeviceChange.UsageChanged);
                        //Debug.Log("Another Version of Speedlink PhantomHawk Joystick is Connected");
                        Debug.Log("Unsupported Version of Speedlink PhantomHawk Joystick is Connected");
                    }
                    break;

                case InputDeviceChange.Enabled:

                    if (inputDevice.description.product == "PS3/PC Gamepad")
                    {
                        JoystickChanged(true, 0, inputDevice, inputDeviceChange);
                    }
                    //else if (inputDevice.description.product == "Generic   USB  Joystick  ")
                    else
                    {
                        UnsupprotedJostickVersionActive?.Invoke();
                        JoystickChanged(false, -1, null, InputDeviceChange.UsageChanged);
                        //Debug.Log("Another Version of Speedlink PhantomHawk Joystick is Connected");
                        Debug.Log("Unsupported Version of Speedlink PhantomHawk Joystick is Connected");
                    }
                    break;

                case InputDeviceChange.Disabled:

                    if (inputDevice.description.product == "PS3/PC Gamepad")
                    {
                        JoystickChanged(false, -1, inputDevice, inputDeviceChange);

                    }
                    //else if (inputDevice.description.product == "Generic   USB  Joystick  ")
                    //{
                    //}
                    break;

                case InputDeviceChange.UsageChanged:
                    break;

                case InputDeviceChange.ConfigurationChanged:
                    break;

                case InputDeviceChange.SoftReset:
                    break;

                case InputDeviceChange.HardReset:
                    break;
                default:
                    break;
            }
        }

        private void JoystickChanged(bool connected, int versionindex, InputDevice inputDevice, InputDeviceChange inputDeviceChange)
        {
            speedLinkJoystickConnected = connected;
            SpeedLinkPhantomHawkJoystickConnected = connected;
            connectedJoystickVersion = versionindex;
            SpeedlinkJoystickDeviceChanged?.Invoke(speedLinkJoystickConnected);
            
            /*if (connected)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }*/
            Cursor.visible = !connected;

            if (inputDevice == null) return;
            Debug.Log($"Speedlink PhantomHawk Joystick  -> \"{inputDevice.displayName}\" = {inputDeviceChange}");
        }

        #endregion

        #region SetInputs

        private void SetBaseInputActions()
        {
            var baseInputs = controls.GameBase;

            baseInputs.ToggleBewMode.performed += ToggleBewModePerformed;
            baseInputs.TogglePauseMenu.performed += ToggleMenuPerformed;
            baseInputs.ToggleQuests.performed += ToggleQuestsPerformed;
            baseInputs.ToggleBook.performed += ToggleBookPerformed;
            baseInputs.ToggleConversations.performed += ToggleConversationsPerformed;

            gameBaseInputActions.Add(baseInputs.ToggleBewMode);
            gameBaseInputActions.Add(baseInputs.TogglePauseMenu);
            gameBaseInputActions.Add(baseInputs.ToggleQuests);
            gameBaseInputActions.Add(baseInputs.ToggleBook);
            gameBaseInputActions.Add(baseInputs.ToggleConversations);

            SetBaseInputActionsStatus(true);
        }

        private void SetBrewInputActions()
        {
            var brewInputs = controls.Brew;

            brewInputs.Mix.performed += MixInputPerformed;

            brewInputActions.Add(brewInputs.Mix);
        }

        private void SetUIInputActions()
        {
            /*var uiInputs = controls.UI;
            uiInputs.NavigateUp.performed += NavigateUpIsPressed;
            uiInputs.NavigateUp.canceled += NavigateUpIsCanceled;
            uiInputActions.Add(uiInputs.NavigateUp);

            uiInputs.NavigateDown.performed += NavigateDownPressed;
            uiInputs.NavigateDown.canceled += NavigateDownCanceled;
            uiInputActions.Add(uiInputs.NavigateDown);

            uiInputs.NavigateLeft.performed += NavigateLeftPressed;
            uiInputs.NavigateLeft.canceled += NavigateLeftCanceled;
            uiInputActions.Add(uiInputs.NavigateLeft);

            uiInputs.NavigateRight.performed += NavigateRightPressed;
            uiInputs.NavigateRight.canceled += NavigateRightCanceled;
            uiInputActions.Add(uiInputs.NavigateRight);

            uiInputs.NavigateNext.performed += NavigateNext_performed;
            uiInputActions.Add(uiInputs.NavigateNext);

            uiInputs.NavigatePrevious.performed += NavigatePrevious_performed;
            uiInputActions.Add(uiInputs.NavigatePrevious);

            uiInputs.PhotoGalleryDeletePicture.performed += PhotoGalleryDeletePicture_performed;
            uiInputs.PhotoGalleryDeletePicture.canceled += PhotoGalleryDeletePicture_canceled;
            uiInputActions.Add(uiInputs.PhotoGalleryDeletePicture);*/

            //SetUIInputActionsStatus(true);
        }

        private void SetUIoptionsInputActions()
        {
            var uiOptionsInputs = controls.UIoptions;

            uiOptionsInputs.OptionsSubSectionBack.performed += OptionsSubSectionBack_performed;
            uiOptionsInputActions.Add(uiOptionsInputs.OptionsSubSectionBack);

            uiOptionsInputs.OptionsSubSectionNavigateRight.performed += OptionsSubSectionNavigateRight_performed;
            uiOptionsInputs.OptionsSubSectionNavigateRight.canceled += OptionsSubSectionNavigateRight_canceled;
            uiOptionsInputActions.Add(uiOptionsInputs.OptionsSubSectionNavigateRight);

            uiOptionsInputs.OptionsSubSectionNavigateLeft.performed += OptionsSubSectionNavigateLeft_performed;
            uiOptionsInputs.OptionsSubSectionNavigateLeft.canceled += OptionsSubSectionNavigateLeft_canceled;
            uiOptionsInputActions.Add(uiOptionsInputs.OptionsSubSectionNavigateLeft);
        }

        #endregion

        #region Enable/DisableInputActions

        public void SetBaseInputActionsStatus(bool enable)
        {
            BaseActionsAreActive = enable;
            if (enable)
            {
                foreach (var inputAction in gameBaseInputActions)
                {
                    inputAction.Enable();
                }
            }
            else
            {
                foreach (var inputAction in gameBaseInputActions)
                {
                    inputAction.Disable();
                }
            }
        }

        public void SetBrewInputActionsStatus(bool enable)
        {
            BrewActionsAreActive = enable;
            if (enable)
            {
                foreach (var inputAction in brewInputActions)
                {
                    inputAction.Enable();
                }
            }
            else
            {
                foreach (var inputAction in brewInputActions)
                {
                    inputAction.Disable();
                }
            }
        }

        public void SetUIInputActionsStatus(bool enable)
        {
            UIActionsAreActive = enable;
            if (enable)
            {
                foreach (var inputAction in uiInputActions)
                {
                    inputAction.Enable();
                }
            }
            else
            {
                foreach (var inputAction in uiInputActions)
                {
                    inputAction.Disable();
                }
            }
        }

        public void SetUiOptionsInputActionsStatus(bool enable)
        {
            UIOptionsActionsAreActive = enable;
            if (enable)
            {
                foreach (var inputAction in uiOptionsInputActions)
                {
                    inputAction.Enable();
                }
            }
            else
            {
                foreach (var inputAction in uiOptionsInputActions)
                {
                    inputAction.Disable();
                }
            }
        }

        public void ChangeAllInputActionListStatus(bool enable)
        {
            SetBaseInputActionsStatus(enable);
            SetUIInputActionsStatus(enable);
            SetUiOptionsInputActionsStatus(enable);
            SetBrewInputActionsStatus(enable);
        }

        #endregion

        #region BaseGameinput

        private void ToggleBewModePerformed(InputAction.CallbackContext callbackContext)
        {
            ToggleBrewMode?.Invoke();
        }

        private void ToggleMenuPerformed(InputAction.CallbackContext callbackContext)
        {
            ToggleMenu?.Invoke();
        }

        private void ToggleQuestsPerformed(InputAction.CallbackContext callbackContext)
        {
            ToggleQuests?.Invoke();
        }

        private void ToggleBookPerformed(InputAction.CallbackContext callbackContext)
        {
            ToggleBook?.Invoke();
        }

        private void ToggleConversationsPerformed(InputAction.CallbackContext callbackContext)
        {
            ToggleConversations?.Invoke();
        }

        #endregion

        #region UIoptionsInput

        private void OptionsSubSectionNavigateLeft_canceled(InputAction.CallbackContext obj)
        {
            UIoptionsNavigatLeftActive = false;
        }

        private void OptionsSubSectionNavigateLeft_performed(InputAction.CallbackContext obj)
        {
            UIoptionsNavigatLeftActive = true;
        }

        private void OptionsSubSectionNavigateRight_performed(InputAction.CallbackContext obj)
        {
            UIoptionsNavigatRightActive = true;
        }

        private void OptionsSubSectionNavigateRight_canceled(InputAction.CallbackContext obj)
        {
            UIoptionsNavigatRightActive = false;
        }

        private void OptionsSubSectionBack_performed(InputAction.CallbackContext obj)
        {
            UIoptionsNavigatBackActive = true;
        }

        #endregion

        #region BrewInput

        private void MixInputPerformed(InputAction.CallbackContext callbackContext)
        {
            Vector2 inputVector = callbackContext.ReadValue<Vector2>();
            MixInputValuesChanged?.Invoke(inputVector);
        }


        #endregion
    }
}