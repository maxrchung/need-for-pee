using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public GameFlag ActivateFlag = GameFlag.NullFlag;
    public GameFlag RequiredFlag = GameFlag.NullFlag;

    public void Interact()
    {
        FlagManager.Set(ActivateFlag);
        gameObject.SetActive(false);
    }

    public bool CanInteract()
    {
        if (!gameObject.activeSelf) return false;
        return RequiredFlag == GameFlag.NullFlag || FlagManager.Check(RequiredFlag);
    }
}