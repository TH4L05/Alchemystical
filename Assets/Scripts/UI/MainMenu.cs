using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Alchemystical
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI joystickInfoTextField;
        private bool speedlinkJoystickActive;

        public void Start()
        {
            GameInput.SpeedlinkJoystickDeviceChanged += SpeedlinkJoystickActive;
        }

        private void OnDestroy()
        {
            GameInput.SpeedlinkJoystickDeviceChanged -= SpeedlinkJoystickActive;
        }


        private void SpeedlinkJoystickActive(bool active)
        {
            speedlinkJoystickActive = active;
            if (joystickInfoTextField)

                if (speedlinkJoystickActive)
                {
                    joystickInfoTextField.text = $"speedlink Phantom Hawk Joystick connected";
                }
                else
                {
                    joystickInfoTextField.text = $"NO speedlink Phantom Hawk Joystick connected";
                }
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;

#else
        Application.Quit();
    
#endif
        }

    }
}

