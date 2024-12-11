using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class Cell : MonoBehaviour
{
    [SerializeField] public List<Notecells> notes;


    [HideInInspector] public int Value;
    [HideInInspector] public int Row;
    [HideInInspector] public int Col;
    [HideInInspector] public bool IsLocked;
    [HideInInspector] public bool IsIncorrect;
    [HideInInspector] public bool IsSimilar;

    [SerializeField] private SpriteRenderer _bgSprite;
    [SerializeField] private TMP_Text _valueText;
    [Space]
    [Header("Start")]
    [SerializeField] private Color _startUnlockedCellColor;
    [SerializeField] private Color _startLockedCellColor;
    [Space]
    [Header("Highlight")]
    [SerializeField] private Color _highlightLockedCellColor;
    [SerializeField] private Color _highlightUnlockedCellColor;
    [SerializeField] private Color _highlightWrongCellColor;
    [SerializeField] private Color _highlightSimilarCellColor;
    [Space]
    [Header("Selected")]
    [SerializeField] private Color _selectedCellColor;
    [SerializeField] private Color _selectedWrongCellColor;
    [Space]
    [Header("Reset")]
    [SerializeField] private Color _resetCellColor;
    [SerializeField] private Color _resetWrongCellColor;

    public void Init(int value)
    {
        IsIncorrect = false;
        Value = value;

        if (value == 0)
        {
            IsLocked = false;
            _bgSprite.color = _startUnlockedCellColor;
            _valueText.text = "";

        }
        else 
        { 
            IsLocked = true; 
            _bgSprite.color= _startLockedCellColor;
            _valueText.text = Value.ToString();
        }
    }
    public void Highlight()
    {
        if(IsSimilar)
        {
            _bgSprite.color = _highlightSimilarCellColor;
        }
        else if (IsLocked)
        {
            _bgSprite.color = _highlightLockedCellColor;
        }
        else 
        {
            if (IsIncorrect)
            {
                _bgSprite.color = _highlightWrongCellColor;
            }
            else
            {
                _bgSprite.color = _highlightUnlockedCellColor;
            }
        }
    }

    public void Select()
    {
        if (IsIncorrect)
        {
            _bgSprite.color = _selectedWrongCellColor;
        }
        else
        {
            _bgSprite.color = _selectedCellColor;
        }
    }
    public void Reset()
    {
        if (IsLocked)
        {
            _bgSprite.color = _startLockedCellColor;
        }
        else
        {
            if (IsIncorrect)
            {
                _bgSprite.color = _resetWrongCellColor;
            }
            
            else
            {
                _bgSprite.color = _resetCellColor;
            }
        }
        
    }

    public void UpdateValue(int value)
    {
        Value = value;
        _valueText.text = Value == 0? "" : Value.ToString();
    }

    public void UpdateWin()
    {
        _bgSprite.color = _startLockedCellColor;
    }
}
