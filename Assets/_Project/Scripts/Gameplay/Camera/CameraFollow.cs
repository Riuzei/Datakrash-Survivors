using PrimeTween;
using UnityEngine;

public class CameraFollow : MonoBehaviour, IUpdatable
{
    public Transform target;
    
    [SerializeField] private float smoothTime = 0.25f;
    [SerializeField] private Vector3 offset = new(0f, 0f, -2f);

    private Vector3 velocity = Vector3.zero;
    void Start()
    {
        GameplayManager.OnPlayerHit += ShakeCamera;
        GameplayManager.Instance.Register(this);
    }
    void OnDisable()
    {
        GameplayManager.Instance.Unregister(this);        
    }
    void ShakeCamera()
    {
        Tween.ShakeCamera(Camera.main, 0.8f, 0.3f, 8);
    }
    public void OnUpdate(float deltaTime){}
    public void OnFixedUpdate(float fixedDeltaTime)
    {
        if (target == null) target = GameplayManager.Instance.player.currentHero.transform; 

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
    }
}