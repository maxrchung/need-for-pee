using System.Threading.Tasks;

namespace Characters
{
    public class Phone : BaseCharacter
    {
        protected override async Task DialogTree()
        {
            await Manager.DisplayChoice("...yes i am phone...", "dial 650 555 2368");
            await Manager.DisplayUnskippableText("ring... ring... ring... ring... ring...");
            var choice =
                await Manager.DisplayChoice("hello? pissin here", "hello this is boss", "hello this is peeter");
            if (choice == 1)
            {
                await Manager.DisplayText("im pissin here go away");
                await Manager.DisplayText("beep");
                return;
            }

            choice = await Manager.DisplayChoice("hello boss what is going on im pissin here",
                "you are getting put on pip", "you are getting a raise", "your wife is dead");
            if (choice != 0)
            {
                await Manager.DisplayText("ok i kinda dont care tho im pissin here");
                await Manager.DisplayText("beep");
                return;
            }

            await Manager.DisplayText("wait no dont put me on pip im pissin here");
            await Manager.DisplayText("im coming to work i promise i wont be late im pissin here");
            await Manager.DisplayText("beep");
            FlagManager.Set(GameFlag.PissGuyLeft);
            gameObject.SetActive(false);
        }
    }
}