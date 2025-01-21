using KinematicCharacterController.Examples;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class BaseCharacter : MonoBehaviour, IInteractable
{
    protected VNManager Manager;
    protected ExamplePlayer Player;

    protected bool InUse = false;

    private GameObject hintLight;

    private void OnEnable()
    {
        // Find the VNManager and ExamplePlayer
        Manager = FindAnyObjectByType<VNManager>();
        Player = FindAnyObjectByType<ExamplePlayer>();

        GameObject prefab = Resources.Load<GameObject>("Prefabs/HintLight");

        hintLight = Instantiate(
            prefab,
            new Vector3(transform.position.x, 7, transform.position.z),
            Quaternion.Euler(90, 0, 0),
            transform
        );
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

    void Update()
    {
        var component = hintLight.GetComponent<Light>();
        var isEnabled = component.enabled;

        if (ShouldBeInteractable() && !isEnabled)
        {
            component.enabled = true;
        }

        if (!ShouldBeInteractable() && isEnabled)
        {
            component.enabled = false;
        }
    }
}