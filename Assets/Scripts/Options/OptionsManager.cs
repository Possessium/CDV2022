using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private GameObject optionsCanvas;

    [SerializeField] private Volume volume;
    private ColorAdjustments colorAdjustments;

    //Contrast Properties
    [SerializeField] private float contrastIntensity = 1.0f;
    public float ContrastIntensity
    {
        get { return contrastIntensity; }
        set
        {
            contrastIntensity = value;

            if (colorAdjustments != null)
            {
                colorAdjustments.contrast.value = ContrastIntensity;
            }
            PlayerPrefs.SetFloat("ContrastIntensity", value);
        }
    }
    //Saturation Properties
    [SerializeField] private float saturationIntensity = 1.0f;
    public float SaturationIntensity
    {
        get { return saturationIntensity; }
        set
        {
            saturationIntensity = value;

            if (colorAdjustments != null)
            {
                colorAdjustments.saturation.value = SaturationIntensity;
            }
            PlayerPrefs.SetFloat("SaturationIntensity", value);
        }
    }
    //Exposure Properties
    [SerializeField] private float exposureIntensity = 1.0f;
    public float ExposureIntensity
    {
        get { return exposureIntensity; }
        set
        {
            exposureIntensity = value;

            if (colorAdjustments != null)
            {
                colorAdjustments.postExposure.value = ExposureIntensity;
            }
            PlayerPrefs.SetFloat("ExposureIntensity", value);
        }
    }



    [SerializeField] private Slider contrastSlider = null;
    [SerializeField] private Slider saturationSlider = null;
    [SerializeField] private Slider exposureSlider = null;

    private void Start()
    {
        volume.profile.TryGet(out colorAdjustments);

        // Get Saturation Player Prefs
        if (PlayerPrefs.HasKey("SaturationIntensity"))
        {
            colorAdjustments.saturation.value = PlayerPrefs.GetFloat("SaturationIntensity");
        }
        else
        {
            PlayerPrefs.SetFloat("SaturationIntensity", colorAdjustments.saturation.value);
        }

        // Get Contrast Player Prefs
        if (PlayerPrefs.HasKey("ContrastIntensity"))
        {
            colorAdjustments.contrast.value = PlayerPrefs.GetFloat("ContrastIntensity");
        }
        else
        {
            PlayerPrefs.SetFloat("ContrastIntensity", colorAdjustments.contrast.value);
        }

        // Get Exposure Player Prefs
        if (PlayerPrefs.HasKey("ExposureIntensity"))
        {
            colorAdjustments.postExposure.value = PlayerPrefs.GetFloat("ExposureIntensity");
        }
        else
        {
            PlayerPrefs.SetFloat("ExposureIntensity", colorAdjustments.postExposure.value);
        }

        // Set the slider to the correct values
        if (contrastSlider) contrastSlider.value = colorAdjustments.contrast.value;
        if (exposureSlider) exposureSlider.value = colorAdjustments.postExposure.value;
        if (saturationSlider) saturationSlider.value = colorAdjustments.saturation.value;
    }

    public void ToggleOptionsMenu(UnityEngine.InputSystem.InputAction.CallbackContext _ctx)
    {
        if (!_ctx.started)
            return;

        optionsCanvas.SetActive(!optionsCanvas.activeInHierarchy);
    }
}