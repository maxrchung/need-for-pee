using System.Collections.Generic;
using System.Threading.Tasks;

namespace Characters
{
    public class TestChar : BaseCharacter
    {
        protected override bool ShouldBeInteractable() => !FlagManager.Check(GameFlag.KnowsCodeGiver);

        protected override async Task DialogTree()
        {
            var choices = new List<string>() { "omg hi", "no" ,"blah"};
            var choice = await Manager.DisplayChoice("hi im hatsune miku", choices.ToArray());
            switch (choice)
            {
                case 1:
                    await Manager.DisplayText("wow frick you dude");
                    await Manager.DisplayText("im gonna flip");
                    gameObject.transform.Rotate(0, 180, 0);
                    await Manager.DisplayText("go away");
                    break;
                case 0:
                    await Manager.DisplayText("yes im miku hatsune");
                    break;
                case 2:
                    await Manager.DisplayText("other");
                    break;
            }
        }
    }
}