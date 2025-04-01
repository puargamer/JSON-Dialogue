using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Xml;
using TMPro;
using Unity.Burst;
using UnityEngine;
using UnityEngine.UI;

//Controls a dialogue box containing text, a profile image, and name based on inputted dialogue data.

//Contains a class DialogueClass to represent dialogue data. It contains:
//      Name: a character name
//      dialogueDictionary: a dictionary of Conversations

//Contains a class Conversations, used to represent conversations. It contains:
//      Portrait: the name of an image file of the character displayed
//      Dialogue: a list of sentences

//Dialogue data is stored in a JSON file, then deserialized to a DialogueClass using Json.NET's JSON to dictionary deserialization method.

//To start Dialogue, the DialogueClass is inserted into StartDialogue(Conversation conversation, string name).
//      conversation: which conversation to load
//      name: name of character

public class DialogueManager : MonoBehaviour
{
    [Header("NPC Database")]
    public NPCDatabase NPCDatabase;

    [Header("Dialogue Box")]
    public TMP_Text text;
    public List<string> lines;
    public float textSpeed;
    private int index;
    private bool isSpeaking;
    public TMP_Text nameText;
    public Image portrait;
    public GameObject canvas;

    [Header("JSON")]
    public TextAsset file;


    public class DialogueClass
    {
        public string Name;
        public Dictionary<string, Conversation> dialogueDictionary;
    }

    
    public class Conversation
    {
        public string portrait;
        public List<string> Dialogue;
    }

    public DialogueClass dialogueClass = new DialogueClass();

    // Start is called before the first frame update
    void Start()
    {
        /*
        //deserialize JSON to dictionary
        deserialize(file);

        //start dialogue
        StartDialogue(dialogueClass.dialogueDictionary["Conversation 1"], dialogueClass.Name);
        */
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isSpeaking)
        {
            if (text.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                text.text = lines[index];
            }
        }
    }

    void deserialize(TextAsset file)
    {
        dialogueClass = JsonConvert.DeserializeObject<DialogueClass>(file.text);
    }

    /*---Dialogue Functions---*/
    #region Dialogue Functions
    public void StartDialogue(Conversation conversation, string name)
    {
        canvas.SetActive(true);

        portrait.sprite = NPCDatabase.portraitDictionary[conversation.portrait];
        portrait.color = Color.white;

        isSpeaking = true;
        lines = conversation.Dialogue;
        text.text = "";
        index = 0;
        nameText.text = name;
        StartCoroutine(TypeLine());

    }

    IEnumerator TypeLine()
    {
        foreach(char c in lines[index].ToCharArray())
        {
            text.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Count - 1)
        {
            index++;
            text.text = "";
            StartCoroutine (TypeLine());
        } else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        canvas.SetActive (false);

        isSpeaking= false;
        text.text = "";
        nameText.text = "";
        portrait.color = Color.clear;
        StopAllCoroutines();
    }

    #endregion

}
