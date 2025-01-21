using System.Threading.Tasks;

namespace Characters
{
    public class SecondDoor : BaseCharacter
    {
        private int _keyCount = 0;

        private async Task PissGuyTree()
        {
            await Manager.DisplayText("heyyy im pissin here");
            var choice = await Manager.DisplayChoice("wait your turn bucko", "ok", "please let me pee");
            if (choice == 0)
                return;
            await Manager.DisplayText("no");
            FlagManager.Set(GameFlag.PissGuyNumberFound);
            await Manager.DisplayText("unless my boss calls me at 650 555 2368");
            await Manager.DisplayText("and tells me im getting put on pip");
            await Manager.DisplayText("ill be here pissing till the sun dies");
        }

        protected override async Task DialogTree()
        {
            if (FlagManager.Check(GameFlag.SecondDoorUnlocked))
            {
                await PissGuyTree();
                return;
            }

            if (!FlagManager.Check(GameFlag.SecondDoorFound))
            {
                FlagManager.Set(GameFlag.SecondDoorFound);
                FlagManager.Set(GameFlag.CanPickUpKey);
            }

            if (FlagManager.Check(GameFlag.HasKey) && !FlagManager.Check(GameFlag.DiscoveredKeyWrong))
            {
                if (_keyCount == 2)
                {
                    await Manager.DisplayText("yippee key worked");
                    gameObject.transform.Rotate(30, 0, 0);
                    FlagManager.Unset(GameFlag.CanPickUpKey);
                    FlagManager.Set(GameFlag.SecondDoorUnlocked);
                    await PissGuyTree();
                    return;
                }

                await Manager.DisplayChoice("key is wrong", "what the frick");
                FlagManager.Set(GameFlag.DiscoveredKeyWrong);
                _keyCount++;
            }

            await Manager.DisplayChoice("oh no am locked need key", "dang it");
        }
    }
}