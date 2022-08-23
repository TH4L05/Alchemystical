using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Alchemystical
{
    public class Game : MonoBehaviour
    {
        public static Game Instance;

        public GameData gameData;
        public Inventory inventory;
        public QuestGiver questGiver;
        public Trader trader;
        public TraderShop traderShop;
        public GameTime gameTime;
        public Conversations conversations;
        public InfoUITexT InfoUIText;
        public IngameMenu ingameMenu;
        public Options options;

        private void Awake()
        {
            Instance = this;
            if(options != null) options.Setup();
            Application.targetFrameRate = 60;
            Time.timeScale = 1f;
        }

        private void Update()
        {
            IncreseMoneyDebug();
            OpenPauseMenu();
        }

        private void IncreseMoneyDebug()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                inventory.AddGold(250);
            }
        }

        private void OpenPauseMenu()
        {
            if (IngameMenu.GamePaused) return;
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                ingameMenu.Pause();
            }
        }
    }
}

