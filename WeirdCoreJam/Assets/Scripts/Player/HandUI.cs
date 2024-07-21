using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandUI : MonoBehaviour
{
    // Hand jiggle components
    public RectTransform rightHand;
    public RectTransform leftHand;
    public float jiggleAmount = 10f;
    private Vector2 rightHandOriginalPos;
    private Vector2 leftHandOriginalPos;

    // Leg jiggle components
    public Transform rightLeg;
    public Transform leftLeg;
    public float legJiggleAmount = .7f; // Smaller jiggle amount for legs
    private Vector3 rightLegOriginalPos;
    private Vector3 leftLegOriginalPos;

    private PlayerState playerState;
    public PlayerMovement playerMov;
    public float JiggleFactor;

    public GameObject InteractionPrompt;

    // Start is called before the first frame update
    void Start()
    {
        playerState = GetComponent<PlayerState>();
        playerMov = GetComponent<PlayerMovement>();

        // Initialize hand positions
        rightHandOriginalPos = rightHand.anchoredPosition;
        leftHandOriginalPos = leftHand.anchoredPosition;

        // Initialize leg positions
        rightLegOriginalPos = rightLeg.localPosition;
        leftLegOriginalPos = leftLeg.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerState.isMoving)
        {
            // Hands jiggle
            float jiggleOffsetRightHand = Mathf.Sin(Time.time * playerMov.currentSpeed * JiggleFactor + 1) * jiggleAmount;
            float jiggleOffsetLeftHand = Mathf.Sin(Time.time * playerMov.currentSpeed * JiggleFactor - 1) * jiggleAmount;

            rightHand.anchoredPosition = rightHandOriginalPos + new Vector2(jiggleOffsetRightHand, 0);
            leftHand.anchoredPosition = leftHandOriginalPos + new Vector2(jiggleOffsetLeftHand, 0);

            // Legs jiggle
            float jiggleOffsetRightLeg = Mathf.Sin(Time.time * playerMov.currentSpeed * JiggleFactor - 1) * legJiggleAmount;
            float jiggleOffsetLeftLeg = Mathf.Sin(Time.time * playerMov.currentSpeed * JiggleFactor + 1) * legJiggleAmount;

            rightLeg.localPosition = rightLegOriginalPos + new Vector3(Mathf.Clamp(jiggleOffsetRightLeg, -legJiggleAmount, legJiggleAmount), 0, 0);
            leftLeg.localPosition = leftLegOriginalPos + new Vector3(Mathf.Clamp(jiggleOffsetLeftLeg, -legJiggleAmount, legJiggleAmount), 0, 0);
        }
        else
        {
            rightHand.anchoredPosition = rightHandOriginalPos;
            leftHand.anchoredPosition = leftHandOriginalPos;

            rightLeg.localPosition = rightLegOriginalPos;
            leftLeg.localPosition = leftLegOriginalPos;
        }
    }
}
