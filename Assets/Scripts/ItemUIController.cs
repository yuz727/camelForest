using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class ItemUIController : MonoBehaviour
{
  public ItemController itemController;
  private readonly List<Vector3> slots = new() { new(-213.9f, 0f, 10f), new(-71.3f, 0f, 10f), new(72f, 0f, 10f) };
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
  public TMP_Text CucumberCount;
  public TMP_Text ArrowCount;

  void Start()
  {
    itemController = FindFirstObjectByType<ItemController>();
    CucumberCount.text = itemController.CucumberCount.ToString();
    ArrowCount.text = itemController.ArrowCount.ToString();
    _occupiedSlots = new GameObject[3];

    for (int i = 0; i < 3; i++)
    {
      Debug.Log(itemController.ItemsOwned[i]);
      UpdateSlots(itemController.ItemsOwned[i], i);
    }
    UpdateSpecialItem(itemController.SpecialItem);
  }

  public void UpdateSelect(int index)
  {
    Select.SetActive(true);
    Select.transform.localPosition = slots[index];
  }

  public void UpdateSlots(Items item, int index)
  {
    switch (item)
    {
      case Items.Key:
        Key.transform.localPosition = slots[index];
        _occupiedSlots[index] = Key;
        Key.SetActive(true);
        break;
      case Items.Bow:
        Bow.transform.localPosition = slots[index];
        _occupiedSlots[index] = Bow;
        Bow.SetActive(true);
        break;
      case Items.Notebook:
        Notebook.transform.localPosition = slots[index];
        _occupiedSlots[index] = Notebook;
        Notebook.SetActive(true);
        break;
      case Items.Cucumber:
        Cucumber.transform.localPosition = slots[index];
        _occupiedSlots[index] = Cucumber;
        Cucumber.SetActive(true);
        break;
      case Items.Mushroom:
        Mushroom.transform.localPosition = slots[index];
        _occupiedSlots[index] = Mushroom;
        Mushroom.SetActive(true);
        break;
      default:
        _occupiedSlots[index]?.SetActive(false);
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
