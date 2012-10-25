// 
// GtkSurface.cs
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
	public class GtkSurface : ISurface
	{
		public double Height { get; private set; }
		public double Width { get; private set; }
		
		private Gtk.Window Window { get; set; }		
		private Window Owner { get; set; }
		
		public GtkSurface (Window owner)
		{
			Owner = owner;
			
			Width = 100;
			Height = 50;
			
			Window = new Gtk.Window (Gtk.WindowType.Toplevel)
			{
				DefaultWidth = (int)Width,
				DefaultHeight = (int)Height,				
				Decorated = false,
				Events = ((global::Gdk.EventMask)(3334))
			};
			
			Window.ExposeEvent += OnExposeEvent;			
			
			var elementHost = new ElementHost (Window, Owner);
			
			Keyboard.Device.RegisterKeyboardInputProvider (new WidgetKeyboardInputProvider (elementHost));
			Mouse.Device.RegistedMouseInputProvider (new WidgetMouseInputProvider (elementHost));
		}
		
		public void Show ()
		{			
			Window.Show ();
		}

		public void Resize (Size size)
		{			
			Width = size.Width;
			Height = size.Height;
			
			Window.Resize ((int)Width, (int)Height);
		}
		
		protected void OnExposeEvent (object o, Gtk.ExposeEventArgs args)
		{
			using (var cr = Gdk.CairoHelper.Create(Window.GdkWindow)) {				
			
				var width = 0;
				var height = 0;
			
				Window.GetSize (out width, out height);
			
				var size = new Size (width, height);
			
				Owner.Measure (size);
				Owner.Arrange (new Rect (size));
				Owner.Render (new CairoContext (cr));
			}		
		}
				
		public void Close ()
		{
			Window.Destroy ();
		}
		
	}
}

