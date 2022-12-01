using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallChecks : MonoBehaviour
{
    [SerializeField] bool _isLeft;
    [SerializeField] WallRun _wallRun;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            _wallRun.WallCheckCount(_isLeft, true);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            _wallRun.WallCheckCount(_isLeft, false);
        }
    }
}
