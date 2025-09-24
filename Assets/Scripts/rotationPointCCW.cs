using System.Collections.Generic;
using UnityEngine;

public class rotationCCW : MonoBehaviour
{
    public GameObject rotationPoint;
    public GameObject prefab;
    private float roatationSpeed = 30f;
    private float time = 1f;
    private float amount = 12f;
    List<GameObject> friends = new List<GameObject>();

    private void Start()
    {
        InvokeRepeating(nameof(flipx), 0, .5f);
    }
    void Update()
    {
        gameObject.transform.Rotate(0, 0, roatationSpeed * Time.deltaTime, Space.World);
        time -= Time.deltaTime;
        if (time <= 0f && amount >= 0)
        {
            GameObject skeleton = Instantiate(prefab, transform.position + new Vector3(0, 3f, 0), Quaternion.identity);
            skeleton.transform.SetParent(rotationPoint.transform);
            friends.Add(skeleton);
            time = 1f;
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
            friend.transform.rotation = Quaternion.identity;
        }
    }
}
