using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Alchemystical
{
    public class OptionsSubsection : MonoBehaviour
    {
        public UnityEvent activateEvent;
        public UnityEvent deactivateEvent;

        public UnityEvent leftButtonEvent;
        public UnityEvent rightButtonEvent;

        [SerializeField] private Image activeImage;
        [SerializeField, Range(0f, 1f)] private float clickwaitTime = 0.25f;
        [SerializeField] private GameObject[] uiInputInfoObjects;

        private bool isActive;
        private int index;
        private GameInput input;
        private bool canClick = true;

        private void OnEnable()
        {
            input = Game.Instance.input;
        }

        private void Update()
        {
            if (!isActive) return;
            CheckInputs();
        }

        private void CheckInputs()
        {
            if(input == null) return;
            if(!isActive) return;

            /*if (input.UIoptionsNavigatBackActive)
            {
                Debug.Log("InputBackPressed");
                input.UIoptionsNavigatBackActive = false;
                Deactivate();             
                return;
            }*/

            if (input.UIoptionsNavigatLeftActive)
            {
                if (!canClick) return;
                canClick = false;
                Debug.Log("InputLeftPressed");
                InvokeLeftEvent();
                return;
            }

            if (input.UIoptionsNavigatRightActive)
            {
                if (!canClick) return;
                canClick = false;
                Debug.Log("InputRightPressed");
                InvokeRightEvent();
                return;
            }
        }

        IEnumerator NextInputWait()
        {
            Debug.Log("Wait start");
            yield return new WaitForSecondsRealtime(clickwaitTime);
            Debug.Log("Wait done");
            canClick = true;
        }

        public void Activate()
        {
            if (isActive) return;
            Debug.Log("Activate");
            isActive = true;
            activateEvent?.Invoke();
            //ChangeUIInputinfoOnjectsStatus(true);
            //EventSystem.current.enabled = false;
            if(activeImage) activeImage.gameObject.SetActive(true);
            //input.SetExtraInputActionsStatus(false);
            input.SetUIInputActionsStatus(false);
            input.SetUiOptionsInputActionsStatus(true);

        }

        public void Deactivate()
        {
            if (!isActive) return;
            Debug.Log("Decativate");
            isActive = false;
            deactivateEvent?.Invoke();
            //ChangeUIInputinfoOnjectsStatus(false);
            //EventSystem.current.enabled = true;
            input.SetUIInputActionsStatus(true);
            if (activeImage) activeImage.gameObject.SetActive(false);
            //rootSectionComponent.SelectButton();
        }

        public void ChangeStatus(bool active)
        {
            isActive = active;
        }

        public void InvokeLeftEvent()
        {
            if (!isActive) return;
            leftButtonEvent?.Invoke();
            StartCoroutine(NextInputWait());
        }

        public void InvokeRightEvent()
        {
            if (!isActive) return;
            rightButtonEvent?.Invoke();
            StartCoroutine(NextInputWait());
        }

        /*public void ChangeUIInputinfoOnjectsStatus(bool active)
        {
            if (uiInputInfoObjects.Length < 1) return;

            foreach (var obj in uiInputInfoObjects)
            {
                obj.SetActive(active);
            }
        }*/



    }
}

