using System.Threading.Tasks;
using UnityEngine;

namespace Characters
{
    public class MikuHatsune : BaseCharacter
    {
        protected override async Task DialogTree()
        {
            var choice = await Manager.DisplayChoice("hi im hatsune miku", "omg hi", "no");
            Debug.Log($"Choice: {choice}");
            if (choice == 1)
            {
                await Manager.DisplayText("wow frick you dude");
                await Manager.DisplayText("im gonna flip");
                gameObject.transform.Rotate(0, 180, 0);
                await Manager.DisplayText("go away");
            }
            else
            {
                await Manager.DisplayText("yes im miku hatsune");
            }
        }
    }
}