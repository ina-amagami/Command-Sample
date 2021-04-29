using System.Collections.Generic;
using UnityEngine;

namespace Undo
{
	public class Unit : MonoBehaviour
	{
		public const float GridSize = 1.5f;

		public int X;
		public int Y;

		private void Start()
		{
			ApplyPosition();
		}

		private void ApplyPosition()
		{
			transform.position = new Vector3(X * GridSize, Y * GridSize, 0);
		}

		public void MoveTo(int x, int y)
		{
			X = x;
			Y = y;
			ApplyPosition();
		}
	}
}
