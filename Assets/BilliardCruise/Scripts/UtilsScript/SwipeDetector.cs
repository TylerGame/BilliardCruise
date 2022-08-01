using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace BilliardCruise.Sava.Scripts
{
    public class SwipeDetector : MonoBehaviour
    {


        public static Action _OnSwipeLeft;
        public static Action _OnSwipeRight;
        public static Action _OnSwipeUp;
        public static Action _OnSwipeDown;

        private Vector2 fingerDownPos;
        private Vector2 fingerUpPos;

        public bool detectSwipeAfterRelease = false;

        public float SWIPE_THRESHOLD = 20f;





        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {


#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

            if (Input.GetMouseButtonDown(0))
            {
                fingerUpPos = Input.mousePosition;
                fingerDownPos = Input.mousePosition;
            }

            else if (Input.GetMouseButton(0))
            {
                if (!detectSwipeAfterRelease)
                {
                    fingerDownPos = Input.mousePosition;
                    DetectSwipe();
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                fingerDownPos = Input.mousePosition;
                DetectSwipe();
            }
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    fingerUpPos = touch.position;
                    fingerDownPos = touch.position;
                }

                //Detects Swipe while finger is still moving on screen
                if (touch.phase == TouchPhase.Moved)
                {
                    if (!detectSwipeAfterRelease)
                    {
                        fingerDownPos = touch.position;
                        DetectSwipe();
                    }
                }

                //Detects swipe after finger is released from screen
                if (touch.phase == TouchPhase.Ended)
                {
                    fingerDownPos = touch.position;
                    DetectSwipe();
                }
            }

#endif



        }

        void DetectSwipe()
        {

            if (VerticalMoveValue() > SWIPE_THRESHOLD && VerticalMoveValue() > HorizontalMoveValue())
            {
                Debug.Log("Vertical Swipe Detected!");
                if (fingerDownPos.y - fingerUpPos.y > 0)
                {
                    OnSwipeUp();
                }
                else if (fingerDownPos.y - fingerUpPos.y < 0)
                {
                    OnSwipeDown();
                }
                fingerUpPos = fingerDownPos;

            }
            else if (HorizontalMoveValue() > SWIPE_THRESHOLD && HorizontalMoveValue() > VerticalMoveValue())
            {
                Debug.Log("Horizontal Swipe Detected!");
                if (fingerDownPos.x - fingerUpPos.x > 0)
                {
                    OnSwipeRight();
                }
                else if (fingerDownPos.x - fingerUpPos.x < 0)
                {
                    OnSwipeLeft();
                }
                fingerUpPos = fingerDownPos;

            }
            else
            {
                Debug.Log("No Swipe Detected!");
            }
        }


        float VerticalMoveValue()
        {
            return Mathf.Abs(fingerDownPos.y - fingerUpPos.y);
        }

        float HorizontalMoveValue()
        {
            return Mathf.Abs(fingerDownPos.x - fingerUpPos.x);
        }

        void OnSwipeUp()
        {
            //Do something when swiped up
            _OnSwipeUp();
        }

        void OnSwipeDown()
        {
            //Do something when swiped down
            _OnSwipeDown();
        }

        void OnSwipeLeft()
        {
            //Do something when swiped left
            _OnSwipeLeft();
        }

        void OnSwipeRight()
        {
            //Do something when swiped right
            _OnSwipeRight();
        }
    }
}

