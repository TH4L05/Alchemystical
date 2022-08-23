
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using TMPro;
using FMOD.Studio;
using FMODUnity;
//using UnityEngine.Audio;

namespace Alchemystical
{
    public class Options : MonoBehaviour
    {
        #region Serialized Fields

        [Header("Data")]
        [SerializeField] private SettingsData data;
        public InputActionAsset inputAsset;

        [SerializeField] private float masterVolumeMaxValue = 100.0f;
        [SerializeField] private float musicVolumeMaxValue = 100.0f;
        [SerializeField] private float sfxVolumeMaxValue = 100.0f;
        //[SerializeField] private AudioMixer audioMixer;


        [Header("Slider")]
        [SerializeField] private Slider sensitivitySlider;
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private Slider gammaSlider;

        [Header("SliderTexts")]
        [SerializeField] private TextMeshProUGUI sensitivitySliderText;
        [SerializeField] private TextMeshProUGUI masterVolumeSliderText;
        [SerializeField] private TextMeshProUGUI musicVolumeSliderText;
        [SerializeField] private TextMeshProUGUI sfxVolumeSliderText;
        [SerializeField] private TextMeshProUGUI gammaSliderText;

        [Header("Dropdowns")]
        [SerializeField] private TMP_Dropdown resolutionsDropdown;
        [SerializeField] private TMP_Dropdown qualityLevelDropdown;
        //[SerializeField] private TMP_Dropdown fullScreenDropdown;
        //[SerializeField] private TMP_Dropdown vSyncDropdown;

        [Header("OptionTexts")]
        [SerializeField] private TextMeshProUGUI fullScreenOptionText;
        [SerializeField] private TextMeshProUGUI vsyncOptionText;
        [SerializeField] private TextMeshProUGUI resolutionOptionText;
        [SerializeField] private TextMeshProUGUI qualtiyOptionText;

        [Header("Volumes")]
        public VolumeProfile gammaProfile;

        #endregion

        #region Private Fields

        private string[] qualityLevel;
        private float mouseSensitivity = 1f;
        private float masterVolume = 1f;
        private float musicVolume = 1f;
        private float sfxVolume = 1f;
        private int currentResolutionIndex = 0;
        private int currentResolutionWidth = 640;
        private int currentResolutionHeight = 480;
        Resolution[] resolutions;
        private bool fullScreen = true;
        private bool vsync = true;
        private int currentQualityLevel = 0;

        private Bus masterBus;
        private Bus musicBus;
        private Bus sfxBus;

        #endregion

        #region Unity Functions

        private void Awake()
        {
            //Setup();
        }

        #endregion

        #region Setup

        public void Setup()
        {
            resolutions = Screen.resolutions;
            qualityLevel = QualitySettings.names;

            masterBus = RuntimeManager.GetBus("bus:/");
            musicBus = RuntimeManager.GetBus("bus:/Music");
            sfxBus = RuntimeManager.GetBus("bus:/SFX");

            Load();
            SetSFXVolume();
            SetMusicVolume();
            SetMasterVolume();
            SetFullScreen();
            SetVsync();
            //SetMouseSensitivity();
            //SetResolution();
            ResolutionDropdownSetup();
            QualityLevelSetup();
            SetQuality();



            for (int i = 0; i < resolutions.Length; i++)
            {
                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                    data.SetResolutionValues(resolutions[i].width, resolutions[i].height, currentResolutionIndex);
                }
            }
        }

        private void ResolutionDropdownSetup()
        {
            resolutionsDropdown.ClearOptions();

            List<string> resolutionStrings = new List<string>();

            for (int i = 0; i < resolutions.Length; i++)
            {
                var resolutionString = resolutions[i].width + " x " + resolutions[i].height;
                resolutionStrings.Add(resolutionString);
            }

            CheckCurrentResolution();
            resolutionsDropdown.AddOptions(resolutionStrings);
            resolutionsDropdown.value = currentResolutionIndex;
        }

        private void CheckCurrentResolution()
        {
            for (int i = 0; i < resolutions.Length; i++)
            {
                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                    currentResolutionWidth = resolutions[i].width;
                    currentResolutionHeight = resolutions[i].height;
                    data.SetResolutionValues(currentResolutionWidth, currentResolutionHeight, currentResolutionIndex);
                    SetResolution();
                    return;

                }
            }

