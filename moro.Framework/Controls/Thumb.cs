//
// Thumb.cs
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
	public class Thumb : Control
	{
		public event EventHandler<DragDeltaEventArgs> DragDelta;

		public bool IsDragging { get; private set; }
		private Point MousePosition { get; set; }

		public Thumb ()
		{
			ButtonPressEvent += HandleButtonPressEvent;
			ButtonReleaseEvent += HandleButtonReleaseEvent;
			MotionNotifyEvent += HandleMotionNotifyEvent;		
		}

		private void HandleMotionNotifyEvent (object sender, MouseButtonEventArgs e)
		{
			if (!IsDragging)
				return; 

			var previosMousePosition = MousePosition;

			MousePosition = PointToScreen (Mouse.GetPosition (this));

			RaiseDragDelta (MousePosition.X - previosMousePosition.X, MousePosition.Y - previosMousePosition.Y);
		}

		private void HandleButtonPressEvent (object sender, MouseButtonEventArgs e)
		{
			IsDragging = true;

			MousePosition = PointToScreen (Mouse.GetPosition (this));

			Mouse.Captured = this;
		}

		private void HandleButtonReleaseEvent (object sender, MouseButtonEventArgs e)
		{
			IsDragging = false;
			Mouse.Captured = null;
		}

		private void RaiseDragDelta (double horizontalChange, double verticalChange)
		{
			if (DragDelta != null)
				DragDelta (this, new DragDeltaEventArgs (horizontalChange, verticalChange));
		}
	}
}

