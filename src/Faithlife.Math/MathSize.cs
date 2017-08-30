using System;
using System.Globalization;

namespace Faithlife.Math
{
	/// <summary>
	/// An immutable 2D size.
	/// </summary>
	public struct MathSize : IEquatable<MathSize>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MathSize"/> struct.
		/// </summary>
		public MathSize(double width, double height)
		{
			m_width = width;
			m_height = height;
		}

		/// <summary>
		/// The width.
		/// </summary>
		public double Width { get { return m_width; } }

		/// <summary>
		/// The height.
		/// </summary>
		public double Height { get { return m_height; } }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		public override bool Equals(object obj)
		{
			return obj is MathSize && Equals((MathSize) obj);
		}

		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		public bool Equals(MathSize other)
		{
			return Width == other.Width && Height == other.Height;
		}

		/// <summary>
		/// Test whether two specified MathSize structures are equivalent.
		/// </summary>
		public static bool operator ==(MathSize left, MathSize right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Test whether two specified MathSize structures are different.
		/// </summary>
		public static bool operator !=(MathSize left, MathSize right)
		{
			return !(left == right);
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		public override int GetHashCode()
		{
			return unchecked(Width.GetHashCode() * 33 + Height.GetHashCode());
		}

		/// <summary>
		/// Provides a string representation of this size.
		/// </summary>
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0},{1}", Width, Height);
		}

		readonly double m_width;
		readonly double m_height;
	}
}
