using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour {

    public static MoneyManager Instance;

    public Text moneyText;
    public int defaultStartingMoney;
    [SerializeField]
    int currentMoney;

    public struct OnMoneyChangedEvent { }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(Instance);
        }
        SetText();
    }

    [SubscribeGlobal]
    public void HandleLevelCreationStart(LevelCreationStartEvent e)
    {
        InitPurchasingStage(defaultStartingMoney);
    }

    public void InitPurchasingStage(int startingMoney)
    {
        currentMoney = startingMoney;
        SetText();
        this.gameObject.PublishEvent(new OnMoneyChangedEvent());
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        SetText();
        this.gameObject.PublishEvent(new OnMoneyChangedEvent());
    }

    public void RemoveMoney(int amount)
    {
        currentMoney -= amount;
        SetText();
        this.gameObject.PublishEvent(new OnMoneyChangedEvent());
    }

    void SetText()
    {
        moneyText.text = "" + currentMoney;
    }

    public bool CanAfford(int cost)
    {
        return currentMoney >= cost;
    }
}
