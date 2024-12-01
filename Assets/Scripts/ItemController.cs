using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class ItemController : MonoBehaviour
{
  public static ItemController S_Instance;
  public PlayerController playerController;
  public ItemUIController itemUIController;
  public CanvasController canvasController;
  public DialogueTrigger dialogueTrigger;
  public SpecialItems SpecialItem;
  public SpecialItems SpecialItemUsed;
  public List<Items> ItemsOwned;
  public int _index;
  public int _itemSpaceRemaining;
  private int _nextOpening;
  private bool _canCucumber = true;
  private bool _canMushroom = true;
  private int _arrowCount;
  private int _cucumberCount;
  public bool ReplacingItem;
  public CartState cartState;

  void Update()
  {
    var newIndex = InputHandling.CheckSwitchItem(_index);
    if (InputHandling.CheckDiscardItem())
    {
      DiscardItem();
    }
    if (newIndex != -1)
    {
      itemUIController.UpdateSelect(newIndex);
      _index = newIndex;
    }
    if (ReplacingItem && InputHandling.CheckInteract())
    {
      ReplacingItem = false;
      playerController.Talking = false;
      canvasController.CloseDialogueBox();
      return;
    }
    if (ReplacingItem && InputHandling.CheckUseItem())
    {
      RemoveItem(_index);
      itemUIController.UpdateSlots(Items.Empty, _index);
      ReplacingItem = false;
      playerController.Talking = false;
      canvasController.CloseDialogueBox();
      return;
    }
    else if (InputHandling.CheckUseItem()) UseItem(_index);
  }

  void DiscardItem()
  {
    canvasController.OpenDialogueBox();
    playerController.Talking = true;
    ReplacingItem = true;
    dialogueTrigger.Dialogue = new() { NPCName = "", Sentences = new string[] { "Press Use on the item to discard it, or Press Interact if you want to replace none of it." } };
    dialogueTrigger.TriggerDialogue();
  }

  public void SetSpecialItem(SpecialItems specialItem)
  {
    if (SpecialItem != specialItem)
    {
      FindObjectOfType<CanvasController>().OpenDialogueBox();
      SpecialItem = specialItem;
      itemUIController.UpdateSpecialItem(specialItem);
      FindObjectOfType<DialogueTrigger>().Dialogue = new() { NPCName = "", Sentences = new string[] { $"Obtained the {specialItem}!" } };
      FindObjectOfType<DialogueTrigger>().TriggerDialogue();
      StartCoroutine(DialogueUITimer());
    }
  }
  public bool AddItem(Items item)
  {
    if (_itemSpaceRemaining == 0)
    {
      canvasController.OpenDialogueBox();
      dialogueTrigger.Dialogue = new() { NPCName = "", Sentences = new string[] { "Inventory is full, need to discard item to make space." } };
      dialogueTrigger.TriggerDialogue();
      StartCoroutine(DialogueUITimer());
      return false;
    }
    if (!ItemsOwned.Contains(item))
    {
      for (int i = 0; i < 3; i++)
      {
        if (ItemsOwned[i] == Items.Empty)
        {
          _nextOpening = i;
          break;
        }
      }
      canvasController.OpenDialogueBox();
      switch (item)
      {
        case Items.Bow:
          _arrowCount = 20;
          itemUIController.ArrowCount.text = "20";
          goto default;
        case Items.Cucumber:
          _cucumberCount = 5;
          itemUIController.CucumberCount.text = "5";
          goto default;
        default:
          ItemsOwned[_nextOpening] = item;
          itemUIController.UpdateSlots(item, _nextOpening);
          _itemSpaceRemaining--;
          dialogueTrigger.Dialogue = new() { NPCName = "", Sentences = new string[] { $"Obtained the {item}!" } };
          dialogueTrigger.TriggerDialogue();
          StartCoroutine(DialogueUITimer());
          return true;
      }
    }
    return false;
  }


  private IEnumerator DialogueUITimer()
  {
    yield return new WaitForSeconds(2f);
    canvasController.CloseDialogueBox();
  }

  public void ShootBow(int index)
  {
    //TODO: Implement
    _arrowCount--;
    itemUIController.ArrowCount.text = _arrowCount.ToString();
    if (_arrowCount <= 0)
    {
      RemoveItem(index);
    }
  }

  private void UseItem(int index)
  {
    switch (ItemsOwned[index])
    {
      case Items.Bow:
        ShootBow(index);
        break;
      case Items.Cucumber:
        if (_canCucumber)
        {
          StartCoroutine(CucumberTrigger());
          playerController.Invincibility = true;
          playerController.Vfx.SetBool("cVfx", true);
          _canCucumber = false;
          _cucumberCount--;
          itemUIController.CucumberCount.text = _cucumberCount.ToString();
          if (_cucumberCount == 0)
          {
            RemoveItem(index);
          }
        }
        break;
      case Items.Mushroom:
        if (_canMushroom)
        {
          StartCoroutine(MushroomTrigger());
          playerController.Vfx.SetBool("mVfx", true);
          playerController.Mushroomed = true;
          _canMushroom = false;
        }
        break;
      default:
        break;
    }
  }

  public void UseItem(Items item)
  {
    for (int i = 0; i < ItemsOwned.Count; i++)
    {
      if (ItemsOwned[i] == item)
      {
        RemoveItem(i);
      }
    }
  }


  private void RemoveItem(int index)
  {
    itemUIController.UpdateSlots(Items.Empty, index);
    ItemsOwned[index] = Items.Empty;
    _itemSpaceRemaining++;
  }

  private IEnumerator CucumberTrigger()
  {
    yield return new WaitForSeconds(5f);
    playerController.Vfx.SetBool("cVfx", false);
    playerController.Anim.SetBool("isCucumber", false);
    playerController.Invincibility = false;
    _canCucumber = true;
  }

  private IEnumerator MushroomTrigger()
  {
    yield return new WaitForSeconds(10f);
    playerController.Vfx.SetBool("mVfx", false);
    playerController.Mushroomed = false;
    _canMushroom = true;
  }

  void Awake()
  {
    if (S_Instance == null)
    {
      S_Instance = this;
      SpecialItem = SpecialItems.None;
      cartState = CartState.Basketball;
      _cucumberCount = -1;
      _arrowCount = -1;
      _itemSpaceRemaining = 3;
      ItemsOwned = new() { Items.Empty, Items.Empty, Items.Empty };
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }

  void OnEnable()
  {
    SceneManager.sceneLoaded += OnSceneLoaded;
  }

  void OnDisable()
  {
    SceneManager.sceneLoaded -= OnSceneLoaded;
  }

  void OnSceneLoaded(Scene scene, LoadSceneMode mode)
  {
    Debug.Log("Entering " + scene.name);
    if (!scene.name.Equals("Menu"))
    {
      itemUIController = FindFirstObjectByType<ItemUIController>();
      canvasController = FindFirstObjectByType<CanvasController>();
      playerController = FindFirstObjectByType<PlayerController>();
      dialogueTrigger = FindFirstObjectByType<DialogueTrigger>();
    }
  }
}

public enum SpecialItems
{
  None,
  Crowbar,
  Dynamite,
  Sword
}
public enum Items
{
  Empty,
  Key,
  Notebook,
  Mushroom,
  Bow,
  Cucumber
}