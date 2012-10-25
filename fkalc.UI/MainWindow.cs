// 
// MainWindow.cs
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
using Gtk;
using fkalc.UI;
using moro.Framework;
using fkalc.UI.ViewModels;

public partial class MainWindow: Gtk.Window
{	
	private DocumentView rootElement = new DocumentView ();
	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
					
		var elementHost = new ElementHost (this, rootElement);
		
		Keyboard.Device.RegisterKeyboardInputProvider (new WidgetKeyboardInputProvider (elementHost));
		Mouse.Device.RegistedMouseInputProvider (new WidgetMouseInputProvider (elementHost));
		
		moro.Framework.Screen.Current = this;

		rootElement.DataContext = new DocumentViewModel ();
	}	
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Gtk.Application.Quit ();
		a.RetVal = true;
	}

	protected void OnExposeEvent (object o, Gtk.ExposeEventArgs args)
	{
		using (var cr = Gdk.CairoHelper.Create(this.GdkWindow)) {				
			
			var width = 0;
			var height = 0;
			
			this.GetSize (out width, out height);
			
			var size = new Size (width, height);
			
			rootElement.Measure (size);
			rootElement.Arrange (new Rect (size));
			rootElement.Render (new CairoContext (cr));
		}		
	}
}
