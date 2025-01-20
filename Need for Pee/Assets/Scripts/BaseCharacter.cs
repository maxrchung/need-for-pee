using System;
using System.Threading.Tasks;
using KinematicCharacterController.Examples;
using UnityEngine;

public class BaseCharacter : MonoBehaviour, IInteractable
{
    protected VNManager Manager;
    protected ExamplePlayer Player;

    protected bool InUse = false;

    private void OnEnable()
    {
        // Find the VNManager and ExamplePlayer
        Manager = FindAnyObjectByType<VNManager>();
        Player = FindAnyObjectByType<ExamplePlayer>();
    }

    protected virtual async Task DialogTree()
    {
        throw new NotImplementedException();
    }
    
    protected virtual bool ShouldBeInteractable() => true;

    private async void TreeWrapper()
    {
        InUse = true;
        Player.Disable();
        await DialogTree();
        InUse = false;
        Player.Enable();
    }

    public void Interact()
    {
        TreeWrapper();
    }

    public bool CanInteract() => !InUse && ShouldBeInteractable();
}