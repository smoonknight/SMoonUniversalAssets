using UnityEngine;

public class WorldScreenLayout : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector2 offset;

    public void Initialize(Transform target) => Initialize(target, Vector2.zero);
    public void Initialize(Transform target, Vector2 offset)
    {
        this.target = target;
        this.offset = offset;
        Validate();
    }

    private void Update()
    {
        if (target == null)
        {
            return;
        }
        Validate();
    }

    void Validate()
    {
        transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, target.position) + offset;
    }
}