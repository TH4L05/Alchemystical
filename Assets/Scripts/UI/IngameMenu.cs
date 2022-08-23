

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class IngameMenu : MonoBehaviour
{
    #region Events

    public UnityEvent TriggerEventOnMenuOpen;
    public UnityEvent TriggerEventOnMenuClose;

    #endregion

    #region Fields

    public static bool GamePaused;

    //[SerializeField] private GameObject uiObject;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionMenu;
    [SerializeField] private bool setTimeScaleZero;

    [Header("Playables")]
    [SerializeField] private PlayableDirector showPauseMenu;
    [SerializeField] private PlayableDirector hidePauseMenu;

    //[Header("Audio")]
    #endregion

    #region UnityFunctions

    private void Awake()
    {
        GamePaused = false;
    }

    #endregion

    public void ToggleMenu()
    {
        if (GamePaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        GamePaused = true;
        if (showPauseMenu == null)
        {
            if (pauseMenu) pauseMenu.SetActive(true);
            if (optionMenu) optionMenu.SetActive(false);
            if (setTimeScaleZero)
            {
                SetTimeScale(0f);
            }
        }
        else
        {
            showPauseMenu.Play();
        }
        TriggerEventOnMenuOpen?.Invoke();
        

        //Debug.Log(GamePaused);
    }

    public void Resume()
    {
        
        GamePaused = false;

        if (hidePauseMenu == null)
        {
            if (pauseMenu) pauseMenu.SetActive(false);
            if (optionMenu) optionMenu.SetActive(false);
            SetTimeScale(1f);
        }
        else
        {
            hidePauseMenu.Play();
        }
        TriggerEventOnMenuClose?.Invoke();
        //Debug.Log(GamePaused);
    }

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
        //Debug.Log(Time.timeScale);
    }

    public void ToggleOptionMenu(bool active)
    {      
        optionMenu.SetActive(active);
        pauseMenu.SetActive(!active);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#else
        Application.Quit();
    
#endif
    }
}


