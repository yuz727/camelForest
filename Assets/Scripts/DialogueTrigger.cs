using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DialogueTrigger : MonoBehaviour
{
  public Dialogue Dialogue { get; set; }

  public void TriggerDialogue()
  {
    FindObjectOfType<DialogueManager>().StartDialogue(Dialogue);
  }

  public void TriggerNextLine()
  {
    FindObjectOfType<DialogueManager>().DisplayNextSentence();
  }
}
