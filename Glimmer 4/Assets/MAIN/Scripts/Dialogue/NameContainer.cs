using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DIALOGUE
{
    [System.Serializable]

    public class NameContainer
    {
        [SerializeField] private GameObject root;
        [SerializeField] private TextMeshProUGUI nameText;

        public void Show(string nameDisplay = "")
        {
            root.SetActive(true);

            if (nameDisplay != string.Empty)
                nameText.text = nameDisplay;
        }

        public void Hide()
        {
            root.SetActive(false);
        }
    }
}