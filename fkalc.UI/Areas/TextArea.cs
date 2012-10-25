// 
// TextArea.cs
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
	public class TextArea : Area
	{				
		private TextBlock textBlock = new TextBlock ();
		private Rectangle rectangle = new Rectangle ();
		
		public TextArea ()
		{
			textBlock.Visibility = Visibility.Collapsed;

			rectangle.HeightRequest = 12;
			rectangle.WidthRequest = 12;
			rectangle.SnapsToDevicePixels = true;

			var grid = new Grid ();
			grid.Children.Add (textBlock);
			grid.Children.Add (rectangle);

			Content = grid;

			BindingOperations.SetBinding (this, "DataContext.Text", textBlock.GetProperty ("Text"));
			BindingOperations.SetBinding (this, "DataContext.FontFamily", textBlock.GetProperty ("FontFamily"));
			BindingOperations.SetBinding (this, "DataContext.FontSize", textBlock.GetProperty ("FontSize"));

			textBlock.GetProperty ("Text").DependencyPropertyValueChanged += TextChanged;

			Margin = new Thickness (1, 0, 1, 0);
		}

		private void TextChanged (object sender, DPropertyValueChangedEventArgs e)
		{
			var str = e.NewValue as string;

			if (string.IsNullOrEmpty (str)) {
				rectangle.Visibility = Visibility.Visible;
				textBlock.Visibility = Visibility.Collapsed;
			} else {
				rectangle.Visibility = Visibility.Collapsed;
				textBlock.Visibility = Visibility.Visible;
			}
		}
	}
}

