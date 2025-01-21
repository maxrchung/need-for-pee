using System.Threading.Tasks;
using UnityEngine;

namespace Characters
{
    public class Businessman : BaseCharacter
    {
        public GameObject phone;
        
        private struct Strings
        {
            public static readonly string Greeting = "business";
        }

        private async Task SalesPitch()
        {
            await Manager.DisplayUnskippableText("ah, sweet business");
            await Manager.DisplayUnskippableText("i could hear the realized profits from here");
            await Manager.DisplayUnskippableText("say... with this capital ive got a business idea");
            await Manager.DisplayUnskippableText("you know whats an untapped");
            await Manager.DisplayUnskippableText("yet unrealized, market?");
            await Manager.DisplayUnskippableText("suckers who need to pee");
            await Manager.DisplayUnskippableText("they have no room to negotiate");
            await Manager.DisplayUnskippableText("you can sit them still and pull from their wallets");
            await Manager.DisplayUnskippableText("and rake in every last drop from them");
            await Manager.DisplayUnskippableText("if youve got the slightest of cash");
            await Manager.DisplayUnskippableText("it would be enough to yellow light this");
            await Manager.DisplayChoice("what do you say eh, lad?","need phone to call bank");
            await Manager.DisplayText("well why didn't you say so sooner?!");
            await Manager.DisplayText("best phone business can buy, lets have at it");
            await Manager.DisplayText("its right on the table over there");
            
            phone.SetActive(true);
            return;
        }

        private async Task EndgameTree()
        {
            var choice = -1;

            if (FlagManager.Check(GameFlag.BusinessDenied))
            {
                choice = await Manager.DisplayChoice("well lad, finally ready for business?","yes","no","maybe");
                switch(choice)
                {
                    case 0:
                        await Manager.DisplayUnskippableText("i knew youd come around");
                        await Manager.DisplayText("theres a business cat around these parts");
                        await Manager.DisplayText("tell them its time to sell the shares");
                        await Manager.DisplayChoice("i trust you can tell apart a business cat","business time"); 
                        FlagManager.Unset(GameFlag.BusinessDenied);
                        FlagManager.Set(GameFlag.RequestCatCommand);
                        return;
                    case 1:
                        await Manager.DisplayText("well then why are you speaking with me?!");
                        return;
                    case 2:
                        await Manager.DisplayText("for business sake, have some conviction lad!");
                        return;
                }
            }

            if (FlagManager.Check(GameFlag.RequestCatCommand))
            {
                if (FlagManager.Check(GameFlag.CatSoldShares))
                {
                    // Give the player the phone? or more conversation?
                    await SalesPitch();
                    return;
                }
                await Manager.DisplayText("get it together");
                await Manager.DisplayText("those shares wont sell themselves!");
                // Yell at the player idk
                return;
            }
            
            choice = await Manager.DisplayChoice("business", "business," , "do you have phone", "busyness");
            switch(choice)
            {
                case 0:
                    await Manager.DisplayUnskippableText("business indeed...");
                    return;
                case 1:
                    await Manager.DisplayUnskippableText("yes, but it is reserved for business");
                    return;
                case 2:
                    await Manager.DisplayUnskippableText("ah yes, busyness, the business of being busy");
                    await Manager.DisplayUnskippableText("is business truly worth being busy for?");
                    await Manager.DisplayUnskippableText("sometimes uncultured folk ask me that");
                    await Manager.DisplayUnskippableText("unscrupulous, wouldn't you say?");
                    await Manager.DisplayUnskippableText("you seem like the sort of fellow who");
                    await Manager.DisplayUnskippableText("understands the busyness of business.");
                    choice = await Manager.DisplayChoice("say... i have some business for you","yes","no","maybe");
                    if(choice == 1)
                    {
                        await Manager.DisplayUnskippableText("pity, i thought i had an eye for talent");
                        FlagManager.Set(GameFlag.BusinessDenied);
                        return;
                    }
                    if(choice == 2)
                    {
                        await Manager.DisplayUnskippableText("make up your mind lad!");
                        FlagManager.Set(GameFlag.BusinessDenied);
                        return;
                    }
                    await Manager.DisplayText("theres a business cat around these parts");
                    await Manager.DisplayText("tell them its time to sell the shares");
                    await Manager.DisplayChoice("i trust you can tell apart a business cat","business time"); 
                    break;
            }
            // Talk to business man about phone idk
            // Business man asks you to tell the cat to sell shares
            FlagManager.Set(GameFlag.RequestCatCommand);
        }

        protected override async Task DialogTree()
        {
            if (FlagManager.Check(GameFlag.PissGuyNumberFound))
            {
                await EndgameTree();
                return;
            }

            var choice = -1;

            if (FlagManager.Check(GameFlag.NeedCode))
            {
                choice = await Manager.DisplayChoice(Strings.Greeting, "ok", "what code");
                if (choice == 0) return;
                await Manager.DisplayChoice("sorry busy with business", "okay");
                return;
            }

            choice = await Manager.DisplayChoice(Strings.Greeting, "ok", "where bathroom");
            if (choice == 0) return;
            await Manager.DisplayText("business is good");
        }
    }
}