using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DundeonClearUI : MonoBehaviour
{
    TextMeshProUGUI item1Text;
    TextMeshProUGUI item2Text;

    TextMeshProUGUI expValueText;
    TextMeshProUGUI goldValueText;
    TextMeshProUGUI featherValueText;

    Button okButton;

    int item1;
    int item2;

    int exp;
    int gold;
    int feather;

    InventoryItem inventoryItem1;
    InventoryItem inventoryItem2;

    private void Awake()
    {
        item1Text = GameObject.Find("Item1Text").GetComponent<TextMeshProUGUI>();
        item2Text = GameObject.Find("Item2Text").GetComponent<TextMeshProUGUI>();

        expValueText = GameObject.Find("ExpValueText").GetComponent<TextMeshProUGUI>();
        goldValueText = GameObject.Find("GoldValueText").GetComponent<TextMeshProUGUI>();
        featherValueText = GameObject.Find("FeatherValueText").GetComponent<TextMeshProUGUI>();

        okButton = GameObject.Find("OkButton").GetComponent<Button>();
        okButton.onClick.AddListener(ClickButton);

        inventoryItem1 = GetComponent<IntermediateHP>();
        inventoryItem2 = GetComponent<IntermediateMP>();
    }

    void Start()
    {
        
    }

    public void SetRewardRandom()
    {
        item1 = Random.Range(5, 11);
        item2 = Random.Range(5, 11);

        exp = Random.Range(300, 501);
        gold = Random.Range(1000, 1501);
        feather = Random.Range(10, 21);

        item1Text.text = item1.ToString();
        item2Text.text = item2.ToString();

        expValueText.text = exp.ToString();
        goldValueText.text = gold.ToString();
        featherValueText.text = feather.ToString();
    }

    public void ClickButton()
    {
        InventoryManager.instance.AddItems(inventoryItem1, item1);
        InventoryManager.instance.AddItems(inventoryItem2, item2);

        PlayerManager.instance.GainExp(exp);
        InventoryManager.instance.Gold.CurrentAmount += gold;
        InventoryManager.instance.Feather.CurrentAmount += feather;

        gameObject.SetActive(false);
    }
}
