using System.Collections;
using TK.Audio;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace Alchemystical
{
    public class IngredientBook : MonoBehaviour
    {
        public UnityEvent ShowBookFullEvent;
        public UnityEvent HideBookFullEvent;


        [SerializeField] private ButtonExtra nextPageButton;
        [SerializeField] private ButtonExtra previousPageButton;
        [SerializeField] private IngredientPage[] pages;
        [SerializeField] private IngredientPage[] worldBookpages;
        [SerializeField] private IngameMenuPanel ingameMenuPanelRef;


        private Potion[] potions;
        private int currentPageIndex = 0;
        private bool showBook = false;

        private void Start()
        {
            potions = Game.Instance.gameData.potions;
            GameInput.ToggleBook += ToggleBook;
            
        }

        private void OnEnable()
        {
            //StartCoroutine(StartSetup());
        }

        IEnumerator StartSetup()
        {
            yield return new WaitForSeconds(1.0f);
            ShowPages(currentPageIndex);
        }

        private void OnDestroy()
        {
            GameInput.ToggleBook -= ToggleBook;
        }

        public void NextPage()
        {

            currentPageIndex += 2;
            if (currentPageIndex > potions.Length -1) currentPageIndex = potions.Length - 1;
            ShowPages(currentPageIndex);
        }

        public void PreviousPage()
        {
            currentPageIndex -= 2;
            if (currentPageIndex < 0)
            {
                currentPageIndex = 0;
            }
            ShowPages(currentPageIndex);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void Show(Potion potion, int pageIndex)
        {
            if (potion == null)
            {
                pages[pageIndex].ShowBlank();
                worldBookpages[pageIndex].ShowBlank();
                return;
            }

            pages[pageIndex].Show(potion);
            worldBookpages[pageIndex].Show(potion);
        }

        private void ShowPages(int index)
        {
            nextPageButton.interactable = true;
            previousPageButton.interactable = true;
        
            var currentIngredientType = potions[currentPageIndex];
            var secondIngredientType = potions[currentPageIndex];
            if (currentPageIndex +1 >= potions.Length)
            {
                if (nextPageButton != null)
                {
                    nextPageButton.interactable = false;
                    ingameMenuPanelRef.SetFirstSelectedButtonByIndex(1);
                    ingameMenuPanelRef.SelectButton();
                }
                secondIngredientType = null;
            }
            else
            {
                secondIngredientType = potions[currentPageIndex +1];
            }
             
            Show(currentIngredientType, 0);
            Show(secondIngredientType, 1);

            if (currentPageIndex == 0)
            {
                if (previousPageButton != null)
                {
                    previousPageButton.interactable = false;
                    ingameMenuPanelRef.SetFirstSelectedButtonByIndex(2);
                    ingameMenuPanelRef.SelectButton();
                }

            }
        }

        public void ToggleBook()
        {
            showBook = !showBook;

            if (showBook)
            {
                ShowBookFullEvent?.Invoke();
                
            }
            else
            {
                HideBookFullEvent?.Invoke();
            }
        }
    }
}
