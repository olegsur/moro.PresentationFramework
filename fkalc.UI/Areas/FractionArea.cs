// 
// FractionArea.cs
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
using System.Linq;

namespace fkalc.UI
{
	public class FractionArea : Area
	{
		private StackPanel panel = new StackPanel () { Orientation = Orientation.Vertical };
		
		public ContentControl Dividend { get; private set; }
		public ContentControl Divisor { get; private set; }
		
		public FractionArea ()
		{
			Content = panel;		
			
			Dividend = new ContentControl () 
			{
				Content = new TextArea(),
				Margin = new Thickness (5, 0, 5, 5)
			};

			Divisor = new ContentControl () 
			{
				Content = new TextArea(),
				Margin = new Thickness (5, 5, 5, 0)
			};
			
			var line = new Line ()
			{
				HeightRequest = 2,
				StrokeThickness = 2,
				HorizontalAlignment = HorizontalAlignment.Stretch
			};
			
			panel.Children.Add (Dividend);
			panel.Children.Add (line);			
			panel.Children.Add (Divisor);

			BindingOperations.SetBinding (this, "DataContext.Dividend", Dividend.GetProperty ("Content"), new TokenAreaConverter ());
			BindingOperations.SetBinding (this, "DataContext.Divisor", Divisor.GetProperty ("Content"), new TokenAreaConverter ());

			Margin = new Thickness (2, 0, 2, 0);
		}
	}
}