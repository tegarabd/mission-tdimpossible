using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsMenuPanel;

    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private TMP_Dropdown resolutionsDropdown;

    private List<string> qualities;
    private List<string> resolutions;

    private bool fullscreen;
    private int resolutionIndex;
    private int qualityIndex;

    private void Start()
    {
        qualities = new List<string>();
        qualities.AddRange(QualitySettings.names);
        qualityIndex = QualitySettings.GetQualityLevel();
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(qualities);
        qualityDropdown.value = qualityIndex;
        qualityDropdown.RefreshShownValue();

        resolutions = new List<string>();
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            resolutions.Add(Screen.resolutions[i].width + " x " + Screen.resolutions[i].height);
            if (Screen.resolutions[i].Equals(Screen.currentResolution))
            {
                resolutionIndex = i;
            }
        }
        resolutionsDropdown.ClearOptions();
        resolutionsDropdown.AddOptions(resolutions);
        resolutionsDropdown.value = resolutionIndex;
        resolutionsDropdown.RefreshShownValue();
    }
    public void GotoMainMenu()
    {
        settingsMenuPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void SetQuality(int qualityIdx)
    {
        qualityIndex = qualityIdx;
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetResolution(int resolutionIdx)
    {
        resolutionIndex = resolutionIdx;
        Screen.SetResolution(Screen.resolutions[resolutionIndex].width, Screen.resolutions[resolutionIndex].height, fullscreen);
    }

    public void SetFullScreen(bool isFullscreen)
    {
        fullscreen = isFullscreen;
        Screen.fullScreen = fullscreen;
    }
}
