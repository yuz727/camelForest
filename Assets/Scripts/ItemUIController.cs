using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ItemUIController : MonoBehaviour
{
  public ItemController itemController;
  private readonly List<Vector3> slots = new() { new(236.6f, -284f, 1f), new(316.3f, -284f, 1f), new(395.9f, -284f, 1f) };
  private GameObject[] _occupiedSlots;
  public GameObject Select;
  public GameObject Bow;
  public GameObject Notebook;
  public GameObject Mushroom;
  public GameObject Key;
  public GameObject Cucumber;
  public GameObject Sword;
  public GameObject Crowbar;
  public GameObject Dynamite;

  void Start()
  {
    _occupiedSlots = new GameObject[] { Bow, Notebook, Mushroom };
    itemController = FindFirstObjectByType<ItemController>();
    for (int i = 0; i < itemController.ItemsOwned.Count; i++)
    {
      UpdateSlots(itemController.ItemsOwned[i], i);
    }
    UpdateSpecialItem(itemController.SpecialItem);
  }

  public void UpdateSelect(int index)
  {
    Debug.Log("Moving");
    Select.SetActive(true);
    Select.transform.position = slots[index];
  }

  public void UpdateSlots(Items item, int index)
  {
    switch (item)
    {
      case Items.Key:
        Key.transform.position = slots[index];
        _occupiedSlots[index] = Key;
        Key.SetActive(true);
        break;
      case Items.Bow:
        Bow.transform.position = slots[index];
        _occupiedSlots[index] = Bow;
        Bow.SetActive(true);
        break;
      case Items.Notebook:
        Notebook.transform.position = slots[index];
        _occupiedSlots[index] = Notebook;
        Notebook.SetActive(true);
        break;
      case Items.Cucumber:
        Cucumber.transform.position = slots[index];
        _occupiedSlots[index] = Cucumber;
        Cucumber.SetActive(true);
        break;
      case Items.Mushroom:
        Mushroom.transform.position = slots[index];
        _occupiedSlots[index] = Mushroom;
        Mushroom.SetActive(true);
        break;
      default:
        _occupiedSlots[index].SetActive(false);
        break;
    }
  }

  public void UpdateSpecialItem(SpecialItems item)
  {
    switch (item)
    {
      case SpecialItems.Sword:
        Sword.SetActive(true);
        Dynamite.SetActive(false);
        Crowbar.SetActive(false);
        break;
      case SpecialItems.Dynamite:
        Sword.SetActive(false);
        Dynamite.SetActive(true);
        Crowbar.SetActive(false);
        break;
      case SpecialItems.Crowbar:
        Sword.SetActive(false);
        Dynamite.SetActive(false);
        Crowbar.SetActive(true);
        break;
      default:
        break;
    }
  }
}
