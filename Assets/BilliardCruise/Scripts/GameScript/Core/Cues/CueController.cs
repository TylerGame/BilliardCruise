
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;


namespace BilliardCruise.Sava.Scripts
{

    public class CueController : MonoBehaviour
    {

        [SerializeField] protected SpriteRenderer cueSpriteRenderer;
        [SerializeField] protected SpriteMask cueMask;
        [SerializeField] protected SpriteRenderer hitCueSpriteRenderer;
        [SerializeField] protected SpriteRenderer fireSpriteRenderer;
        public float disabledOpacity = 0.6f;
        [SerializeField] protected float offsetFromCueBall = 0.7f;
        [SerializeField] protected GameObject guideLinePrefab;

        [SerializeField] private float accuracyModeCueRotSpeed = 1;
        [SerializeField] private float roateSpeed = 1f;
        [SerializeField] protected AudioClip hitSound;




        [HideInInspector]
        public Player owner;

        public bool isTutorial = false;
        public float MinStrength
        {
            get;
            set;
        }

        public float MaxStrength
        {
            get;
            set;
        }

        public float MaxSpin
        {
            get;
            set;
        }

        public float AimLength
        {
            get;
            set;
        }

        public float MaxTimePerMove
        {
            get;
            set;
        }

        protected PoolManager poolManager;
        protected InputManager inputManager;
        protected GuideLineController guideLine;
        protected GameUI gameUI;
        protected Ball cueBall;
        protected Rigidbody cueBallRB;
        protected Slider cueSlider;
        protected SpinKnobButton spinKnobBtn;
        protected SpinMenu spinMenu;
        protected SpinKnob spinKnob;

        protected bool isActive = false;
        protected Vector3 initialTouchPos;

        // Guide line
        protected Vector3 origin;
        protected Vector3 castDirection;
        protected float radius = 0;
        protected Ray ray;
        protected RaycastHit hit;

        private float cueRotationSpeed = 15f;
        private float deltaAngle = 0f;

        private float prevSignX = 1f;
        private float prevSignY = 1f;
        private Quaternion _lookRotation;


        protected virtual void Awake()
        {
            poolManager = GameObject.FindObjectOfType<PoolManager>();
            inputManager = GameObject.FindObjectOfType<InputManager>();
            //     inputManager.IgnoreUI(true);
            //    gameUI = GameObject.FindObjectOfType<GameUI>();
            if (gameUI != null)
            {
                cueSlider = gameUI.cueSlider;
                spinKnobBtn = gameUI.spinKnobBtn;
                spinMenu = gameUI.spinMenu;
                spinKnob = spinMenu.spinKnob;
            }
            GameObject guideLineObj = Instantiate(guideLinePrefab);
            guideLine = guideLineObj.GetComponent<GuideLineController>();
        }
        public bool isReal = false;
        protected virtual void Start()
        {
            cueBall = poolManager.CueBall;
            cueBallRB = cueBall.GetComponent<Rigidbody>();
            guideLine.HideGuideLine();
            HideIndicators();
            HideFireball();
        }

        Vector3 targetpos = Vector3.zero;
        Vector3 target_vetor = Vector3.zero;
        float delta_angle = 0f;
        Vector3 start_pos = Vector3.zero;


