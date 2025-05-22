using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [SerializeField] private float checkRate = 0.05f;
    [SerializeField] private float lastCheckTime;
    [SerializeField] private float maxCheckDistance;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject curInteractGameObject;
    [SerializeField] private IInteractable curInteractable;
    [SerializeField] private TextMeshProUGUI promptText;
    private PlayerController playerController;
    private PlayerCondition playerCondition;
    private ItemData itemData;

    private Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        playerController = CharacterManager.Instance.Player.playerController;
        playerCondition = CharacterManager.Instance.Player.playerCondition;
        camera = Camera.main;

        CharacterManager.Instance.Player.useItem += UseItem;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    public void UseItem()
    {
        itemData = CharacterManager.Instance.Player.itemData;

        if (itemData.type == ItemType.Consumable)
        {
            for (int i = 0; i < itemData.consumables.Length; i++)
            {
                switch (itemData.consumables[i].type)
                {
                    case ConsumableType.Health:
                        playerCondition.Heal(itemData.consumables[i].value);
                        break;
                    case ConsumableType.Speed:
                        StartCoroutine(TemporaryChangeSpeed(itemData.consumables[i].value));
                        break;
                    case ConsumableType.Jump:
                        StartCoroutine(TemporaryChangeJump(itemData.consumables[i].value));
                        break;
                }
            }
        }
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }

    IEnumerator TemporaryChangeSpeed(float value)
    {
        float originalSpeed = playerController.MoveSpeed;
        playerController.ChangeSpeed(value);
        yield return new WaitForSeconds(5f);
        playerController.ChangeSpeed(-value);

    }

    IEnumerator TemporaryChangeJump(float value)
    {
        float originalJumpPower = playerController.JumpPower;
        playerController.ChangeJumpPower(value);
        yield return new WaitForSeconds(5f);
        playerController.ChangeJumpPower(-value);

    }
}
