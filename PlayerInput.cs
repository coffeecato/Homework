using UnityEngine;

namespace Framework
{
    [RequireComponent(typeof(FingerDownDetector))]
    [RequireComponent(typeof(FingerUpDetector))]
    [RequireComponent(typeof(SwipeRecognizer))]
    [RequireComponent(typeof(PinchRecognizer))]
    [RequireComponent(typeof(ScreenRaycaster))]
    public class PlayerInput : MonoBehaviour
    {
        static PlayerInput _instance = null;

        public delegate void PinchEventHandler(PinchGesture gesture);
        public event PinchEventHandler OnPinchEvent;

        public delegate void SwipeEventHandler(SwipeGesture gesture);
        public event SwipeEventHandler OnSwipeEvent;

        /// <summary>
        /// 输入事件过滤类型
        /// </summary>
        public enum EventHandleType
        {
            UIHandle,       ///仅UI处理
            GameHandle,     ///仅FingerGesture处理
            Both,           ///UI及FingerGesture都需要处理
        }
        EventHandleType _evtHandleType;

        void Awake()
        {
            instance = this;

            FingerDownDetector downDetector = GetComponent<FingerDownDetector>();
            downDetector.UseSendMessage = false;

            FingerUpDetector upDetector = GetComponent<FingerUpDetector>();
            upDetector.UseSendMessage = false;

            SwipeRecognizer swipeRecognizer = GetComponent<SwipeRecognizer>();
            swipeRecognizer.UseSendMessage = false;
            //swipeRecognizer.Delegate = gameObject.AddComponent<SwipeGestureRecognizerDelegate>();

            PinchRecognizer pinchRecognizer = GetComponent<PinchRecognizer>();
            pinchRecognizer.UseSendMessage = false;
            //pinchRecognizer.Delegate = gameObject.AddComponent<PinchGestureRecognizerDelegate>();
        }

        void OnEnable()
        {
            GetComponent<FingerDownDetector>().OnFingerDown += OnFingerDown;
            GetComponent<FingerUpDetector>().OnFingerUp += OnFingerUp;
            GetComponent<SwipeRecognizer>().OnGesture += OnSwipe;
            GetComponent<PinchRecognizer>().OnGesture += OnPinch;
        }

        void OnDisable()
        {
            GetComponent<FingerDownDetector>().OnFingerDown -= OnFingerDown;
            GetComponent<FingerUpDetector>().OnFingerUp -= OnFingerUp;
            GetComponent<SwipeRecognizer>().OnGesture -= OnSwipe;
            GetComponent<PinchRecognizer>().OnGesture -= OnPinch;
        }

        void Start()
        {
            InitFingerGestureFilterParam();
        }

        public static PlayerInput instance
        {
            get
            {
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        void OnFingerDown(FingerDownEvent e)
        {
        }
        void OnFingerUp(FingerUpEvent e)
        {
        }

        void OnSwipe(SwipeGesture gesture)
        {
            if (OnSwipeEvent != null)
                OnSwipeEvent(gesture);
        }

        void OnPinch(PinchGesture gesture)
        {
            if (OnPinchEvent != null)
                OnPinchEvent(gesture);
        }

        /// <summary>
        /// 初始化 FingerGestures 全局输入过滤器参数
        /// 默认值为： EventHandleType.UIHandle 表示优先处理UI层的输入事件
        /// </summary>
        void InitFingerGestureFilterParam()
        {
            EvtHandleType = EventHandleType.Both;
            FingerGestures.GlobalTouchFilter = FingerGestureShouldProcessTouch;
        }

        public EventHandleType EvtHandleType
        {
            get { return _evtHandleType; }
            set { _evtHandleType = value; }
        }
        /// <summary>
        /// 设置 FingerGestures 全局输入过滤器 执行接口
        /// <returns>True: FingerGestures需要响应输入事件，False: 表示无需响应输入事件</returns>
        /// </summary>
        bool FingerGestureShouldProcessTouch(int fingerIndex, Vector2 position)
        {
            bool FGpressTouch = true;                                           
            switch (_evtHandleType)
            {
                case EventHandleType.UIHandle:
                    FGpressTouch = !HandleUIEvent(fingerIndex, position);       ///< UI层处理过，FingerGesture无需处理
                    break;
                case EventHandleType.GameHandle:                                ///< 忽略UI层事件，需要针对UI层的事件响应进行额外的处理
                case EventHandleType.Both:
                    FGpressTouch = true;
                    break;
                default:
                    FGpressTouch = !HandleUIEvent(fingerIndex, position);       ///< UI层处理过，FingerGesture无需处理
                    break;
            }
            return FGpressTouch;
        }

        /// <summary>
        /// 通过Physics.Raycast判断是否点击在UI层上
        /// </summary>
        /// <returns>True: UI层已经处理了输入事件， False: UI层没有处理输入事件</returns>
        bool HandleUIEvent(int fingerIndex, Vector2 position)
        {
            Ray ray = UICamera.currentCamera.ScreenPointToRay(position);
            bool hanlded = Physics.Raycast(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("UI"));
            return hanlded;
        }
    }

    public class DragGestureRecognizerDelegate : GestureRecognizerDelegate
    {
        //PlayerInput _op;
        void Start()
        {
            //_op = GetComponent<PlayerInput>();
        }
        override public bool CanBegin(Gesture gesture, FingerGestures.IFingerList touches)
        {
            //if (!_op.dragReady || _op.gestureType != PlayerInput.GestureType.GestureType_None)
            //    return false;
            return true;
        }
    }

    public class SwipeGestureRecognizerDelegate : GestureRecognizerDelegate
    {
        //PlayerInput _op;
        void Start()
        {
            //_op = GetComponent<PlayerInput>();
        }
        override public bool CanBegin(Gesture gesture, FingerGestures.IFingerList touches)
        {
            //if (_op.dragReady || _op.gestureType != PlayerInput.GestureType.GestureType_None)
            //    return false;
            return true;
        }
    }

    public class PinchGestureRecognizerDelegate : GestureRecognizerDelegate
    {
        //PlayerInput _op;
        void Start()
        {
            //_op = GetComponent<PlayerInput>();
        }
        override public bool CanBegin(Gesture gesture, FingerGestures.IFingerList touches)
        {
            //if (_op.dragReady || _op.gestureType != PlayerInput.GestureType.GestureType_None)
            //    return false;
            return true;
        }
    }
}