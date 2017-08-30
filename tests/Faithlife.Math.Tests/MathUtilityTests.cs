using System;
using NUnit.Framework;
using static System.Math;

namespace Faithlife.Math.Tests
{
	[TestFixture]
	public class MathUtilityTests
	{
		[Test]
		public void DegToRadTest()
		{
			Assert.AreEqual(2 * PI, MathUtility.DegreesToRadians(360.0f), Epsilon);
		}

		[Test]
		public void RadToDegTest()
		{
			Assert.AreEqual(360.0f, MathUtility.RadiansToDegrees(2 * PI), Epsilon);
		}
		
		[Test]
		public void GreatCircleDistanceTest()
		{
			// measure aprox. distance from Bellingham, WA to Mt. Vernon, WA
			// TODO: is this a valid delta?
			Assert.AreEqual(26.5233299782886, MathUtility.GreatCircleDistance(3963, -48.791485, -122.482967, -48.419859, -122.339973), 1000000 * Epsilon);
		}

		// the smallest value such that 1.0 + epsilon != 1.0
		private readonly double Epsilon = 2.2204460492503131e-016;
	}
}
