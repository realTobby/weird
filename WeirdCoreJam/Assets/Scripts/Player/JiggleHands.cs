using UnityEngine;

public class JiggleBodyParts : MonoBehaviour
{
    public RectTransform rightHand;
    public RectTransform leftHand;
    public RectTransform rightLeg;
    public RectTransform leftLeg;
    public float jiggleAmount = 10f;
    public float jiggleSpeed = 10f;

    private Vector2 rightHandOriginalPos;
    private Vector2 leftHandOriginalPos;
    private Vector2 rightLegOriginalPos;
    private Vector2 leftLegOriginalPos;
    private bool isMoving;

    void Start()
    {
        rightHandOriginalPos = rightHand.anchoredPosition;
        leftHandOriginalPos = leftHand.anchoredPosition;
        rightLegOriginalPos = rightLeg.anchoredPosition;
        leftLegOriginalPos = leftLeg.anchoredPosition;
    }

    void Update()
    {
        if (isMoving)
        {
            float jiggleOffset = Mathf.Sin(Time.time * jiggleSpeed) * jiggleAmount;
            // Animate hands
            rightHand.anchoredPosition = rightHandOriginalPos + new Vector2(jiggleOffset, 0);
            leftHand.anchoredPosition = leftHandOriginalPos + new Vector2(jiggleOffset, 0);
            // Animate legs
            rightLeg.anchoredPosition = rightLegOriginalPos + new Vector2(0, jiggleOffset);
            leftLeg.anchoredPosition = leftLegOriginalPos + new Vector2(0, jiggleOffset);
        }
        else
        {
            rightHand.anchoredPosition = rightHandOriginalPos;
            leftHand.anchoredPosition = leftHandOriginalPos;
            rightLeg.anchoredPosition = rightLegOriginalPos;
            leftLeg.anchoredPosition = leftLegOriginalPos;
        }
    }

    public void SetMoving(bool moving)
    {
        isMoving = moving;
    }
}
