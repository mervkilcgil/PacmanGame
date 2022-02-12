using UnityEngine;

public static class Vector2Extension 
{

    public static Vector2 PerpendicularClockwise(this Vector2 vector2)
    {
        return Quaternion.Euler(0, 0, 90f) * vector2;
    }
    
    public static Vector2 PerpendicularCounterClockwise(this Vector2 vector2)
    {
        return Quaternion.Euler(0, 0, -90f) * vector2;
    }
    
    public static Vector2 Rotate(this Vector2 vector2, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * vector2;
    }
    
    public static Vector2 GetMiddlePoint(this Vector2 p1, Vector2 p2)
    {
        return new Vector2((p1.x + p2.x) / 2, (p1.y + p2.y) / 2);
    }
    
    public static bool IsInBetween(this Vector2 point, Vector2 p1, Vector2 p2)
    {
        float angleArea = Vector3.Angle(p1, p2);
        float p1Area =  Vector3.Angle(p1, point), p2Area = Vector3.Angle(p2, point);
        return p1Area < angleArea && p2Area < angleArea;
    }
    
}
