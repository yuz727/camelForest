using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialItemsTrigger : MonoBehaviour
{
  public LayerMask Player;
  public GameObject RockSprite;
  public Animator anim;
  public BoxCollider2D Rock;
  public ItemController itemController;
  public CanvasController canvasController;
  public DialogueTrigger dialogueTrigger;
  private bool _animating = false;

  void Start()
  {
    itemController = FindFirstObjectByType<ItemController>();
    canvasController = FindFirstObjectByType<CanvasController>();
    dialogueTrigger = FindFirstObjectByType<DialogueTrigger>();
  }

  void Update()
  {
    if (Rock.IsTouchingLayers(Player))
    {
      if (InputHandling.CheckUseItem() && !_animating)
      {
        switch (itemController.SpecialItem)
        {
          case SpecialItems.Crowbar:
            anim.SetBool("isCrowbar", true);
            StartCoroutine(AnimTimer(0.6f));
            break;
          case SpecialItems.Dynamite:
            anim.SetBool("isBomb", true);
            StartCoroutine(AnimTimer(1.3f));
            break;
          case SpecialItems.Sword:
            anim.SetBool("isSword", true);
            StartCoroutine(AnimTimer(0.9f));
            break;
          default:
            canvasController.OpenDialogueBox();
            dialogueTrigger.Dialogue = new() { NPCName = "", Sentences = new string[] { "It seems like you need something to move the rock." } };
            dialogueTrigger.TriggerDialogue();
            StartCoroutine(SpecialItemHint());
            break;
        }
        _animating = true;
      }
    }
  }

  private IEnumerator SpecialItemHint()
  {
    yield return new WaitForSeconds(3f);
    canvasController.CloseDialogueBox();
    _animating = false;
  }

  IEnumerator AnimTimer(float time)
  {
    yield return new WaitForSeconds(time);
    RockSprite.SetActive(false);
    switch (itemController.SpecialItem)
    {
      case SpecialItems.Crowbar:
        anim.SetBool("isCrowbar", false);
        break;
      case SpecialItems.Dynamite:
        anim.SetBool("isBomb", false);
        break;
      case SpecialItems.Sword:
        anim.SetBool("isSword", false);
        break;
      default:
        break;
    }
  }
}
