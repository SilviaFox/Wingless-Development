using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DamageNumbers : MonoBehaviour
{
    [SerializeField] GameObject numText;
    [SerializeField] float offsetY = 50;
    [SerializeField] float moveSpeed = 10;
    [SerializeField] float growSpeed = 0.5f;

    [SerializeField] DamageNumberInfo[] damageNumberInfo = new DamageNumberInfo[4];

    [SerializeField] float timeUntilDestroy = 0.8f;

    Canvas thisCanvas;

    private void Start()
    {
        thisCanvas = GetComponent<Canvas>();
    }

    // This is triggered when an enemy gets hit
    public void OnHit(Vector2 enemyPosition, float damage)
    {
        // Instantiate the Damage Numbers
        GameObject numbers = Instantiate(numText);
        // Set Parent to this Canvas
        numbers.transform.SetParent(transform);
        // Set the text to the amount of damage
        TextMeshProUGUI numberText = numbers.GetComponent<TextMeshProUGUI>();
        numberText.text = damage.ToString();

        switch (damage)
        {
            case var expression when damage < 15:
                numberText.color = damageNumberInfo[0].fontColor;
                numberText.fontSize = damageNumberInfo[0].fontSize;
            break;
            case var expression when damage < 25:
                numberText.color = damageNumberInfo[1].fontColor;
                numberText.fontSize = damageNumberInfo[1].fontSize;
            break;
            case var expression when damage < 35:
                numberText.color = damageNumberInfo[2].fontColor;
                numberText.fontSize = damageNumberInfo[2].fontSize;
            break;
            default:
                numberText.color = damageNumberInfo[3].fontColor;
                numberText.fontSize = damageNumberInfo[3].fontSize;
            break;
        }
        // Move position of the numbers to the enemy
        numbers.transform.position = worldToUISpace(thisCanvas, enemyPosition) + new Vector3(0, offsetY);
        StartCoroutine(MoveAndDestroy(numbers));
    }

    IEnumerator MoveAndDestroy(GameObject numbers)
    {
        float destroyTime = Time.time + timeUntilDestroy;
        // Get the text so we can make it grow over time
        TextMeshProUGUI numberText = numbers.GetComponent<TextMeshProUGUI>();
        while (destroyTime > Time.time)
        {
            numbers.transform.position = numbers.transform.position + new Vector3(0, moveSpeed);
            numberText.fontSize += growSpeed;
            yield return new WaitForFixedUpdate();
        }
        Destroy(numbers);
    }

    public Vector3 worldToUISpace(Canvas parentCanvas, Vector3 worldPos)
    {
        //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 movePos;

        //Convert the screenpoint to ui rectangle local point
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
        //Convert the local point to world point
        return parentCanvas.transform.TransformPoint(movePos);
    }
}
