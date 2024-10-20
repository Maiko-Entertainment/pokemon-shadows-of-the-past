using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class UIShopViewer : MonoBehaviour
{
    public UIShopItem shopItemPrefab;
    public AudioClip selectSound;
    public AudioClip buySound;
    public AudioClip noMoneySound;
    
    public Transform itemList;
    public ScrollRect scrollRect;
    public Transform modeSelectionList;

    public TextMeshProUGUI itemName;
    public TextMeshProUGUI description;
    public Image icon;
    public TextMeshProUGUI inventoryAmount;
    public TextMeshProUGUI yourMoneyAmount;

    public TransitionCanvasGroup modeSelectedScreen;
    public TransitionCanvasGroup buyButton;
    public TextMeshProUGUI buySellButton;

    public float minMoneyChangeSpeed = 500;

    protected ItemData currentItem;
    protected List<ItemData> itemsForSale = new List<ItemData>();
    protected float moneyShown = 0;

    public bool isModeSelected = false;
    public bool isSelling = true;

    public void Load(List<ItemData> itemsForSale)
    {
        this.itemsForSale = itemsForSale;
    }

    public void Buy(ItemData item)
    {
        int money = InventoryMaster.GetInstance().GetMoney();
        if (item.price <= money)
        {
            InventoryMaster.GetInstance().ChangeItemAmount(item.GetItemId(), 1);
            InventoryMaster.GetInstance().ChangeMoney(item.price * -1);
            AudioMaster.GetInstance().PlaySfx(buySound);
            foreach (Transform previous in itemList)
            {
                previous.GetComponent<UIShopItem>().Reload();
            }
        }
        else
        {
            AudioMaster.GetInstance().PlaySfx(noMoneySound);
        }
        View(currentItem);
    }
    public void Sell(ItemData item)
    {
        ItemInventory ii = InventoryMaster.GetInstance().GetItem(item.GetItemId());
        if (ii.GetAmount() > 0 && ii.itemData.CanSell())
        {
            InventoryMaster.GetInstance().ChangeItemAmount(item.GetItemId(), -1);
            InventoryMaster.GetInstance().ChangeMoney((int)(item.price * InventoryMaster.sellModifier));
            AudioMaster.GetInstance().PlaySfx(buySound);
            bool deletedSomething = false;
            Transform previousValid = null;
            foreach (Transform previous in itemList)
            {
                int amount = InventoryMaster.GetInstance().GetItem(previous.GetComponent<UIShopItem>().GetItem().GetItemId()).GetAmount();
                if (amount == 0)
                {
                    Destroy(previous.gameObject);
                    deletedSomething = true;
                }
                else if (!deletedSomething || !previousValid)
                {
                    previousValid = previous;
                }
                previous.GetComponent<UIShopItem>().Reload();
            }
            if (deletedSomething && previousValid)
            {
                UtilsMaster.SetSelected(previousValid.gameObject);
            }
        }
        else
        {
            AudioMaster.GetInstance().PlaySfx(noMoneySound);
        }
        View(currentItem);
    }

    public void View(ItemData item)
    {
        int money = InventoryMaster.GetInstance().GetMoney();
        currentItem = item;
        itemName.text = item.GetName();
        description.text = item.GetDescription();
        icon.sprite = item.icon;

        ItemInventory ii = InventoryMaster.GetInstance().GetItem(item.GetItemId());
        if (ii != null)
        {
            inventoryAmount.text = ii.GetAmount()+ "";
        }
        if (item.price <= money || isModeSelected)
        {
            buyButton?.FadeIn();
        }
        else
        {
            buyButton.FadeOut();
        }
        foreach (RectTransform uIItem in itemList)
        {
            if (uIItem.GetComponent<UIShopItem>().GetItem() == item)
            {
                UtilsMaster.GetSnapToPositionToBringChildIntoView(scrollRect, uIItem);
            }
        }
        AudioMaster.GetInstance().PlaySfx(selectSound);
    }

    public void HandleClose(CallbackContext context)
    {
        if (context.phase == UnityEngine.InputSystem.InputActionPhase.Started)
        {
            HandleClose();
        }
    }

    public void HandleClose()
    {
        if (isModeSelected)
        {
            UtilsMaster.SetSelected(modeSelectionList.GetChild(0).gameObject);
            isModeSelected = false;
            modeSelectedScreen.FadeOut();
        }
        else
        {
            UIPauseMenuMaster.GetInstance().CloseCurrentMenu();
        }
    }

    public void UpdateMoney()
    {
        moneyShown = InventoryMaster.GetInstance().GetMoney();
    }

    public void SetBuyMode()
    {
        isModeSelected = true;
        isSelling = false;
        modeSelectedScreen.FadeIn();
        foreach (Transform previous in itemList)
            Destroy(previous.gameObject);
        List<Selectable> selectables = new List<Selectable>();
        foreach (ItemData id in itemsForSale)
        {
            UIShopItem sp = Instantiate(shopItemPrefab, itemList).Load(id);
            sp.onHover += View;
            sp.onClick += Buy;
            selectables.Add(sp.GetComponent<Selectable>());
        }
        UtilsMaster.LineSelectables(selectables);
        UtilsMaster.SetSelected(selectables[0].gameObject);
        UpdateMoney();
        if (buySellButton)
        {
            buySellButton.text = "Buy";
        }
    }

    public void SetSellMode()
    {
        isModeSelected = true;
        isSelling = true;
        modeSelectedScreen.FadeIn();
        foreach (Transform previous in itemList)
            Destroy(previous.gameObject);
        List<Selectable> selectables = new List<Selectable>();
        List<ItemInventory> inventory = InventoryMaster.GetInstance().GetItems();
        foreach (ItemInventory id in inventory)
        {
            if (id.GetAmount() > 0 && id.itemData.CanSell())
            {
                UIShopItem sp = Instantiate(shopItemPrefab, itemList).Load(id.itemData, true);
                sp.onHover += View;
                sp.onClick += Sell;
                selectables.Add(sp.GetComponent<Selectable>());
            }
        }
        UtilsMaster.LineSelectables(selectables);
        if (selectables.Count > 0)
            UtilsMaster.SetSelected(selectables[0].gameObject);
        UpdateMoney();
        if (buySellButton)
        {
            buySellButton.text = "Sell";
        }
    }

    private void Update()
    {
        int money = InventoryMaster.GetInstance().GetMoney();
        moneyShown = Mathf.MoveTowards(moneyShown, money, Mathf.Max(
            minMoneyChangeSpeed, 
            Mathf.Abs(moneyShown - money) * 2
            ) * Time.deltaTime);
        yourMoneyAmount.text = (int) moneyShown + "";
    }
}
