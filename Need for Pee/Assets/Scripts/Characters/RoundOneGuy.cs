using System.Threading.Tasks;

namespace Characters
{
    public class RoundOneGuy : BaseCharacter
    {
        private struct Strings
        {
            public static string Greeting => "hello i am round one guy";
            public static string AskBathroom => "where bathroom";
            public static string AskCode => "what is code";
        }


        protected override bool ShouldBeInteractable() => true;

        private async Task PreFoundDialogue()
        {
            var choice = await Manager.DisplayChoice(Strings.Greeting, Strings.AskBathroom, "bye");
            if (choice == 1) return;
            await Manager.DisplayText("bathroom is in back");
        }

        private async Task PostFoundDialogue()
        {
            if (FlagManager.Check(GameFlag.NeedClubCard))
            {
                await Manager.DisplayText("you still dont have round one club card");
                return;
            }

            var choice = await Manager.DisplayChoice(Strings.Greeting, Strings.AskCode, "bye");
            if (choice == 1) return;
            if (!FlagManager.Check(GameFlag.HasClubCard))
            {
                await Manager.DisplayText("you dont have round one club card");
                choice = await Manager.DisplayChoice("go find one", "ok", "no frick you");
                FlagManager.Set(GameFlag.NeedClubCard);
                if (choice == 0) return;
                await Manager.DisplayText("frick you too buddy");
            }
        }

        private async Task PostClubCardDialogue()
        {
            if (FlagManager.Check(GameFlag.KnowsCodeGiver))
            {
                await Manager.DisplayText("ur smelly go away");
                return;
            }

            var choice = await Manager.DisplayChoice(Strings.Greeting, Strings.AskCode, "bye");
            if (choice == 1) return;
            await Manager.DisplayText("dont know");
            await Manager.DisplayChoice("ask girl", "ok");
            FlagManager.Set(GameFlag.KnowsCodeGiver);
        }

        private async Task KeyDialogue()
        {
            var choice = -1;
            if (FlagManager.Check(GameFlag.DiscoveredKeyWrong) && FlagManager.Check(GameFlag.HasKeyRound1Guy))
            {
                await Manager.DisplayChoice(Strings.Greeting, "key wrong");
                await Manager.DisplayText("not my problem");
                await Manager.DisplayChoice("ill take it back tho", "ok");
                SoundManager.PlaySound(SoundType.ITEM);
                FlagManager.Unset(GameFlag.HasKey);
                FlagManager.Unset(GameFlag.HasKeyRound1Guy);
                FlagManager.Set(GameFlag.HadKeyRound1Guy);
                FlagManager.Unset(GameFlag.DiscoveredKeyWrong);
                return;
            }

            choice = await Manager.DisplayChoice(Strings.Greeting, "do you have key", "nevermind");
            if (choice == 1) return;
            if (FlagManager.Check(GameFlag.HasKey))
            {
                await Manager.DisplayText("you have key already" +
                                          (FlagManager.Check(GameFlag.DiscoveredKeyWrong) ? " go return it" : ""));
                return;
            }

            await Manager.DisplayChoice("you need round1 club card", "i have", "what the frick");
            await Manager.DisplayText("just kidding...");
            SoundManager.PlaySound(SoundType.ITEM);
            await Manager.DisplayChoice("here u go", "thank u", "frick u");
            FlagManager.Set(GameFlag.HasKey);
            FlagManager.Set(GameFlag.HasKeyRound1Guy);
        }

        protected override async Task DialogTree()
        {
            if (FlagManager.Check(GameFlag.PissGuyNumberFound))
            {
                await Manager.DisplayChoice(Strings.Greeting, "do you have phone");
                await Manager.DisplayText("no");
                return;
            }

            if (FlagManager.Check(GameFlag.CanPickUpKey) && !FlagManager.Check(GameFlag.HadKeyRound1Guy))
            {
                await KeyDialogue();
                return;
            }

            if (FlagManager.Check(GameFlag.NeedCode))
            {
                if (FlagManager.CheckAll(GameFlag.NeedClubCard, GameFlag.HasClubCard))
                {
                    await PostClubCardDialogue();
                    return;
                }

                await PostFoundDialogue();
                return;
            }

            if (FlagManager.Check(GameFlag.BathroomFound))
            {
                await Manager.DisplayText(Strings.Greeting);
                return;
            }

            // If no flags are set, we're in the intro
            await PreFoundDialogue();
            return;
        }
    }
}