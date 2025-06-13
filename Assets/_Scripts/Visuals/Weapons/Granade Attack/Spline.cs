using UnityEngine;

/// <summary>
/// Simple spline implementation for the granade mechanic
/// </summary>
public class Spline : MonoBehaviour
{
    [SerializeField]
    private Transform _start, _middle, _end;

    public Transform Start { get => _start;}
    public Transform Middle { get => _middle;}
    public Transform End { get => _end; }

    [SerializeField]
    private bool showGizmos = true;

    private void Awake()
    {
        Debug.Assert(_start != null && _middle != null && _end != null, "YOu need to assign all the fields for the spline to work!", gameObject);
    }

    public Vector2 CalculatePosition(float interpolationAmount01)
    {
        interpolationAmount01 = Mathf.Clamp01(interpolationAmount01);
        Vector2 startMiddle = Vector2.Lerp(_start.position,_middle.position, interpolationAmount01);
        Vector2 middleEnd = Vector2.Lerp(_middle.position,_end.position, interpolationAmount01);
        return Vector2.Lerp(startMiddle, middleEnd, interpolationAmount01);
    }

    private void OnDrawGizmos()
    {
        if (showGizmos && _start != null && _middle != null && _end != null) 
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_start.position, 0.1f);
            Gizmos.DrawSphere(_end.position, 0.1f);
            Gizmos.DrawSphere(_middle.position, 0.1f);
            Gizmos.color = Color.magenta;
            int granularity = 5;
            for (int i = 0; i < granularity; i++)
            {
                Vector2 startPoint = i == 0 ? _start.position : CalculatePosition(i / (float)granularity);
                Vector2 endPoint = i == granularity ? _end.position : CalculatePosition((i+1) / (float)granularity);
                Gizmos.DrawLine(startPoint, endPoint);
            }
            
        }
    }
}
