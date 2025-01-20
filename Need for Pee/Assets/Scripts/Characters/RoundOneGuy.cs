using System.Threading.Tasks;

namespace Characters
{
    public class RoundOneGuy : BaseCharacter
    {
        protected override bool ShouldBeInteractable() => !FlagManager.Check(GameFlag.KnowsCodeGiver);

        protected override async Task DialogTree()
        {
            await Manager.DisplayText("hello i am round one guy");
            if (!FlagManager.Check(GameFlag.RoundOneClubCard))
            {
                await Manager.DisplayText("you dont have round one club card");
                await Manager.DisplayText("i will not speak to you");
                await Manager.DisplayText("come back when have");
                FlagManager.Set(GameFlag.ClubCardKnown);
            }
            else
            {
                await Manager.DisplayChoice("thank you for have club card", "what is code");
                await Manager.DisplayText("dont know");
                await Manager.DisplayText("ask girl");
                FlagManager.Set(GameFlag.KnowsCodeGiver);
            }
        }
    }
}