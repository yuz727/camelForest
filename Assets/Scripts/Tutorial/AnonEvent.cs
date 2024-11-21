using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class AnonEvent : MonoBehaviour
{
  public BoxCollider2D Player;
  public BoxCollider2D NPC;
  public GameObject Prompt;
  public DialogueManager Manager;
  public TextAsset NpcDialogue;
  public Queue<Dialogue> Dialogues;

  private bool _canTalk;
  private bool _isTalking;

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.E) && _canTalk)
    {
      FindObjectOfType<CanvasController>().OpenDialogueBox();
      if (!_isTalking)
      {
        Dialogues = JsonConvert.DeserializeObject<DialogueCollection>(NpcDialogue.text).CollectionToQueue();
        _isTalking = true;
      }
      NextTrigger();
    }
  }

  void FixedUpdate()
  {
    if (Player.IsTouching(NPC))
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
    _isTalking = false;
  }
}
