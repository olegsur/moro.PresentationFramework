// 
// CursorLines.cs
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
using fkalc.UI.ViewModels.MathRegion.Tokens;
using fkalc.UI.ViewModels.MathRegion;

namespace fkalc.UI
{
	public class CursorLines : Adorner
	{
		private MathRegion Region { get; set; }

		public CursorLines (MathRegion region)
		{
			Region = region;
		}	

		protected override void OnRender (DrawingContext dc)
		{
			if (!Region.IsFocused)
				return;

			DrawVLine (dc);
			DrawHLine (dc);
		}	

		private void DrawVLine (DrawingContext dc)
		{
			var activeArea = GetArea (Region.Selection.SelectedToken, Region);
			if (activeArea == null)
				return;

			var width = Region.Selection.Type == SelectionType.Left ? -2 : activeArea.Width + 2;

			if (Region.Selection.SelectedToken is TextToken) {
				var textToken = Region.Selection.SelectedToken as TextToken;

				if (!string.IsNullOrEmpty (textToken.Text) && Region.Selection.Position != 0) {
					var ft = new FormattedText (textToken.Text.Substring (0, Region.Selection.Position), textToken.FontFamily, textToken.FontSize, Colors.Black);

					width = ft.Width + 2;
				} else
					width = 0;
			}

			var startPoint = activeArea.PointToScreen (new Point (width, -2));
			startPoint = Region.PointFromScreen (startPoint);

			var endPoint = activeArea.PointToScreen (new Point (width, activeArea.Height + 2));
			endPoint = Region.PointFromScreen (endPoint);

			dc.DrawLine (new Pen (Colors.Red, 2), startPoint, endPoint);
		}

		private void DrawHLine (DrawingContext dc)
		{
			var activeArea = GetArea (Region.Selection.SelectedToken, Region);
			if (activeArea == null)
				return;

			var startPoint = activeArea.PointToScreen (new Point (-1, activeArea.Height + 2));
			startPoint = Region.PointFromScreen (startPoint);

			var endPoint = activeArea.PointToScreen (new Point (activeArea.Width + 3, activeArea.Height + 2));
			endPoint = Region.PointFromScreen (endPoint);

			dc.DrawLine (new Pen (Colors.Red, 2), startPoint, endPoint);
		} 

		private Area GetArea (Token token, UIElement uielement)
		{
			if (uielement == null)
				return null;

			if (uielement is FrameworkElement && (uielement as FrameworkElement).DataContext == token)
				return uielement as Area;

			
			for (int i = 0; i < uielement.VisualChildrenCount; i++) {
				var child = GetArea (token, uielement.GetVisualChild (i) as UIElement);

				if (child != null)
					return child;
			}

			return null;
		}
	}
}

