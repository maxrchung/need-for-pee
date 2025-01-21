using System.Collections.Generic;
using System.Threading.Tasks;

namespace Characters
{
    public class DeiGirl : BaseCharacter
    {
        private struct Strings
        {
            public static string Greeting => FlagManager.Check(GameFlag.IsStupid) ? StupidGreeting : DefaultGreeting;
            public static readonly string DefaultGreeting = "hi i am character";
            public static readonly string StupidGreeting = "you are stupid and smelly";
        }

        private async Task PreBathroomFound()
        {
            var choice = await Manager.DisplayChoice(Strings.Greeting, "where bathroom", "bye");
            if (choice == 1) return;
            if (FlagManager.Check(GameFlag.IsStupid))
            {
                await Manager.DisplayText("go away stupid");
                return;
            }

            FlagManager.Set(GameFlag.IsStupid);
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
            FlagManager.Unset(GameFlag.NeedCode);
            FlagManager.Set(GameFlag.HasCode);
        }

        private async Task KeyTree()
        {
            var choice = -1;
            if (FlagManager.Check(GameFlag.DiscoveredKeyWrong) && FlagManager.Check(GameFlag.HasKeyGirl))
            {
                choice = await Manager.DisplayChoice(Strings.Greeting, "key is wrong", "bye");
                if (choice == 1) return;
                await Manager.DisplayText("for what");
                await Manager.DisplayText("i gave you key");
                await Manager.DisplayChoice("give it back", "ok");
                SoundManager.PlaySound(SoundType.ITEM);
                await Manager.DisplayText("go bother someone else");
                FlagManager.Unset(GameFlag.HasKey);
                FlagManager.Unset(GameFlag.HasKeyGirl);
                FlagManager.Set(GameFlag.HadKeyGirl);
                FlagManager.Unset(GameFlag.DiscoveredKeyWrong);
                return;
            }

            choice = await Manager.DisplayChoice(Strings.Greeting, "give key", "bye");
            if (choice == 1) return;
            if (FlagManager.Check(GameFlag.HasKey))
            {
                await Manager.DisplayText("you have key already stupid" +
                                          (FlagManager.Check(GameFlag.DiscoveredKeyWrong) ? " go return it" : ""));
                return;
            }

            if (FlagManager.Check(GameFlag.GirlKeyQuest))
            {
                if (FlagManager.Check(GameFlag.HasShrek2))
                {
                    SoundManager.PlaySound(SoundType.ITEM);
                    await Manager.DisplayText("here is key");
                    FlagManager.Set(GameFlag.HasKey);
                    FlagManager.Set(GameFlag.HasKeyGirl);
                    FlagManager.Unset(GameFlag.GirlKeyQuest);
                    return;
                }

                await Manager.DisplayText("still no shrek 2 on dvd stupid");
            }

            await Manager.DisplayText("help and i will give key");
            await Manager.DisplayText("i want shrek 2 on dvd");
            FlagManager.Set(GameFlag.GirlKeyQuest);
        }

        protected override async Task DialogTree()
        {
            if (FlagManager.Check(GameFlag.PissGuyNumberFound))
            {
                var choice = await Manager.DisplayChoice(Strings.Greeting, "bye", "give phone");
                if (choice == 0) return;
                await Manager.DisplayText("no");
                return;
            }

            if (FlagManager.Check(GameFlag.CanPickUpKey) && !FlagManager.Check(GameFlag.HadKeyGirl))
            {
                await KeyTree();
                return;
            }

            if (FlagManager.Check(GameFlag.NeedCode))
            {
                await CodeTree();
                return;
            }

            if (FlagManager.Check(GameFlag.BathroomFound))
            {
                await Manager.DisplayChoice(Strings.Greeting, "bye");
                return;
            }

            await PreBathroomFound();
        }
    }
}