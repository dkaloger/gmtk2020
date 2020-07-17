using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Vine : MonoBehaviour
{
    [SerializeField]
    GameObject _segmentPrefab;

    [SerializeField]
    int _segments = 1;

    [SerializeField]
    float _segmentDistance = .21f;

    [SerializeField]
    bool _snapFirst, _snapLast;

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Spawn ()
	{
        int count = (int)(_segments / _segmentDistance);

        for (int i = 0; i < count; ++i)
		{
            GameObject temp = Instantiate(_segmentPrefab, new Vector3(transform.position.x, transform.position.y + _segmentDistance * (i + 1), transform.position.z), Quaternion.identity, transform);
            temp.transform.eulerAngles = new Vector3(180, 0, 0);
            temp.name = transform.childCount.ToString();

            if (i == 0)
			{
                Destroy(temp.GetComponent<CharacterJoint>());

                if(_snapFirst)
                    temp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			}
			else
			{
                temp.GetComponent<CharacterJoint>().connectedBody = transform.Find((transform.childCount - 1).ToString()).GetComponent<Rigidbody>();
			}
        }

        if(_snapLast)
            transform.Find(transform.childCount.ToString()).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
	}
}
