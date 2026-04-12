using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionSettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private Resolution[] availableResolutions;
    private List<Resolution> filteredResolutions = new List<Resolution>();

    private const string ResolutionIndexKey = "ResolutionIndex";

    private void Start()
    {
        availableResolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < availableResolutions.Length; i++)
        {
            Resolution res = availableResolutions[i];

            bool alreadyAdded = false;
            for (int j = 0; j < filteredResolutions.Count; j++)
            {
                if (filteredResolutions[j].width == res.width &&
                    filteredResolutions[j].height == res.height)
                {
                    alreadyAdded = true;
                    break;
                }
            }

            if (alreadyAdded)
                continue;

            filteredResolutions.Add(res);
            options.Add(res.width + " x " + res.height);

            if (res.width == Screen.currentResolution.width &&
                res.height == Screen.currentResolution.height)
            {
                currentResolutionIndex = filteredResolutions.Count - 1;
            }
        }

        resolutionDropdown.AddOptions(options);

        int savedIndex = PlayerPrefs.GetInt(ResolutionIndexKey, currentResolutionIndex);
        savedIndex = Mathf.Clamp(savedIndex, 0, filteredResolutions.Count - 1);

        resolutionDropdown.value = savedIndex;
        resolutionDropdown.RefreshShownValue();

        ApplyResolution(savedIndex);

        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    public void SetResolution(int resolutionIndex)
    {
        ApplyResolution(resolutionIndex);
        PlayerPrefs.SetInt(ResolutionIndexKey, resolutionIndex);
        PlayerPrefs.Save();
    }

    private void ApplyResolution(int resolutionIndex)
    {
        Resolution res = filteredResolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
}