        float delta_angle_min = 360f;
        float delta_angle_max = 0;
        bool isDrawableGuideLine = false;
        float hitCharge = 0f;
        protected virtual void Update()
        {

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

            if (inputManager.State == InputManager.TouchState.START && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                initialTouchPos = inputManager.TouchPosition;
                //deltaAngle = Mathf.Atan(initialTouchPos.y / initialTouchPos.x);
                initialTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(inputManager.TouchPosition.x,
                    inputManager.TouchPosition.y, Camera.main.transform.position.y -
                    cueBall.transform.position.y));
                start_pos = initialTouchPos;
                target_vetor = (new Vector3(initialTouchPos.x, cueBall.transform.position.y, initialTouchPos.z) - cueBall.transform.position);
                isDrawableGuideLine = true;
                ShowIndicators();
            }
            else if (inputManager.State == InputManager.TouchState.STAY && isDrawableGuideLine)
            {
                initialTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(inputManager.TouchPosition.x,
                    inputManager.TouchPosition.y, Camera.main.transform.position.y -
                    cueBall.transform.position.y));
                if (!poolManager.IsCueBallSelected(initialTouchPos))
                {
                    delta_angle = Vector3.Angle((initialTouchPos - cueBall.transform.position), target_vetor);
                    if (delta_angle < 60f)
                    {
                        target_vetor = (new Vector3(initialTouchPos.x, cueBall.transform.position.y, initialTouchPos.z) - cueBall.transform.position);
                        _lookRotation = Quaternion.LookRotation(target_vetor);
                        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * cueRotationSpeed);
                        if (Vector3.Distance(initialTouchPos, cueBall.transform.position) > Vector3.Distance(start_pos, cueBall.transform.position))
                        {
                            hitCharge -= MathF.Abs(Vector3.Distance(initialTouchPos, cueBall.transform.position) - Vector3.Distance(start_pos, cueBall.transform.position)) / 3f;
                        }
                        else
                        {
                            hitCharge += MathF.Abs(Vector3.Distance(initialTouchPos, cueBall.transform.position) - Vector3.Distance(start_pos, cueBall.transform.position)) / 3f;
                        }
                        hitCharge = Mathf.Lerp(0f, 1f, hitCharge);
                        cueMask.transform.localScale = Vector3.one * 0.5f * (1f + hitCharge);
                        hitCueSpriteRenderer.transform.localPosition = Vector3.back * hitCharge;
                    }
                    else
                    {
                        target_vetor = (cueBall.transform.position - new Vector3(initialTouchPos.x, cueBall.transform.position.y, initialTouchPos.z));
                        _lookRotation = Quaternion.LookRotation(target_vetor);
                        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * cueRotationSpeed);
                        if (Vector3.Distance(initialTouchPos, cueBall.transform.position) < Vector3.Distance(start_pos, cueBall.transform.position))
                        {
                            hitCharge -= MathF.Abs(Vector3.Distance(initialTouchPos, cueBall.transform.position) - Vector3.Distance(start_pos, cueBall.transform.position)) / 3f;
                        }
                        else
                        {
                            hitCharge += MathF.Abs(Vector3.Distance(initialTouchPos, cueBall.transform.position) - Vector3.Distance(start_pos, cueBall.transform.position)) / 3f;
                        }
                        hitCharge = Mathf.Lerp(0f, 1f, hitCharge);
                        cueMask.transform.localScale = Vector3.one * 0.5f * (1f + hitCharge);
                        hitCueSpriteRenderer.transform.localPosition = Vector3.back * hitCharge;
                    }

                    start_pos = initialTouchPos;
                }
                else
                {
                    // hitCharge = 0f;
                    // cueMask.transform.localScale = Vector3.one * 0.5f;
                    // hitCueSpriteRenderer.transform.localPosition = Vector3.zero;
                }
            }
            else if (inputManager.State == InputManager.TouchState.END)
            {
                cueMask.transform.localScale = Vector3.one * 0.5f;
                hitCueSpriteRenderer.transform.localPosition = Vector3.zero;
                float strength = hitCharge;
                if (poolManager.IsCueBallSelected(initialTouchPos))
                    strength = 1f;
                else if (Vector3.Dot(castDirection, initialTouchPos - cueBall.transform.position) < 0)
                    strength = 1f;
                if (isDrawableGuideLine)
                {
                    if (strength > 0.05f)
                    {
                        HitCueBall(strength);
                        GameManager.Instance.UpdateMoves();
                    }

                    HideIndicators();
                }
                isDrawableGuideLine = false;
                hitCharge = 0f;
            }

            if (isDrawableGuideLine)
            {
                DrawGuideLine();
            }
            else
            {
                guideLine.HideGuideLine();
            }
            SetCuePosition();


