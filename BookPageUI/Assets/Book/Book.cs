using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour {
    [SerializeField] Page _pageTurner;
    [SerializeField] RectTransform[] _pages;
    [SerializeField] float _turnSpeed = 0.1f;
    [SerializeField] AnimationCurve _speedCurve;

    int _currentPage = 0;
    public int _targetPage = 0;

    float _currentT;

    private void Start() {
        PageUpdate();
    }

    [ContextMenu("GO NEXT")]
    public void NextPage() {
        _currentT = 0;
        PageUpdate();

        _targetPage += 2;
    }

    [ContextMenu("GO BACK")]
    public void PreviousPage() {
        _currentT = 1;
        PageUpdate();

        _targetPage -= 2;
    }

    RectTransform[] allPages = new RectTransform[6];
    void PageUpdate() {
        // Need to check whether turning back/forward
        // Shift by 2 

        for (int i = 0; i < allPages.Length; i++) {
            int i2 = (i + _currentPage) - 2;
            allPages[i] = i2 >= 0 && i2 < _pages.Length ? _pages[i2] : null;
        }
        _pageTurner.SetContentArray(allPages);
    }

    private void Update() {
        if (_currentPage != _targetPage) {
            bool pageInc = _currentT <= _targetPage;

            _pageTurner.UpdateT(_speedCurve.Evaluate(_currentT));
            _currentT += (Time.deltaTime * _turnSpeed) * (pageInc ? 1f : -1f);

            if ((pageInc && _currentT >= 1) || (!pageInc && _currentT <= 0)) {
                Debug.Log("Page flipped");
                _currentPage += (_currentPage < _targetPage) ? 2 : -2;
            }
        }
    }
}
