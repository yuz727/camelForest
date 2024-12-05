using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class TutorialTrigger : NPCController
{
  private bool _isTalking;

  // Start is called before the first frame update
  void Start()
  {
    playerController = FindFirstObjectByType<PlayerController>();
    canvasController = FindFirstObjectByType<CanvasController>();
    dialogueTrigger = FindFirstObjectByType<DialogueTrigger>();
    dialogueManager = FindFirstObjectByType<DialogueManager>();
    itemController = null;
    Dialogues = JsonConvert.DeserializeObject<DialogueCollection>(NPCDialogue.text).CollectionToQueue();
    playerController.Talking = true;
    canvasController.OpenDialogueBox();
    _isTalking = DisplayDialogue();
  }

  // Update is called once per frame
  void Update()
  {
    if (InputHandling.CheckInteract())
    {
      if (!_isTalking)
      {
        this.gameObject.SetActive(false);
      }
      _isTalking = DisplayDialogue();
    }
  }
}
