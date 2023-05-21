using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject newObjectPrefab;
    public GameObject[] objectsToChangeColor;

    private int itemsConsumed = 0;
    private Color[] colors = { Color.red, new Color(1, 0.5f, 0), Color.yellow, Color.green, Color.blue, Color.cyan, Color.magenta };

    public void ItemConsumed()
    {
        if (itemsConsumed < colors.Length && itemsConsumed < objectsToChangeColor.Length)
        {
            var renderer = objectsToChangeColor[itemsConsumed].GetComponent<SpriteRenderer>();
            renderer.color = colors[itemsConsumed];
        }
        itemsConsumed++;

        if (itemsConsumed >= 7)
        {
            // 오브젝트 생성위치
            Instantiate(newObjectPrefab, new Vector2(0, 0), Quaternion.identity);
        }
    }
}
