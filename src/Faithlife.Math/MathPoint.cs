using System;

namespace Faithlife.Math
{
	/// <summary>
	/// An immutable 2D point.
	/// </summary>
	public struct MathPoint : IEquatable<MathPoint>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MathPoint"/> struct.
		/// </summary>
		/// <param name="fX">The X value.</param>
		/// <param name="fY">The Y value.</param>
		public MathPoint(double fX, double fY)
		{
			m_fX = fX;
			m_fY = fY;
		}

		/// <summary>
		/// Gets the X value.
		/// </summary>
		/// <value>The X value.</value>
		public double X
		{
			get { return m_fX; }
		}

		/// <summary>
		/// Gets the Y value.
		/// </summary>
		/// <value>The Y value.</value>
		public double Y
		{
			get { return m_fY; }
		}

		/// <summary>
		/// Provides a string representation of this point.
		/// </summary>
		/// <returns>
		/// A string representation of this point.
		/// </returns>
		public override string ToString()
		{
			return m_fX + "," + m_fY;
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		public bool Equals(MathPoint other)
		{
			return other.m_fX == m_fX && other.m_fY == m_fY;
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
			return obj is MathPoint && Equals((MathPoint) obj);
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>
		/// A 32-bit signed integer that is the hash code for this instance.
		/// </returns>
		public override int GetHashCode()
		{
			return unchecked(m_fX.GetHashCode() * 33 + m_fY.GetHashCode());
		}

		/// <summary>
		/// Test whether two specified MathPoint structures are equivalent.
		/// </summary>
		/// <param name="left">The MathPoint that is to the left of the equality operator.</param>
		/// <param name="right">The MathPoint that is to the right of the equality operator.</param>
		/// <returns>true if the two MathPoint structures are equivalent; otherwise, false.</returns>
		public static bool operator ==(MathPoint left, MathPoint right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Test whether two specified color structures are different.
		/// </summary>
		/// <param name="left">The MathPoint that is to the left of the inequality operator.</param>
		/// <param name="right">The MathPoint that is to the right of the inequality operator.</param>
		/// <returns>true if the two MathPoint structures are different; otherwise, false.</returns>
		public static bool operator !=(MathPoint left, MathPoint right)
		{
			return !left.Equals(right);
		}

		readonly double m_fX;
		readonly double m_fY;
	}
}
