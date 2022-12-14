using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeatherIconUI : MonoBehaviour
{
    [SerializeField] private GetWeather getWeather;
    private RawImage rawImage;

    private async void Start()
    {
        rawImage = GetComponent<RawImage>();
        await TaskUtils.WaitUntil(getWeather.WeatherAcquired);
        SetWeatherIcon();
    }

    private void SetWeatherIcon()
    {
        rawImage.texture = getWeather.weatherInfo.Weather.imageTexture;
        rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, Mathf.Lerp(0f, 255f, 3f));
    }
}
