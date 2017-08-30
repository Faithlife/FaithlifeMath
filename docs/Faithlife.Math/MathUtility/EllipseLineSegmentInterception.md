# MathUtility.EllipseLineSegmentInterception method (1 of 2)

Gets the points, if any, where a line segment intersects an ellipse centered at the origin.

```csharp
public static IEnumerable<ValueTuple<double, double>> EllipseLineSegmentInterception(double fSemimajorAxis, 
    double fSemiminorAxis, double fXOne, double fYOne, double fXTwo, double fYTwo)
```

| parameter | description |
| --- | --- |
| fSemimajorAxis | Semimajoraxis of the ellipse. |
| fSemiminorAxis | Semiminoraxis of the ellipse. |
| fXOne | X coordinate of first point on the line. |
| fYOne | Y coordinate of first point on the line. |
| fXTwo | X coordinate of second point on the line. |
| fYTwo | Y coordinate of second point on the line. |

## See Also

* class [MathUtility](../MathUtility.md)
* namespace [Faithlife.Math](../../Faithlife.Math.md)

---

# MathUtility.EllipseLineSegmentInterception method (2 of 2)

Gets the points, if any, where a line segment intersects an ellipse centered at an arbitrary point.

```csharp
public static IEnumerable<ValueTuple<double, double>> EllipseLineSegmentInterception(double fSemimajorAxis, 
    double fSemiminorAxis, double fCenterX, double fCenterY, double fXOne, double fYOne, double fXTwo, double fYTwo)
```

| parameter | description |
| --- | --- |
| fSemimajorAxis | Semimajoraxis of the ellipse. |
| fSemiminorAxis | Semiminoraxis of the ellipse. |
| fCenterX | X coordinate of center of the ellipse. |
| fCenterY | Y coordinate of first point on the line. |
| fXOne | X coordinate of first point on the line. |
| fYOne | Y coordinate of first point on the line. |
| fXTwo | X coordinate of second point on the line. |
| fYTwo | Y coordinate of second point on the line. |

## See Also

* class [MathUtility](../MathUtility.md)
* namespace [Faithlife.Math](../../Faithlife.Math.md)

<!-- DO NOT EDIT: generated by xmldocmd for Faithlife.Math.dll -->