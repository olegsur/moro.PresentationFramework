// 
// GtkApplication.cs
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
using System.Collections.Generic;
using System.Linq;
using moro.Framework.Data;

namespace moro.Framework
{
	public class GtkApplication : IApplication
	{
		public GtkApplication ()
		{
		}
		
		public void Run ()
		{			
			Gtk.Application.Run ();
		}		
		
		public void Init ()
		{
			Gtk.Application.Init ();
		}
		
		public void Shutdown ()
		{
			Gtk.Application.Quit ();
		}

		public void RegisterWindow (Window window)
		{
			var width = window.WidthRequest ?? 100;
			var height = window.HeightRequest ?? 50;

			var surface = new GtkSurface (window, window.Left, window.Top, width, height, Gtk.WindowType.Toplevel);
			window.Showed += (sender, e) => surface.ShowSurface ();
			window.Closed += (sender, e) => surface.CloseSurface ();

			window.GetProperty ("Left").DependencyPropertyValueChanged += (sender, e) => surface.Move ((int)window.Left, (int)window.Top);
			window.GetProperty ("Top").DependencyPropertyValueChanged += (sender, e) => surface.Move ((int)window.Left, (int)window.Top);
		}

		public void UnregisterWindow (Window window)
		{
		}	

		public void RegisterPopup (Popup popup)
		{
			var width = popup.WidthRequest ?? 100;
			var height = popup.HeightRequest ?? 50;

			var surface = new GtkSurface (popup, 0, 0, width, height, Gtk.WindowType.Popup);
			popup.Opened += (sender, e) =>
			{
				var p = popup.PlacementTarget.PointToScreen (new Point (0, popup.PlacementTarget.DesiredSize.Height));
				surface.Move ((int)(p.X + popup.HorizontalOffset), (int)(p.Y + popup.VerticalOffset));

				surface.ShowSurface ();
			};
			popup.Closed += (sender, e) => surface.Hide ();
		}

	}
}

