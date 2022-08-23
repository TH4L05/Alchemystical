using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alchemystical
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private Options options;


        private void Awake()
        {
            if (options != null) options.Setup();
            Application.targetFrameRate = 60;
            Time.timeScale = 1f;
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;

#else
        Application.Quit();
    
#endif
        }

    }
}