            if (currentResolutionIndex == 0)
            {
                SetDefaultResolution();
            }

        }

        private void SetDefaultResolution()
        {
            currentResolutionWidth = data.dfaultCurrentResolution_width;
            currentResolutionHeight = data.dfaultCurrentResolution_height;

            //Screen.SetResolution(currentResolutionWidth, currentResolutionHeight, true);
            CheckCurrentResolution();
        }

        private void QualityLevelSetup()
        {
            qualityLevelDropdown.ClearOptions();

            List<string> qualityLevelStrings = new List<string>();

            for (int i = 0; i < qualityLevel.Length; i++)
            {
                var qualityLevelString = qualityLevel[i];
                qualityLevelStrings.Add(qualityLevelString);
            }

            qualityLevelDropdown.AddOptions(qualityLevelStrings);

            currentQualityLevel = data.QualityLevelIndex;
            qualityLevelDropdown.value = currentQualityLevel;

        }

        #endregion

        #region Change Options

        /*public void ChangeMouseSensitivity(float value)
        {
            data.SetSensitivity(value);
            SetMouseSensitivity();
        }*/

        public void ChangeMasterVolume(float volumeValue)
        {
            data.SetMasterVolume(volumeValue);
            SetMasterVolume();
        }

        public void ChangeMusicVolume(float volumeValue)
        {
            data.SetMusicVolume(volumeValue);
            SetMusicVolume();
        }

        public void ChangeSFXVolume(float volumeValue)
        {
            data.SetSFXVolume(volumeValue);
            SetSFXVolume();
        }

        public void ChangeQuality(int indx)
        {
            currentQualityLevel = indx;
            data.SetQualityLevelIndex(currentQualityLevel);
            QualitySettings.SetQualityLevel(currentQualityLevel);
        }

        /*public void ChangeQualityMinus()
        {
            if (currentQualityLevel > 0) currentQualityLevel--;
            data.SetQualityLevelIndex(currentQualityLevel);
            SetQuality();
        }*/

        /*public void ChangeQualityPlus()
        {
            if (currentQualityLevel < qualityLevel.Length - 1) currentQualityLevel++;
            data.SetQualityLevelIndex(currentQualityLevel);
            SetQuality();
        }*/

     
        public void ChangeResolution(int idx)
        {
            currentResolutionIndex = idx;
            currentResolutionWidth = resolutions[idx].width;
            currentResolutionHeight = resolutions[idx].height;
            data.SetResolutionValues(currentResolutionWidth, currentResolutionHeight, currentResolutionIndex);
            SetResolution();
        }

        /*public void ChangeResolutionMinus()
        {
            if (currentResolutionIndex > 0)
            {
                currentResolutionIndex--;
            }
            else
            {
                currentResolutionIndex = resolutions.Length - 1;
            }

            currentResolutionWidth = resolutions[currentResolutionIndex].width;
            currentResolutionHeight = resolutions[currentResolutionIndex].height;
            data.SetResolutionValues(currentResolutionWidth, currentResolutionHeight, currentResolutionIndex);
            SetResolution();
        }*/

        /*public void ChangeResolutionPlus()
        {
            if (currentResolutionIndex < resolutions.Length - 1)
            {
                currentResolutionIndex++;
            }
            else
            {
                currentResolutionIndex = 0;
            }

            currentResolutionWidth = resolutions[currentResolutionIndex].width;
            currentResolutionHeight = resolutions[currentResolutionIndex].height;
            data.SetResolutionValues(currentResolutionWidth, currentResolutionHeight, currentResolutionIndex);
            SetResolution();
        }*/

        public void ChangeFullScreen(bool isFullscreen)
        {
            fullScreen = isFullscreen;
            data.SetFullscreenStatus(fullScreen);

            SetFullScreen();
        }

        public void ChangeVsync(bool isVsync)
        {
            vsync = isVsync;
            data.SetVsyncStatus(vsync);
            SetVsync();
        }

        public void ChangeGamma(float value)
        {
            LiftGammaGain lgg;
            if (gammaProfile.TryGet(out lgg))
            {
                var gvalue = (value / 10);
                var newGamma = new Vector4(1, 1, 1, gvalue);
                lgg.gamma.Override(newGamma);
                //lgg.lift.Override(newGamma);
                //lgg.gain.Override(newGamma);
            }

            //Debug.Log(lgg.gamma + "/" + lgg.lift + "/" + lgg.gain);

            if (gammaSliderText) gammaSliderText.text = value.ToString();
        }

        #endregion

        #region Set Options

        /*public void SetMouseSensitivity()
        {
            var sensitivity = 1f;

            if (data.Sensitivity == 0)
            {
                sensitivity = data.dfaultSensitivity;
            }
            else
            {
                sensitivity = data.Sensitivity;
            }

            if (sensitivitySlider) sensitivitySlider.value = sensitivity;
            if (sensitivitySliderText) sensitivitySliderText.text = sensitivity.ToString("0.0");
        }*/

        public void SetMasterVolume()
        {
            var volume = data.MasterVolume;
            masterVolume = volume;
            if (masterVolumeSlider) masterVolumeSlider.value = volume;
            if (masterVolumeSliderText) masterVolumeSliderText.text = volume.ToString("000");
            masterBus.setVolume(volume / masterVolumeMaxValue);
        }

        public void SetMusicVolume()
        {
            var volume = data.MusicVolume;
            musicVolume = volume;
            if (musicVolumeSlider) musicVolumeSlider.value = volume;
            if (musicVolumeSliderText) musicVolumeSliderText.text = volume.ToString("000");

            musicBus.setVolume(volume / musicVolumeMaxValue);
            //volume -= 80;
            //audioMixer.SetFloat("MusicVolume", volume);
        }

        public void SetSFXVolume()
        {
            var volume = data.SfxVolume;
            sfxVolume = volume;
            if (sfxVolumeSlider) sfxVolumeSlider.value = volume;
            if (sfxVolumeSliderText) sfxVolumeSliderText.text = volume.ToString("000");
            sfxBus.setVolume(volume / sfxVolumeMaxValue);
            //volume -= 80;
            //audioMixer.SetFloat("SFXVolume", volume);
        }

        public void SetFullScreen()
        {
            fullScreen = data.FullScreen;
            Screen.fullScreen = fullScreen;

            if (fullScreen)
            {
                fullScreenOptionText.text = "ON";
                Screen.fullScreen = true;
            }
            else
            {
                fullScreenOptionText.text = "OFF";
                Screen.fullScreen = false;
            }
        }

        public void SetVsync()
        {
            vsync = data.VSync;
            if (vsync)
            {
                vsyncOptionText.text = "ON";
                QualitySettings.vSyncCount = 1;
            }
            else
            {
                vsyncOptionText.text = "OFF";
                QualitySettings.vSyncCount = 0;
            }
        }

        public void SetResolution()
        {
            currentResolutionIndex = data.CurrentResolutionIndex;
            currentResolutionWidth = data.CurrentResolution_width;
            currentResolutionHeight = data.CurrentResolution_height;
            resolutionsDropdown.value = currentResolutionIndex;

            if(resolutionOptionText != null) resolutionOptionText.text = currentResolutionWidth + " x " + currentResolutionHeight + " , " + resolutions[currentResolutionIndex].refreshRate;
            Screen.SetResolution(currentResolutionWidth, currentResolutionHeight, fullScreen);
        }

        public void SetQuality()
        {
            currentQualityLevel = data.QualityLevelIndex;

            QualitySettings.SetQualityLevel(currentQualityLevel);
            if(qualtiyOptionText != null) qualtiyOptionText.text = qualityLevel[currentQualityLevel];
        }

        #endregion


        public void Save()
        {
            //string binds = inputAsset.SaveBindingOverridesAsJson();
            //data.SetBindings(binds);
            data.SaveValues();
        }

        public void Load()
        {
            data.LoadValues();
            /*if (data.InputActionRebindings != string.Empty)
            {
                inputAsset.LoadBindingOverridesFromJson(data.InputActionRebindings);
            }*/

        }

        public void LoadDefaults()
        {
            data.SetDefaultValues();
            SetSFXVolume();
            SetMusicVolume();
            SetMasterVolume();
            SetFullScreen();
            SetVsync();
            //SetMouseSensitivity();
            SetResolution();
            SetQuality();
            //inputAsset.RemoveAllBindingOverrides();
            //Serialization.DeleteFile("rebindings.data");
        }
    }
}


