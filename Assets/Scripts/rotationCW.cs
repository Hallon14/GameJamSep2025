using System.Collections.Generic;
using UnityEngine;

public class rotationCW : MonoBehaviour
{
    public GameObject rotationPoint;
    public GameObject prefab;
    private float roatationSpeed = 30f;
    private float time = 2f;
    private float amount = 6f;
    List<GameObject> friends = new List<GameObject>();

    private void Start()
    {
        InvokeRepeating(nameof(flipx), .5f, .5f);
    }

    void Update()
    {
        gameObject.transform.Rotate(0, 0, -roatationSpeed * Time.deltaTime, Space.World);
        time -= Time.deltaTime;
        if (time <= 0f && amount >= 0)
        {
            GameObject skeleton = Instantiate(prefab, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
            skeleton.transform.SetParent(rotationPoint.transform);
            friends.Add(skeleton);
            time = 2f;
            amount--;
        }
        foreach (GameObject friend in friends)
        {
            friend.transform.rotation = Quaternion.identity;
        }
    }

    void flipx()
    {
        foreach (GameObject friend in friends)
        {
            SpriteRenderer sr = friend.GetComponent<SpriteRenderer>();
            sr.flipX = !sr.flipX;
        }
    }
}
