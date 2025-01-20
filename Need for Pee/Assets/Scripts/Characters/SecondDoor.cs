using System.Threading.Tasks;

namespace Characters
{
    public class SecondDoor : BaseCharacter
    {
        protected override async Task DialogTree()
        {
            FlagManager.Set(GameFlag.SecondDoorFound);
            await Manager.DisplayChoice("oh no am locked need key", "dang it");
        }
    }
}