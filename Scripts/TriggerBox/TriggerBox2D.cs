using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class TriggerBox2D : MonoBehaviour
{
    public LayerMask layerMask;
    private BoxCollider2D boxCollider2D;
    protected virtual void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = true;
    }

    public void SetLayerMask(LayerMask layerMask) => this.layerMask = layerMask;


    void OnTriggerEnter2D(Collider2D collision) => OntriggerEvent2D(collision);

    void OntriggerEvent2D(Collider2D collision)
    {
        if (!ValidateTrigger(collision))
        {
            return;
        }

        OnTrigger(collision);
    }
    protected virtual bool ValidateTrigger(Collider2D collision)
    {
        return layerMask.CompareWithLayerIndex(collision.gameObject.layer);
    }

    protected abstract void OnTrigger(Collider2D collision);
}