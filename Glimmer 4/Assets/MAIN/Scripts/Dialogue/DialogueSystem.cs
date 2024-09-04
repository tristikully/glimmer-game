using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DIALOGUE
{
    public class DialogueSystem : MonoBehaviour
    {
        public DialogueContainer dialogueContainer;
        private ConversationManager conversationManager;
        private TextArchitect architect;

        public static DialogueSystem instance { get; private set; }

        public delegate void DialogueSystemEvent();
        public event DialogueSystemEvent onUserPrompt;

        public bool isRunningConversation => conversationManager.isRunning;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                Initialize();
            }
            else
                DestroyImmediate(gameObject);
        }

        bool _initialized = false;

        private void Initialize()
        {
            if (_initialized)
                return;

            architect = new TextArchitect(dialogueContainer.dialogueText);
            conversationManager = new ConversationManager(architect);
            _initialized = true;
        }

        public void OnUserPrompt() // Event to listen for user input for next line/skip typing
        {
            onUserPrompt?.Invoke();
        }


        public void ShowSpeakerName(string speakerName = "")
        {   
            if (speakerName.ToLower() != "narrator" && speakerName.ToLower() != "self")
            {
                dialogueContainer.nameContainer.Show(speakerName);
            }
            else
            {
                HideSpeakerName();
            }
        }

        public void HideSpeakerName() => dialogueContainer.nameContainer.Hide();

        public void Say(string speaker, string dialogue)
        {
            List<string> conversation = new List<string>() { $"{ speaker } \" { dialogue } \""};
            Say(conversation);
        }

        public void Say(List<string> conversation)
        {
            conversationManager.StartConversation(conversation);
        }
    }
}