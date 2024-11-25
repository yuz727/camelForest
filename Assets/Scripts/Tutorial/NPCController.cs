using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class NPCController : MonoBehaviour
{
  public LayerMask Player;
  public BoxCollider2D NPC;
  public GameObject Prompt;
  public DialogueManager dialogueManager;
  public DialogueTrigger dialogueTrigger;
  public CanvasController canvasController;
  public PlayerController playerController;
  public ItemController itemController;
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
    if (dialogueManager.dialogueSentences.Count > 0)
    {
      dialogueManager.DisplayNextSentence();
      return true;
    }
    if (Dialogues.Count > 0)
    {
      dialogueTrigger.Dialogue = Dialogues.Dequeue();
      dialogueTrigger.TriggerDialogue();
      return true;
    }
    canvasController.CloseDialogueBox();
    playerController.Talking = false;
    return false;
  }
}
