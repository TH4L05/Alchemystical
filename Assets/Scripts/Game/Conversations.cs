

using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Alchemystical
{
    public enum ConversationType
    {
        Invalid = -1,
        QuestStartConversation,
        QuestEndConversation,
        TraderConversation,
    }

    public class Conversations : MonoBehaviour
    {
        [SerializeField] private GameObject converstionUI;
        [SerializeField] private Button conversationAcceptButton;
        [SerializeField] private Button conversationDeclineButton;
        [SerializeField] private Button conversationOkButton;

        [SerializeField] private TextMeshProUGUI conversationAcceptButtonTextField;
        [SerializeField] private TextMeshProUGUI conversationDeclineButtonTextField;
        [SerializeField] private Image customerImage;
        [SerializeField] private TextMeshProUGUI conversationTextField;

        [SerializeField] private Trader trader;
        [SerializeField] private QuestGiver questGiver;

        private void Awake()
        {
            QuestGiver.StartCustomerConversation += StartConversation;
            Trader.StartTraderConversation += StartConversation;
        }

        private void OnDestroy()
        {
            QuestGiver.StartCustomerConversation -= StartConversation;
            Trader.StartTraderConversation -= StartConversation;
        }


        private void StartConversation(Customer customer, ConversationType conversationType)
        {
            CLoseConversationUI();
            Game.Instance.gameTime.PauseGameTime(true);

            switch (customer.customerType)
            {
                case CustomerType.Invalid:
                    Debug.LogError("Cant start Conversation -> Invalid CustomerType");
                    return;

                case CustomerType.Trader:
                    StartTraderConversation(customer, conversationType);

                    break;


                case CustomerType.QuestGiver:
                    StartQuestGiverConversation(customer, conversationType);
                    break;

                default:
                    break;
            }
        }

        private void StartTraderConversation(Customer customer, ConversationType conversationType)
        {
            conversationAcceptButton.gameObject.SetActive(true);
            conversationDeclineButton.gameObject.SetActive(true);
            conversationOkButton.gameObject.SetActive(false);

            conversationAcceptButtonTextField.text = "Trade";
            conversationAcceptButton.onClick.RemoveAllListeners();
            conversationAcceptButton.onClick.AddListener(trader.ShowTraderShop);

            conversationDeclineButtonTextField.text = "No thanks!";
            conversationDeclineButton.onClick.RemoveAllListeners();
            conversationDeclineButton.onClick.AddListener(CLoseConversationUI);

            customerImage.sprite = customer.customerSprite;
            conversationTextField.text = customer.conversationText;
            converstionUI.SetActive(true);
        }

        private void StartQuestGiverConversation(Customer customer, ConversationType conversationType)
        {
            switch (conversationType)
            {
                case ConversationType.Invalid:
                    throw new System.ArgumentException("Invalid ConversationType");


                case ConversationType.QuestStartConversation:
                    conversationAcceptButton.gameObject.SetActive(true);
                    conversationDeclineButton.gameObject.SetActive(true);
                    conversationOkButton.gameObject.SetActive(false);

                    conversationAcceptButtonTextField.text = "Accept";
                    conversationAcceptButton.onClick.RemoveAllListeners();
                    conversationAcceptButton.onClick.AddListener(questGiver.AcceptQuest);

                    conversationDeclineButtonTextField.text = "Decline";
                    conversationDeclineButton.onClick.RemoveAllListeners();
                    conversationDeclineButton.onClick.AddListener(questGiver.DeclineQuest);
                    break;


                case ConversationType.QuestEndConversation:
                    conversationAcceptButton.gameObject.SetActive(false);
                    conversationDeclineButton.gameObject.SetActive(false);
                    conversationOkButton.gameObject.SetActive(true);

                    conversationAcceptButton.onClick.RemoveAllListeners();
                    conversationDeclineButton.onClick.RemoveAllListeners();
                    conversationOkButton.onClick.RemoveAllListeners();
                    conversationOkButton.onClick.AddListener(questGiver.RemoveQuest);
                    break;


                case ConversationType.TraderConversation:
                    return;

                default:
                    return;
            }
            customerImage.sprite = customer.customerSprite;
            conversationTextField.text = customer.conversationText;
            converstionUI.SetActive(true);
            
        }

        //private IEnumerator OnConversationBegin()
        //{
        //    yield return new WaitForSeconds(1f);
        //    conversationAcceptButton.gameObject.SetActive(true);
        //    conversationDeclineButton.gameObject.SetActive(true);
        //}

        public void CLoseConversationUI()
        {
            Game.Instance.gameTime.PauseGameTime(false);
            converstionUI.SetActive(false);
        }
    }
}

