using System.Threading.Tasks;

namespace Characters
{
    public class FirstDoor : BaseCharacter
    {
        protected override async Task DialogTree()
        {
            if (FlagManager.Check(GameFlag.HasCode))
            {
                await Manager.DisplayChoice("what is code", "miku", "miku", "miku");
                gameObject.SetActive(false);
                return;
            }

            await Manager.DisplayText("oh no am locked");
            FlagManager.Set(GameFlag.BathroomFound);
        }
    }
}