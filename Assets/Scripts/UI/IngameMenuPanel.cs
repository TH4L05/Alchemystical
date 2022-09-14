/// <author>Thoams Krahl</author>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Alchemystical
{
    public class IngameMenuPanel : MonoBehaviour
    {
        #region Fields

        [SerializeField] private bool active = true;
        [SerializeField] private bool updateFirstButton = true;
        [SerializeField] private bool resetFirstButtonOnDisable = false;
        [SerializeField] private ButtonExtra firstSelectedButton;
        [SerializeField] private ButtonExtra defaultSelectedButton;
        [SerializeField] private List<ButtonExtra> buttons = new List<ButtonExtra>();
        

        #endregion

        #region UnityFunctions

        private void OnEnable()
        {
            if(firstSelectedButton == null) firstSelectedButton = defaultSelectedButton;
            ResetButtonTransitions();
            SelectButton();
        }

        private void OnDisable()
        {
            if(!resetFirstButtonOnDisable) return;
            ResetFirstSelectedButton();
        }

        public void ResetButtonTransitions()
        {
            foreach (var button in buttons)
            {
                button.ResetTransition();
            }
        }

        public void AddButton(ButtonExtra button)
        {
            buttons.Add(button);
        }

        public void RemoveButton(ButtonExtra button)
        {
            buttons.Remove(button);
        }

        public void SetFirstSelectedButtonByIndex(int index)
        {
            if (index < 0) return;
            if(index > buttons.Count) return;
            firstSelectedButton = buttons[index];
        }

        public void ResetFirstSelectedButton()
        {
            firstSelectedButton = defaultSelectedButton;
        }

        public void SetSelectedButton()
        {
            if(!updateFirstButton) return;
            var selectedObj = EventSystem.current.currentSelectedGameObject;
            selectedObj.TryGetComponent(out firstSelectedButton);
        }

        public void SelectButton()
        {
            if (firstSelectedButton && active) StartCoroutine(SelectFirstButton());
        }

        #endregion

        IEnumerator SelectFirstButton()
        {
            EventSystem.current.SetSelectedGameObject(null);
            yield return new WaitForEndOfFrame();
            Debug.Log("First Button of Enabled Panel gets Selected");
            firstSelectedButton.Select();
        }
    }
}
