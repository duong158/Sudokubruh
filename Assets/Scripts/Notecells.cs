using UnityEngine;
using TMPro;
public class Notecells : MonoBehaviour
{
    [SerializeField] private TMP_Text notevalue;
    [SerializeField] private Color _basecolor;
    [SerializeField] private Color _color;
    public void UpdateNoteValue(int value)
    {
        //notevalue.color = _basecolor;
        int Value = int.Parse(notevalue.text);
        if (value == Value)
        {
            notevalue.color = _color;
        }
    }
    public void Reset()
    {
        notevalue.color = _basecolor;
    }
}
