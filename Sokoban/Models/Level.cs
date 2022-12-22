using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Sokoban.Models
{
	enum CellType
	{
		None,
		Wall,
		Placeholder,
		Box,
		BoxPlaceholder,
		Player,
		PlayerPlaceholder
	}

	internal class Level
	{
		public int Number { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public List<CellType> Cells { get; set; }  = new List<CellType>();

		public Player Sokoban { get; set; } = new Player();
		public List<Box> Boxes { get; set; } = new List<Box>();

		public void Load(int number)
		{
			Number = number;

			string fileName = string.Format("Assets/Levels/level_{0:D2}.txt", Number);

			var lines = File.ReadAllLines(fileName);
			int playerIndex = 0;
			int boxIndex = 0;

			Cells.Clear();
			Boxes.Clear();

			Width = 0;
			Height = 0;
			foreach (var line in lines)
			{
				if (string.IsNullOrEmpty(line)) continue;
				
				if (Width == 0)
				{
					Width = line.Length;
				}

				foreach (var ch in line)
				{
					switch (ch)
					{
					case ' ': Cells.Add(CellType.None);        break;
					case 'X': Cells.Add(CellType.Wall);        break;
					case '.': Cells.Add(CellType.Placeholder); break;
					case '*':
						boxIndex = Cells.Count;
						Cells.Add(CellType.Box);
						Boxes.Add(new Box(boxIndex % Width, boxIndex / Width));
						break;
					case '@':
						playerIndex = Cells.Count;
						Cells.Add(CellType.Player);
						break;
					}
				}
				Height++;
			}

			Sokoban.X = playerIndex % Width;
			Sokoban.Y = playerIndex / Width;
		}

		public CellType Check(int x, int y)
		{
			int index = (y * Width) + x;
			if (index < 0 || index > Cells.Count - 1)
			{
				return CellType.None;
			}

			return Cells[index];
		}

		public bool SetPlayer(int dx, int dy)
		{
			var source = Check(Sokoban.X, Sokoban.Y);
			var destination = Check(Sokoban.X + dx, Sokoban.Y + dy);
			if (destination == CellType.Wall)
			{
				return false;
			}

			if (destination == CellType.Box || destination == CellType.BoxPlaceholder)
			{
				var after = Check(Sokoban.X + dx * 2, Sokoban.Y + dy * 2);
				if (after == CellType.Wall)
				{
					return false;
				}
				if (after == CellType.Box || after == CellType.BoxPlaceholder)
				{
					return false;
				}
				if (after == CellType.Placeholder || after == CellType.None)
				{
					// это мы двигаем кубик
					var afterType =
						after == CellType.Placeholder ?
						CellType.BoxPlaceholder :
						CellType.Box;
					SetCell(Sokoban.X + dx * 2, Sokoban.Y + dy * 2, afterType);
					Box found = Boxes.Find((Box box) => {
						return box.X == Sokoban.X + dx && box.Y == Sokoban.Y + dy;
					});
					if (found.X != 0)
					{
						found.Set(Sokoban.X + dx * 2, Sokoban.Y + dy * 2);
					}

					// установка игрока после того, как он подвинул кубик.
					var targetType =
						destination == CellType.BoxPlaceholder ?
						CellType.PlayerPlaceholder :
						CellType.Player;
					SetCell(Sokoban.X + dx, Sokoban.Y + dy, targetType);
				}
			}
			else
			{
				// это установка игрока там, куда он пошел
				var targetType =
					destination == CellType.Placeholder ?
					CellType.PlayerPlaceholder :
					CellType.Player;
				SetCell(Sokoban.X + dx, Sokoban.Y + dy, targetType);
			}

			// это стирание игрока там, где он был
			var sourceType =
				source == CellType.Player ?
				CellType.None :
				CellType.Placeholder;
			SetCell(Sokoban.X, Sokoban.Y, sourceType);

			Sokoban.Set(Sokoban.X + dx, Sokoban.Y + dy);

			return true;
		}

		public void SetCell(int x, int y, CellType type)
		{
			int index = (y * Width) + x;
			Cells[index] = type;
		}

		public bool CheckLevelComplete()
		{
			foreach (var cell in Cells)
			{
				if (cell == CellType.Placeholder)
				{
					return false;
				}
			}

			// if all boxes is BoxPlaceholder
			return true;
		}
	}
}
