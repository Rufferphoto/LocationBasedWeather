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
        StartCoroutine(GetIP());
    }

    private IEnumerator GetIP()
    {
        var www = new UnityWebRequest("https://ip.seeip.org/geoip")
        {
            downloadHandler = new DownloadHandlerBuffer()
        };

        yield return www.SendWebRequest();
        
        if (www.result == UnityWebRequest.Result.Success)
        {
            locationInfo = JsonUtility.FromJson<LocationInfo>(www.downloadHandler.text);
            IPAddress = locationInfo.ip;
            latitude = locationInfo.latitude;
            longitude = locationInfo.longitude;
            initialized = true;            
        }
        else
        {
            Debug.Log("Could not gather IP address.");
            yield break;
        }        
    }

    public bool AddressAcquired()
    {
        return initialized;
    }
}
