using UnityEngine;

public class rotationTitleScreen : MonoBehaviour
{
    private int rotationspeed = 90;
    void Update()
    {
        gameObject.transform.Rotate(0, 0, rotationspeed * Time.deltaTime, Space.World);
        
    }
}
