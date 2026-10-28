
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    public GameObject blockPrefab;
    public int maxSize = 5;
    public float blockHeight = 0.16f;

    private List<GameObject> blocks = new List<GameObject>();
    private bool isAnimating = false;

    public void Push()
    {
        if (isAnimating) return;

        if (blocks.Count >= maxSize)
        {
            Debug.Log("Stack Overflow!");
            return;
        }

        StartCoroutine(PushBlock());
    }

    IEnumerator PushBlock()
    {
        isAnimating = true;

        float y = 0.08f + blocks.Count * blockHeight;

        GameObject block = Instantiate(blockPrefab, transform);
        Vector3 target = new Vector3(0, y, 0);
        Vector3 start = target + Vector3.up * 0.4f;

        block.transform.localPosition = start;

        float time = 0f;

        while (time < 0.4f)
        {
            time += Time.deltaTime;
            block.transform.localPosition =
                Vector3.Lerp(start, target, time / 0.4f);

            yield return null;
        }

        block.transform.localPosition = target;
        blocks.Add(block);

        Debug.Log("PUSH: " + blocks.Count);
        isAnimating = false;
    }

    public void Pop()
    {
        if (isAnimating) return;

        if (blocks.Count == 0)
        {
            Debug.Log("Stack Underflow!");
            return;
        }

        StartCoroutine(PopBlock());
    }

    IEnumerator PopBlock()
    {
        isAnimating = true;

        int last = blocks.Count - 1;
        GameObject block = blocks[last];

        Vector3 start = block.transform.localPosition;
        Vector3 target = start + Vector3.up * 0.4f;

        float time = 0f;

        while (time < 0.4f)
        {
            time += Time.deltaTime;
            block.transform.localPosition =
                Vector3.Lerp(start, target, time / 0.4f);

            yield return null;
        }

        blocks.RemoveAt(last);
        Destroy(block);

        Debug.Log("POP completed");
        isAnimating = false;
    }

    public void Peek()
    {
        if (blocks.Count == 0)
        {
            Debug.Log("Stack is empty");
            return;
        }

        Debug.Log("Top block: " + blocks.Count);
    }
}
