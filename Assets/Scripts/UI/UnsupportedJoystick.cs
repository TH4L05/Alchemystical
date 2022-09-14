using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alchemystical
{
    internal class UnsupportedJoystick : MonoBehaviour
    {
        [SerializeField] private GameObject uiPanel;

        private void Awake()
        {
            GameInput.UnsupprotedJostickVersionActive += ShowInfo;
        }

        private void OnDestroy()
        {
            GameInput.UnsupprotedJostickVersionActive -= ShowInfo;
        }

        public void ShowInfo()
        {
            uiPanel.SetActive(true);
        }

    }
}

