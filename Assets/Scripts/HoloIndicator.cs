using UnityEngine;

public class HoloIndicator : MonoBehaviour
{
    public static GravitySide GravitySideEnum = GravitySide.NORMAL;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject holoCharacter;
    [SerializeField] private GravitySide gravityType;
    [SerializeField] private bool canSwitchCharacter;
    private float playerPositionZ;
    private bool canManipulate = false;
    void Start()
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            canSwitchCharacter = !canSwitchCharacter;
            holoCharacter.SetActive(canSwitchCharacter);
            canManipulate = true;

        }
        SetHoloPosition();
    }

    private void SetPlayerPosition(GravitySide Gravity)
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("enter " + Gravity);
            playerMovement.Player.transform.SetPositionAndRotation(holoCharacter.transform.position, holoCharacter.transform.rotation);
            holoCharacter.SetActive(false);
            GravitySideEnum = Gravity;
        }
    }

    private void SetHoloPosition()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow) && canManipulate)
        {
            playerPositionZ = 90;
            gravityType = GravitySide.RIGHT;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && canManipulate)
        {
            playerPositionZ = -90;
            gravityType = GravitySide.LEFT;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && canManipulate)
        {
            playerPositionZ = 0;
            gravityType = GravitySide.NORMAL;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && canManipulate)
        {
            Debug.Log("W pressed");
            playerPositionZ = 180;
            gravityType = GravitySide.UP;
        }
        holoCharacter.transform.rotation = Quaternion.Euler(0, 0, playerPositionZ);
        SetPlayerPosition(gravityType);
    }
}
