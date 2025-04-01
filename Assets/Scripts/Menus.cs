using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Methods to interact with data in NPCDatabase and input it to DialogueManager
public class Menus : MonoBehaviour
{
    [Header("NPC Database")]
    public NPCDatabase NPCDatabase;

    [Header("Button Prefab")]
    public GameObject button;

    [Header("Menus")]
    public GameObject JSONMenuContent;
    public GameObject conversationMenuContent;

    private List<GameObject> conversationMenuButtonList = new List<GameObject>();

    public DialogueManager dialogueManager;

    // Start is called before the first frame update
    void Start()
    {
        LoadJSONMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*---Load menus---*/
    void LoadJSONMenu()
    {
        foreach (TextAsset textAsset in NPCDatabase.JSONList)
        {
            GameObject JSONButton = Instantiate(button, JSONMenuContent.transform);
            JSONButton.name = textAsset.name;
            JSONButton.GetComponentInChildren<TMP_Text>().text = textAsset.name;

            DialogueManager.DialogueClass dialogueClass = new DialogueManager.DialogueClass();
            dialogueClass = JsonConvert.DeserializeObject<DialogueManager.DialogueClass>(textAsset.text);

            JSONButton.GetComponent<Button>().onClick.AddListener(() => { JSONMenuButton(dialogueClass); });
        }
    }

    void LoadConversationMenu(DialogueManager.DialogueClass dialogueClass)
    {
        //clear list
        foreach(GameObject button in conversationMenuButtonList)
        {
            Destroy(button);
        }

        //generate list
        foreach (var entry in dialogueClass.dialogueDictionary)
        {
            GameObject conversationButton = Instantiate(button, conversationMenuContent.transform);
            conversationButton.name = entry.Key;
            conversationButton.GetComponentInChildren<TMP_Text>().text = entry.Key;

            conversationButton.GetComponent<Button>().onClick.AddListener(() => { ConversationMenuButton(entry.Key, dialogueClass); });

            conversationMenuButtonList.Add(conversationButton);
        }
    }

    public void JSONMenuButton(DialogueManager.DialogueClass dialogueClass)
    {
        LoadConversationMenu(dialogueClass);
    }

    public void ConversationMenuButton(string entry, DialogueManager.DialogueClass dialogueClass)
    {
        dialogueManager.StartDialogue(dialogueClass.dialogueDictionary[entry], dialogueClass.Name);
    }

    public void Reload()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
