using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class ShroomEvent : MonoBehaviour
{
  public BoxCollider2D Player;
  public BoxCollider2D NPC;
  public GameObject Prompt;
  public DialogueManager Manager;
  public TextAsset NpcDialogue;
  public Queue<Dialogue> Dialogues;
  private bool _startRepeat = false;
  private bool _canTalk;
  private bool _isTalking;


  private readonly Dialogue _repeatDialogue = new()
  {
    NPCName = "Shroom",
    Sentences = new string[] { "Don't Worry!", "It's Free!" }
  };

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.E) && _canTalk)
    {
      FindObjectOfType<CanvasController>().OpenDialogueBox();
      if (!_isTalking)
      {
        if (_startRepeat)
        {
          Dialogues = new Queue<Dialogue>();
          Dialogues.Enqueue(_repeatDialogue);
        }
        else
        {
          Dialogues = JsonConvert.DeserializeObject<DialogueCollection>(NpcDialogue.text).CollectionToQueue();

        }
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
    if (FindObjectOfType<DialogueManager>().dialogueSentences.Count > 0)
    {
      FindObjectOfType<DialogueManager>().DisplayNextSentence();
      return;
    }
    if (Dialogues.Count > 0)
    {
      FindObjectOfType<DialogueTrigger>().Dialogue = Dialogues.Dequeue();
      FindObjectOfType<DialogueTrigger>().TriggerDialogue();
      return;
    }
    FindObjectOfType<CanvasController>().CloseDialogueBox();
    if (!_startRepeat)
    {
      _startRepeat = !_startRepeat;
    }
    _isTalking = false;
  }
}
