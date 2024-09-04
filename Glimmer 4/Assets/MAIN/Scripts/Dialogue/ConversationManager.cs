using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE
{
    public class ConversationManager
    {
        private DialogueSystem dialogueSystem => DialogueSystem.instance;
        private Coroutine process = null;
        public bool isRunning => process != null;
        public TextArchitect architect = null;
        public CommandsManager commandsManager = null;

        private bool userInput = false;

        public ConversationManager(TextArchitect architect)
        {
            this.architect = architect;
            dialogueSystem.onUserPrompt += OnUserPrompt;
        }

        private void OnUserPrompt()
        {
            userInput = true;
        }

        public void StartConversation(List <string> conversation)
        {
            StopConversation();

            process = dialogueSystem.StartCoroutine(RunningConversation(conversation)); // Start coroutine
        }

        public void StopConversation()
        {
            if (!isRunning)
                return;

            dialogueSystem.StopCoroutine(process);
            process = null;
        }

        IEnumerator RunningConversation(List<string> conversation)
        {
            for (int i = 0; i < conversation.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(conversation[i]))
                    continue;

                DIALOGUE_LINE line = DialogueParser.Parse(conversation[i]);

                //Show dialogue
                if (line.hasDialogue)
                {
                    yield return Line_RunDialogue(line);
                    
                }

                //Show commands
                if (line.hasCommands)
                {
                    yield return Line_RunCommands(line);
                    
                }
                yield return WaitForUserInput();
            }
        }

        IEnumerator Line_RunDialogue(DIALOGUE_LINE line)
        {
            if (line.hasSpeaker)
                dialogueSystem.ShowSpeakerName(line.speaker);

            //Run dialogue
            yield return BuildLineSegments(line.dialogue);

        }
        
        IEnumerator Line_RunCommands(DIALOGUE_LINE line)
        {
            Debug.Log($"Executing command: {line.commands}");
            
            yield return null;
        }

        IEnumerator BuildLineSegments(DL_DIALOGUE_DATA line)
        {
            for (int i = 0; i < line.segments.Count; i++)
            {
                DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment = line.segments[i];

                yield return WaitForDialogueSegmentSignal(segment);

                yield return BuildDialogue(segment.dialogue, segment.appendText);
            }
        }
        
        IEnumerator WaitForDialogueSegmentSignal(DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment)
        {
            switch(segment.startSignal)
            {
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.C:
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.A:
                    yield return WaitForUserInput();
                    break;
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.WC:
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.WA:
                    yield return new WaitForSeconds(segment.signalDelay);
                    break;
                default:
                    break;

            }
        }

        IEnumerator BuildDialogue(string dialogue, bool append = false)
        {
            //Build or Append dialogue
            if (!append)
                architect.Build(dialogue);
            else
                architect.Append(dialogue);

            //Wait for dialogue to complete building
            while (architect.isBuilding)
            {
                if (userInput)
                {
                    architect.ForceComplete();
                    userInput = false;
                }
                yield return null;
            }
        }

        IEnumerator WaitForUserInput()
        {
            while(!userInput)
            {
                yield return null;
            }

            userInput = false;
        }




    }
}
