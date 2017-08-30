using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace Faithlife.Math
{
	/// <summary>
	/// Provides static methods for uncommon mathematical functions.
	/// </summary>
	public static class MathUtility
	{
		/// <summary>
		/// Calculates the euclidian distance between two points.
		/// </summary>
		/// <param name="ptOne">The first point.</param>
		/// <param name="ptTwo">The second point.</param>
		/// <returns>The euclidian distance between the two points.</returns>
		public static double CalculateEuclidianDistance(MathPoint ptOne, MathPoint ptTwo)
		{
			return CalculateEuclidianDistance(ptOne.X, ptOne.Y, ptTwo.X, ptTwo.Y);
		}

		/// <summary>
		/// Calculates the euclidian distance between two points.
		/// </summary>
		/// <param name="fXOne">X coordinate of first point.</param>
		/// <param name="fYOne">Y coordinate of first point.</param>
		/// <param name="fXTwo">X coordinate of second point.</param>
		/// <param name="fYTwo">Y coordinate of second point.</param>
		/// <returns>The euclidian distance between the two points.</returns>
		public static double CalculateEuclidianDistance(double fXOne, double fYOne, double fXTwo, double fYTwo)
		{
			return Sqrt(Pow(fXTwo - fXOne, 2) + Pow(fYTwo - fYOne, 2));
		}

		/// <summary>
		/// Clamps the specified value between a minimum and maximum.
		/// </summary>
		/// <param name="fValue">The value.</param>
		/// <param name="fMin">The minimum allowed value.</param>
		/// <param name="fMax">The maximum allowed value.</param>
		/// <returns></returns>
		public static double Clamp(double fValue, double fMin, double fMax)
		{
			return fValue >= fMax ? fMax : fValue <= fMin ? fMin : fValue;
		}

		/// <summary>
		/// Converts degrees to radians.
		/// </summary>
		/// <param name="fDegrees">Degrees to be converted.</param>
		/// <returns>Number of radians.</returns>
		public static double DegreesToRadians(double fDegrees)
		{
			return (fDegrees / 180.0) * PI;
		}

		/// <summary>
		/// Gets the points of a line, if any, that intercept an ellipse centered at the origin.
		/// </summary>
		/// <param name="fSemimajorAxis">Semimajoraxis of the ellipse.</param>
		/// <param name="fSemiminorAxis">Semiminoraxis of the ellipse.</param>
		/// <param name="fLineSlope">Slope of the line.</param>
		/// <param name="fLineYIntercept">Y-intercept of the line.</param>
		/// <returns>Points of the line that intercept the ellipse.</returns>
		public static IEnumerable<ValueTuple<double, double>> EllipseLineIntercepts(double fSemimajorAxis, double fSemiminorAxis, double fLineSlope, double fLineYIntercept)
		{
			// Quadratic: 0 = Ax^2 + Bx + C, solve for x.
			double fA = (1 / Pow(fSemimajorAxis, 2)) + (Pow(fLineSlope, 2) / (Pow(fSemiminorAxis, 2)));
			double fB = (fLineSlope * fLineYIntercept) / Pow(fSemiminorAxis, 2);
			double fC = (Pow(fLineYIntercept, 2) / Pow(fSemiminorAxis, 2)) - 1;

			Func<double, double, double, double> rootPartQuadratic =
				(a, b, c) => Sqrt(Pow(b, 2) - (4 * a * c));

			double fFirstXCoordinate = (-fB + rootPartQuadratic(fA, fB, fC)) / (2 * fA);
			double fSecondXCoordinate = (-fB - rootPartQuadratic(fA, fB, fC)) / (2 * fA);

			double fFirstYCoordinate = (fLineSlope * fFirstXCoordinate) + fLineYIntercept;
			double fSecondYCoordinate = (fLineSlope * fSecondXCoordinate) + fLineYIntercept;

			// Rounding to 14 decimal places to sneak by rounding errors.
			const int fPrecision = 14;

			if (PointIsOnEllipse(fSemimajorAxis, fSemiminorAxis, fFirstXCoordinate, fFirstYCoordinate, fPrecision))
				yield return new ValueTuple<double, double>(fFirstXCoordinate, fFirstYCoordinate);

			if (PointIsOnEllipse(fSemimajorAxis, fSemiminorAxis, fSecondXCoordinate, fSecondYCoordinate, fPrecision))
				yield return new ValueTuple<double, double>(fSecondXCoordinate, fSecondYCoordinate);
		}

		/// <summary>
		/// Gets the points, if any, where a line segment intersects an ellipse centered at an arbitrary point.
		/// </summary>
		/// <param name="fSemimajorAxis">Semimajoraxis of the ellipse.</param>
		/// <param name="fSemiminorAxis">Semiminoraxis of the ellipse.</param>
		/// <param name="fCenterX">X coordinate of center of the ellipse.</param>
		/// <param name="fCenterY">Y coordinate of first point on the line.</param>
		/// <param name="fXOne">X coordinate of first point on the line.</param>
		/// <param name="fYOne">Y coordinate of first point on the line.</param>
		/// <param name="fXTwo">X coordinate of second point on the line.</param>
		/// <param name="fYTwo">Y coordinate of second point on the line.</param>
		/// <returns></returns>
		public static IEnumerable<ValueTuple<double, double>> EllipseLineSegmentInterception(double fSemimajorAxis, double fSemiminorAxis, double fCenterX, double fCenterY,
			double fXOne, double fYOne, double fXTwo, double fYTwo)
		{
			// shift the coordinates so the center of the ellipse is at 0,0 for the call to the overload
			return EllipseLineSegmentInterception(fSemimajorAxis, fSemiminorAxis, fXOne - fCenterX, fYOne - fCenterY, fXTwo - fCenterX, fYTwo - fCenterY)
				.Select(tuple => ValueTuple.Create(tuple.Item1 + fCenterX, tuple.Item2 + fCenterY));
		}

		/// <summary>
		/// Gets the points, if any, where a line segment intersects an ellipse centered at the origin.
		/// </summary>
		/// <param name="fSemimajorAxis">Semimajoraxis of the ellipse.</param>
		/// <param name="fSemiminorAxis">Semiminoraxis of the ellipse.</param>
		/// <param name="fXOne">X coordinate of first point on the line.</param>
		/// <param name="fYOne">Y coordinate of first point on the line.</param>
		/// <param name="fXTwo">X coordinate of second point on the line.</param>
		/// <param name="fYTwo">Y coordinate of second point on the line.</param>
		/// <returns></returns>
		public static IEnumerable<ValueTuple<double, double>> EllipseLineSegmentInterception(double fSemimajorAxis, double fSemiminorAxis, double fXOne, double fYOne, double fXTwo, double fYTwo)
		{
			double fLineSlope;
			double fLineYIntercept;
			GetLineEquation(fXOne, fYOne, fXTwo, fYTwo, out fLineSlope, out fLineYIntercept);

			// If the slope is infinity, rotate clockwise and try again.
			if (double.IsInfinity(fLineSlope))
			{
				IEnumerable<ValueTuple<double, double>> points = EllipseLineSegmentInterception(fSemiminorAxis, fSemimajorAxis, fYOne, -fXOne, fYTwo, -fXTwo);

				foreach (ValueTuple<double, double> p in points)
					yield return new ValueTuple<double, double>(-p.Item2, p.Item1);

				yield break;
			}

			IEnumerable<ValueTuple<double, double>> listXValues = EllipseLineIntercepts(fSemimajorAxis, fSemiminorAxis, fLineSlope, fLineYIntercept);

			foreach (ValueTuple<double, double> fX in listXValues)
			{
				// Only need part of the line.
				if (!((fX.Item1 >= fXOne && fX.Item1 <= fXTwo) || (fX.Item1 >= fXTwo && fX.Item1 <= fXOne)))
					continue;

				if (!((fX.Item2 >= fYOne && fX.Item2 <= fYTwo) || (fX.Item2 >= fYTwo && fX.Item2 <= fYOne)))
					continue;

				// Rounding to 14 decimal places to sneak by rounding errors.
				yield return new ValueTuple<double, double>(fX.Item1, fX.Item2);
			}
		}

		/// <summary>
		/// Gets the equation of a line given two points on the line.
		/// </summary>
		/// <param name="fXOne">X coordinate of first point on the line.</param>
		/// <param name="fYOne">Y coordinate of first point on the line.</param>
		/// <param name="fXTwo">X coordinate of second point on the line.</param>
		/// <param name="fYTwo">Y coordinate of second point on the line.</param>
		/// <param name="fSlope">Slope of the line.</param>
		/// <param name="fYIntercept">Y-intercept of the line.</param>
		public static void GetLineEquation(double fXOne, double fYOne, double fXTwo, double fYTwo, out double fSlope, out double fYIntercept)
		{
			fSlope = (fYTwo - fYOne) / (fXTwo - fXOne);
			fYIntercept = fYOne - (fSlope * fXOne);
		}

		/// <summary>
		/// Determines if a point is on an ellipse centered at the origin.
		/// </summary>
		/// <param name="fSemimajorAxis">Semimajoraxis of the ellipse.</param>
		/// <param name="fSemiminorAxis">Semiminoraxis of the ellipse.</param>
		/// <param name="fPointX">X coordinate of the point.</param>
		/// <param name="fPointY">Y coordinate of the point.</param>
		/// <param name="precision">The rounding precision in decimal places to use while comparing calculations.</param>
		/// <returns>True, if the point is on the ellipse.</returns>
		public static bool PointIsOnEllipse(double fSemimajorAxis, double fSemiminorAxis, double fPointX, double fPointY, int precision)
		{
			return Round(Pow(fPointX / fSemimajorAxis, 2) + Pow(fPointY / fSemiminorAxis, 2), precision) == 1;
		}

		/// <summary>
		/// Converts radians to degrees.
		/// </summary>
		/// <param name="fRadians">Radians to be converted.</param>
		/// <returns>Number of degrees.</returns>
		public static double RadiansToDegrees(double fRadians)
		{
			return (fRadians / PI) * 180.0;
		}

		/// <summary>
		/// Calculates the shortest distance between to points on a the surface of a sphere.
		/// </summary>
		/// <param name="fRadius">Radius of the sphere.</param>
		/// <param name="fLatitudeOne">Latitude of the first point in degrees.</param>
		/// <param name="fLongitudeOne">Longitude of the first point in degrees.</param>
		/// <param name="fLatitudeTwo">Latitude of the second point in degrees.</param>
		/// <param name="fLongitudeTwo">Longitude of the second poin in degrees.</param>
		/// <returns>The shortest distance between the two points.</returns>
		public static double GreatCircleDistance(double fRadius, double fLatitudeOne, double fLongitudeOne, double fLatitudeTwo, double fLongitudeTwo)
		{
			// convert Latitude and Longitude to radians
			fLatitudeOne = DegreesToRadians(fLatitudeOne);
			fLongitudeOne = DegreesToRadians(fLongitudeOne);
			fLatitudeTwo = DegreesToRadians(fLatitudeTwo);
			fLongitudeTwo = DegreesToRadians(fLongitudeTwo);

			return fRadius * Acos(Sin(fLatitudeOne) * Sin(fLatitudeTwo) + Cos(fLatitudeOne) * Cos(fLatitudeTwo) * Cos(fLongitudeOne - fLongitudeTwo));
		}

		/// <summary>
		/// Calculates the intersection point of two lines.
		/// </summary>
		/// <param name="fXOne">X coordinate of first point on line one.</param>
		/// <param name="fYOne">Y coordinate of first point on line one.</param>
		/// <param name="fXTwo">X coordinate of second point on line one.</param>
		/// <param name="fYTwo">Y coordinate of second point on line one.</param>
		/// <param name="fXThree">X coordinate of first point on line two.</param>
		/// <param name="fYThree">Y coordinate of first point on line two.</param>
		/// <param name="fXFour">X coordinate of second point on line two.</param>
		/// <param name="fYFour">Y coordinate of second point on line two.</param>
		/// <param name="fXIntersect">X coordinate of intersection.</param>
		/// <param name="fYIntersect">Y coordinate of intersection.</param>
		/// <returns>True if the two lines intersect; otherwise, false.</returns>
		public static bool LineIntersection(double fXOne, double fYOne, double fXTwo, double fYTwo, double fXThree, double fYThree, double fXFour, double fYFour, out double fXIntersect, out double fYIntersect)
		{
			return LineIntersectionCore(false, fXOne, fYOne, fXTwo, fYTwo, fXThree, fYThree, fXFour, fYFour, out fXIntersect, out fYIntersect);
		}

		/// <summary>
		/// Calculates the intersection point of two line segments.
		/// </summary>
		/// <param name="fXOne">X coordinate of start point for segment one.</param>
		/// <param name="fYOne">Y coordinate of start point for segment one.</param>
		/// <param name="fXTwo">X coordinate of end point for segment one.</param>
		/// <param name="fYTwo">Y coordinate of end point for segment one.</param>
		/// <param name="fXThree">X coordinate of start point for segment two.</param>
		/// <param name="fYThree">Y coordinate of start point for segment two.</param>
		/// <param name="fXFour">X coordinate of end point for segment two.</param>
		/// <param name="fYFour">Y coordinate of end point for segment two.</param>
		/// <param name="fXIntersect">X coordinate of intersection.</param>
		/// <param name="fYIntersect">Y coordinate of intersection.</param>
		/// <returns>True if the two segments intersect; otherwise, false.</returns>
		public static bool LineSegmentIntersection(double fXOne, double fYOne, double fXTwo, double fYTwo, double fXThree, double fYThree, double fXFour, double fYFour, out double fXIntersect, out double fYIntersect)
		{
			return LineIntersectionCore(true, fXOne, fYOne, fXTwo, fYTwo, fXThree, fYThree, fXFour, fYFour, out fXIntersect, out fYIntersect);
		}

		/// <summary>
		/// Calculates the first intersection point of a rect and a line segment starting with the left side of the rect and going clockwise.
		/// </summary>
		/// <param name="fRectX">X coordinate of the Rect.</param>
		/// <param name="fRectY">Y coordinate of the Rect.</param>
		/// <param name="fRectWidth">Width of the Rect.</param>
		/// <param name="fRectHeight">Height of the Rect.</param>
		/// <param name="fXOne">X coordinate of start point for segment.</param>
		/// <param name="fYOne">Y coordinate of start point for segment.</param>
		/// <param name="fXTwo">X coordinate of end point for segment.</param>
		/// <param name="fYTwo">Y coordinate of end point for segment.</param>
		/// <param name="fXIntersect">X coordinate of intersection.</param>
		/// <param name="fYIntersect">Y coordinate of intersection.</param>
		/// <returns>True if the Rect and segment intersect; otherwise, false.</returns>
		public static bool RectLineSegmentIntersection(double fRectX, double fRectY, double fRectWidth, double fRectHeight, double fXOne, double fYOne, double fXTwo, double fYTwo, out double fXIntersect, out double fYIntersect)
		{
			// left of rect intersection
			if (LineSegmentIntersection(fRectX, fRectY, fRectX, fRectY + fRectHeight, fXOne, fYOne, fXTwo, fYTwo, out fXIntersect, out fYIntersect))
				return true;

			// top of rect intersection
			if (LineSegmentIntersection(fRectX, fRectY, fRectX + fRectWidth, fRectY, fXOne, fYOne, fXTwo, fYTwo, out fXIntersect, out fYIntersect))
				return true;

			// right of rect intersection
			if (LineSegmentIntersection(fRectX + fRectWidth, fRectY, fRectX + fRectWidth, fRectY + fRectHeight, fXOne, fYOne, fXTwo, fYTwo, out fXIntersect, out fYIntersect))
				return true;

			// bottom of rect intersection
			if (LineSegmentIntersection(fRectX, fRectY + fRectHeight, fRectX + fRectWidth, fRectY + fRectHeight, fXOne, fYOne, fXTwo, fYTwo, out fXIntersect, out fYIntersect))
				return true;

			return false;
		}

		/// <summary>
		/// Returns coordinates of a point at an angle along a circle, relative to its center.
		/// </summary>
		/// <param name="fAngle">The angle in degrees.</param>
		/// <param name="fRadius">The radius of the circle.</param>
		/// <param name="fX">The resulting x coordinate.</param>
		/// <param name="fY">The resulting y coordinate.</param>
		public static void PointOnCircle(double fAngle, double fRadius, out double fX, out double fY)
		{
			double fAngleRadians = DegreesToRadians(fAngle);

			fX = fRadius * Cos(fAngleRadians);
			fY = fRadius * Sin(fAngleRadians);
		}

		/// <summary>
		/// Returns coordinates of a point at an angle along an ellipse, relative to its center.
		/// </summary>
		/// <param name="fAngle">The angle in degrees.</param>
		/// <param name="fWidth">The full width of the ellipse.</param>
		/// <param name="fHeight">The full height of the ellipse.</param>
		/// <param name="fX">The resulting x coordinate.</param>
		/// <param name="fY">The resulting y coordinate.</param>
		public static void PointOnEllipse(double fAngle, double fWidth, double fHeight, out double fX, out double fY)
		{
			double fAngleRadians = DegreesToRadians(fAngle);
			double fRadiusX = fWidth / 2;
			double fRadiusY = fHeight / 2;

			fX = fRadiusX * Cos(fAngleRadians);
			fY = fRadiusY * Sin(fAngleRadians);
		}

		/// <summary>
		/// Returns the median value from a sequence of values.
		/// </summary>
		/// <param name="afValues">Sequence of value to find the median.</param>
		/// <returns>The median value.</returns>
		public static double GetMedian(double[] afValues)
		{
			if (afValues.Length == 0)
				return 0;
			if (afValues.Length == 1)
				return afValues[0];

			int nIndex = (afValues.Length - 1) / 2;
			return (afValues[nIndex] + afValues[nIndex + 1]) / 2;
		}

		/// <summary>
		/// Gets the linear interpolation of two values.
		/// </summary>
		/// <param name="fMinValue">The minimum value.</param>
		/// <param name="fMaxValue">The maximum value.</param>
		/// <param name="fInterpolation">The interpolation weight.</param>
		/// <returns></returns>
		public static double GetLinearInterpolation(double fMinValue, double fMaxValue, double fInterpolation)
		{
			fInterpolation = Clamp(fInterpolation, 0.0, 1.0);

			if (fInterpolation == 0.0) return fMinValue;
			if (fInterpolation == 1.0) return fMaxValue;

			double fInvInterpolation = 1.0 - fInterpolation;
			return (fInvInterpolation * fMinValue) + (fInterpolation * fMaxValue);
		}

		/// <summary>
		/// Gets the angle of the vector, in radians.
		/// </summary>
		/// <param name="vectorX">The X coordinate of the vector for which the angle should be calculated.</param>
		/// <param name="vectorY">The Y coordinate of the vector for which the angle should be calculated.</param>
		/// <returns>The angle of the vector, in radians.</returns>
		/// <remarks>The value returned by this method will always be between 0 and 2 * Pi</remarks>
		public static double GetVectorAngle(double vectorX, double vectorY)
		{
			double fVectorLength = GetVectorLength(vectorX, vectorY);
			double fAngle = Acos(vectorX / fVectorLength);

			// Arccosine only returns angles that correspond to positive Y values.
			// If our vector has a negative Y value, we must reflect the angle across
			// the X-axis--which we can do by negating the value; because we want
			// to return only positive values, we add 2 * Pi to the negated value for
			// an equivalent, positive angle.
			if (vectorY < 0)
				fAngle = (PI * 2) - fAngle;

			return fAngle;
		}

		/// <summary>
		/// Calculates the Length of a vector without using System.Windows.Vector
		/// </summary>
		/// <param name="vectorX">The X coordinate of Vector.</param>
		/// <param name="vectorY">The Y coordinate of Vector.</param>
		/// <returns>The Length of the vector that points in the specified direction.</returns>
		public static double GetVectorLength(double vectorX, double vectorY)
		{
			return Sqrt((vectorX * vectorX) + (vectorY * vectorY));
		}

		/// <summary>
		/// Gets the angle of the vector, in radians without using System.Windows.Vector
		/// </summary>
		/// <param name="pt1X">The X coordinate of Starting point.</param>
		/// <param name="pt1Y">The Y coordinate of Starting point.</param>
		/// <param name="pt2X">The X coordinate of Ending point.</param>
		/// <param name="pt2Y">The Y coordinate of Ending point.</param>
		/// <returns>The angle of the vector, in radians.</returns>
		/// <remarks>The value returned by this method will always be between 0 and 2 * Pi</remarks>
		public static double GetVectorAngle(double pt1X, double pt1Y, double pt2X, double pt2Y)
		{
			return GetVectorAngle(pt1X - pt2X, pt1Y - pt2Y);
		}

		/// <summary>
		/// Creates a unit vector that points in the specified direction.
		/// </summary>
		/// <param name="fAngle">The angle in which the vector should point, in radians.</param>
		/// <param name="fVectorX">The resulting vector X .</param>
		/// <param name="fVectorY">The resulting vector Y.</param>
		/// <returns>A unit vector that points in the specified direction.</returns>
		public static void CreateUnitVectorWithAngle(double fAngle, out double fVectorX, out double fVectorY)
		{
			fVectorX = Cos(fAngle);
			fVectorY = Sin(fAngle);
		}

		/// <summary>
		/// Calculates the Length of a vector without using System.Windows.Vector
		/// </summary>
		/// <param name="pt1X">The X coordinate of Starting point.</param>
		/// <param name="pt1Y">The Y coordinate of Starting point.</param>
		/// <param name="pt2X">The X coordinate of Ending point.</param>
		/// <param name="pt2Y">The Y coordinate of Ending point.</param>
		/// <returns>The Length of the vector that points in the specified direction.</returns>
		public static double GetVectorLength(double pt1X, double pt1Y, double pt2X, double pt2Y)
		{
			return GetVectorLength(pt1X - pt2X, pt1Y - pt2Y);
		}

		/// <summary>
		/// Calculates the center of an arc.
		/// </summary>
		/// <param name="ptStartX">The X coordinate of the Starting point.</param>
		/// <param name="ptStartY">The Y coordinate of the Starting point.</param>
		/// <param name="ptEndX">The X coordinate of the Ending point.</param>
		/// <param name="ptEndY">The Y coordinate of the Ending point.</param>
		/// <param name="fRadiusX">The X radius of the Ellipse.</param>
		/// <param name="fRadiusY">The Y radius of the Ellipse.</param>
		/// <param name="bIsLargeArc">True if the arc is a large arc.</param>
		/// <param name="bIsClockwise">True if the arc is drawn clockwise.</param>
		/// <param name="ptCenterX">The resulting X coordinate of the Center point.</param>
		/// <param name="ptCenterY">The resulting Y coordinate of the Center point.</param>
		/// <param name="fStartAngle">The resulting start angle of the arc.</param>
		/// <param name="fEndAngle">The resulting end angle of the arc.</param>
		public static void GetArcCenter(double ptStartX, double ptStartY, double ptEndX, double ptEndY, double fRadiusX, double fRadiusY, bool bIsLargeArc, bool bIsClockwise, out double ptCenterX, out double ptCenterY, out double fStartAngle, out double fEndAngle)
		{
			GetArcCenter(ptStartX / fRadiusX, ptStartY / fRadiusY, ptEndX / fRadiusX, ptEndY / fRadiusY, 1.0, bIsLargeArc, bIsClockwise, out ptCenterX, out ptCenterY, out fStartAngle, out fEndAngle);
			ptCenterX *= fRadiusX;
			ptCenterY *= fRadiusY;
		}

		/// <summary>
		/// Calculates the center point of an Arc
		/// </summary>
		/// <param name="ptStartX">The X coordinate of Starting point.</param>
		/// <param name="ptStartY">The Y coordinate of Starting point.</param>
		/// <param name="ptEndX">The X coordinate of Ending point.</param>
		/// <param name="ptEndY">The Y coordinate of Ending point.</param>
		/// <param name="fRadius">The radius.</param>
		/// <param name="bIsLargeArc">true if this is a Large Arc we are dealing with</param>
		/// <param name="bIsClockwise">true if we are to use the clockwise coordinate.</param>
		/// <param name="ptCenterX">The resulting center point X coordinate</param>
		/// <param name="ptCenterY">The resulting center point Y coordinate.</param>
		/// <param name="fStartAngle">The resulting starting angle between the starting point and the center point</param>
		/// <param name="fEndAngle">The resulting end angle between the ending point and the center point.</param>
		public static void GetArcCenter(double ptStartX, double ptStartY, double ptEndX, double ptEndY, double fRadius, bool bIsLargeArc, bool bIsClockwise, out double ptCenterX, out double ptCenterY, out double fStartAngle, out double fEndAngle)
		{
			if (ptStartX == ptEndX && ptStartY == ptEndY)
				throw new InvalidOperationException("The given starting and ending points must not be the same.");

			//// Mathy stuff:
			//// Given two distinct points, A and B, there are at most two circles of radius r that pass through both points*. Let C
			//// be the origin of one of these circles, without loss of generality. Let Theta be the angle formed between points B, A,
			//// and C. Let D be the midpoint of the line segment AB.

			//// It can be inferred that triangle ACD is a right triangle whose hypoteneuse is AC and has length r. Since
			//// we know that the length segment AD is half the length of segment AB, we can calculate Theta as the arccosine of
			//// half the length of segment AB divided by r.

			double fVectorLength = GetVectorLength(ptEndX, ptEndY, ptStartX, ptStartY);

			//// * If the distance between A and B is greater than 2 * r, there are no circles, and if the distance between A and B is
			//// exactly 2 * r, then there is only one circle.
			bool bFudgedRadius = false;
			if (fVectorLength > fRadius * 2)
			{
				if (Abs(fVectorLength - fRadius * 2) > c_fEpsilon)
					throw new InvalidOperationException("The distance between starting and ending points must be less than or equal to the diameter.");

				// Tweak the radius to account for small double rounding errors when fVectorLength should be equal to 2 * fRadius
				fRadius = fVectorLength / 2;
				bFudgedRadius = true;
			}

			// Avoid rounding errors when calculating fRadiusAngle if fVectorLength == 2 * fRadius
			double fRadiusAngle = Acos(bFudgedRadius ? 1.0 : (fVectorLength / 2) / fRadius);
			double fSegmentAngle = GetVectorAngle(ptEndX, ptEndY, ptStartX, ptStartY);

			//// Let Phi be the angle of the vector formed by the difference between points A and B. We can calculate the location
			//// of C by adding or subtracting** Theta from Phi and adding a vector with this angle of length r to point A.

			//// ** We add the values here; if we later determine that we picked the wrong circle, we find the other center point
			//// by subtracting the angles.

			double fUnitVectorX;
			double fUnitVectorY;

			CreateUnitVectorWithAngle(fSegmentAngle + fRadiusAngle, out fUnitVectorX, out fUnitVectorY);

			ptCenterX = ptStartX + fUnitVectorX * fRadius;
			ptCenterY = ptStartY + fUnitVectorY * fRadius;

			fStartAngle = GetVectorAngle(ptStartX, ptStartY, ptCenterX, ptCenterY);
			fEndAngle = GetVectorAngle(ptEndX, ptEndY, ptCenterX, ptCenterY);

			//// Now that we have the center of one of the circles that intersects A and B, we must determine if we have chosen the
			//// correct circle. To do this, we begin by ensuring that the difference between the end angle is no more than Pi; given
			//// this precondition, we can determine the direction of the small arc by simply comparing the end angle to the start
			//// angle--a larger end angle implies a clockwise small arc.
			bool bSmallArcIsClockwise;
			if (Abs(fEndAngle - fStartAngle) > PI)
			{
				if (fEndAngle > fStartAngle)
					bSmallArcIsClockwise = (fEndAngle - (2 * PI)) > fStartAngle;
				else
					bSmallArcIsClockwise = fEndAngle > (fStartAngle - (2 * PI));
			}
			else
			{
				bSmallArcIsClockwise = fEndAngle > fStartAngle;
			}

			bool bIsCorrectCircle = bSmallArcIsClockwise ? bIsLargeArc != bIsClockwise : bIsLargeArc == bIsClockwise;
			if (!bIsCorrectCircle)
			{
				CreateUnitVectorWithAngle(fSegmentAngle - fRadiusAngle, out fUnitVectorX, out fUnitVectorY);

				ptCenterX = ptStartX + fUnitVectorX * fRadius;
				ptCenterY = ptStartY + fUnitVectorY * fRadius;

				fStartAngle = GetVectorAngle(ptStartX, ptStartY, ptCenterX, ptCenterY);
				fEndAngle = GetVectorAngle(ptEndX, ptEndY, ptCenterX, ptCenterY);
			}

			fStartAngle *= (180 / PI);
			fEndAngle *= (180 / PI);
		}

		private static bool LineIntersectionCore(bool bSegments, double fXOne, double fYOne, double fXTwo, double fYTwo, double fXThree, double fYThree, double fXFour, double fYFour, out double fXIntersect, out double fYIntersect)
		{
			// default values
			fXIntersect = 0;
			fYIntersect = 0;

			double fUnknownDenominator = ((fYFour - fYThree) * (fXTwo - fXOne)) - ((fXFour - fXThree) * (fYTwo - fYOne));
			double fUnknownANumerator = ((fXFour - fXThree) * (fYOne - fYThree)) - ((fYFour - fYThree) * (fXOne - fXThree));
			double fUnknownBNumerator = ((fXTwo - fXOne) * (fYOne - fYThree)) - ((fYTwo - fYOne) * (fXOne - fXThree));

			if (fUnknownDenominator != 0)
			{
				double fUnknownA = fUnknownANumerator / fUnknownDenominator;
				double fUnknownB = fUnknownBNumerator / fUnknownDenominator;

				if (!bSegments || (fUnknownA > 0 && fUnknownA < 1 && fUnknownB > 0 && fUnknownB < 1))
				{
					fXIntersect = fXOne + (fUnknownA * (fXTwo - fXOne));
					fYIntersect = fYOne + (fUnknownA * (fYTwo - fYOne));
					return true;
				}
			}

			return false;
		}

		const double c_fEpsilon = 0.001;
	}
}
