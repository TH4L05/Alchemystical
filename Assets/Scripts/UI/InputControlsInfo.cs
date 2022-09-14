using Alchemystical;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControlsInfo : MonoBehaviour
{
    [SerializeField] private GameObject joystickObj;
    [SerializeField] private GameObject keyboardObj;

    private void OnEnable()
    {
        if (GameInput.SpeedLinkPhantomHawkJoystickConnected)
        {
            joystickObj.SetActive(true);
            keyboardObj.SetActive(false);
        }
        else
        {
            joystickObj.SetActive(false);
            keyboardObj.SetActive(true);
        }
    }


}
