using System.Threading.Tasks;
using KinematicCharacterController.Examples;
using UnityEngine;

public class BaseCharacter : MonoBehaviour, IInteractable
{
    public VNManager Manager;
    public ExamplePlayer Player;

    private bool _inUse = false;

    private async void DialogTree()
    {
        var choice = await Manager.DisplayChoice("hi im hatsune miku", "omg hi", "no");
        Debug.Log($"Choice: {choice}");
        if (choice == 1)
        {
            await Manager.DisplayText("wow frick you dude");
            await Manager.DisplayText("im gonna flip");
            gameObject.transform.Rotate(0, 180, 0);
            await Manager.DisplayText("go away");
        }
        else
        {
            await Manager.DisplayText("yes im miku hatsune");
        }

        _inUse = false;
        Player.Enable();
    }

    public void Interact()
    {
        _inUse = true;
        Player.Disable();
        DialogTree();
    }

    public bool CanInteract() => !_inUse;
}