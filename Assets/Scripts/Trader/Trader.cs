
using System;
using TK.Audio;
using UnityEngine;

namespace Alchemystical
{
    public class Trader : MonoBehaviour
    {
        public static Action<Customer,ConversationType> StartTraderConversation;
        public static Action<bool> ChangeTraderInfo;
        public static Action ExitConversation;

        #region Fields

        [SerializeField] private Sprite traderSprite;
        [SerializeField] private string[] traderConversationTexts;
        [SerializeField] private GameObject traderShopUI;
        private Customer trader;

        [Header("Settings")]
        [SerializeField] private AudioEventList audioEventList;

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
            ChangeTraderInfo?.Invoke(true);
        }

        public void ShowTraderShop()
        {
            audioEventList.PlayAudioEventOneShot("ButtonClicked");
            traderShopUI.SetActive(true);
            ExitConversation?.Invoke();
        }

        public void StartConversation()
        {
            trader.conversationText = SetConversationText();
            StartTraderConversation?.Invoke(trader, ConversationType.TraderConversation);
            ChangeTraderInfo?.Invoke(false);
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

