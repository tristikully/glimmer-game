using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE
{
    public class UserInput : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                PromptAdvance();
        }

        public void PromptAdvance()
        {
            DialogueSystem.instance.OnUserPrompt();
        }
    }
}
