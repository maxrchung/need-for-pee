using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemObject : BaseCharacter
{
    public GameFlag activateFlag = GameFlag.NullFlag;
    public GameFlag requiredFlag = GameFlag.NullFlag;
    public string text = "i am item";
    public string choiceYes = "take";

    protected override bool ShouldBeInteractable()
    {
        if (!gameObject.activeSelf) return false;
        return requiredFlag == GameFlag.NullFlag || FlagManager.Check(requiredFlag);
    }

    protected override async Task DialogTree()
    {
        var choice = await Manager.DisplayChoice(text, choiceYes, "no");
        if (choice == 1) return;
        FlagManager.Set(activateFlag);
        gameObject.SetActive(false);
    }
}