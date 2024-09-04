using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DIALOGUE
{
    public class CommandsManager
    {

        public void ExecuteCommand(string commandString)
        {

            Debug.Log($"This is inside the execute command");
            // Pattern: command(argument)
            string pattern = @"(\w+)\(""(.+)""\)";
            Match match = Regex.Match(commandString, pattern);

            if (match.Success)
            {
                string command = match.Groups[1].Value;
                string argument = match.Groups[2].Value;

                Debug.Log($"Command: {command}/nArguments: {argument}");
            }
            else
            {
                Debug.LogError("Invalid command format: " + commandString);
            }
        }

        private void GetItem(string itemName)
        {
            Debug.Log("Getting item: " + itemName);
        }

        private void SetScene(string sceneName)
        {
            Debug.Log("Setting scene to: " + sceneName);
        }
    }
}
