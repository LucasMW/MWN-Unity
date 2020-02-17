using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject viewPort;
    public Dictionary<string,Sprite> imageDict;
    public gabAPI gabApiObject;
    Item[] items;
    GameObject[] fixedCells;
    private float time = 0.0f;
    public float interpolationPeriod = 5;
    // Start is called before the first frame update
    void Start()
    {
        imageDict = new Dictionary<string, Sprite>(30);
        items = new Item[0];
        fixedCells = GameObject.FindGameObjectsWithTag("Cell");
        Debug.LogWarning("fixedCells " + fixedCells.Length);
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time >= interpolationPeriod)
        {
            time = time - interpolationPeriod;

            items = gabApiObject.getItems();
            if (items.Length > 1)
            {
                UpdateUI(items);
            }
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
            Image img = cell.GetComponentInChildren<Image>();
            
            Text[] texts = cell.GetComponentsInChildren<Text>();
            Debug.Log(texts.Length);
            Debug.Log(texts[0].name);
            texts[0].text = item.title;
            texts[1].text = item.description;
            if(!imageDict.ContainsKey(item.url))
                LoadImage(item.url, img);
            else
            {
                Debug.Log(img);
                Debug.Log(imageDict[item.url]);
                img.overrideSprite = imageDict[item.url];
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
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                imageDict.Add(MediaUrl, sprite);
        }
            //YourRawImage.mainTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            
    }
}
