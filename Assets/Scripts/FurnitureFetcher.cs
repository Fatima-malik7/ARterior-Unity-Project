using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class FurnitureItem
{

public GameObject itemPrefab;
public Transform contentHolder;

    public string name;
    public float price;
    public string imageUrl;
}

public class FurnitureFetcher : MonoBehaviour
{
    public GameObject itemPrefab;           // Drag prefab here
    public Transform contentHolder;         // Drag Content here
    public string apiUrl;                   // Your backend API

    void Start()
    {
        StartCoroutine(FetchFurnitureData());
    }

    IEnumerator FetchFurnitureData()
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            GameObject item = Instantiate(itemPrefab, contentHolder);

            string json = "{\"items\":" + request.downloadHandler.text + "}";
            FurnitureDataWrapper wrapper = JsonUtility.FromJson<FurnitureDataWrapper>(json);

            foreach (FurnitureItem item in wrapper.items)
            {
                GameObject newItem = Instantiate(itemPrefab, contentHolder);
                newItem.transform.Find("ItemName").GetComponent<TMP_Text>().text = item.name;
                newItem.transform.Find("ItemPrice").GetComponent<TMP_Text>().text = "$" + item.price;

                StartCoroutine(LoadImage(item.imageUrl, newItem.transform.Find("ItemImage").GetComponent<RawImage>()));
            }
        }
        else
        {
            Debug.LogError("Error fetching data: " + request.error);
        }
    }

    IEnumerator LoadImage(string url, RawImage image)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            image.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
        else
        {
            Debug.LogError("Error loading image: " + request.error);
        }
    }

    [System.Serializable]
    public class FurnitureDataWrapper
    {
        public List<FurnitureItem> items;
    }
}
