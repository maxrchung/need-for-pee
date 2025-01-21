using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Characters
{
    public class MikuHatsune : BaseCharacter
    {
        protected override async Task DialogTree()
        {
            var choices = new List<string>() { "omg hi", "no" };
            if (FlagManager.Check(GameFlag.NeedCode))
            {
                choices.Add("what is code");
            }

            var choice = await Manager.DisplayChoice("hi im hatsune miku", choices.ToArray());
            switch (choice)
            {
                case 1:
                    await Manager.DisplayText("wow frick you dude");
                    await Manager.DisplayText("im gonna flip");
                    gameObject.transform.Rotate(0, 180, 0);
                    SoundManager.PlaySound(SoundType.HATSUNE_MIKU);
                    await Manager.DisplayUnskippableText("seeeeeekaaaaaaaaaai de");
                    break;
                case 0:
                    await Manager.DisplayText("yes im miku hatsune");
                    break;
                case 2:
                    if (FlagManager.Check(GameFlag.KnowsCodeGiver))
                        await Manager.DisplayText("wrong girl ur sexist");
                    else
                        await Manager.DisplayText("dont know");
                    break;
            }
        }
    }
}