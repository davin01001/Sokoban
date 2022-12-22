using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

using Sokoban.Models;

namespace Sokoban
{
	public partial class MainWindow : Window
	{
		DispatcherTimer _timer = new DispatcherTimer();

		public MainWindow()
		{
			InitializeComponent();

			_timer.Tick += GameLoop;
			_timer.Interval = TimeSpan.FromMilliseconds(20);
			_timer.Start();
		}

		private void GameLoop(object sender, EventArgs args)
		{
			if (level.GoLeft)
			{
				level.MoveLeft();
			}

			if (level.GoRight)
			{
				level.MoveRight();
			}

			if (level.GoUp)
			{
				level.MoveUp();
			}

			if (level.GoDown)
			{
				level.MoveDown();
			}
		}
	}
}
