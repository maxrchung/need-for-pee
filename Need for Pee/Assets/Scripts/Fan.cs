using UnityEngine;

public class Fan : MonoBehaviour
{
    public float RotationAmount = 100f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, RotationAmount * Time.deltaTime, 0));
    }
}
