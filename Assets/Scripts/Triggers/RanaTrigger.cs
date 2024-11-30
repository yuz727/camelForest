using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class RanaTrigger : NPCController
{
  public GameObject Floor;

  private bool _canTalk;
  private bool _isTalking;
  private bool _done;

  private readonly Dialogue _end = new()
  {
    NPCName = "Cat",
    Sentences = new string[] { "You're holding too much items, Wood.",
    "I would give you something but I have to keep your bag light." }
  };
  void Start()
  {
    playerController = FindFirstObjectByType<PlayerController>();
    canvasController = FindFirstObjectByType<CanvasController>();
    dialogueTrigger = FindFirstObjectByType<DialogueTrigger>();
    dialogueManager = FindFirstObjectByType<DialogueManager>();
    itemController = null;
  }

  void Update()
  {
    if (InputHandling.CheckInteract() && _canTalk)
    {
      canvasController.OpenDialogueBox();
      if (!_isTalking)
      {
        if (_done)
        {
          Dialogues = new Queue<Dialogue>();
          Dialogues.Enqueue(_end);
        }
        else
        {
          Dialogues = JsonConvert.DeserializeObject<DialogueCollection>(NPCDialogue.text).CollectionToQueue();
        }
        _isTalking = true;
        playerController.Talking = true;
      }
      _isTalking = DisplayDialogue();
      if (_isTalking) return;
      if (_done)
      {
        Floor.SetActive(false);
        gameObject.SetActive(false);
        return;
      }
      _done = true;
      playerController.CanDoubleJump = true;
      dialogueTrigger.Dialogue = new() { NPCName = "", Sentences = new string[] { "You can now Double Jump!" } };
      dialogueTrigger.TriggerDialogue();
    }
  }

  void FixedUpdate()
  {
    _canTalk = CheckPlayerOverlap();
  }
}
