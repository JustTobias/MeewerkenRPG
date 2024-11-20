using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text dialogueText;
    public Text nameText;
    public GameObject dialogueBox;
    public GameObject nameBox;

    public string[] dialogueLines;

    public int currentLine;

    // Start is called before the first frame update
    void Start()
    {
        dialogueText.text = dialogueLines[currentLine];
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueBox.activeInHierarchy) {
            if (Input.GetButtonUp("Fire1")) {
                currentLine++;

                if (currentLine >= dialogueLines.Length) {
                    dialogueBox.SetActive(false);
                } else {
                    dialogueText.text = dialogueLines[currentLine];
                }
            }
        }
    }
}
