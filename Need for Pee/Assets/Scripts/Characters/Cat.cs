using System.Collections.Generic;
using System.Threading.Tasks;

namespace Characters
{
    public class Cat : BaseCharacter
    {
        protected override async Task DialogTree()
        {
            if (FlagManager.Check(GameFlag.CatRequestedCake) && !FlagManager.Check(GameFlag.CatSoldShares))
            {
                if (!FlagManager.Check(GameFlag.CakeObtained))
                {
                    await Manager.DisplayText("no cake");
                    return;
                }

                await Manager.DisplayChoice("meow", "here is cake");
                await Manager.DisplayText("meowwwww");
                await Manager.DisplayText("done with business");
                FlagManager.Set(GameFlag.CatSoldShares);
            }

            var choices = new List<string>() { "meow", "meoww" };
            if (FlagManager.Check(GameFlag.RequestCatCommand) && !FlagManager.Check(GameFlag.CatSoldShares))
            {
                choices.Add("do business please");
            }

            var choice = await Manager.DisplayChoice("meow", choices.ToArray());
            if (choice != 2)
            {
                await Manager.DisplayText("meowwwww");
                return;
            }

            await Manager.DisplayText("meow give cake");
            FlagManager.Set(GameFlag.CatRequestedCake);
        }
    }
}