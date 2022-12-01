using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    [SerializeField] PlayerMovement _playerMovement;
    [SerializeField] Camera _playerCamera;
    bool _isOnWall = false;
    bool _tempisOnWall;

    int _wallChkL = 0;
    int _wallChkR = 0;
   

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        
        if (!_playerMovement.ImportGrnd())
        {
            RaycastHit hit;

            if (Physics.Raycast(_playerCamera.gameObject.transform.position, _playerMovement.ImportDir() * 10, out hit, 10))
            {
                //Debug.Log("Something Detected in air");

                if (_wallChkL>=2 || _wallChkR >= 2)
                {
                    _isOnWall = true;
                    Debug.Log("OnWall");
                }
            }
        }
        

        Debug.DrawRay(_playerCamera.gameObject.transform.position, _playerMovement.ImportDir() * 10 , Color.red); //This ray will change length based on player speed, but it is representing a raycast that is always max length meaning 10 long
    }





    public void WallCheckCount(bool left, bool add)
    {
        if (add)
        {
            if (left)
            {
                _wallChkL++;
            }
            else _wallChkR++;
        }
        else 
        {
            if (left)
            {
                _wallChkL--;
            }
            else _wallChkR--;
        }
    }
}
