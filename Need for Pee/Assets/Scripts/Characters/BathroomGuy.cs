using System.Threading.Tasks;

namespace Characters
{
    public class BathroomGuy : BaseCharacter
    {
        private struct Strings
        {
            public static readonly string Greeting = "bathroom broke use other";
        }

        protected async Task KeyTree()
        {
            var choice = -1;
            if (FlagManager.Check(GameFlag.DiscoveredKeyWrong) && FlagManager.Check(GameFlag.HasKeyBathroomGuy))
            {
                await Manager.DisplayChoice(Strings.Greeting, "key wrong");
                await Manager.DisplayText("oh sorgy");
                await Manager.DisplayChoice("give key back", "ok");
                SoundManager.PlaySound(SoundType.ITEM);
                await Manager.DisplayText("someone else probably has key");
                FlagManager.Unset(GameFlag.HasKey);
                FlagManager.Unset(GameFlag.HasKeyBathroomGuy);
                FlagManager.Set(GameFlag.HadKeyBathroomGuy);
                FlagManager.Unset(GameFlag.DiscoveredKeyWrong);
                return;
            }

            choice = await Manager.DisplayChoice(Strings.Greeting, "give key", "no");
            if (choice == 1) return;
            if (FlagManager.Check(GameFlag.HasKey))
            {
                await Manager.DisplayText("you have key already" + (FlagManager.Check(GameFlag.DiscoveredKeyWrong) ? " go return it" :""));
                return;
            }

            SoundManager.PlaySound(SoundType.ITEM);
            await Manager.DisplayChoice("here u go", "thank u", "frick u");
            FlagManager.Set(GameFlag.HasKey);
            FlagManager.Set(GameFlag.HasKeyBathroomGuy);
        }

        protected override async Task DialogTree()
        {
            if (FlagManager.Check(GameFlag.CanPickUpKey) && !FlagManager.Check(GameFlag.HadKeyBathroomGuy))
            {
                await KeyTree();
                return;
            }

            await Manager.DisplayText("bathroom is broke use other one");
        }
    }
}