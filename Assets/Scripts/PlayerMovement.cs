using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMovement : MonoBehaviour
{

    [SerializeField] CharacterController _controller;

    [SerializeField] float _speed = .1f;

    [SerializeField] Transform _groundCheck;
    [SerializeField] float _groundDistance = .4f;
    [SerializeField] LayerMask _groundMask;
    private bool _isGrounded;
    bool _tempIsGrounded;

    Vector3 _velocity;

    [SerializeField] float _gravity = -9.81f;
    [SerializeField] float _jumpHeight = 3f;

    bool _isSprinting = false;
    float _speedHold;
    [SerializeField] float _sprintModifier = 2;


    float x;        //These are the basis building blocks of character movement and momentum direction
    float z;
    float _power;   //Hypotenuse of triangle made by custom axis returns - it is equivalent to the speed the character is moving
    float _horizontalAxis = 0;
    float _verticalAxis = 0;
    float _limitedHorizontalAxis = 0;
    float _limitedVerticalAxis = 0;


    [SerializeField] float _accelerationModifier = 1; // These are modifiers to limit control the player has over direction at any given time (Momentum modifiers)
    [SerializeField] float _dragModifier = 1;
    [SerializeField] float _inAirModifier = .1f;


    bool _aPressed = false; //These are for tracking button inputs for the purposes of a custom axis system
    bool _dPressed = false;
    bool _wPressed = false;
    bool _sPressed = false;


    float _holdRotation = 400;    //These are specific to the rotation correction system
    float _jumpRotation;          //this variable is stupid but necessary lol
    float _deltaRotation;
    float _previousRotation;
    float _additiveDeltaRotation = 0;
    float _deltaTheta;
    float _initialTheta;



    float _customTheta;   //These are variables specific to the Custom Axis Limter system of methods
    int _quadrant;
    float _maxZ;
    float _maxX;

    // Update is called once per frame
    void Update()
    {
        

        

        if (_isGrounded && _velocity.y < 0)
        {
            VelocityReset();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Sprinting(true, _speed);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Sprinting(false, _speed);
        }




        

        if(Input.GetButtonDown("Jump") && _isGrounded)
        {
            if (_holdRotation == 400) 
                {
                    _holdRotation = this.transform.rotation.eulerAngles.y;
                    _jumpRotation = _holdRotation;
                _previousRotation = _holdRotation;
                    //Debug.Log(_holdRotation);
                }

            _initialTheta = ThetaCheck();






            //Debug.Log(_initialTheta);
            JumpPressed();
            

        }

        

    }

    private void LateUpdate()
    {
        
        CustomAxis();
        
    }


    private void FixedUpdate()
    {
        //CustomAxis();
        SetPlayerPosition();
        SetVelocity();
        _tempIsGrounded = _isGrounded;
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);

        if(_tempIsGrounded != _isGrounded)
        {
            //Debug.Log("In-air Status has changed");
            if(_isGrounded)                                 //this is a marker for the moment the player lands on the ground
            {
                _horizontalAxis = _limitedHorizontalAxis;
                _verticalAxis = _limitedVerticalAxis;

            }
            if (!_isGrounded)
            {
                if (_holdRotation == 400)
                {
                    _holdRotation = this.transform.rotation.eulerAngles.y;
                    _jumpRotation = _holdRotation;
                    _previousRotation = _holdRotation;
                    //Debug.Log(_holdRotation);
                }

                _initialTheta = ThetaCheck();
            }
        }

    }

    

    private void VelocityReset() 
    {
        _velocity.y = -2f;
    }

    private void SetPlayerPosition()
    {
        
        
        x = _limitedHorizontalAxis;
        z = _limitedVerticalAxis;



        Vector3 move = transform.right * x + transform.forward * z;

        _controller.Move(move * _speed);
    }



    private void JumpPressed()
    {
        _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
    }

    private void SetVelocity()
    {
        _velocity.y += _gravity * Time.fixedDeltaTime;

        _controller.Move(_velocity * Time.fixedDeltaTime);
    }


    private void Sprinting(bool isSprinting, float speedHold)
    {
        _isSprinting = isSprinting; 
        if (_isSprinting == true)
        {
            _speedHold = speedHold;
            _speed *= _sprintModifier;
        }else { _speed = _speedHold; }
    }








    private void CustomAxis() //GetAxis wasn't giving me enough control to accurately control my momentum so this method will take it's place
    {







        float inAirModifier;
        if (_isGrounded == false)
        {
            inAirModifier = _inAirModifier;
        }else { inAirModifier = 1; }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            _aPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            _aPressed = false;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            _dPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            _dPressed = false;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _sPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            _sPressed = false;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            _wPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            _wPressed = false;
        }




        if (_aPressed && _horizontalAxis > -1)
        {
            _horizontalAxis -= 1 * Time.deltaTime * _accelerationModifier * inAirModifier;
            if (_horizontalAxis < -1)
            {
                _horizontalAxis = -1;
            }
            //Debug.Log("HorizontalAxis =" + _horizontalAxis);
        }

        if (!_aPressed && _horizontalAxis<0 && !_dPressed && _isGrounded)
        {
            _horizontalAxis += 1 * Time.deltaTime * _dragModifier * inAirModifier;
            if (_horizontalAxis > 0)
            {
                _horizontalAxis = 0;
            }
            //Debug.Log("HorizontalAxis =" + _horizontalAxis);
        }

        if (_dPressed && _horizontalAxis < 1)
        {
            _horizontalAxis += 1 * Time.deltaTime * _accelerationModifier * inAirModifier;
            if (_horizontalAxis > 1)
            {
                _horizontalAxis = 1;
            }
            //Debug.Log("HorizontalAxis =" + _horizontalAxis);
        }

        if (!_dPressed && _horizontalAxis > 0 && !_aPressed && _isGrounded)
        {
            _horizontalAxis -= 1 * Time.deltaTime * _dragModifier * inAirModifier;
            if (_horizontalAxis < 0)
            {
                _horizontalAxis = 0;
            }
            //Debug.Log("HorizontalAxis =" + _horizontalAxis);
        }

        if (_sPressed && _verticalAxis > -1)
        {
            _verticalAxis -= 1 * Time.deltaTime * _accelerationModifier * inAirModifier;
            if (_verticalAxis < -1)
            {
                _verticalAxis = -1;
            }
            //Debug.Log("VerticalAxis =" + _verticalAxis);
        }

        if (!_sPressed && _verticalAxis < 0 && !_wPressed && _isGrounded)
        {
            _verticalAxis += 1 * Time.deltaTime * _dragModifier * inAirModifier;
            if (_verticalAxis > 0)
            {
                _verticalAxis = 0;
            }
            //Debug.Log("VerticalAxis =" + _verticalAxis);
        }

        if (_wPressed && _verticalAxis < 1)
        {
            _verticalAxis += 1 * Time.deltaTime * _accelerationModifier * inAirModifier;
            if (_verticalAxis > 1)
            {
                _verticalAxis = 1;
            }
            //Debug.Log("VerticalAxis =" + _verticalAxis);
        }

        if (!_wPressed && _verticalAxis > 0 && !_sPressed && _isGrounded)
        {
            _verticalAxis -= 1 * Time.deltaTime * _dragModifier * inAirModifier;
            if (_verticalAxis < 0)
            {
                _verticalAxis = 0;
            }
            //Debug.Log("VerticalAxis =" + _verticalAxis);
        }


        if ((_wPressed || _sPressed || _aPressed || _dPressed) || (_horizontalAxis!=0 || _verticalAxis!=0) && _isGrounded)
        {
            RawPowerCheck();
            PowerAdjustment(CustomThetaAdjustmentRet(ThetaCheck()));
            CustomAxisLimit(ThetaCheck());
            //ThetaCheck();

            //Debug.Log(_power);
        }

        if (!_isGrounded)
        {
            RawPowerCheck();
            PowerAdjustment(CustomThetaAdjustmentRet(ThetaCheck()));
            RotationCorrection();
        }
        else
        {
            _holdRotation = 400;
            _additiveDeltaRotation = 0;
            //Debug.Log("Reset Addition" + _additiveDeltaRotation);
            //Debug.Log("Hold Rotation Reset");
        }

        //_secondaryHoldX = _verticalAxis;
        //_secondaryHoldZ = _horizontalAxis;
    }


    private void CustomAxisLimit(float theta) //I THINK that if you give this method an angle, it will adjust your direction to move in that angle within the unit circle.
    {
        _customTheta = theta;
        int quadrant = CustomThetaAdjustment();
        _limitedHorizontalAxis = _power * Mathf.Sin(_customTheta * Mathf.Deg2Rad);
        //Debug.Log("_horizontal: " + _power * Mathf.Sin(_customTheta * Mathf.Deg2Rad));
        _limitedVerticalAxis = _power * Mathf.Cos(_customTheta * Mathf.Deg2Rad);
        //Debug.Log("_vertical: " + _power * Mathf.Sin(_customTheta * Mathf.Deg2Rad));

        CoordinateCompensation();


        
    }

    public float ThetaCheck()
    {
        float theta = 0;
        int quadrant;


        if (_horizontalAxis != 0 && _verticalAxis != 0)
        {
            theta = Mathf.Rad2Deg * Mathf.Atan(_horizontalAxis / _verticalAxis);

        }
        else if (_horizontalAxis == 0 && _verticalAxis > 0)
        {
            theta = 0;
        }
        else if (_horizontalAxis == 0 && _verticalAxis < 0)
        {
            theta = 180;
        }
        else if (_verticalAxis == 0 && _horizontalAxis > 0)
        {
            theta = 90;
        }
        else if (_verticalAxis == 0 && _horizontalAxis < 0)
        {
            theta = 270;
        }
        else if (_verticalAxis == 0 && _horizontalAxis == 0)
        {
            theta = 0;
        }

        quadrant = QuadrantCheck();

        if(quadrant == 2)
        {
            theta = Mathf.Abs(theta) + 90;
        }
        if (quadrant == 3)
        {
            theta = theta + 180;
        }
        if (quadrant == 4)
        {
            theta = Mathf.Abs(theta) + 270;
        }




        //Debug.Log(theta);
        return theta;
    }


    public void RawPowerCheck()
    {
        _power = Mathf.Pow(_verticalAxis , 2)  + Mathf.Pow(_horizontalAxis , 2);
        //Debug.Log("Pre-Power " + _verticalAxis);
        _power = Mathf.Sqrt(_power);

        //Debug.Log("Starting power: " + _power);
    }



    public void PowerAdjustment(float theta)
    {

        float AdjustedinitialTheta = theta;
        float AdjustmentAmount;

        if (AdjustedinitialTheta > 45)
        {
            if (_verticalAxis != 0)
            {
                _maxX = 1;                                              //If angle is above 45 I want to find the point of intersection between adjusted initial theta and the line x=1
                _maxZ = _maxX / (_horizontalAxis / _verticalAxis);        // x=1 and x = (slope)z so z = 1/slope

                //_maxZ = _maxX / (Mathf.Tan(AdjustedinitialTheta));

                AdjustmentAmount = Mathf.Pow(_maxZ, 2) + Mathf.Pow(_maxX, 2);  // Take the (z,x) coordinates of the max hypotenuse and square them both to start pythagorean theorem 
                AdjustmentAmount = Mathf.Sqrt(AdjustmentAmount);                     // Take square root of result to get hypotenuse measurment of max hypotenuse
                AdjustmentAmount = 1 / AdjustmentAmount;                             // Divide length of hypotenuse by one to see percentage to reduce to match unit circle
                _power *= AdjustmentAmount;                                          // Multiply power by percentage to reduce power to circle
            }else
            {
                _power = Mathf.Abs(_horizontalAxis);
            }

        }
        else
        {

            if (_verticalAxis != 0)
            {
                _maxZ = 1;
                _maxX = (_horizontalAxis / _verticalAxis);
                //_maxX = (Mathf.Tan(AdjustedinitialTheta));

                AdjustmentAmount = Mathf.Pow(_maxZ, 2) + Mathf.Pow(_maxX, 2);  // Take the (z,x) coordinates of the max hypotenuse and square them both to start pythagorean theorem 
                AdjustmentAmount = Mathf.Sqrt(AdjustmentAmount);                     // Take square root of result to get hypotenuse measurment of max hypotenuse
                AdjustmentAmount = 1 / AdjustmentAmount;                             // Divide length of hypotenuse by one to see percentage to reduce to match unit circle
                _power *= AdjustmentAmount;                                          // Multiply power by percentage to reduce power to circle
            }
            else
            {
                _power = Mathf.Abs(_horizontalAxis);

            }


        }

        //Debug.Log("Adjusted power: " + _power);


    }

    public float InitialThetaAdjustment()
    {
        float AdjustedInitialTheta = _initialTheta;

        while (AdjustedInitialTheta > 90)
        {
            AdjustedInitialTheta -= 90;
        }

        return AdjustedInitialTheta;
    }


    public int CustomThetaAdjustment()
    {
        float AdjustedCustomTheta = _customTheta;
        int quadrant = 1;

        while (AdjustedCustomTheta >= 90)
        {
            AdjustedCustomTheta -= 90;
            quadrant++;
        }



        _customTheta = AdjustedCustomTheta;

        //Debug.Log("Quadrant: " + quadrant + " Adjusted Theta: " + _customTheta);
        _quadrant = quadrant;

        return quadrant;

    }

    public float CustomThetaAdjustmentRet(float theta) // Cutom Theta Adjustment that doesn't change custom theta and returns it instead
    {
        float AdjustedCustomTheta = theta;
        int quadrant = 1;

        while (AdjustedCustomTheta >= 90)
        {
            AdjustedCustomTheta -= 90;
            quadrant++;
        }



        //_customTheta = AdjustedCustomTheta;

        //Debug.Log("Quadrant: " + quadrant + " Adjusted Theta: " + _customTheta);
        //_quadrant = quadrant;

        //Debug.Log(AdjustedCustomTheta);

        return AdjustedCustomTheta;

    }


    public void CoordinateCompensation()
    {
        float theta = ThetaCheck();
        float tempVert = _limitedVerticalAxis; //z
        float tempHori = _limitedHorizontalAxis; //x
        if (_quadrant == 1)
        {
           
        }
        else if(_quadrant == 2)
        {
            _limitedVerticalAxis = -1 * tempVert;
            _limitedHorizontalAxis = tempHori;
        }
        else if (_quadrant == 3)
        {
            _limitedVerticalAxis = -1 * tempVert;
            _limitedHorizontalAxis = -1 * tempHori;
        }
        else if (_quadrant == 4)
        {
            _limitedVerticalAxis = tempVert;
            _limitedHorizontalAxis = -1 * tempHori;
        }

        /*if (theta == 0)
        {
            _limitedVerticalAxis = _verticalAxis;
            _limitedHorizontalAxis = _horizontalAxis;
        }*/
        if (theta == 90)
        {
            _limitedVerticalAxis = _verticalAxis;
            _limitedHorizontalAxis = _horizontalAxis;
        }
        else if (theta == 180)
        {
            _limitedVerticalAxis = _verticalAxis;
            _limitedHorizontalAxis = _horizontalAxis;
        }
        else if (theta == 270)
        {
            _limitedVerticalAxis = _verticalAxis;
            _limitedHorizontalAxis = _horizontalAxis;
        }

        //Debug.Log(_quadrant);

    }

    public int QuadrantCheck()
    {
        int quadrant = 0;
        if (_verticalAxis>0 && _horizontalAxis > 0)
        {
            quadrant = 1;
        }
        else if(_verticalAxis<0 && _horizontalAxis > 0)
        {
            quadrant = 2;
        }
        else if (_verticalAxis<0 && _horizontalAxis < 0)
        {
            quadrant = 3;
        }
        else if (_verticalAxis>0 && _horizontalAxis < 0)
        {
            quadrant = 4;
        }
        else
        {
            quadrant = 5;
        }

       return quadrant;
    }    
    
    private void RotationCorrection()
    {
        if (this.transform.localRotation.eulerAngles.y != _jumpRotation)
        {
            

            //Debug.Log("Rotational Change Observed Hold: " + _holdRotation + " Current: " + this.transform.rotation.eulerAngles.y);

            _deltaRotation = this.transform.rotation.eulerAngles.y - _previousRotation;
            if ((_previousRotation < 10) && (this.transform.rotation.eulerAngles.y > 350))
            {
                Debug.Log("Passed from right to left");
                _deltaRotation = -1*(360 - _deltaRotation);
            }
            else if ((_previousRotation > 350) && (this.transform.rotation.eulerAngles.y < 10))
            {
                Debug.Log("Passed from left to right");
                _deltaRotation = (360 + _deltaRotation);
            }
            //Debug.Log("Pre addition" + _additiveDeltaRotation);

            _additiveDeltaRotation += _deltaRotation;
            //Debug.Log( "Post addition" + _additiveDeltaRotation);


            //Debug.Log(_deltaTheta);


            //CustomAxisLimit(_deltaTheta);
            _deltaTheta = _initialTheta - _additiveDeltaRotation;
            while(_deltaTheta<-360)
            {
                _deltaTheta += 360;
            }
            while (_deltaTheta >= 360)
            {
                _deltaTheta -= 360;
            }

            Debug.Log(_deltaTheta);

            CustomAxisLimit(_deltaTheta);


            _previousRotation = this.transform.rotation.eulerAngles.y;


        }
    }
}
