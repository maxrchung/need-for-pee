using System.Collections.Generic;
using System.Threading.Tasks;

namespace Characters
{
    public class Cat : BaseCharacter
    {
        public bool isBusinessCat = false;

        protected async Task NonBusinessCat()
        {
            await Manager.DisplayUnskippableText("meow....");
            await Manager.DisplayUnskippableText("meow!");
            await Manager.DisplaySlowChoice("meowww", "do business already", "what the frick");
            await Manager.DisplayUnskippableText("meoooowww");
            var choice = -1;
            while (choice != 1)
                choice = await Manager.DisplaySlowChoice("meow", "meow", "wait youre not a business cat");
            FlagManager.Set(GameFlag.TrickedByCat);
        }

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

            if (!isBusinessCat)
            {
                await NonBusinessCat();
                return;
            }

            await Manager.DisplayText("meow give cake");
            FlagManager.Set(GameFlag.CatRequestedCake);
        }
    }
}