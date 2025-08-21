using System.Drawing;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D NomalCursor;
    public Texture2D AttackCursor;
    public Texture2D BreathCursor;

    public enum Cursor_type { Nomal, Attack, Breath };

    public void SetCursor(Cursor_type cursor_type)
    {
        Cursor.SetCursor(GetCursorTexture2D(cursor_type), Vector2.zero, CursorMode.Auto);
    }

    private Texture2D GetCursorTexture2D(Cursor_type cursor_type)
    {
        Texture2D cursor = NomalCursor;
        switch(cursor_type)
        {
            case Cursor_type.Nomal:
                cursor = NomalCursor;
                break;
            case Cursor_type.Attack:
                cursor = AttackCursor;
                break;
            case Cursor_type.Breath:
                cursor = BreathCursor;
                break;
        }
        return cursor;
    }
}
