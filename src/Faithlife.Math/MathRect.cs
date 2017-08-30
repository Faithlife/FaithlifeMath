using System;
using System.Globalization;
using System.Linq;

namespace Faithlife.Math
{
	/// <summary>
	/// An immutable 2D rect.
	/// </summary>
	public struct MathRect : IEquatable<MathRect>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MathRect"/> struct.
		/// </summary>
		/// <param name="fX">The X value.</param>
		/// <param name="fY">The Y value.</param>
		/// <param name="fWidth">The width.</param>
		/// <param name="fHeight">The height.</param>
		public MathRect(double fX, double fY, double fWidth, double fHeight)
		{
			if (fWidth < 0)
				throw new ArgumentException("Width must be non-negative", "fWidth");
			if (fHeight < 0)
				throw new ArgumentException("Height must be non-negative", "fHeight");

			m_fX = fX;
			m_fY = fY;
			m_fWidth = fWidth;
			m_fHeight = fHeight;
		}

		/// <summary>
		/// Gets the X (left) value.
		/// </summary>
		/// <value>The X value.</value>
		public double X
		{
			get { return m_fX; }
		}

		/// <summary>
		/// Gets the Y (top) value.
		/// </summary>
		/// <value>The Y value.</value>
		public double Y
		{
			get { return m_fY; }
		}

		/// <summary>
		/// Gets the width.
		/// </summary>
		/// <value>The width.</value>
		public double Width
		{
			get { return m_fWidth; }
		}

		/// <summary>
		/// Gets the height.
		/// </summary>
		/// <value>The height.</value>
		public double Height
		{
			get { return m_fHeight; }
		}

		/// <summary>
		/// Gets the left value.
		/// </summary>
		public double Left
		{
			get { return m_fX; }
		}

		/// <summary>
		/// Gets the right value.
		/// </summary>
		public double Right
		{
			get { return m_fX + m_fWidth; }
		}

		/// <summary>
		/// Gets the top value.
		/// </summary>
		public double Top
		{
			get { return m_fY; }
		}

		/// <summary>
		/// Gets the bottom value.
		/// </summary>
		public double Bottom
		{
			get { return m_fY + m_fHeight; }
		}

		/// <summary>
		/// Provides a string representation of this rectangle.
		/// </summary>
		/// <returns>
		/// A string representation of this rectangle.
		/// </returns>
		public override string ToString()
		{
			return ToString(CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Provides a string representation of this rectangle.
		/// </summary>
		/// <returns>
		/// A string representation of this rectangle.
		/// </returns>
		public string ToString(IFormatProvider provider)
		{
			return string.Format(provider, "{0},{1},{2},{3}", m_fX, m_fY, m_fWidth, m_fHeight);
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		public bool Equals(MathRect other)
		{
			return other.m_fX == m_fX && other.m_fY == m_fY && other.m_fWidth == m_fWidth && other.m_fHeight == m_fHeight;
		}

		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <param name="obj">Another object to compare to.</param>
		/// <returns>
		/// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
		/// </returns>
		public override bool Equals(object obj)
		{
			return obj is MathRect && Equals((MathRect) obj);
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>
		/// A 32-bit signed integer that is the hash code for this instance.
		/// </returns>
		public override int GetHashCode()
		{
			unchecked
			{
				// simple hash mixing from https://stackoverflow.com/a/263416
				var hash = 17;
				hash = hash * 23 + m_fX.GetHashCode();
				hash = hash * 23 + m_fY.GetHashCode();
				hash = hash * 23 + m_fWidth.GetHashCode();
				hash = hash * 23 + m_fHeight.GetHashCode();
				return hash;
			}
		}

		/// <summary>
		/// Test whether two specified MathRect structures are equivalent.
		/// </summary>
		/// <param name="left">The MathRect that is to the left of the equality operator.</param>
		/// <param name="right">The MathRect that is to the right of the equality operator.</param>
		/// <returns>true if the two MathRect structures are equivalent; otherwise, false.</returns>
		public static bool operator ==(MathRect left, MathRect right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Test whether two specified color structures are different.
		/// </summary>
		/// <param name="left">The MathRect that is to the left of the inequality operator.</param>
		/// <param name="right">The MathRect that is to the right of the inequality operator.</param>
		/// <returns>true if the two MathRect structures are different; otherwise, false.</returns>
		public static bool operator !=(MathRect left, MathRect right)
		{
			return !left.Equals(right);
		}

		/// <summary>
		/// Creates a new rectangle from the specified string representation.
		/// </summary>
		/// <para name="source">The string representation of the rectangle, in the form "x,y,width,height".</para>
		public static MathRect Parse(string source)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			string[] astrElements = source.Split(new[] { ',' });

			if (astrElements.Length != 4)
				throw new ArgumentException(source + " is an invalid format. Expected format is \"x,y,width,height\" where x, y, width, and height are of type double.");

			var listElements = astrElements
				.Select(strPart =>
				{
					double fValue;
					return double.TryParse(strPart, NumberStyles.Float, CultureInfo.InvariantCulture, out fValue) ? (double?) fValue : default(double?);
				})
				.ToList();

			if (listElements.Any(fValue => fValue == null))
				throw new ArgumentException(source + " is an invalid format. Expected format is \"x,y,width,height\" where x, y, width, and height are of type double.");

			return new MathRect(listElements[0].Value, listElements[1].Value, listElements[2].Value, listElements[3].Value);
		}

		readonly double m_fX;
		readonly double m_fY;
		readonly double m_fWidth;
		readonly double m_fHeight;
	}
}
