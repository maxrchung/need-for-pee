using System.Threading.Tasks;

namespace Characters
{
    public class BathroomGuy : BaseCharacter
    {
        protected override async Task DialogTree()
        {
            await Manager.DisplayText("bathroom is broke use other one");
        }
    }
}