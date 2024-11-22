using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class NPCController : MonoBehaviour
{
  public LayerMask Player;
  public BoxCollider2D NPC;
  public GameObject Prompt;
  public DialogueManager Manager;
  public Queue<Dialogue> Dialogues;
  public TextAsset NPCDialogue;

  public bool CheckPlayerOverlap()
  {
    if (NPC.IsTouchingLayers(Player))
    {
      Prompt.SetActive(true);
      return true;
    }
    else
    {
      Prompt.SetActive(false);
      return false;
    }
  }
  public bool DisplayDialogue()
  {
    if (FindObjectOfType<DialogueManager>().dialogueSentences.Count > 0)
    {
      FindObjectOfType<DialogueManager>().DisplayNextSentence();
      return true;
    }
    if (Dialogues.Count > 0)
    {
      FindObjectOfType<DialogueTrigger>().Dialogue = Dialogues.Dequeue();
      FindObjectOfType<DialogueTrigger>().TriggerDialogue();
      return true;
    }
    FindObjectOfType<CanvasController>().CloseDialogueBox();
    FindObjectOfType<PlayerController>().Talking = false;
    return false;
  }
}
