using UnityEngine;

public class SnailController : MonoBehaviour
{
    public Transform PathContainer;
    public Rigidbody2D[] _rigidbodies;
    public Transform Target;
    public Rigidbody2D Rigidbody2D;
    
    private Transform[] _path;
    private int _targetIndex;
    private int _currentIndex;

    private void Awake()
    {
        var len = PathContainer.childCount;
        _path = new Transform[len];
        for (var i = 0; i < len; i++)
        {
            _path[i] = PathContainer.GetChild(i);
        }
    }
    
    private void Start()
    {
        transform.position = _path[0].position;
        Target = _path[++_targetIndex];
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            
            if (_targetIndex < _path.Length)
            {
                var nextPos = _path[_targetIndex].position;
                var dir = nextPos - _path[_targetIndex - 1].position;
                //Debug.Log(dir);
                transform.position = transform.position + dir.normalized * 2f * Time.deltaTime;
                if (Vector3.Distance(nextPos, transform.position) <= 0.01f)
                {
                    _targetIndex++;
                    Debug.Log(_targetIndex);
                    //LookAt2D(_path[_targetIndex -1], _path[_targetIndex]);
                    Target = _path[_targetIndex];
                }
            }
            

            /*
            foreach (var r in _rigidbodies)
            {
                r.velocity = Vector2.left / 4f;
            }
            */
        }
        else
        {
            /*
            foreach (var r in _rigidbodies)
            {
                r.velocity = Vector2.zero;
            }
            */
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            
        }
    }
    
    public void LookAt2D(Transform from, Transform target)
    {
        var dir = (target.position - from.position).normalized;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle-180f, Vector3.forward);
    }
}
