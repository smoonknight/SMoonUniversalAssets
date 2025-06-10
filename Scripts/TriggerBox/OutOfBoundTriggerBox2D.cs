using UnityEngine;

public partial class OutOfBoundTriggerBox2D : TriggerBox2D
{
    protected override void OnTrigger(Collider2D collision)
    {
        if (collision.TryGetComponent(out IOutOfBoundable component))
        {
            if (component.CheckOutOfBound())
            {
                component.OutOfBoundChangeLocation();
            }
        }
    }
}