#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE

	for(int i = 0; i < Input.touchCount; i++) {
            Touch touch = Input.GetTouch(i);

            if (inputManager.State == InputManager.TouchState.START && touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                initialTouchPos = inputManager.TouchPosition;
                //deltaAngle = Mathf.Atan(initialTouchPos.y / initialTouchPos.x);
                initialTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(inputManager.TouchPosition.x,
                    inputManager.TouchPosition.y, Camera.main.transform.position.y -
                    cueBall.transform.position.y));
                start_pos = initialTouchPos;
                target_vetor = (new Vector3(initialTouchPos.x, cueBall.transform.position.y, initialTouchPos.z) - cueBall.transform.position);
                isDrawableGuideLine = true;
                ShowIndicators();
            }
            else if (inputManager.State == InputManager.TouchState.STAY && isDrawableGuideLine)
            {
                initialTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(inputManager.TouchPosition.x,
                    inputManager.TouchPosition.y, Camera.main.transform.position.y -
                    cueBall.transform.position.y));
                if (!poolManager.IsCueBallSelected(initialTouchPos))
                {
                    delta_angle = Vector3.Angle((initialTouchPos - cueBall.transform.position), target_vetor);
                    if (delta_angle < 60f)
                    {
                        target_vetor = (new Vector3(initialTouchPos.x, cueBall.transform.position.y, initialTouchPos.z) - cueBall.transform.position);
                        _lookRotation = Quaternion.LookRotation(target_vetor);
                        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * cueRotationSpeed);
                        if (Vector3.Distance(initialTouchPos, cueBall.transform.position) > Vector3.Distance(start_pos, cueBall.transform.position))
                        {
                            hitCharge -= MathF.Abs(Vector3.Distance(initialTouchPos, cueBall.transform.position) - Vector3.Distance(start_pos, cueBall.transform.position)) / 3f;
                        }
                        else
                        {
                            hitCharge += MathF.Abs(Vector3.Distance(initialTouchPos, cueBall.transform.position) - Vector3.Distance(start_pos, cueBall.transform.position)) / 3f;
                        }
                        hitCharge = Mathf.Lerp(0f, 1f, hitCharge);
                        cueMask.transform.localScale = Vector3.one * 0.5f * (1f + hitCharge);
                        hitCueSpriteRenderer.transform.localPosition = Vector3.back * hitCharge;
                    }
                    else
                    {
                        target_vetor = (cueBall.transform.position - new Vector3(initialTouchPos.x, cueBall.transform.position.y, initialTouchPos.z));
                        _lookRotation = Quaternion.LookRotation(target_vetor);
                        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * cueRotationSpeed);
                        if (Vector3.Distance(initialTouchPos, cueBall.transform.position) < Vector3.Distance(start_pos, cueBall.transform.position))
                        {
                            hitCharge -= MathF.Abs(Vector3.Distance(initialTouchPos, cueBall.transform.position) - Vector3.Distance(start_pos, cueBall.transform.position)) / 3f;
                        }
                        else
                        {
                            hitCharge += MathF.Abs(Vector3.Distance(initialTouchPos, cueBall.transform.position) - Vector3.Distance(start_pos, cueBall.transform.position)) / 3f;
                        }
                        hitCharge = Mathf.Lerp(0f, 1f, hitCharge);
                        cueMask.transform.localScale = Vector3.one * 0.5f * (1f + hitCharge);
                        hitCueSpriteRenderer.transform.localPosition = Vector3.back * hitCharge;
                    }

                    start_pos = initialTouchPos;
                }
                else
                {
                    // hitCharge = 0f;
                    // cueMask.transform.localScale = Vector3.one * 0.5f;
                    // hitCueSpriteRenderer.transform.localPosition = Vector3.zero;
                }
            }
            else if (inputManager.State == InputManager.TouchState.END)
            {
                cueMask.transform.localScale = Vector3.one * 0.5f;
                hitCueSpriteRenderer.transform.localPosition = Vector3.zero;
                float strength = hitCharge;
                if (poolManager.IsCueBallSelected(initialTouchPos))
                    strength = 1f;
                else if (Vector3.Dot(castDirection, initialTouchPos - cueBall.transform.position) < 0)
                    strength = 1f;                 
                if (isDrawableGuideLine)
                {
                    if (strength > 0.05f)
                    {
                    
                        HitCueBall(strength);
                        GameManager.Instance.UpdateMoves();
                    }
                    HideIndicators();
                }
                                
                isDrawableGuideLine = false;
                hitCharge = 0f;
            }

            if (isDrawableGuideLine)
            {
                DrawGuideLine();
            }
            else
            {
                guideLine.HideGuideLine();
            }
            SetCuePosition();
    }
