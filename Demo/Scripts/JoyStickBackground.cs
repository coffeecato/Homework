using UnityEngine;
using System.Collections;

public class JoyStickBackground : MonoBehaviour
{

    public Transform thumb;

    private bool isReset = true;
    [SerializeField]
    protected Vector3 originPos = Vector3.zero;                 // 原点
    protected Vector3 offsetFromOrigin = Vector3.zero;          // 原点到拖拽位置的向量
    public Vector3 OffsetFromOrigin
    {
        set { offsetFromOrigin = value; }
        get { return offsetFromOrigin; }
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnPress(bool pressed)
    {
        if (pressed)
        {
            Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
            float dist = 0f;
            Vector3 currentPos = ray.GetPoint(dist);
            Debug.LogWarning("OnPress(true) pos: " + currentPos);

            thumb.position = currentPos;
            isReset = false;
            ///< 更新当前坐标到原点的向量
            UpdateVector3FromOrigin(currentPos);
            Debug.LogWarning("OnPress OffsetFromOrigin: " + OffsetFromOrigin);
        }
        else
        {
            Debug.LogWarning("OnPress(false)");
            thumb.position = Vector3.zero;
            isReset = true;
        }
    }

    void OnDrag(Vector2 delta)
    {
        Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
        float dist = 0f;

        Vector3 currentPos = ray.GetPoint(dist);
        thumb.position = currentPos;

    }

    ///< 更新当前坐标到原点的向量
    void UpdateVector3FromOrigin(Vector3 pos)
    {
        OffsetFromOrigin = pos - originPos;
    }
}
