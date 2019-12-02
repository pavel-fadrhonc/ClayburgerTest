using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public enum ePersonState
    {
        Idling,
        Jumping
    }
    
    public class PersonController : MonoBehaviour
    {
        public Animator personAnimator;
        
        [Header("Reflection")] 
        [Range(1f, 2f)]
        public float reflectionDisplacement;
        public GameObject reflection;
        
        [Header("Jumping")]
        public List<Transform> jumpingPoints;

        public float jumpDuration = 1.5f;
        public float jumpSpeedScale = 1;
        public AnimationCurve jumpSpeedCurve;
        public float jumpCurveHeight;
        
        private int _currentJumpPointIdx;
        private ePersonState _personState;

        private Vector3 _startJumpPos;
        private Vector3 _endJumpPos;
        private float _jumpTimeProgress;
        private Vector3 _jumpDirNormal;

        private PersonModel _personModel;

        private Animator _personReflectionAnimator;
        
        private Vector3 _reflectionDefaultLocalPos;
        private Transform _transform;

        private void Awake()
        {
            _personReflectionAnimator = reflection.GetComponent<Animator>();
            _transform = GetComponent<Transform>();
        }

        private void Start()
        {
            _personModel = Locator.Instance.PersonModel;
            _reflectionDefaultLocalPos = reflection.transform.localPosition;
        }

        private void Update()
        {
            switch (_personState)
            {
                case ePersonState.Idling:
                    Transform pointToJump = null;
                    
                    if (_jump)
                    {
                        var nextJumpPointIdx = _currentJumpPointIdx + 1;
                        nextJumpPointIdx %= jumpingPoints.Count;
                        pointToJump = jumpingPoints[nextJumpPointIdx];

                        _jump = false;
                    }
                    
                    var jumpDir = Input.GetAxis("Horizontal");
                    if (jumpDir > 0)
                    {
                        Transform closestRight = null;
                        for (int i = 0; i < jumpingPoints.Count; i++)
                        {
                            if (jumpingPoints[i] == jumpingPoints[_currentJumpPointIdx] ||
                                jumpingPoints[i].position.x < jumpingPoints[_currentJumpPointIdx].position.x)
                                continue;
                            
                            if (closestRight == null)
                                closestRight = jumpingPoints[i];

                            if (jumpingPoints[i].position.x - _transform.position.x <
                                (closestRight.position.x - _transform.position.x))
                                closestRight = jumpingPoints[i];
                        }

                        pointToJump = closestRight;
                    }
                    else if (jumpDir < 0)
                    {
                        Transform closestLeft = null;
                        for (int i = 0; i < jumpingPoints.Count; i++)
                        {
                            if (jumpingPoints[i] == jumpingPoints[_currentJumpPointIdx] ||
                                jumpingPoints[i].position.x > jumpingPoints[_currentJumpPointIdx].position.x)
                                continue;

                            if (closestLeft == null)
                                closestLeft = jumpingPoints[i];

                            if (_transform.position.x - jumpingPoints[i].position.x <
                                (_transform.position.x - closestLeft.position.x))
                                closestLeft = jumpingPoints[i];
                        }
                        
                        pointToJump = closestLeft;
                    }

                    if (pointToJump != null && pointToJump != jumpingPoints[_currentJumpPointIdx])
                    {
                        _personState = ePersonState.Jumping;
                        
                        _startJumpPos = jumpingPoints[_currentJumpPointIdx].position;
                        _endJumpPos = pointToJump.position;
                        _jumpTimeProgress = 0;
                        var startToEndVector = (_endJumpPos - _startJumpPos);
                        _jumpDirNormal = new Vector3(startToEndVector.normalized.y,  Mathf.Abs(startToEndVector.normalized.x), 0); 
                        
                        _currentJumpPointIdx = jumpingPoints.IndexOf(pointToJump);
                        
                        personAnimator.SetTrigger("Jump");
                        _personReflectionAnimator.SetTrigger("Jump");
                    }
                    
                    break;
                case ePersonState.Jumping:
                    _jumpTimeProgress += Time.deltaTime;
                    var jumpTimeProgressNorm = _jumpTimeProgress / jumpDuration;
                    var jumpDistanceProgressNorm = jumpSpeedCurve.Evaluate(jumpTimeProgressNorm);
                    if (jumpTimeProgressNorm >= 1)
                    {
                        _transform.position = _endJumpPos;
                        _personState = ePersonState.Idling;
                        _jump = false;
                        _personModel.NumJumps++;
                        _transform.localScale = new Vector3(_transform.localScale.x * -1, _transform.localScale.y, _transform.localScale.z);
                    }
                    else
                    {
                        var jumpDisplacement = Mathf.Sin(jumpDistanceProgressNorm * Mathf.PI) * jumpCurveHeight * _jumpDirNormal;
                        var travelPosition = Vector3.Lerp(_startJumpPos, _endJumpPos, jumpDistanceProgressNorm);
                        _transform.position = travelPosition + jumpDisplacement;
                        reflection.transform.localPosition = _reflectionDefaultLocalPos - reflectionDisplacement * jumpDisplacement;
                    }

                    break;
                default:
                    break;
            }
        }

        private bool _jump = false;
        
        public void Jump()
        {
            _jump = true;
        }
    }
}