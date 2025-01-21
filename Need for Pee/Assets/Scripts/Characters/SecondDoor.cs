using System.Threading.Tasks;

namespace Characters
{
    public class SecondDoor : BaseCharacter
    {
        private int _keyCount = 0;

        protected override async Task DialogTree()
        {
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