using System.Threading.Tasks;

namespace Characters
{
    public class Businessman : BaseCharacter
    {
        private struct Strings
        {
            public static readonly string Greeting = "business";
        }

        private async Task SalesPitch()
        {
            // yap at the player about business
            // phone will spawn here soontm
            return;
        }

        private async Task EndgameTree()
        {
            var choice = -1;

            if (FlagManager.Check(GameFlag.RequestCatCommand))
            {
                if (!FlagManager.Check(GameFlag.CatSoldShares))
                {
                    // Give the player the phone? or more conversation?
                    return;
                }
                // Yell at the player idk
                return;
            }
            
            choice = await Manager.DisplayChoice("business", "do you have phone", "business");
            if (choice == 1)
            {
                await Manager.DisplayText("i love business");
                return;
            }
            // Talk to business man about phone idk
            // Business man asks you to tell the cat to sell shares
            FlagManager.Set(GameFlag.RequestCatCommand);
        }

        protected override async Task DialogTree()
        {
            if (FlagManager.Check(GameFlag.PissGuyNumberFound))
            {
                await EndgameTree();
                return;
            }

            var choice = -1;

            if (FlagManager.Check(GameFlag.NeedCode))
            {
                choice = await Manager.DisplayChoice(Strings.Greeting, "ok", "what code");
                if (choice == 0) return;
                await Manager.DisplayChoice("sorry busy with business", "okay");
                return;
            }

            choice = await Manager.DisplayChoice(Strings.Greeting, "ok", "where bathroom");
            if (choice == 0) return;
            await Manager.DisplayText("business is good");
        }
    }
}