
using System;
using UnityEngine;

namespace Alchemystical
{
    public class Trader : MonoBehaviour
    {
        public static Action<Customer,ConversationType> StartTraderConversation;

        #region Fields

        [SerializeField] private Sprite traderSprite;
        [SerializeField] private string[] traderConversationTexts;
        [SerializeField] private GameObject traderShopUI;
        [SerializeField] private CustomerInfoUI customerInfoUI;
        private Customer trader;

        #endregion

        private void Awake()
        {
            trader = new Customer(CustomerType.Trader, traderSprite, traderConversationTexts[0]);
            
        }

        private void Start()
        {
            GameTime.TraderAppeared += ShowTrader;
        }


        private void OnDestroy()
        {
            GameTime.TraderAppeared -= ShowTrader;
        }
        private void ShowTrader()
        {
            customerInfoUI.ChangeMerchantStatus(true);
        }

        public void ShowTraderShop()
        {
            traderShopUI.SetActive(true);
            Game.Instance.conversations.CLoseConversationUI();
        }

        public void StartConversation()
        {
            trader.conversationText = SetConversationText();
            StartTraderConversation?.Invoke(trader, ConversationType.TraderConversation);
            customerInfoUI.ChangeMerchantStatus(false);
        }

        private string SetConversationText()
        {
            int random = UnityEngine.Random.Range(0, traderConversationTexts.Length);
            return traderConversationTexts[random];
        }

        public void CloseTraderShop()
        {
        }
    }
}

