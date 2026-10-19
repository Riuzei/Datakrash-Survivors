using System;
using PrimeTween;
using UnityEngine;

public class UIAnimSequencer : MonoBehaviour
{
    [Serializable]
    private struct Entry
    {
        public UIAnimator element;
        public float enterDelay;
        public float exitDelay;
    }

    [SerializeField] private Entry[] animEntries;
    [SerializeField] private bool exitInSameOrder = false;
    [SerializeField] private bool animateOnEnable = true;
    void OnEnable()
    {
        if(animateOnEnable)
        {
            PlayEnter();
        }
    }
    public void PlayEnter(Action onComplete = null)
    {
        foreach (Entry entry in animEntries)
            entry.element.gameObject.SetActive(false);
        ResetAll();
        Sequence seq = Sequence.Create();
        foreach (Entry entry in animEntries)
        {
            Entry captured = entry;
            if (entry.enterDelay > 0f) seq.ChainDelay(entry.enterDelay);
            seq.ChainCallback(() => captured.element.gameObject.SetActive(true));
        }
        if (onComplete != null)
            seq.ChainCallback(() => onComplete());
    }

    public void PlayExit(Action onComplete = null)
    {
        Sequence seq = Sequence.Create();
        float longestExit = 0f;
        int start = exitInSameOrder ? 0 : animEntries.Length - 1;
        int end = exitInSameOrder ? animEntries.Length : -1;
        int step = exitInSameOrder ? 1 : -1;
        for (int i = start; i != end; i += step)
        {
            Entry captured = animEntries[i];
            float exitEnd = captured.exitDelay + captured.element.Duration;
            if (exitEnd > longestExit) longestExit = exitEnd;
            if (captured.exitDelay > 0f) seq.ChainDelay(captured.exitDelay);
            seq.ChainCallback(() => captured.element.PlayExit());
        }
        if (longestExit > 0f) seq.ChainDelay(longestExit);
        seq.ChainCallback(() =>
        {
            foreach (Entry e in animEntries)
                e.element.gameObject.SetActive(false);
            onComplete?.Invoke();
        });
    }

    public void ResetAll()
    {
        foreach (Entry entry in animEntries) entry.element.ResetState();
    }
    
    public void AddEntry(UIAnimator _element, int index, float _enterDelay, float _exitDelay)
    {
        Entry entry = new Entry { element = _element, enterDelay = _enterDelay, exitDelay = _exitDelay};
        animEntries[index] = entry;
    }
    public void SetEntries(int count)
    {
        animEntries = new Entry[count];
    }
}
