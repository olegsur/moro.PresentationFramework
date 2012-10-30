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
using moro.Framework.Data;

namespace moro.Framework
{
	public class GtkSurface : Gtk.Window, IElementHost
	{
		public double Height { get; private set; }
		public double Width { get; private set; }

		private Window Owner { get; set; }

		Visual IElementHost.Child { get { return Owner; } }
		
		public GtkSurface (Window owner): base (Gtk.WindowType.Toplevel)
		{
			Owner = owner;

			Move ((int)Owner.Left, (int)Owner.Top);

			Width = owner.WidthRequest ?? 100;
			Height = owner.HeightRequest ?? 50;

			DefaultWidth = (int)Width;
			DefaultHeight = (int)Height;
			Decorated = false;
			Events = Gdk.EventMask.ExposureMask
				| Gdk.EventMask.ButtonPressMask
				| Gdk.EventMask.ButtonReleaseMask
				| Gdk.EventMask.PointerMotionMask
				| Gdk.EventMask.KeyPressMask
				| Gdk.EventMask.KeyReleaseMask;  
						
			ExposeEvent += OnExposeEvent;			

			Owner.Closed += HandleClosed;
			Owner.Showed += HandleShowed;

			Keyboard.Device.RegisterKeyboardInputProvider (new WidgetKeyboardInputProvider (this));
			Mouse.Device.RegistedMouseInputProvider (new WidgetMouseInputProvider (this, owner));

			Application.Current.RegisterRoot (this);

			owner.GetProperty ("Left").DependencyPropertyValueChanged += HandlePosionChanged;
			owner.GetProperty ("Top").DependencyPropertyValueChanged += HandlePosionChanged;
		}

		private void HandlePosionChanged (object sender, DPropertyValueChangedEventArgs e)
		{		
			Move ((int)Owner.Left, (int)Owner.Top);
		}
						
		public void Resize (Size size)
		{			
			Width = size.Width;
			Height = size.Height;
			
			Resize ((int)Width, (int)Height);
		}
		
		protected void OnExposeEvent (object o, Gtk.ExposeEventArgs args)
		{
			using (var cr = Gdk.CairoHelper.Create(GdkWindow)) {				
			
				var width = 0;
				var height = 0;
			
				GetSize (out width, out height);
			
				var size = new Size (width, height);
			
				Owner.Measure (size);
				Owner.Arrange (new Rect (size));
				Owner.Render (new CairoContext (cr));
			}		
		}
				
		private void HandleClosed (object sender, EventArgs e)
		{
			Destroy ();
		}

		private void HandleShowed (object sender, EventArgs e)
		{		
			var size = new Size (Width, Height);

			Owner.Measure (size);
			Owner.Arrange (new Rect (Owner.DesiredSize));

			Resize (new Size (Owner.Width, Owner.Height));
			Show ();
		}

		public void Render ()
		{
			QueueDraw ();
		}

		public Point GetPosition ()
		{
			int x;
			int y;

			base.GetPosition (out x, out y);

			return new Point (x, y);
		}	
	}
}

