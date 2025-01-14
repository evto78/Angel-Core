using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxScript : MonoBehaviour
{
    public List<string> tagsToCollideWith;
    public List<GameObject> collidedObjects;
    bool colliding;
    void Start()
    {
        collidedObjects.Clear();

        colliding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(collidedObjects.Count > 0)
        {
            colliding = true;
        }
        else
        {
            colliding = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (tagsToCollideWith.Contains(other.gameObject.tag))
        {
            collidedObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (tagsToCollideWith.Contains(other.gameObject.tag))
        {
            collidedObjects.Remove(other.gameObject);
        }
    }
}
