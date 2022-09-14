using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Alchemystical
{
    public class Game : MonoBehaviour
    {
        public static Game Instance;

        public GameData gameData;
        public Inventory inventory;
        public GameTime gameTime;
        public IngameMenu ingameMenu;
        public Options options;
        public GameInput input;
        public bool isMenu;

        private void Awake()
        {
            Instance = this;
            if(options != null) options.Setup();
            Application.targetFrameRate = 60;
            Time.timeScale = 1f;
        }

        private void Start()
        {
            GameInput.ToggleMenu += TogglePauseMenu;
        }

        private void OnDestroy()
        {
            GameInput.ToggleMenu -= TogglePauseMenu;
        }

        /*private void Update()
        {
            //if(isMenu) return;
            //IncreseMoney();
            
        }*/

        private void IncreaseMoneyTEST()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                inventory.AddGold(250);
            }
        }

        private void TogglePauseMenu()
        {
            if (IngameMenu.GamePaused)
            {
                ingameMenu.Resume();
            }
            else
            {
                ingameMenu.Pause();
            }         
        }
    }
}

