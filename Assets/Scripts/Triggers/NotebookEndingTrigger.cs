using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class NotebookEndingTrigger : NPCController
{
  private bool _isTalking;
  private bool _triggered;

  // Start is called before the first frame update
  void Start()
  {
    itemController = FindFirstObjectByType<ItemController>();
    if (itemController.ItemsOwned.Contains(Items.Notebook))
    {
      playerController = FindFirstObjectByType<PlayerController>();
      canvasController = FindFirstObjectByType<CanvasController>();
      dialogueTrigger = FindFirstObjectByType<DialogueTrigger>();
      dialogueManager = FindFirstObjectByType<DialogueManager>();
    }
    else
    {
      this.gameObject.SetActive(false);
    }
  }

  // Update is called once per frame
  void Update()
  {
    if (NPC.IsTouchingLayers(Player) && !_triggered)
    {
      LoadDialogue(NPCDialogue);
    }
    if (_triggered && InputHandling.CheckInteract())
    {
      if (!_isTalking)
      {
        this.gameObject.SetActive(false);
      }
      _isTalking = DisplayDialogue();
    }

  }

  void LoadDialogue(TextAsset dialogue)
  {
    _triggered = true;
    Dialogues = JsonConvert.DeserializeObject<DialogueCollection>(dialogue.text).CollectionToQueue();
    playerController.Talking = true;
    canvasController.OpenDialogueBox();
    _isTalking = DisplayDialogue();
  }
}
