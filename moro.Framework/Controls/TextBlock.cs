// 
// TextBlock.cs
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

namespace moro.Framework
{
	public class TextBlock : FrameworkElement
	{
		private readonly DependencyProperty<string> text;
		private readonly DependencyProperty<string> fontFamily;
		private readonly DependencyProperty<double> fontSize;
		private readonly DependencyProperty<Color> foreground;

		public string Text { 
			get { return text.Value;} 
			set { text.Value = value; }
		}

		public string FontFamily { 
			get { return fontFamily.Value;} 
			set { fontFamily.Value = value; }
		}

		public double FontSize { 
			get { return fontSize.Value;} 
			set { fontSize.Value = value; }
		}
		
		public Color Foreground { 
			get { return foreground.Value; }
			set { foreground.Value = value; }				
		}
		
		public TextBlock ()
		{
			text = BuildProperty<string> ("Text");
			fontFamily = BuildProperty<string> ("FontFamily");
			fontSize = BuildProperty<double> ("FontSize");
			foreground = BuildProperty<Color> ("Foreground");			
			
			StyleHelper.ApplyStyle (this, typeof(TextBlock));			
		}	
		
		protected override void OnRender (DrawingContext dc)
		{	
			dc.DrawText (new FormattedText (Text, FontFamily, FontSize, Foreground), new Point (0, Height));
		}
		
		protected override Size MeasureOverride (Size availableSize)
		{
			var formatedText = new FormattedText (Text, FontFamily, FontSize, Foreground);

			return new Size (formatedText.Width, formatedText.Height);
		}		
	}
}

