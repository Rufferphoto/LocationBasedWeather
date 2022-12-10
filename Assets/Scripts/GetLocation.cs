using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class GetLocation : MonoBehaviour
{
    [SerializeField] private string IPAddress;
    public LocationInfo locationInfo;
    public float latitude;
    public float longitude;
    public bool initialized;
    
    // Start is called before the first frame update
    void Awake()
    {
        GetIP();
    }

    private async void GetIP()
    {
        var www = new UnityWebRequest("https://ip.seeip.org/geoip")
        {
            downloadHandler = new DownloadHandlerBuffer()
        };

        UnityWebRequest.Result result = await www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            return;
        }

        locationInfo = JsonUtility.FromJson<LocationInfo>(www.downloadHandler.text);
        IPAddress = locationInfo.ip;
        latitude = locationInfo.latitude;
        longitude = locationInfo.longitude;
        initialized = true;
    }

    public bool AddressAcquired()
    {
        return initialized;
    }
}
