//
// FormattedText.cs
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
	public class FormattedText
	{
		public string Text { get; private set; }
		public string FontFamily { get; private set; }
		public double FontSize { get; set; }
		public double Width { get; private set; }
		public double Height { get; private set; }
		public Color Foreground { get; private set; }

		public FormattedText (string text, string fontFamily, double fontSize, Color foreground)
		{
			Text = text;
			FontFamily = fontFamily;
			FontSize = fontSize;
			Foreground = foreground;

			var size = Measure ();

			Width = size.Width;
			Height = size.Height;
		}

		private Size Measure ()
		{
			var surface = new Cairo.ImageSurface (Cairo.Format.ARGB32, 1, 1);
			
			using (Cairo.Context cr = new Cairo.Context(surface)) {			
				cr.SelectFontFace (FontFamily, Cairo.FontSlant.Normal, Cairo.FontWeight.Normal);
				cr.SetFontSize (FontSize);
			
				var textExtents = cr.TextExtents (Text);
			
				return new Size (textExtents.Width, textExtents.Height);
			}
		}
	}
}

