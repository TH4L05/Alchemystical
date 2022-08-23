using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Alchemystical
{
    public class GameTime : MonoBehaviour
    {
        public static Action TraderAppeared;
        public static Action CustomerAppeared;

        #region Fields

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI dayTextField;
        [SerializeField] private TextMeshProUGUI weekTextField;
        [SerializeField] private TextMeshProUGUI timeTextField;
        [SerializeField] private int traderDay = 2;
        [SerializeField] private bool randomTraderApearanceTime = false;
        [SerializeField] private float traderApearanceTime = 10f;
        [SerializeField] private float traderApearanceTimeOffset = 2.0f;
        [SerializeField] private float customerApearanceTimeOffset = 0.65f;
        [SerializeField] private Slider clock;

        [Header("---")]
        [SerializeField] private float timeSpeedMultiplierDefault = 1f;
        [SerializeField] private string[] names = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        [SerializeField] private float dayTimeStart = 6f;
        [SerializeField] private float dayTimeEnd = 18f;
        [SerializeField] private CustomerInfoUI customerInfoUI;

        private float internalMultiplier = 60f;
        private float daySeconds;
        private float currentSecond;
        private float currentTimeSpeedMultiplier;
        private int currentHour;
        private int currentMinute;
        private int dayIndex = 1;
        private int weekIndex = 1;
        private float customerAppearanceTime;
        private bool pauseTime;

        private bool customerCanAppear;
        private bool traderCanAppear;
        private bool pauseAppearance;

        public string NameOfDay => names[dayIndex - 1];

        #endregion

        private void Awake()
        {
            traderApearanceTime *= 3600;
            clock.minValue = dayTimeStart * 3600;
            clock.maxValue = dayTimeEnd * 3600;
            currentTimeSpeedMultiplier = timeSpeedMultiplierDefault;
        }

        private void Start()
        {
            StartDay();
        }

        private void Update()
        {
            if (pauseTime) return;
            UpdateDayTime();
            CheckCustomerAppearance(daySeconds);
            CheckTraderAppearance(daySeconds);
        }

        private void UpdateDayTime()
        {
            daySeconds += Time.deltaTime * internalMultiplier * currentTimeSpeedMultiplier;
            currentSecond += Time.deltaTime * internalMultiplier * currentTimeSpeedMultiplier;
            clock.value = daySeconds;

            if (currentSecond >= 60)
            {
                currentSecond = 0f;
                currentMinute++;
            }

            if (currentMinute >= 60)
            {
                currentMinute = 0;
                currentHour++;
            }

            if (daySeconds >= dayTimeEnd * 3600) EndDay();
            timeTextField.text = currentHour.ToString("00") + ":" + currentMinute.ToString("00");
        }

        private void CheckCustomerAppearance(float dayseconds)
        {
            if(!customerCanAppear) return;
            if (dayseconds >= customerAppearanceTime)
            {
                customerCanAppear = false;
                Debug.Log("JeffAppearenceTime");
                CustomerAppeared?.Invoke();
            }
        }

        private void CheckTraderAppearance(float dayseconds)
        {
            if (dayIndex != traderDay) return;
            if (!traderCanAppear) return;
            if (dayseconds >= traderApearanceTime)
            {
                traderCanAppear = false;
                Debug.Log("TraderAppearenceTime");
                TraderAppeared?.Invoke();
            }
        }

        private void StartDay()
        {
            customerInfoUI.ResetCounter();
            Game.Instance.questGiver.ClearWaitingList();
            customerInfoUI.ChangeMerchantStatus(false);

            daySeconds = dayTimeStart * 3600;
            currentSecond = daySeconds;
            currentHour = (int)dayTimeStart;
            RandomCustomerAppearanceTime();
            UpdateUI();
            customerCanAppear = true;
            traderCanAppear = true;
        }

        private void EndDay()
        {
            dayIndex++;

            if (dayIndex > 7)
            {
                EndWeek();
            }
            StartDay();
        }

        private void RandomCustomerAppearanceTime()
        {
            float time = UnityEngine.Random.Range(dayTimeStart + customerApearanceTimeOffset, dayTimeEnd - customerApearanceTimeOffset) * 3600;
            customerAppearanceTime = time;
        }

        private void RandomTraderAppearanceTime()
        {
            float time = UnityEngine.Random.Range(dayTimeStart + traderApearanceTimeOffset, dayTimeEnd - traderApearanceTimeOffset) * 3600;
            traderApearanceTime = time;
        }

        internal void UpdateUI()
        {
            dayTextField.text = NameOfDay;
            weekTextField.text = "Week: " + weekIndex.ToString();
        }

        private void EndWeek()
        {
            dayIndex = 1;
            weekIndex++;
            StartDay();
            if (!randomTraderApearanceTime)
            RandomTraderAppearanceTime();         
        }

        public void PauseGameTime(bool pause)
        {
            pauseTime = pause;
        }

        public void SetCurrentTimeSpeedMultiplier(float multiplier)
        {
            currentTimeSpeedMultiplier = multiplier;
        }

        public void SetHalfCurrentTimeSpeedMultiplier()
        {
            currentTimeSpeedMultiplier /= 2;
        }

        public void ResetTimeSpeedMultiplier()
        {
            currentTimeSpeedMultiplier = timeSpeedMultiplierDefault;
        }
    }
}

