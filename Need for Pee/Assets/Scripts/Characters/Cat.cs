using System.Threading.Tasks;

namespace Characters
{
    public class Cat : BaseCharacter
    {
        protected override async Task DialogTree()
        {
            await Manager.DisplayChoice("meow", "meow", "meoww");
            await Manager.DisplayText("meowwwww");
        }
    }
}