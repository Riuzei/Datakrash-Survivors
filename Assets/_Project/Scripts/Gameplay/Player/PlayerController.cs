using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public HeroBrain currentHero;
    
    private GamePrefabs gamePrefabs;
    private Vector2 moveInput;
    
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        currentHero.HandleInput(moveInput); 
    }
    
    void Start()
    {
        gamePrefabs = GameplayManager.Instance.gamePrefabs;
        GameObject go = Instantiate(gamePrefabs.playerPrefab, Vector3.zero, Quaternion.identity);
        currentHero = go.GetComponent<HeroBrain>();
    }
    
}
