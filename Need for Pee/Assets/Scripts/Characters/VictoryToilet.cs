using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class VictoryToilet : BaseCharacter
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    protected override async Task DialogTree()
    {
        var choice = -1;
        if (FlagManager.Check(GameFlag.PeeShy))
        {
            await Manager.DisplayText("well ello ello");
            choice = await Manager.DisplaySlowChoice("back to ave a piss then?", "yeah...", "mmm......");
            switch (choice)
            {
                case 0:
                    await Manager.DisplayUnskippableText("well dont hold back then");
                    await GG();
                    return;
                case 1:
                    await Manager.DisplayText("well ill be ere all night");
                    return;
            }
        }

        await Manager.DisplayText("well ello ello, lookin to ave a piss?");
        choice = await Manager.DisplaySlowChoice("got a nice place for that roight ere", "ok sure", "erm...");
        switch (choice)
        {
            case 0:
                await Manager.DisplayText("well ave at it then");
                await GG();
                return;
            case 1:
                await Manager.DisplayText("roight then");
                await Manager.DisplayText("must not be that urgent i suppose");
                FlagManager.Set(GameFlag.PeeShy);
                return;
        }
    }

    async Task GG()
    {
        Manager.ClearButtons();
        Manager.ClearText();
        Player.Disable();
        await Manager.DisplayUnskippableText("you peed!!!");
        await Task.Delay(Timeout.Infinite);
    }
}