#endif
        }

        protected virtual void HideIndicators()
        {
            cueSpriteRenderer.enabled = false;
            hitCueSpriteRenderer.enabled = false;

        }

        protected virtual void ShowIndicators()
        {
            cueSpriteRenderer.enabled = true;
            hitCueSpriteRenderer.enabled = true;
        }

        protected virtual void HideFireball()
        {
            fireSpriteRenderer.enabled = false;
            HideIndicators();

        }

        protected virtual void ShowFireball()
        {
            fireSpriteRenderer.enabled = true;
            HideIndicators();
        }

        // protected virtual float getAppliedSign(Vector3 from, Vector3 to)
        // {

        // }

        protected virtual void OnDisable()
        {
            guideLine.HideGuideLine();
        }

        public virtual void Activate()
        {
            this.gameObject.SetActive(true);
            isActive = true;


            // cueSlider.Pressed = OnSliderPressed;
            // cueSlider.Dragged = OnSliderDragged;
            // cueSlider.Released = OnSliderReleased;

            // spinKnobBtn.Released = OnSpinKnobBtnReleased;

            // spinKnob.Dragged = OnSpinKnobDragged;
            // spinKnob.Released = OnSpinKnobReleased;

            // spinMenu.Released = OnSpinMenuReleased;
        }

        public virtual void Deactivate()
        {
            this.gameObject.SetActive(false);
            isActive = false;
            guideLine.HideGuideLine();
        }

        protected void OnSliderPressed()
        {
            // Take action for slider pressed
        }

        protected void OnSliderDragged()
        {

        }

        protected virtual void OnSliderReleased()
        {
            Debug.Log("release");
            if (cueSlider.Value <= cueSlider.ignoreBelowValue)
            {
                return;
            }

            float t = Mathf.InverseLerp(cueSlider.ignoreBelowValue, 1, cueSlider.Value);

            Vector3 direction = transform.forward;
            float magnitude = MaxStrength * t;
            magnitude = Mathf.Clamp(magnitude, MinStrength, MaxStrength);

            Vector3 force = direction * magnitude;

            // For left & right spin ==> y axis
            // For top & bottom spin ==> z axis
            Vector2 spin = spinKnob.Input;

            float spinMagnitude = Mathf.Lerp(0, 1, cueSlider.Value / 1);
            Vector3 horizontalSpin = new Vector3(0, -spin.x * MaxSpin * spinMagnitude, 0);
            Vector3 verticalSpin = transform.right * spin.y * MaxSpin * spinMagnitude * 5;

            Vector3 angularVelocity = new Vector3(verticalSpin.x, horizontalSpin.y, verticalSpin.z);

            HitCueBall(force, angularVelocity);

            owner.EndTurn(false);

            // if (isTutorial)
            //     (poolManager as TutorialController).ToGame();


        }


        protected virtual void HitCueBall(float strength)
        {
            float t = 0.6f;
            Vector3 direction = transform.forward;
            float magnitude = Mathf.Lerp(MinStrength, 2f * MaxStrength, strength);
            //     magnitude = Mathf.Clamp(magnitude, MinStrength, MaxStrength);

            Vector3 force = direction * magnitude;

            // For left & right spin ==> y axis
            // For top & bottom spin ==> z axis
            //     Vector2 spin = spinKnob.Input;

            Vector2 spin = Vector2.zero;



            float spinMagnitude = Mathf.Lerp(0, 1, 1);
            Vector3 horizontalSpin = new Vector3(0, -spin.x * MaxSpin * spinMagnitude, 0);
            Vector3 verticalSpin = transform.right * spin.y * MaxSpin * spinMagnitude * 5;

            Vector3 angularVelocity = new Vector3(verticalSpin.x, horizontalSpin.y, verticalSpin.z);

            HitCueBall(force, angularVelocity);
            GameManager.Instance.isMoving = true;
            owner.EndTurn(false);
        }



        protected virtual void OnSpinKnobBtnReleased()
        {
            ShowSpinMenu();
        }

        protected virtual void OnSpinKnobDragged()
        {
            SetKnobPosition(spinKnob.Input);
        }

        protected virtual void OnSpinKnobReleased()
        {
            HideSpinMenu();
        }

        protected virtual void OnSpinMenuReleased()
        {
            HideSpinMenu();
        }

        protected void DrawGuideLine()
        {


            origin = cueBall.transform.position;
            castDirection = transform.forward;
            radius = cueBall.Radius;
            ray = new Ray(origin, castDirection);

            if (Physics.SphereCast(ray, radius, out hit, 100))
            {

                Vector3 collisionCentroid = cueBall.transform.position + (castDirection.normalized * hit.distance);

                guideLine.DrawLineMain(cueBall.transform.position, collisionCentroid);
                // guideLine.DrawLineMain(cueBall.transform.position, collisionCentroid + castDirection.normalized * 5f);

                //   guideLine.DrawDottedLine(cueBall.transform.position, collisionCentroid);

                bool ballAllowed = true;

                Ball targetBall = hit.collider.GetComponent<Ball>();
                if (targetBall != null)
                {
                    ballAllowed = poolManager.IsBallAllowed(owner, targetBall);

                    if (ballAllowed)
                    {
                        Vector3 cueBallDirection = Vector3.ProjectOnPlane(ray.direction, hit.normal);
                        Vector3 cueBallPoint2 = collisionCentroid + (cueBallDirection * AimLength * 20f);
                        //         guideLine.DrawLineSource(collisionCentroid, cueBallPoint2);

                        Vector3 targetBallDirection = hit.normal;
                        Vector3 targetBallPoint2 = targetBall.transform.position - (targetBallDirection * AimLength * 20);
                        //       guideLine.DrawLineTarget(targetBall.transform.position, targetBallPoint2);

                        //cueRotationSpeed = accuracyModeCueRotSpeed;



                        //        cueRotationSpeed = 1f;
                        //cueRotationSpeed = 1.5f;
                        //cueRotationSpeed = 2f;
                        //cueRotationSpeed = 2.5f;
                    }
                }
                else
                {

                    //      cueRotationSpeed = 1f;
                    //cueRotationSpeed = 1.5f;
                    //cueRotationSpeed = 2f;
                    //cueRotationSpeed = 2.5f;

                    guideLine.HideLineSource();
                    guideLine.HideLineTarget();
                }

                // guideLine.DrawGhostBall(collisionCentroid, ballAllowed);

                guideLine.DrawGhostBall(collisionCentroid, true);

                if (!ballAllowed)
                {
                    guideLine.HideLineSource();
                    guideLine.HideLineTarget();
                }
            }
            else
            {
                guideLine.HideGuideLine();
            }
        }

        protected float GetAngle(Vector3 position)
        {
            Vector3 dir = (position - cueBall.transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            float angle = Quaternion.Angle(transform.rotation, targetRotation);

            return angle;
        }

        public void RotateCue(float angle)
        {
            transform.RotateAround(cueBall.transform.position, Vector3.up, angle);
        }

        protected void SetCuePosition()
        {
            if (cueBall == null)
            {
                return;
            }

            // Vector3 cueDirection = (cueBall.transform.position - transform.position).normalized;
            // // float offset = offsetFromCueBall + (cueSlider.Value / 1.5f);
            // float offset = offsetFromCueBall;
            // Vector3 cuePosition = cueBall.transform.position - (cueDirection * offset);
            // cuePosition.y = cueBall.transform.position.y;
            // transform.position = cuePosition;

            transform.position = cueBall.transform.position;

            //  transform.LookAt(cueBall.transform);
        }

        public void LookAt(Vector3 point)
        {
            SetCuePosition();

            float angle = GetAngle(point);
            if (Mathf.Abs(angle) > 90)
            {
                angle *= -1;
            }

            transform.RotateAround(cueBall.transform.position, Vector3.up, angle);
        }

        public void SetCueOpacity(float opacity)
        {
            Color clr = cueSpriteRenderer.color;
            clr.a = opacity;

            cueSpriteRenderer.color = clr;

            guideLine.SetGuideLineOpacity(opacity);
        }

        protected virtual void ShowSpinMenu()
        {
            spinMenu.ShowMenu();
        }

        protected virtual void HideSpinMenu()
        {
            spinMenu.HideMenu();
        }

        protected virtual void SetKnobPosition(Vector2 position)
        {
            spinKnob.SetKnobPosition(position);
            spinKnobBtn.SetKnobPosition(position);
        }

        public virtual void HitCueBall(Vector3 force, Vector3 angularVelocity)
        {
            cueBallRB.AddForce(force, ForceMode.VelocityChange);
            cueBallRB.angularVelocity = angularVelocity;

            // if (GameManager.Instance.isTriggerArrowEffect)
            // {

            // }
            // else if (GameManager.Instance.isTriggerDiceEffect)
            // {

            // }
            // else if (GameManager.Instance.isTriggerEyeEffect)
            // {

            // }
            // else if (GameManager.Instance.isTriggerStrengthEffect)
            // {

            // }
            PlayHitSound();
        }

        protected virtual void PlayHitSound()
        {
            AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position);
        }

        public void SetStats(string cueId)
        {
            CueStats cueStats = PlayerInfo.Instance.GetCue(cueId);

            SetCueSprite(cueStats.CueSprite);

            MinStrength = cueStats.MinStrength;
            MaxStrength = cueStats.MaxStrength;
            MaxSpin = cueStats.MaxSpin;
            AimLength = cueStats.AimLength;
            MaxTimePerMove = cueStats.TimePerMove;
        }

        private void SetCueSprite(Sprite sprite)
        {
            if (cueSpriteRenderer == null)
            {
                return;
            }

            cueSpriteRenderer.sprite = sprite;
        }

    }

}

