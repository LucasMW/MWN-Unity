using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class Author
{
    public string name;
    public string url;
}

[Serializable]
public class Item
{
    public string id;
    public string url;
    public string title;
    public string description;
    public string image;
    public string date_published;
    public string date_modified;
    public Author author;
}
[Serializable]
public class RootObject
{
    public string version;
    public string title;
    public string home_page_url;
    public string feed_url;
    public string description;
    public string icon;
    public Author author ;
    public Item[] items ;
    public static RootObject CreateFromJSon(string json)
    {
        RootObject root = JsonUtility.FromJson<RootObject>(json);
        Debug.Log(root.version);
        Debug.Log(root.author.name);
        foreach(Item item in root.items)
        {
            Debug.Log(item.description);
        }
        return root;
    }
}


public class gabAPI : MonoBehaviour
{
    String lastJsonResult = null;
    String url = "https://trends.gab.com/trend-feed/json";
    RootObject root;
    // Start is called before the first frame update
    void Start()
    {
        PlayerInfo pi = PlayerInfo.CreateFromJSON("{\"name\":\"Dr Charles\",\"lives\":3,\"health\":0.8}");
        //RootObject r = RootObject.CreateFromJSon("{\"version\": \"FakeFeed\"," +
        //                                          "\"title\": \"GabTrends.com Feed\"," +
        //                                          "\"home_page_url\": \"https://trends.gab.com/\"," +
        //                                          "\"feed_url\": \"https://trends.gab.com/trend-feed\"," +
        //                                          "\"description\": \"Hourly updates from the people-powered newsroom.\"," +
        //                                          "\"icon\": \"https://trends.gab.com/img/social-card.jpg\"" +
        //                                        "}");
        //Debug.Log(r.description);
        Debug.Log(pi.name);
        requestData();
    }
    public Item[] getItems()
    {
        if (root == null)
            return new Item[0];
        return root.items;
    }

    // Update is called once per frame
    void Update()
    {
        //if (this.lastJsonResult != null)
        //{
        //    this.root = RootObject.CreateFromJSon(lastJsonResult);
        //}
    }
    void requestData()
    {
        Debug.Log("Requesting Data");
        StartCoroutine(getData());


    }
    IEnumerator getData()
    {
        string url = this.url;
        UnityWebRequest www = UnityWebRequest.Get(url);
        //www.chunkedTransfer = false;
        yield return www.Send();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            if (www.isDone)
            {
                string jsonResult =
                    System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                Debug.Log(jsonResult);
                this.lastJsonResult = jsonResult;

                this.root = RootObject.CreateFromJSon(jsonResult);
                

                //entities.OrderBy(p => p.title).Select(x => new UnityEngine.UI.Dropdown.OptionData()
                //{

                //    text = x.title
                //}).ToList();
                // ddlCountries.options.AddRange(
                //    entities.OrderBy(p => p.name).Select(x =>
                //                  new UnityEngine.UI.Dropdown.OptionData()
                //                  {
                //                      text = x.name
                //                  }).ToList());
                //ddlCountries.value = 0;

            }
            //ddlCountries.options.AddRange(entities.
        }
    }

}


[Serializable]
public class PlayerInfo
{
    public string name;
    public int lives;
    public float health;

    public static PlayerInfo CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<PlayerInfo>(jsonString);
    }

    // Given JSON input:
    // {"name":"Dr Charles","lives":3,"health":0.8}
    // this example will return a PlayerInfo object with
    // name == "Dr Charles", lives == 3, and health == 0.8f.
}
