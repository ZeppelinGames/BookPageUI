// Based off work from https://gnupart.tistory.com/m/entry/Unity3D-Simple-Page-Curl-Effect

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Page : MonoBehaviour {

    [Header("Rect Elements")]
    [SerializeField] RectTransform _front;
    [SerializeField] RectTransform _back;
    [SerializeField] RectTransform _mask;
    [SerializeField] RectTransform _gradShadow;

    [Header("Content Elements")]
    [SerializeField] RectTransform _backContent;
    [SerializeField] RectTransform _frontContent;
    [SerializeField] RectTransform _prevPageContent;
    [SerializeField] RectTransform _nextPageContent;

    [SerializeField][Range(0, 1)] float t = 0.0f;
    [SerializeField] float arcHeight = 50.0f;

    float _frontWidth;

    private void Awake() {
        _frontWidth = _front.sizeDelta.x;
    }

    void LateUpdate() {
        t = Mathf.Clamp01(t);

        Vector3 frontPos = GetFrontPosition(1 - t);
        _front.localPosition = frontPos;

        HandleRotationAndMasking(1 - t);
        ZeroBack();
    }

    void ZeroBack() {
        _back.position = transform.position;
        _back.eulerAngles = Vector3.zero;
    }

    Vector3 GetFrontPosition(float t) {
        float xPos = Mathf.Lerp(0, _frontWidth * 2f, t);
        float yPos = arcHeight * Mathf.Sin(t * Mathf.PI);
        return new Vector3(xPos, yPos, 0);
    }

    void HandleRotationAndMasking(float t) {
        Vector3 pos = _front.localPosition;
        float theta = Mathf.Atan2(pos.y, pos.x) * 180.0f / Mathf.PI;
        theta = Mathf.Clamp(theta, 0, 90f);

        float deg = -(90.0f - theta) * 2.0f;
        _front.eulerAngles = new Vector3(0.0f, 0.0f, deg);

        Vector3 mid = _front.localPosition * 0.5f;
        _mask.localPosition = mid;
        _mask.eulerAngles = new Vector3(0.0f, 0.0f, deg * 0.5f);

        _gradShadow.position = _mask.position;
        _gradShadow.eulerAngles = new Vector3(0.0f, 0.0f, deg * 0.5f + 90.0f);
    }

    public void UpdateT(float newT) {
        t = newT;
    }

    public RectTransform currentFront => _currentFront;
    public RectTransform currentBack => _currentBack;
    public RectTransform currentPrevious => _currentPrevious;
    public RectTransform currentNext => _currentNext;
    RectTransform _currentFront;
    RectTransform _currentBack;
    RectTransform _currentPrevious;
    RectTransform _currentNext;


    public void SetContentArray(RectTransform[] arr) {
        RectTransform[] contentParents = new RectTransform[6] {
            null,
            _prevPageContent,
            _frontContent,
            _backContent,
            _nextPageContent,
            null
        };
        RectTransform[] contentSets = new RectTransform[6] {
               null,
               _currentPrevious,
               _currentFront,
               _currentBack,
               _currentNext,
               null
        };
        for (int i = 0; i < contentParents.Length; i++) {
            SetContent(arr[i], contentParents[i], ref contentSets[i]);
        }
    }

    public void SetFrontContent(RectTransform frontContent) {
        SetContent(frontContent, _frontContent, ref _currentFront);
    }
    public void SetBackContent(RectTransform backContent) {
        SetContent(backContent, _backContent, ref _currentBack);
    }
    public void SetPreviousContent(RectTransform content) {
        SetContent(content, _prevPageContent, ref _currentPrevious);
    }
    public void SetNextContent(RectTransform content) {
        SetContent(content, _nextPageContent, ref _currentNext);
    }

    void SetContent(RectTransform content, RectTransform parent, ref RectTransform set) {
        if (set != null)
            set.gameObject.SetActive(false);

        set = content;

        if (content == null) {
            Debug.Log("Null content");
            return;
        }
        if(parent == null) {
            Debug.LogWarning("Null parent");
            return;
        }

        content.SetParent(parent, false);
        content.localPosition = Vector3.zero;
        content.localEulerAngles = Vector3.zero;
        content.gameObject.SetActive(true);
    }
}
