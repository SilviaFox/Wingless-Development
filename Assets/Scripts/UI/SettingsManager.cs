using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    Resolution[] resolutions; // Array of resolutions
    [SerializeField] Dropdown resDropdown; // dropdown used for selecting resolution

    private void Start()
    {
        resolutions = Screen.resolutions; // Get all available screen resolutions
        resDropdown.ClearOptions();

        List<string> options = new List<string>(); // Create a list to store all the resolutions as strings

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++) // For every resolution, add it to the resolution list
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].height == Screen.height &&
                resolutions[i].width == Screen.width) // If this resolution is the current screen resolution
                currentResolutionIndex = i; // this resolution is the currently selected one
            
        }

        resDropdown.AddOptions(options); // Add the options to the dropdown
        resDropdown.value = currentResolutionIndex;
        resDropdown.RefreshShownValue(); // Refresh the value on start
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
