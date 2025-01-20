using System.Collections.Generic;
using System.Threading.Tasks;

namespace Characters
{
    public class DeiGirl : BaseCharacter
    {
        private struct Strings
        {
            public static string Greeting = "hi i am dei girl";
        }

        private async Task PreBathroomFound()
        {
            var choice = await Manager.DisplayChoice(Strings.Greeting, "where bathroom", "bye");
            if (choice == 1) return;
            await Manager.DisplayChoice("are you stupid", "yes", "yes");
        }

        private async Task CodeTree()
        {
            var choice = await Manager.DisplayChoice(Strings.Greeting, "what is code", "bye");
            if (choice == 1) return;
            var choices = new List<string>() { "ok" };
            if (FlagManager.Check(GameFlag.KnowsCodeGiver)) choices.Add("liar you know");
            choice = await Manager.DisplayChoice("dont know", choices.ToArray());
            if (choice == 0) return;
            await Manager.DisplayText("fine ok code is miku");
            FlagManager.Set(GameFlag.HasCode);
        }

        protected override async Task DialogTree()
        {
            if (FlagManager.Check(GameFlag.BathroomFound))
            {
                await CodeTree();
                return;
            }

            await PreBathroomFound();
        }
    }
}