using System.Threading.Tasks;

namespace Characters
{
    public class Businessman : BaseCharacter
    {
        private struct Strings
        {
            public static readonly string Greeting = "business";
        }

        protected override async Task DialogTree()
        {
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