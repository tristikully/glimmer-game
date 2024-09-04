using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

public class TestArchitect : MonoBehaviour
{
    DialogueSystem ds;
    TextArchitect architect;

    string[] lines = new string[5]
    {
        "This is a random line of dialogue",
        "I want to say something, come over here",
        "In the previous chapter, you learned about arrays",
        "However, if you want to store data as a tabular form",
        "Arrays can have any number of dimensions"
    };


    void Start()
    {
        ds = DialogueSystem.instance;
        architect = new TextArchitect(ds.dialogueContainer.dialogueText);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (architect.isBuilding)
            {
                architect.ForceComplete();
            }
            else
                architect.Build(lines[Random.Range(0, lines.Length)]);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            architect.Append(lines[Random.Range(0, lines.Length)]);
        }
    }
}
