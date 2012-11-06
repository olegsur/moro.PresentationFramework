// 
// Application.cs
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
using moro.Framework.Data;
using System.Linq;

namespace moro.Framework
{
	public class Application
	{
		public static Application Current { get; private set; }
		
		public ResourceDictionary Resources { get; private set; }
		public Window MainWindow { get; private	set; }		
		public IEnumerable<Window> Windows { get { return windows; } }
		
		internal static readonly bool IsInitialized = false;
		
		private IApplication aplication;
		private readonly List<Window> windows = new List<Window> ();
		private readonly List<IElementHost> roots = new List<IElementHost> ();
		
		static Application ()
		{	
			Current = new Application ();
			IsInitialized = true;
		}
		
		private Application ()
		{
			aplication = new GtkApplication ();			
			aplication.Init ();
			
			Resources = new DefaultTheme ();			
		}
		
		public void Run (Window window)
		{
			MainWindow = window;
			MainWindow.Closed += (sender, e) => aplication.Shutdown ();
			
			window.Show ();
			
			aplication.Run ();			
		}
		
		internal void RegisterWindow (Window window)
		{
			windows.Add (window);
			aplication.RegisterWindow (window);
		}
		
		internal void UnregisterWindow (Window window)
		{
			windows.Remove (window);
			aplication.UnregisterWindow (window);
		}
		
		public void RegisterRoot (IElementHost elementHost)
		{
			roots.Add (elementHost);
		}
		
		public IElementHost GetRoot (Visual visual)
		{
			return roots.FirstOrDefault (r => r.Child == visual);
		}
	}
}

