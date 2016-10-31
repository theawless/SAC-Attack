using UnityEngine;
using System.Collections;

public class PickableObjectSpawner : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {

    }

    void ColliderGenerator(Transform transform)
    {
        var colliderObject = new GameObject();
        var sphereCollider = colliderObject.AddComponent<SphereCollider>();
        sphereCollider.radius = 0.2f;
        colliderObject.tag = TagsTypeString.Pickable.ToString();
        sphereCollider.isTrigger = true;
        //colliderObject = Instantiate(colliderObject);
        colliderObject.transform.parent = transform;
        colliderObject.transform.localPosition = new Vector3(0, 0, 0);
    }
}
