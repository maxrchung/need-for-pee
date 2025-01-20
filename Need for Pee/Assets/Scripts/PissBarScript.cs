using System.Collections.Generic;
using KinematicCharacterController.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PissBarScript : MonoBehaviour
{
    public ExamplePlayer player;
    public VNManager vn;
    public Texture2D[] pissBar;
    Texture2D currentPissBar;
    public Texture2D pissContainer;
    public float maxPiss;//seconds
    float currentPiss;
    float x = 0f;
    float y = 0f;
    float width = 1f;
    float height = 1f;
    public float maxSwitchTime;
    public float minSwitchTime;
    float currentSwitchTime;
    float switchTime = 0f;
    float initialWidth,initialHeight,scaledWidth,scaledHeight;
    bool gg = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialWidth = pissBar[0].width;
        initialHeight = pissBar[0].height;
        scaledWidth = initialWidth/5;
        scaledHeight = initialHeight/10;
        currentPiss = 0;
        currentPissBar = pissBar[0];
        player = FindAnyObjectByType<ExamplePlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!gg)
        {
            if(currentPiss < maxPiss)
            {
                currentPiss += Time.deltaTime;
            }
            switchTime += Time.deltaTime;
            currentSwitchTime = (1-(currentPiss/maxPiss)) * (maxSwitchTime-minSwitchTime) + minSwitchTime;
            if(switchTime > currentSwitchTime)
            {
                switchTime = 0;
                if(currentPissBar == pissBar[0])
                {
                    currentPissBar = pissBar[1];
                }
                else
                {
                    currentPissBar = pissBar[0];
                }
            }
            if(currentPiss >= maxPiss)
            {
                gg = true;
                GameOver();
            }
        }
    }

    public void EjectPiss(float amount)
    {
        currentPiss -= amount;
    }

    public float GetPiss()
    {
        return currentPiss/maxPiss;
    }

    void OnGUI()
    {
        Rect position = new Rect(10f,30f,scaledWidth*(currentPiss/maxPiss),scaledHeight);
        Rect coords = new Rect(x,y,width*(currentPiss/maxPiss),height);
        GUI.DrawTextureWithTexCoords(position,currentPissBar,coords);
        GUI.DrawTextureWithTexCoords(new Rect(10f,30f,scaledWidth,scaledHeight),pissContainer,
        new Rect(x,y,width,height));
    }

    async void GameOver()
    {
        vn.ClearButtons();
        vn.ClearText();
        player.Disable();
        var choices0 = new List<string>() {"pee again...","pee again...","pee again..."};
        var choice0 = await vn.DisplaySlowChoice("you peed...",choices0.ToArray());
        SceneManager.LoadScene("Level");
    }
}
