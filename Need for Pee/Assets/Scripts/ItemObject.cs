using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public GameFlag ActivateFlag = GameFlag.NullFlag;

    public void Interact()
    {
        FlagManager.Set(ActivateFlag);
        gameObject.SetActive(false);
    }

    public bool CanInteract() => gameObject.activeSelf;
}