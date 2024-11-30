using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Unity.VisualScripting;

public class SoyoEvent : NPCController
{
  public bool StartRepeat = false;
  private bool _canTalk;
  private bool _isTalking;


  private readonly Dialogue _repeatDialogue = new()
  {
    NPCName = "Veggie",
    Sentences = new string[] { "Make sure to bring Camel Back." }
  };

  private readonly Dialogue _itemFull = new()
  {
    NPCName = "Veggie",
    Sentences = new string[] { "You're holding too much items, Wood.",
    "I would give you something but I have to keep your bag light." }
  };

  void Start()
  {
    playerController = FindFirstObjectByType<PlayerController>();
    canvasController = FindFirstObjectByType<CanvasController>();
    dialogueTrigger = FindFirstObjectByType<DialogueTrigger>();
    dialogueManager = FindFirstObjectByType<DialogueManager>();
    itemController = FindFirstObjectByType<ItemController>();
  }

  void Update()
  {
    if (itemController.ReplacingItem) return;
    if (InputHandling.CheckInteract() && _canTalk)
    {
      canvasController.OpenDialogueBox();
      if (!_isTalking)
      {
        if (StartRepeat)
        {
          Dialogues = new Queue<Dialogue>();
          Dialogues.Enqueue(_repeatDialogue);
        }
        else if (itemController._itemSpaceRemaining == 0)
        {
          Dialogues = new Queue<Dialogue>();
          Dialogues.Enqueue(_itemFull);
        }
        else
        {
          Dialogues = JsonConvert.DeserializeObject<DialogueCollection>(NPCDialogue.text).CollectionToQueue();
        }
        _isTalking = true;
        playerController.Talking = true;
      }
      _isTalking = DisplayDialogue();
      if (_isTalking)
      {
        return;
      }
      if (itemController._itemSpaceRemaining != 0 && !StartRepeat && itemController.AddItem(Items.Cucumber))
      {
        StartRepeat = true;
      }
    }
  }
  void FixedUpdate()
  {
    _canTalk = CheckPlayerOverlap();
  }
}
