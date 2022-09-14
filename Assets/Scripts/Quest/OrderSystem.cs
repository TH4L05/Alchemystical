using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;
using TK.Audio;

namespace Alchemystical
{
    public class OrderSystem : MonoBehaviour
    {
        #region Events

        public static Action<Customer,ConversationType> StartCustomerConversation;
        public static Action IncraseCustomerUICounter;
        public static Action DecraseCustomerUICounter;
        public static Action ExitConversation;

        #endregion

        #region SerializedFields

        [Header("Info")]
        [SerializeField] private List<Customer> waitingCustomers = new List<Customer>();
        [SerializeField] private List<QuestUISlot> activeOrders = new List<QuestUISlot>();
        [SerializeField] private List<QuestUISlot> completedOrders = new List<QuestUISlot>();
        [SerializeField] private List<Potion> brewedPotions = new List<Potion>();

        [Header("ConversationSettings")]
        [SerializeField] private string[] conversationTexts;
        [SerializeField] private string[] endConversationTexts;
        [SerializeField] private Sprite[] customerSprites;

        [Header("ComponentReferences")]
        [SerializeField] private GameObject orderUISlotPrefab;
        [SerializeField] private GameObject orderUIListParent;
        [SerializeField] private GameObject orderFullUI;
        [SerializeField] private IngameMenuPanel ingameMenuPanelReference;

        [Header("Settings")]
        [SerializeField] private Color questDoneColor = Color.green;
        [SerializeField] private int activeCustomerLimit = 3;
        [SerializeField] private int goldMin = 20;
        [SerializeField] private int goldMax = 80;        
        [SerializeField] private AudioEventList audioEventList;

        #endregion

        #region PrivateFields

        private Potion[] potions;
        private Customer currentCustomer;
        private Customer currentEndCustomer;
        private string[] test;
        private int activeQuestCount;
        private bool uiActive;

        #endregion

        #region UnityFunctions

        private void Start()
        {
            potions = Game.Instance.gameData.potions;
            GameTime.CustomerAppeared += CreateCustomer;
            GameTime.NewDayStarted += ClearWaitingList;
            GameInput.ToggleQuests += ToggleQuestUI;
            Brew.PotionIsBrewed += PotionBrewed;
            QuestUISlot.QuestbuttonClicked += EndQuest;
        }

        public void OnDestroy()
        {
            GameTime.CustomerAppeared -= CreateCustomer;
            GameInput.ToggleQuests -= ToggleQuestUI;
            Brew.PotionIsBrewed -= PotionBrewed;
            QuestUISlot.QuestbuttonClicked -= EndQuest;
            GameTime.NewDayStarted -= ClearWaitingList;
        }

        #endregion

        #region Quest

        private void ToggleQuestUI()
        {
            uiActive = !uiActive;
            orderFullUI.SetActive(uiActive);
            audioEventList.PlayAudioEventOneShot("ButtonClick");
        }

        public void AcceptQuest()
        {
            AddQuest(currentCustomer);
            waitingCustomers.Remove(currentCustomer);
            currentCustomer = null;
            ExitConversation?.Invoke();
            CheckBrewedPotions();
        }

        public void DeclineQuest()
        {
            currentCustomer = null;
            ExitConversation?.Invoke();
        }

        public void EndQuest(Customer customer)
        {
            Debug.Log("= EndQuest =");
            currentEndCustomer = customer;
            customer.conversationText = EndConversationText(customer.gold);
            StartCustomerConversation?.Invoke(customer, ConversationType.QuestEndConversation);

            for (int i = 0; i < brewedPotions.Count; i++)
            {
                if (currentEndCustomer.potion.potionName == brewedPotions[i].potionName)
                {
                    brewedPotions.Remove(brewedPotions[i]);
                    return;
                }
            }

            brewedPotions.Remove(currentEndCustomer.potion);
        }

        public void AddQuest(Customer customer)
        {
            var questUISlotObject = Instantiate(orderUISlotPrefab);
            questUISlotObject.name = "Quest_" + (activeOrders.Count + 1) + "_" + customer.potion.potionName;
            questUISlotObject.transform.parent = orderUIListParent.transform;

            var slot = questUISlotObject.GetComponent<QuestUISlot>();
            slot.UpdateSlot(customer);
            activeOrders.Add(slot);

            var button = slot.GetComponent<ButtonExtra>();
            ingameMenuPanelReference.AddButton(button);
            if (activeOrders.Count > 0) ingameMenuPanelReference.SetFirstSelectedButtonByIndex(1);

        }

