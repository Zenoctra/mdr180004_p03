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

    bool _wallOnL;
   

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        _tempisOnWall = _isOnWall;
        
        if (!_playerMovement.ImportGrnd())
        {
            RaycastHit hit;

            if (Physics.Raycast(_playerCamera.gameObject.transform.position, _playerMovement.ImportDir() * 10, out hit, 50))
            {
                //Debug.Log("Something Detected in air");

                if (_wallChkL >= 2 || _wallChkR >= 2)
                {
                    if (_wallChkL != _wallChkR)
                    {
                        _isOnWall = true;
                        //Debug.Log("OnWall");
                        if (_wallChkL > _wallChkR)
                        {
                            _wallOnL = true;
                            
                        }
                        else _wallOnL = false;
                    }
                    else _isOnWall = false;
                    
                }
                else _isOnWall = false;
            }
            else _isOnWall = false;
        }

        if (_tempisOnWall != _isOnWall)
        {
            _playerMovement.SetIsWall(_isOnWall, _wallOnL);
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
