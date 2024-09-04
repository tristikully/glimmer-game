using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class TestDialogue : MonoBehaviour
{
    // Field for assigning a TextAsset in the Inspector
    [SerializeField] private TextAsset textFile = null;

    // Start is called before the first frame update
    void Start()
    {
        StartConversation();
    }

    // Method to start the conversation
    void StartConversation()
    {
        if (textFile != null)
        {
            // Convert the text file content into a list of strings
            List<string> lines = new List<string>(textFile.text.Split('\n'));
            
            // Pass the lines to DialogueSystem
            DialogueSystem.instance.Say(lines);
        }
        else
        {
            Debug.LogWarning("Text file not assigned in the Inspector!");
        }
    }
}