        public void RemoveQuest()
        {
            Debug.Log("= RemoveQuest =");
            Game.Instance.inventory.AddGold(currentEndCustomer.gold);
            ExitConversation?.Invoke();

            QuestUISlot slot = null;


            foreach (var quest in completedOrders)
            {
                var questCustomer = quest.GetCustomer();

                if (questCustomer.questDone && 
                    questCustomer.potion == currentEndCustomer.potion  && 
                    questCustomer.customerSprite == currentEndCustomer.customerSprite)
                {
                    completedOrders.Remove(quest);                 
                    currentEndCustomer = null;
                    slot = quest;
                    break;
                }
            }

            var button = slot.GetComponent<ButtonExtra>();
            ingameMenuPanelReference.RemoveButton(button);
            if (activeOrders.Count > 0) ingameMenuPanelReference.SetFirstSelectedButtonByIndex(1);
            slot.Destroy();

        }

        #endregion

        #region Customer

        public void CreateCustomer()
        {
            int index = 0;

            if (activeOrders.Count >= activeCustomerLimit)
            {
                Debug.Log("Can't create new customer -> active customer limit is reached");
                return;
            }

            int gold = GetRandomGoldValue();

            index = GetRandomIndex();
            Potion potion = potions[index];

            string conversationText = SetConversationtext(gold, potion);

            index = GetRandomIndex();
            Sprite sprite = customerSprites[index];

            Customer newCustomer = new Customer(CustomerType.QuestGiver, sprite, conversationText);
            newCustomer.potion = potion;
            newCustomer.gold = gold;
            newCustomer.conversationText = conversationText;
            IncraseCustomerUICounter?.Invoke();          
            waitingCustomers.Add(newCustomer);
        }

        private int GetRandomGoldValue()
        {
            return UnityEngine.Random.Range(goldMin, goldMax);
        }

        private int GetRandomIndex()
        {
            return UnityEngine.Random.Range(0, potions.Length - 1);
        }

        #endregion

        #region Conversation

        private string SetConversationtext(int gold, Potion potion)
        {
            string conversationText = string.Empty;

            int index = UnityEngine.Random.Range(0, conversationTexts.Length - 1);
            string tempText = conversationTexts[index];

            test = tempText.Split("%p");

            conversationText += test[0];
            conversationText += potion.potionName;
            conversationText += test[1];

            tempText = conversationText;
            test = tempText.Split("%g");

            conversationText = string.Empty;
            conversationText += test[0];
            conversationText += gold.ToString();
            conversationText += test[1];

            if (string.IsNullOrEmpty(conversationText))
            {
                conversationText = "NO TEXT AVIALABLE";
            }

            return conversationText;
        }

        private string EndConversationText(int gold)
        {
            string endConversationText = string.Empty;

            int index = UnityEngine.Random.Range(0, endConversationTexts.Length - 1);
            string tempText = endConversationTexts[index];

            test = tempText.Split("%g");

            endConversationText = string.Empty;
            endConversationText += test[0];
            endConversationText += gold.ToString();
            endConversationText += test[1];

            if (string.IsNullOrEmpty(endConversationText))
            {
                endConversationText = "NO TEXT AVIALABLE";
            }

            return endConversationText;

        }

        public void NewCustomerConversation()
        {
            if (waitingCustomers.Count < 1) return;
            currentCustomer = waitingCustomers[0];
            StartConversation(currentCustomer);
            DecraseCustomerUICounter?.Invoke();
        }

        public void StartConversation(Customer customer)
        {
            StartCustomerConversation?.Invoke(customer, ConversationType.QuestStartConversation);
        }

        #endregion

        #region Potion

        public void PotionBrewed(Potion potion)
        {
            brewedPotions.Add(potion);
            CheckBrewedPotions();
        }

        public void CheckBrewedPotions()
        {
            Debug.Log("= CheckBrewedPotion =");
            

            if (activeOrders.Count < 1) return;
            if (brewedPotions.Count < 1) return;

            foreach (var quest in activeOrders)
            {

                for (int i = 0; i < brewedPotions.Count; i++)
                {
                    if (quest.GetPotion().potionName == brewedPotions[i].potionName)
                    {
                        Debug.Log("= QuestSuccess =");
                        Customer customer = quest.GetCustomer();
                        quest.SetQuestDone(questDoneColor);
                        completedOrders.Add(quest);
                        activeOrders.Remove(quest);
                        return;
                    }
                }              
            }          
        }

        #endregion

        public void ClearWaitingList()
        {
            waitingCustomers.Clear();
        }
    }
}


        