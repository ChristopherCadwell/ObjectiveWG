using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEditor;

public enum SceneIndexes
{
    Main_Menu = 0,
    Prologue = 1,
}
public enum ActiveMenu
{
    Game,
    Options,
    Pause,
    GameOver,
    Main
}

public class GameSettings : MonoBehaviour
{
    #region Variables
    [Header("Menus")]
    public List<GameObject> menuNames = new List<GameObject>();
    public static GameSettings Instance { get; private set; }
    #region Audio
    [Header("Audio"), Tooltip("Main audio control for everything")]
    public AudioMixer mixer;
    public AudioSource musicSource,
        fxSource;
    public AudioClip buttonClick,
        buttonHover,
        menu,
        scene1,
        back,
        quitGame,
        load,
        newGame;
    #endregion
    #region HUD
    public Slider healthBarSlider;
    #endregion
    public bool isPaused;
    public ActiveMenu activeMenu;
    #endregion
    #region Canvas Components
    [Header("Elements")]
    public Slider masterVolumeSlider,
        musicVolumeSlider,
        effectsVolumeSlider;
    public Toggle fullScreenToggle;
    public TMPro.TMP_Dropdown resolutionDropDown,
    qualityDropDown;
    public Resolution[] resolutions;
    public List<GameObject> background = new List<GameObject>();
    #endregion
    #region Quality
    private void Awake()
    {
        //make sure there is always only 1 instance
        if (GameSettings.Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion
    private void Start()
    {
        SelectMenu("MainMenuCanvas");
        activeMenu = ActiveMenu.Main;
        SetUpOptions();
    }
    #region Setup
    protected void SetUpOptions()
    {
        DataManager.Instance.LoadSettings();//load values


        resolutionDropDown.ClearOptions();//clear any res options

        List<string> options = GetResolutions();//generate the list of resolutions
        resolutionDropDown.AddOptions(options);//add list to the dropdown

        // Build quality levels
        qualityDropDown.ClearOptions();//clear anything that might already exist
        qualityDropDown.AddOptions(QualitySettings.names.ToList());//add the quality levels to the dropdown


        //get values from settings data
        masterVolumeSlider.value = DataManager.Instance.settingsData.masterVolume;
        musicVolumeSlider.value = DataManager.Instance.settingsData.musicVolume;
        effectsVolumeSlider.value = DataManager.Instance.settingsData.effectsVolume;
        resolutionDropDown.value = DataManager.Instance.settingsData.resolution;
        fullScreenToggle.isOn = DataManager.Instance.settingsData.fullScreen;
        qualityDropDown.value = DataManager.Instance.settingsData.quality;

        //match values
        mixer.SetFloat("masterVolume", Mathf.Log10(masterVolumeSlider.value) * 20);
        mixer.SetFloat("musicVolume", Mathf.Log10(musicVolumeSlider.value) * 20);
        mixer.SetFloat("effectsVolume", Mathf.Log10(effectsVolumeSlider.value) * 20);
        SetResolution(resolutionDropDown.value);
        ScreenToggle(fullScreenToggle.isOn);
        SetQuality(qualityDropDown.value);
    }
    #endregion

    #region Button Presses
    //exit game
    public void QuitGame()
    {
        Application.Quit();//quit game

        //#if is preprossor code, if unity editor is running, this statement is true
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//stop editor
#endif
    }
    //start the game
    public void NewGame()
    {
        //load whatever scene our zone1 is
        SceneManager.LoadSceneAsync((int)SceneIndexes.Prologue);
        //display HUD canvas
        SelectMenu("HUD Canvas");
        //Set enum to Game (Used to tell what screen is active)
        activeMenu = ActiveMenu.Game;
        Camera.main.GetComponent<AudioSource>().clip = scene1;
        Time.timeScale = 1;
    }
    public void ContinueGame()
    {

        //load whatever scene our zone1 is
        SceneManager.LoadSceneAsync((int)SceneIndexes.Prologue);
        //display HUD canvas
        SelectMenu("HUD Canvas");
        //Set enum to Game (Used to tell what screen is active)
        activeMenu = ActiveMenu.Game;
        Camera.main.GetComponent<AudioSource>().clip = scene1;
        Time.timeScale = 1;
    }
    #endregion
    #region Misc Functions
    public void ButtonHighlight()
    {
        fxSource.PlayOneShot(buttonHover);
    }
    public void ButtonClick()
    {
        fxSource.PlayOneShot(buttonClick);
    }
    public int BoolToInt(bool value)
    {
        if (value == true)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    public bool IntToBool(int value)
    {
        if (value == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion
    #region Menu Navigation
    public void SetMain()
    {
        activeMenu = ActiveMenu.Main;
        SelectMenu("MainMenuCanvas");
    }
    public void OptionsBack()
    {
        if (activeMenu == ActiveMenu.Main)
            MainBack();
        else
            GameBack();
    }
    public void MainBack()
    {
        SelectMenu("MainMenuCanvas");
    }
    public void GameBack()
    {
        SelectMenu("PauseCanvas");
    }
    public void BKSelect(string name)
    {
        for (int i = 0; i < background.Count; i++)
        {
            background[i].SetActive(background[i].name.Equals(name));
        }
    }
    public void SelectMenu(string name)
    {
        for (int i = 0; i < menuNames.Count; i++)
        {
            menuNames[i].SetActive(menuNames[i].name.Equals(name));
        }
    }
    public void PauseUnpause()
    {
        if (isPaused)
        {
            SelectMenu("HUD Canvas");
            activeMenu = ActiveMenu.Game;
            Time.timeScale = 1.0f;
            isPaused = false;
        }
        else
        {
            SelectMenu("PauseCanvas");
            activeMenu = ActiveMenu.Pause;
            Time.timeScale = 0.0f;//stop time
            isPaused = true;
        }
    }
    public void ResumeGame()
    {
        GameManager.Instance.ResetSpawn();
        SelectMenu("HUD Canvas");
        Time.timeScale = 1.0f;
        activeMenu = ActiveMenu.Game;
    }
    #endregion
    #region Volume Bars
    public void UpdateMaster()
    {
        //this works better than using an animation curve, min value must be above 0
        mixer.SetFloat("masterVolume", Mathf.Log10(masterVolumeSlider.value) * 20);

        //save data
        DataManager.Instance.settingsData.masterVolume = masterVolumeSlider.value;//update data
        DataManager.Instance.SaveSettings();//save value
    }
    public void UpdateMusic()
    {
        //this works better than using an animation curve, min value must be above 0       
        mixer.SetFloat("musicVolume", Mathf.Log10(musicVolumeSlider.value) * 20);

        //save data
        DataManager.Instance.settingsData.musicVolume = musicVolumeSlider.value;//update data
        DataManager.Instance.SaveSettings();//save value
    }
    public void UpdateEffects()
    {
        //this works better than using an animation curve, min value must be above 0
        mixer.SetFloat("effectsVolume", Mathf.Log10(effectsVolumeSlider.value) * 20);

        //save data
        DataManager.Instance.settingsData.effectsVolume = effectsVolumeSlider.value;//update data
        DataManager.Instance.SaveSettings();//save value
    }
    #endregion

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);//pass the dropdown selection to qualitylevel

        DataManager.Instance.settingsData.quality = index;//update data
        DataManager.Instance.SaveSettings();//save
    }
    #region Screen
    public List<string> GetResolutions()
    {
        //This should stop duplicate resolutions from showing up, and leave us with only max refresh rates

        resolutions = Screen.resolutions;//get resolution array
        //build dropdown for screen resolutions and quality levels
        List<Resolution> allRes = new List<Resolution>();//create list to hold resolutions
        List<string> options = new List<string>();//and a list for unique reses

        resolutionDropDown.ClearOptions();//clear anything that might be there

        for (int index = 0; index < resolutions.Length; index++)//loop through all possible resolutions system can use
        {
            allRes.Add(resolutions[index]);//add to list    
        }
        //Create a list of resolutions
        //only use ones that have a refresh rate of at least 60
        //sort by refresh rate then resolution
        //highest width on top
        allRes = (from Resolution in allRes
                  where Resolution.refreshRate >= 60.0f
                  orderby Resolution.refreshRate, Resolution.width
                  descending
                  select Resolution).ToList();

        allRes = allRes.Distinct().ToList();//remove duplicate resolutions --This should leave us with only the max refresh rate per resolution


        //convert to string list
        for (int i = 0; i < allRes.Count; i++)
        {
            options.Add(string.Format("{0} x {1}", allRes[i].width, allRes[i].height));//add each to the list
        }

        return options;
    }
    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];//pass index to resolution array
        Screen.SetResolution(resolution.width, resolution.height, fullScreenToggle);//set resolution and fullscreen

        DataManager.Instance.settingsData.resolution = index;//update data
        DataManager.Instance.SaveSettings();//save
    }
    public void ScreenToggle(bool toggle)
    {
        Screen.fullScreen = toggle;

        DataManager.Instance.settingsData.fullScreen = toggle;//update data
        DataManager.Instance.SaveSettings();//save
    }
    #endregion
    #region Clear Player Prefs
    [MenuItem("Settings/Clear Player Prefs")]
    static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
    #endregion

    private void OnDestroy()
    {
        Destroy(Instance);
        Instance = null;
    }

}
