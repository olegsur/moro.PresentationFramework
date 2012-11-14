using System;
using moro.Framework;

namespace Test1
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Window window = new Window();
			window.Title = "Hello, world";
			window.WidthRequest = 400;
			window.HeightRequest = 300;
			Application.Current.Run (window);
		}
	}
}
