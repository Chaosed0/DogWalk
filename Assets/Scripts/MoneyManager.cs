using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour {

    public static MoneyManager Instance;

    public Text moneyText;
    public int firstRoundStartMoney;
    public int secondRoundStartMoney;

    public struct OnMoneyChangedEvent { }

    int stage;
    int currentMoney;

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

        currentMoney = firstRoundStartMoney;

        SetText();

        ++stage;
    }

    [SubscribeGlobal]
    public void HandleLevelCreationStart(LevelCreationStartEvent e)
    {
        InitPurchasingStage();
    }

    public void InitPurchasingStage()
    {
        int startingMoney;
        if (stage <= 1)
        {
            startingMoney = firstRoundStartMoney;
        }
        else
        {
            startingMoney = secondRoundStartMoney;
        }

        currentMoney = startingMoney;
        SetText();
        this.gameObject.PublishEvent(new OnMoneyChangedEvent());
        ++stage;
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
