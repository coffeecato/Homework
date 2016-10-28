using UnityEngine;

namespace Framework
{
    public class PlayerCamera : MonoBehaviour
    {
        static PlayerCamera _instance;

        internal Camera _mainCamera;
        bool _bInitInputEvent = false;

        public float _swipeSpeed = 1.0f;

        protected virtual void Awake()
        {
            instance = this;

            _mainCamera = gameObject.GetComponent<Camera>();

            if( _mainCamera == null )
                _mainCamera = Camera.main;

            if( _mainCamera == null )
                LogManager.LogWarning("", "Missing main camera, plz check all camera whether if has been tagged MainCamera");
        }

        protected virtual void Start()
        {
            SetInputEvent(true);
        }

        protected virtual void OnEnable()
        {
            SetInputEvent(true);
        }

        protected virtual void OnDisable()
        {
            SetInputEvent(false);
        }

        public static PlayerCamera instance
        {
            get
            {
                return _instance;
            }
            set
            {
                if (_instance != value)
                    _instance = value;
            }
        }

        public Camera mainCamera
        {
            get
            {
                return instance._mainCamera;
            }
        }

        protected virtual void SetInputEvent(bool bSet)
        {
            if (PlayerInput.instance == null)
                return;

            if (bSet && !_bInitInputEvent)
            {
                PlayerInput.instance.OnSwipeEvent += OnSwipe;
                PlayerInput.instance.OnPinchEvent += OnPinch;
                _bInitInputEvent = true;
            }
            else if (!bSet && _bInitInputEvent)
            {
                PlayerInput.instance.OnSwipeEvent -= OnSwipe;
                PlayerInput.instance.OnPinchEvent -= OnPinch;
                _bInitInputEvent = false;
            }
        }

        protected virtual void LateUpdate()
        {
        }

        protected virtual void OnPinch(PinchGesture gesture)
        {
        }

        protected virtual void OnSwipe(SwipeGesture gesture)
        {
            if (gesture.Phase == ContinuousGesturePhase.Updated)
            {
                if (_mainCamera != null)
                {
                    _mainCamera.transform.position -= new Vector3(gesture.Move.x * _swipeSpeed, gesture.Move.y * _swipeSpeed, 0);
                }
            }
        }

        public void Move(Vector3 delta)
        {
            if (_mainCamera != null)
            {
                _mainCamera.transform.position += delta;
            }
        }

        public void MoveTo(Vector3 pos)
        {
            if (_mainCamera != null)
            {
                _mainCamera.transform.position = pos;
            }
        }
    }
}