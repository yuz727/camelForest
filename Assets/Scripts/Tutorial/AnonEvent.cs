using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class AnonEvent : MonoBehaviour
{
  public LayerMask Player;
  public BoxCollider2D NPC;
  public GameObject Prompt;
  public DialogueManager Manager;
  public TextAsset NpcDialogue;
  public Queue<Dialogue> Dialogues;

  private bool _canTalk;
  private bool _isTalking;

  void Update()
  {
    if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton0)) && _canTalk)
    {
      FindObjectOfType<CanvasController>().OpenDialogueBox();
      if (!_isTalking)
      {
        Dialogues = JsonConvert.DeserializeObject<DialogueCollection>(NpcDialogue.text).CollectionToQueue();
        _isTalking = true;
        FindObjectOfType<PlayerController>().Talking = true;
      }
      NextTrigger();
    }
  }

  void FixedUpdate()
  {
    if (NPC.IsTouchingLayers(Player))
    {
      _canTalk = true;
      Prompt.SetActive(true);
    }
    else
    {
      _canTalk = false;
      Prompt.SetActive(false);
    }
  }

  void NextTrigger()
  {
    if (Dialogues.Count > 0)
    {
      FindObjectOfType<DialogueTrigger>().Dialogue = Dialogues.Dequeue();
      FindObjectOfType<DialogueTrigger>().TriggerDialogue();
      return;
    }
    FindObjectOfType<CanvasController>().CloseDialogueBox();
    FindObjectOfType<PlayerController>().Talking = false;
    _isTalking = false;
  }
}
