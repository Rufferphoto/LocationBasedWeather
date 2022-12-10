using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeatherInfo
{
    public main main;
    public weather[] weather;
}

[System.Serializable]
public class main
{
    public float temp;
    public float feels_like;
    public float temp_min;
    public float temp_max;
    public float pressure;
    public float humidity;
}

[System.Serializable] 
public class weather
{
    public string id;
    public string main;
    public string description;
    public string icon;
    public Sprite imageIcon;
}