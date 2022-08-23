using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;

namespace Alchemystical
{
    public class QuestGiver : MonoBehaviour
    {
        public static Action<Customer,ConversationType> StartCustomerConversation;

        [Header("- - -")]
        [SerializeField] private List<Customer> waitingCustomers = new List<Customer>();
        [SerializeField] private List<QuestUISlot> activeQuests = new List<QuestUISlot>();
        [SerializeField] private List<QuestUISlot> doneQuests = new List<QuestUISlot>();
        [SerializeField] private Color questDoneColor = Color.green;
        [SerializeField] private List<Potion> brewedPotions = new List<Potion>();

        [Header("- - -")]
        [SerializeField] private string[] conversationTexts;
        [SerializeField] private string[] endConversationTexts;
        [SerializeField] private Sprite[] customerSprites;
        [SerializeField] private GameObject questUISlotPrefab;
        [SerializeField] private GameObject questUIListParent;
        [SerializeField] private CustomerInfoUI customerInfoUI;

        [Header("Settings")]
        [SerializeField] private int activeCustomerLimit = 3;
        [SerializeField] private int goldMin = 20;
        [SerializeField] private int goldMax = 80;

        private Potion[] potions;
        private Customer currentCustomer;
        private Customer currentEndCustomer;
        public bool passportControl;
        private string[] test;
        private int activeQuestCount;

        public void Awake()
        {            
            GameTime.CustomerAppeared += CreateCustomer;
            Brew.APotionIsFinished += PotionBrewed;
            QuestUISlot.QuestbuttonClicked += EndQuest;
        }

        private void Start()
        {
            potions = Game.Instance.gameData.potions;
        }

        public void OnDestroy()
        {
            GameTime.CustomerAppeared -= CreateCustomer;
            Brew.APotionIsFinished -= PotionBrewed;
            QuestUISlot.QuestbuttonClicked -= EndQuest;
        }

        public void CreateCustomer()
        {
            int index = 0;
            if (passportControl) return;
            if (activeQuests.Count >= activeCustomerLimit)
            {
                Debug.Log("Cant Create new customer -> active customer limit reached");
                return;
            }

            int gold = UnityEngine.Random.Range(goldMin, goldMax);

            index = UnityEngine.Random.Range(0, potions.Length - 1);
            Potion potion = potions[index];

            string conversationText = SetConversationtext(gold, potion);

            index = UnityEngine.Random.Range(0, customerSprites.Length - 1);
            Sprite sprite = customerSprites[index];

            Customer jeff = new Customer(CustomerType.QuestGiver, sprite, conversationText);
            jeff.potion = potion;
            jeff.gold = gold;
            jeff.conversationText = conversationText;
            //currentCustomer = jeff;
            //customerInfoUI.IncreaseCounter();
            customerInfoUI.UpdateInfoIncrease();
          
            waitingCustomers.Add(jeff);
            //StartCustomerConversation?.Invoke(jeff);
        }

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
            //customerInfoUI.DecreaseCounter();
            customerInfoUI.UpdateInfoDecrease();
        }

        public void StartConversation(Customer customer)
        {
            StartCustomerConversation?.Invoke(customer, ConversationType.QuestStartConversation);
        }

        public void SetPassportControlStatus(bool active)
        {
            passportControl = active;
        }

        public void AcceptQuest()
        {
            SetPassportControlStatus(false);
            AddQuest(currentCustomer);
            waitingCustomers.Remove(currentCustomer);
            currentCustomer = null;
            Game.Instance.conversations.CLoseConversationUI();
            CheckBrewedPotions();
        }

        public void DeclineQuest()
        {
            SetPassportControlStatus(false);
            currentCustomer = null;
            Game.Instance.conversations.CLoseConversationUI();
        }

        public void AddQuest(Customer customer)
        {
            var questUISlotObject = Instantiate(questUISlotPrefab);
            questUISlotObject.transform.parent = questUIListParent.transform;

            var slot = questUISlotObject.GetComponent<QuestUISlot>();
            slot.UpdateSlot(customer);
            activeQuests.Add(slot);
        }

        public void PotionBrewed(Potion potion)
        {
            brewedPotions.Add(potion);
            CheckBrewedPotions();
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

        public void RemoveQuest()
        {
            Debug.Log("= RemoveQuest =");
            Game.Instance.inventory.AddGold(currentEndCustomer.gold);           
            Game.Instance.conversations.CLoseConversationUI();

            foreach (var quest in doneQuests)
            {
                var questCustomer = quest.GetCustomer();

                if (questCustomer.questDone && 
                    questCustomer.potion == currentEndCustomer.potion  && 
                    questCustomer.customerSprite == currentEndCustomer.customerSprite)
                {
                    doneQuests.Remove(quest);                 
                    currentEndCustomer = null;
                    return;
                }
            }
        }

        public void CheckBrewedPotions()
        {
            Debug.Log("= CheckBrewedPotion =");
            

            if (activeQuests.Count < 1) return;
            if (brewedPotions.Count < 1) return;

            foreach (var quest in activeQuests)
            {

                for (int i = 0; i < brewedPotions.Count; i++)
                {
                    if (quest.GetPotion().potionName == brewedPotions[i].potionName)
                    {
                        Debug.Log("= QuestSuccess =");
                        Customer customer = quest.GetCustomer();
                        quest.SetQuestDone(questDoneColor);
                        doneQuests.Add(quest);
                        //EndQuest(customer);
                        activeQuests.Remove(quest);
                        //quest.Destroy();
                        return;
                    }
                }              
            }          
        }

        public void ClearWaitingList()
        {
            waitingCustomers.Clear();
        }
    }
}


        