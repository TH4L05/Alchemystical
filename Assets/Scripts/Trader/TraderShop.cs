using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Alchemystical
{
    public class TraderShop : MonoBehaviour
    {
        public UnityEvent OnShopEnabled;
        public UnityEvent OnShopDisabled;

        [SerializeField] private Ingredient[] ingredients;
        [SerializeField] private List<Offer> offers = new List<Offer>();
        [SerializeField, Range(0, 99)] private int maxOfferSize = 50;

        private void Awake()
        {
            Offer.BuyOffer += BuyAOffer;
        }

        private void OnEnable()
        {
            ingredients = Game.Instance.gameData.ingredients;
            UpdateOffers();
            Game.Instance.inventory.ChangeInvewntoryUIObjectStatus(true);
            OnShopEnabled?.Invoke();
        }

        private void OnDisable()
        {
            if (Brew.onBrewMode) return;
            Game.Instance.inventory.ChangeInvewntoryUIObjectStatus(false);
            OnShopDisabled?.Invoke();
        }

        public void UpdateOffers()
        {
            foreach (var offer in offers)
            {
                offer.SetOfferSize(maxOfferSize);
                int index = Random.Range(0, ingredients.Length);
                var ingridient = ingredients[index];
                offer.SetIngriedient(ingridient);
            }
        }
        private void BuyAOffer(Ingredient type, int offerSize)
        {
            int requiredAmount = type.shopPrice * offerSize;
            bool canBuy = SufficientMoney(requiredAmount);

            if (!canBuy) return;
            Game.Instance.inventory.RemoveGold(requiredAmount);
            Game.Instance.inventory.AddItemAmount(type, offerSize);

            foreach (var offer in offers)
            {
                if (offer.OfferSize == offerSize && offer.IngredientType.ingredientName == type.ingredientName)
                {
                    offer.ResetSlot();
                    return;
                }
            }
        }

        private bool SufficientMoney(int requiredAmount)
        {
            int currentAmount = Game.Instance.inventory.Gold;

            if (currentAmount >= requiredAmount)
            {
                return true;
            }

            Debug.Log("NOT ENGOUGH MONEY TO BUY OFFER");
            return false;
        }
    }
}
