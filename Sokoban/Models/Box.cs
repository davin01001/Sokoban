using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban.Models
{
	internal class Box
	{
		public int X { get; set; }
		public int Y { get; set; }

		public int newX { get; set; }
		public int newY { get; set; }

		public bool Moved { get; set; } = false;

		public Box(int x, int y)
		{
			X = x;
			Y = y;
		}

		public void Set(int x, int y)
		{
			newX = x;
			newY = y;
			Moved = true;
		}

		public void Move()
		{
			X = newX;
			Y = newY;
			Moved = false;
		}
	}
}
