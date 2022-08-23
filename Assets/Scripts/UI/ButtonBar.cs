/// <author>Thoams Krahl</author>

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Alchemystical
{
    public class ButtonBar : MonoBehaviour
    {
        #region SerializedFields

        [SerializeField] protected List<GameObject> panels = new List<GameObject>();
        [SerializeField] protected List<GameObject> decorations = new List<GameObject>();
        [SerializeField] protected bool activateFirstPanelOnEnable = false;

        #endregion

        #region PrivateFields

        protected bool isActive;
        protected int lastIndex = 0;
        protected int lastDecorationIndex = 0;
        protected int currentPanelIndex;

        #endregion

        #region UnityFunctions

        private void Start()
        {
            currentPanelIndex = 0;
            lastDecorationIndex = 0;
            lastIndex = currentPanelIndex;
        }

        private void OnEnable()
        {
            isActive = true;
            DisableAllPanels();
            DisableAllDecorations();
            if (!activateFirstPanelOnEnable) return;
            ActivatePanel(currentPanelIndex);
            if(decorations.Count > 0) ActivateDecoration(lastDecorationIndex);
                 
        }

        #endregion

        public void DisableAllPanels()
        {
            foreach (var panels in panels)
            {
                panels.SetActive(false);
            }
        }

        public void ActivatePanel(int index)
        {         
            lastIndex = index;
            panels[index].SetActive(true);
        }

        public void DisablePanel(int index)
        {
            lastIndex = index;
            panels[index].SetActive(false);
        }

        public void ActivateDecoration(int index)
        {
            if (decorations.Count < 1) return;           
            decorations[index].SetActive(true);
        }  

        public void SetLastDecorationIndex(int index)
        {
            lastDecorationIndex = index;
        }

        public void DisableAllDecorations()
        {
            if (decorations.Count < 1) return;
            foreach (var deco in decorations)
            {
                deco.SetActive(false);
            }
        }

        public void ActivateLastDecoration()
        {
            if (decorations.Count < 1) return;
            decorations[lastDecorationIndex].SetActive(true);
        }
    }
}

