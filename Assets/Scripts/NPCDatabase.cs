using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

//contains a list of NPC JSON files and a list of portrait images. The image list is turned to a dictionary for access purposes.
//also imports user content from the /StreamingAssets folder.

//NOTE: This script is executed early in the project setting's Script Execution Order to build the portrait dictionary first.

public class NPCDatabase : MonoBehaviour
{
    [Header("JSON Database")]
    public List<TextAsset> JSONList;

    [Header("Portrait Database")]
    public List<Sprite> portraitList;
    public Dictionary<string, Sprite> portraitDictionary = new Dictionary<string, Sprite>();

    private void Start()
    {

        /*---Import User JSON Files---*/
        foreach (string fileName in Directory.GetFiles(Application.streamingAssetsPath + "/JSON", "*.json"))
        {
            Debug.Log(fileName);
            using StreamReader reader = new(fileName);

            TextAsset textAsset = new TextAsset(reader.ReadToEnd());
            textAsset.name = Path.GetFileNameWithoutExtension(fileName);
            JSONList.Add(textAsset);
        }

        /*---Import User Portrait Files---*/
        foreach (string fileName in Directory.GetFiles(Application.streamingAssetsPath + "/Portraits", "*.jpg"))
        {

            Texture2D tex = new Texture2D(2, 2);
            ImageConversion.LoadImage(tex, System.IO.File.ReadAllBytes(fileName));

            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f));
            sprite.name = Path.GetFileNameWithoutExtension(fileName);
            portraitList.Add(sprite);
        }

        foreach (string fileName in Directory.GetFiles(Application.streamingAssetsPath + "/Portraits", "*.png"))
        {

            Texture2D tex = new Texture2D(2, 2);
            ImageConversion.LoadImage(tex, System.IO.File.ReadAllBytes(fileName));

            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f));
            sprite.name = Path.GetFileNameWithoutExtension(fileName);
            portraitList.Add(sprite);
        }

        /*---convert portraitList to portraitDictionary---*/
        foreach (Sprite sprite in portraitList)
        {
            portraitDictionary.Add(sprite.name, sprite);
        }
    }
}

