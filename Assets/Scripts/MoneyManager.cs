using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour {

    public static MoneyManager Instance;

    public Text moneyText;
    public int defaultStartingMoney;
    [SerializeField]
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
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        SetText();
    }

    public void RemoveMoney(int amount)
    {
        currentMoney -= amount;
        SetText();
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
