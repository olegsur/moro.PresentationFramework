//
// AssignmentArea.cs
//
// Author:
//       Oleg Sur <oleg.sur@gmail.com>
//
// Copyright (c) 2012 Oleg Sur
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using moro.Framework;

namespace fkalc.UI
{
	public class AssignmentArea : Area
	{
		public AssignmentArea ()
		{
			var grid = new Grid ();

			var ellipse1 = new Ellipse ()
			{
				WidthRequest = 3,
				HeightRequest = 3,
				StrokeThickness = 1,
				Fill = Brushes.Black,
				Margin = new Thickness(1)
			};

			var ellipse2 = new Ellipse ()
			{
				WidthRequest = 3,
				HeightRequest = 3,
				StrokeThickness = 1,
				Fill = Brushes.Black,
				Margin = new Thickness(1)
			};

			var line1 = new Line ()
			{
				WidthRequest = 12,
				HeightRequest = 2,
				StrokeThickness = 2,
				Stroke = Colors.Black,
				SnapsToDevicePixels = true,
				Margin = new Thickness (0,2,0,0)
			};

			var line2 = new Line ()
			{
				WidthRequest = 12,
				HeightRequest = 2,
				StrokeThickness = 2,
				Stroke = Colors.Black,
				SnapsToDevicePixels = true,
				Margin = new Thickness (0,2,0,0)
			};
						
			grid.Children.Add (ellipse1);
			grid.Children.Add (ellipse2);
			grid.Children.Add (line1);
			grid.Children.Add (line2);

			grid.SetRow (0, ellipse1);
			grid.SetColumn (0, ellipse1);

			grid.SetRow (0, line1);
			grid.SetColumn (1, line1);


			grid.SetRow (1, ellipse1);
			grid.SetColumn (0, ellipse1);

			grid.SetRow (1, line2);
			grid.SetColumn (1, line2);
						
			Content = grid;
		}
	}
}

