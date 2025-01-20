using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour
{
    public VNManager vn;
    public TextManagerScript text;
    public int startText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startText = text.PissTextGeneration("NEED2PEE", new Vector2(Screen.width*0.15f,Screen.height*0.35f),0.35f,true);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            text.ClearText(startText);
            menu();
        }
    }

    async void menu()
    {
        await DialogTree();
        menu();
    }


    async Task DialogTree()
        {
            var choices0 = new List<string>() { "so whats the deal"};
            var choices1 = new List<string>() { "yes","no"};
            var choices2 = new List<string>() {"lets pee"};
            var choice0 = await vn.DisplayChoice("Welcome to need2pee", choices0.ToArray());
            switch (choice0)
            {
                case 0:
                    await vn.DisplayText("need for pee so you entered store");
                    await vn.DisplayText("find pee place please");
                    await vn.DisplayText("WASD and Mouse to move");
                    await vn.DisplayText("p to interact when prompted");
                    var choice1 = await vn.DisplayChoice("get it?", choices1.ToArray());
                    switch (choice1)
                    {
                        case 0:
                            var choice2 = await vn.DisplayChoice("OK, last chance to adjust volume", choices2.ToArray());
                            switch (choice2)
                            {
                                case 0:
                                    SceneManager.LoadScene("Level");
                                    break;
                            }
                            break;
                        case 1:
                            break;
                    }
                    break;
            }
        }
}
