using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System;

public class GetWeather : MonoBehaviour
{
    public GetLocation getLocation;
    public WeatherInfo weatherInfo;
    [SerializeField] private float lat;
    [SerializeField] private float lon;
    private bool locationInit;

    public async void WaitForInit()
    {
        await TaskUtils.WaitUntil(getLocation.AddressAcquired);
        GetLocData();
        GetWeatherInfo();
    }

    public void GetLocData()
    {
        lat = getLocation.latitude;
        lon = getLocation.longitude;
    }


    private void Awake()
    {
        WaitForInit();
    }

    private async void GetWeatherInfo()
    {
        string webrequest = "https://fcc-weather-api.glitch.me/api/current?lat=" + lat + "&lon=" + lon;
        var www = new UnityWebRequest(webrequest)
        {
            downloadHandler = new DownloadHandlerBuffer()
        };

        UnityWebRequest.Result result = await www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            return;
        }
        else
        {
            weatherInfo = JsonUtility.FromJson<WeatherInfo>(www.downloadHandler.text);
            Debug.Log(webrequest);
        }
        
        
        webrequest = weatherInfo.weather[0].icon;
        var texwww = UnityWebRequestTexture.GetTexture(webrequest);
        result = await texwww.SendWebRequest();

        if (texwww.isNetworkError || texwww.isHttpError)
        {
            return;
        }
        else
        {
            Texture2D img = ((DownloadHandlerTexture)texwww.downloadHandler).texture;
            weatherInfo.weather[0].imageIcon = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(.5f, .5f));
            Debug.Log(weatherInfo.weather[0].icon);
        }
    }
}