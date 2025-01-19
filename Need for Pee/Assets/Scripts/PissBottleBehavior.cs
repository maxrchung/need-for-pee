using UnityEngine;

public class PissBottleBehavior : MonoBehaviour, IInteractable
{
    public MeshRenderer EmptyBottleMesh;
    public MeshRenderer FullBottleMesh;

    private bool _used = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Interact()
    {
        _used = true;
        Debug.Log($"Enabled 1: {EmptyBottleMesh.enabled}, 2: {FullBottleMesh.enabled}");
        EmptyBottleMesh.enabled = false;
        FullBottleMesh.enabled = true;
    }

    public bool CanInteract() => !_used;
}