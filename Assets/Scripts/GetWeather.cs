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
    private bool initialized = false;

    public async void WaitForInit()
    {
        await TaskUtils.WaitUntil(getLocation.AddressAcquired);

        GetLocData();
        StartCoroutine(GetWeatherInfo());
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

    private IEnumerator GetWeatherInfo()
    {
        string webrequest = "https://fcc-weather-api.glitch.me/api/current?lat=" + lat + "&lon=" + lon;

        var www = new UnityWebRequest(webrequest)
        {
            downloadHandler = new DownloadHandlerBuffer()
        };

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            weatherInfo = JsonUtility.FromJson<WeatherInfo>(www.downloadHandler.text);
            Debug.Log(webrequest);
            
        }
        else
        {
            Debug.Log("Could not gather weather info.");
            yield break;
        }


        // Image
        webrequest = weatherInfo.Weather.icon;
        

        var texwww = UnityWebRequestTexture.GetTexture(webrequest);
        texwww.useHttpContinue = false;

        yield return texwww.SendWebRequest();

        if (texwww.result == UnityWebRequest.Result.Success)
        {
            Texture2D img = ((DownloadHandlerTexture)texwww.downloadHandler).texture;
            weatherInfo.Weather.imageTexture = img;
            weatherInfo.Weather.imageIcon = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(.5f, .5f));
            Debug.Log(weatherInfo.Weather.icon);
            initialized = true;
        }
        else
        {
            Debug.Log("Could not gather weather image.");
            yield break;
        }
    }

    public bool WeatherAcquired()
    {
        return initialized;
    }
}