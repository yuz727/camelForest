using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnonEvent : MonoBehaviour
{
  public BoxCollider2D player;
  public BoxCollider2D NPC;
  public GameObject dialogueUI;
  public GameObject Prompt;
  public DialogueManager manager;
  bool canTalk;
  bool isTalking;
  public TextAsset npcDialogue;
  public Queue<Dialogue> dialogues;

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.E) && canTalk)
    {
      dialogueUI.SetActive(true);
      if (!isTalking)
      {
        dialogues = JsonUtility.FromJson<DialogueCollection>(npcDialogue.text).CollectionToQueue();
        isTalking = true;
      }
      NextTrigger();
    }
  }

  void FixedUpdate()
  {
    if (player.IsTouching(NPC))
    {
      canTalk = true;
      Prompt.SetActive(true);
    }
    else
    {
      canTalk = false;
      Prompt.SetActive(false);
    }
  }

  void NextTrigger()
  {
    if (dialogues.Count > 0)
    {
      FindObjectOfType<DialogueTrigger>().Dialogue = dialogues.Dequeue();
      FindObjectOfType<DialogueTrigger>().TriggerDialogue();
      return;
    }
    dialogueUI.SetActive(false);
    isTalking = false;
  }
}
