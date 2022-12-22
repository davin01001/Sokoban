using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban.Models
{
	internal class Player
	{
		public int X { get; set; }
		public int Y { get; set; }

		public void Set(int x, int y)
		{
			X = x;
			Y = y;
		}
	}
}
