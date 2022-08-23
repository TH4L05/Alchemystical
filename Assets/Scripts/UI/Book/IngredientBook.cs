using UnityEngine;
using UnityEngine.Playables;

namespace Alchemystical
{
    public class IngredientBook : MonoBehaviour
    {
        [SerializeField]
        private Ingredient[] ingredients;
        private Potion[] potions;

        [SerializeField] private GameObject nextPageButton;
        [SerializeField] private GameObject previousPageButton;

        private int currentPageIndex = 0;
        [SerializeField] private IngredientPage[] pages;
        [SerializeField] private IngredientPage[] worldBookpages;

        [SerializeField] private PlayableDirector showFull;
        [SerializeField] private PlayableDirector hideFull;

        private void Start()
        {
            ingredients = Game.Instance.gameData.ingredients;
            potions = Game.Instance.gameData.potions;
            Setup();
        }

        public void NextPage()
        {

            currentPageIndex += 2;
            if (currentPageIndex > potions.Length -1) currentPageIndex = potions.Length - 1;
            ShowPage(currentPageIndex);
        }

        public void PreviousPage()
        {
            currentPageIndex -= 2;
            if (currentPageIndex < 0)
            {
                currentPageIndex = 0;
            }
            ShowPage(currentPageIndex);
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

        private void ShowPage(int index)
        {
            nextPageButton.SetActive(true);
            previousPageButton.SetActive(true);
        
            var currentIngredientType = potions[currentPageIndex];
            var secondIngredientType = potions[currentPageIndex];
            if (currentPageIndex +1 >= potions.Length)
            {
                if (nextPageButton != null) nextPageButton.SetActive(false);
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
                if (previousPageButton != null) previousPageButton.SetActive(false);
            }
        }

        public void Setup()
        {
            ShowPage(currentPageIndex);
        }

        public void ShowInFull(bool show)
        {
            if (show)
            {
                if (showFull != null) showFull.Play();
            }
            else
            {
                if (hideFull != null) hideFull.Play();
            }
        }
    }
}
