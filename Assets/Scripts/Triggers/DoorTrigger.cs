using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
  public LayerMask Player;
  public GameObject RockSprite;
  public BoxCollider2D Door;
  public ItemController itemController;
  public CanvasController canvasController;
  public DialogueTrigger dialogueTrigger;
  void Start()
  {
    itemController = FindFirstObjectByType<ItemController>();
    canvasController = FindFirstObjectByType<CanvasController>();
    dialogueTrigger = FindFirstObjectByType<DialogueTrigger>();
  }

  void Update()
  {
    if (InputHandling.CheckUseItem() && Door.IsTouchingLayers(Player))
    {
      Debug.Log("?");
      if (itemController.ItemsOwned[itemController._index] == Items.Key)
      {
        this.gameObject.SetActive(false);
        return;
      }
      canvasController.OpenDialogueBox();
      dialogueTrigger.Dialogue = new() { NPCName = "", Sentences = new string[] { "It seems like you need some lock opening shaped item." } };
      dialogueTrigger.TriggerDialogue();
      StartCoroutine(Hint());

    }
  }
  private IEnumerator Hint()
  {
    yield return new WaitForSeconds(2f);
    canvasController.CloseDialogueBox();
  }
}



