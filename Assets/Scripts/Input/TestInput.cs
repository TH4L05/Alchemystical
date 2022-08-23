using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestInput : MonoBehaviour
{
    public GameObject inventoryGameobject;
    private bool inventoryActive;

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            ToggleInventory();
        }
    }

    private void ToggleInventory()
    {
        inventoryActive = !inventoryActive;
        inventoryGameobject.SetActive(inventoryActive);
    }

}
