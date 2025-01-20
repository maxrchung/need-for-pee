using System.Threading.Tasks;

namespace Characters
{
    public class RoundOneGuy : BaseCharacter
    {
        protected override async Task DialogTree()
        {
            await Manager.DisplayText("hello i am round one guy");
            if (!FlagManager.Check(GameFlag.RoundOneClubCard))
            {
                await Manager.DisplayText("you dont have round one club card");
            }
            else
            {
                await Manager.DisplayText("thank you for have club card");
                await Manager.DisplayText("code is 1234");
                FlagManager.Set(GameFlag.HasCode1);
            }
        }
    }
}