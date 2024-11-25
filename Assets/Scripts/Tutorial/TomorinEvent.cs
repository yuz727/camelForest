using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class TomorinEvents : NPCController
{
  public bool StartRepeat = false;
  private bool _canTalk;
  private bool _isTalking;
  private readonly Dialogue _repeatDialogue = new()
  {
    NPCName = "GOAT",
    Sentences = new string[] { "Give this to Camel when you see her.", "She'll understand." }
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
      if (!StartRepeat)
      {
        StartRepeat = true;
        itemController.AddItem(Items.Notebook);
      }
    }
  }

  void FixedUpdate()
  {
    _canTalk = CheckPlayerOverlap();
  }
}
