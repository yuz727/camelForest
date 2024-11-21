using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GabeEvent : MonoBehaviour
{
  public BoxCollider2D Player;
  public BoxCollider2D NPC;
  public GameObject DialogueUI;
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
      DialogueUI.SetActive(true);
      if (!_isTalking)
      {
        Dialogues = JsonUtility.FromJson<DialogueCollection>(NpcDialogue.text).CollectionToQueue();
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
    DialogueUI.SetActive(false);
    _canTalk = false;
  }
}
