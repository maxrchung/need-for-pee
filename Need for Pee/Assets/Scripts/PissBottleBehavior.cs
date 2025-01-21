using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class PissBottleBehavior : BaseCharacter
{
    public MeshRenderer EmptyBottleMesh;
    public MeshRenderer FullBottleMesh;
    PissBarScript pissBar;
    private bool _used = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pissBar = FindAnyObjectByType<PissBarScript>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    protected override async Task DialogTree()
    {
        var choices0 = new List<string>() {"yes...","not yet"};

        await Manager.DisplayUnskippableText("an empty gatorade bottle...");
        var choice0 = await Manager.DisplayChoice("is this what your life has come to?", choices0.ToArray());
        switch (choice0)
        {
            case 0:
                int pissTime = (int)(pissBar.BeginEjectingPiss() * 0.1f) * 12;
                string pissString = "";
                for(int i = 0; i < pissTime; i++)
                {
                    pissString += ".";
                }
                SoundManager.PlaySound(SoundType.PEE);
                await Manager.DisplayUnskippableText(pissString);
                SoundManager.StopSound();
                _used = true;
                EmptyBottleMesh.enabled = false;
                FullBottleMesh.enabled = true;
                await Manager.DisplayText("although momentarily, the urge is abated");
                break;
            case 1:
                break;
        }
    }
    protected override bool ShouldBeInteractable() => !_used;
}