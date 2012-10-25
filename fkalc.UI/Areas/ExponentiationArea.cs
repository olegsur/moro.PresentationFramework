//
// ExponentiationArea.cs
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
	public class ExponentiationArea : Area
	{
		public ContentControl Base { get; private set; }
		public ContentControl Power { get; private set; }

		public ExponentiationArea ()
		{
			Base = new ContentControl () 
			{
				Content = new TextArea(),
				Margin = new Thickness (0, 3, 3, 0)
			};

			Power = new ContentControl () 
			{
				Content = new TextArea(),
				RenderTransform = new ScaleTransform(0.75, 0.75),
				LayoutTransform = new ScaleTransform(0.75, 0.75),
				//BorderColor = Colors.Blue
			};

			var grid = new Grid ();

			grid.Children.Add (Base);
			grid.Children.Add (Power);

			grid.SetRow (1, Base);
			grid.SetColumn (0, Base);

			grid.SetRow (0, Power);
			grid.SetColumn (1, Power);

			BindingOperations.SetBinding (this, "DataContext.Base", Base.GetProperty ("Content"), new TokenAreaConverter ());
			BindingOperations.SetBinding (this, "DataContext.Power", Power.GetProperty ("Content"), new TokenAreaConverter ());

			Content = grid;
		}
	}
}

