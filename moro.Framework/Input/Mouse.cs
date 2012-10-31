// 
// Mouse.cs
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
	public static class Mouse
	{
		public static MouseDevice Device { get; private set; }
		
		static Mouse ()
		{
			Device = new MouseDevice ();
		}
		
		public static event EventHandler<MouseButtonEventArgs> PreviewButtonPressEvent {
			add { Device.PreviewButtonPressEvent.Event += value; }
			remove { Device.PreviewButtonPressEvent.Event -= value; }
		}
		
		public static event EventHandler<MouseButtonEventArgs> ButtonPressEvent {
			add { Device.ButtonPressEvent.Event += value; }
			remove { Device.ButtonPressEvent.Event -= value; }
		}

		public static event EventHandler<MouseButtonEventArgs> PreviewButtonReleaseEvent {
			add { Device.PreviewButtonReleaseEvent.Event += value; }
			remove { Device.PreviewButtonReleaseEvent.Event -= value; }
		}
		
		public static event EventHandler<MouseButtonEventArgs> ButtonReleaseEvent {
			add { Device.ButtonReleaseEvent.Event += value; }
			remove { Device.ButtonReleaseEvent.Event -= value; }
		}
		
		public static event EventHandler<MouseButtonEventArgs> PreviewMotionNotifyEvent {
			add { Device.PreviewMotionNotifyEvent.Event += value; }
			remove { Device.PreviewMotionNotifyEvent.Event -= value; }
		}
		
		public static event EventHandler<MouseButtonEventArgs> MotionNotifyEvent {
			add { Device.MotionNotifyEvent.Event += value; }
			remove { Device.MotionNotifyEvent.Event -= value; }
		}
		
		public static event EventHandler<EventArgs> MouseEnterEvent {
			add { Device.MouseEnterEvent.Event += value; }
			remove { Device.MouseEnterEvent.Event -= value; }
		}
		
		public static event EventHandler<EventArgs> MouseLeaveEvent {
			add { Device.MouseLeaveEvent.Event += value; }
			remove { Device.MouseLeaveEvent.Event -= value; }
		}

		public static Visual Captured {
			get { return Device.Captured; }
			set { Device.Captured = value; }
		}

		public static Point GetPosition (Visual visual)
		{
			return Device.GetPosition (visual);
		}
	}
}

