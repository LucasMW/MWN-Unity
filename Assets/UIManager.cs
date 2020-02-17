using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject viewPort;
    public Dictionary<string,Image> imageDict;
    public gabAPI gabApiObject;
    Item[] items;
    GameObject[] fixedCells;
    // Start is called before the first frame update
    void Start()
    {
        imageDict = new Dictionary<string, Image>(30);
        items = new Item[0];
        fixedCells = GameObject.FindGameObjectsWithTag("Cell");
        Debug.LogWarning("fixedCells " + fixedCells.Length);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.items.Length > 1)
        {
            return;
        }
        items = gabApiObject.getItems();
        if (items.Length > 1)
        {
            UpdateUI(items);
        }
        
    }
    void UpdateUI(Item[] items)
    {
        Debug.Log("UpdateUI");
        Debug.Log(items.Length);
        int index = 0;
        foreach (Item item in items)
        {
            GameObject cell = fixedCells[index];
            Image img = cell.GetComponent<Image>();
            
            Text[] texts = cell.GetComponentsInChildren<Text>();
            Debug.Log(texts.Length);
            Debug.Log(texts[0].name);
            texts[0].text = item.title;
            texts[1].text = item.description;
            if(!imageDict.ContainsKey(item.url))
                LoadImage(item.url, img);
            else
            {
                img.overrideSprite = imageDict[item.url].sprite;
            }
            
            //cell.transform.position = new Vector3(viewPort.transform.position.x, viewPort.transform.position.y - index * 300);
            //cell.GetComponent<RectTransform>().SetParent(viewPort.transform);
            //cell.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 10);
            //cell.layer = 5;
            index += 1;
            


        }
    }
    void LoadImage(string url, Image imageRef)
    {
        StartCoroutine(DownloadImage(url,imageRef));
    }

    IEnumerator DownloadImage(string MediaUrl, Image YourRawImage)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            //YourRawImage.overrideSprite = Sprite.Create(((DownloadHandlerTexture)request.downloadHandler).texture, YourRawImage.GetPixelAdjustedRect(), new Vector2(0, 0));
            imageDict.Add(MediaUrl, YourRawImage);
        }
            //YourRawImage.mainTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            
    }
}
