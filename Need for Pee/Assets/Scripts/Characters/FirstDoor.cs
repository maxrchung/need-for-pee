using System.Threading.Tasks;

namespace Characters
{
    public class FirstDoor : BaseCharacter
    {
        private struct Strings
        {
            public static readonly string Greeting = "oh no am locked need code";
        }

        protected override async Task DialogTree()
        {
            if (FlagManager.Check(GameFlag.HasCode))
            {
                await Manager.DisplayChoice(Strings.Greeting, "miku", "miku", "miku");
                gameObject.SetActive(false);
                SoundManager.PlaySound(SoundType.DOOR_OPEN);
                return;
            }

            await Manager.DisplayText(Strings.Greeting);
            FlagManager.Set(GameFlag.BathroomFound);
            FlagManager.Set(GameFlag.NeedCode);
        }
    }
}