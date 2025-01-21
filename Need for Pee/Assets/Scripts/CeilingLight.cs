using System.Collections;
using UnityEngine;

public class CeilingLight : MonoBehaviour
{

    public float DelayMinTimeInSex = 2.0f;
    public float DelayMaxTimeInSex = 30.0f;
    public float FlickerTimeInSex = 0.1f;


    private Light[] lights;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lights = GetComponentsInChildren<Light>();
        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            var delay = Random.Range(DelayMinTimeInSex, DelayMaxTimeInSex);
            yield return new WaitForSeconds(delay);

            var index = Random.Range(-1, lights.Length);

            if (index == -1)
            {
                foreach (var light in lights)
                {
                    light.enabled = false;
                }
            }
            else
            {
                lights[index].enabled = false;
            }

            yield return new WaitForSeconds(FlickerTimeInSex);

            if (index == -1)
            {
                foreach (var light in lights)
                {
                    light.enabled = true;
                }
            }
            else
            {
                lights[index].enabled = true;
            }
        }

    }
}
