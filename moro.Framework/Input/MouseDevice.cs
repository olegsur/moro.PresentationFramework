// 
// MouseDevice.cs
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
using System.Collections.Generic;
using System.Linq;

namespace moro.Framework
{
	public class MouseDevice
	{
		public RoutedEvent<ButtonPressEventArgs> PreviewButtonPressEvent { get; private set; }
		public RoutedEvent<ButtonPressEventArgs> ButtonPressEvent { get; private set; }
		
		public RoutedEvent<MotionNotifyEventArgs> PreviewMotionNotifyEvent { get; private set; }
		public RoutedEvent<MotionNotifyEventArgs> MotionNotifyEvent { get; private set; }
		
		public RoutedEvent<EventArgs> MouseEnterEvent { get; private set; }
		public RoutedEvent<EventArgs> MouseLeaveEvent { get; private set; }
		
		private Visual targetElement;
		
		private List<IMouseInputProvider> providers = new List<IMouseInputProvider> ();
		
		public MouseDevice ()
		{
			PreviewButtonPressEvent = new TunnelingEvent<ButtonPressEventArgs> ();
			ButtonPressEvent = new BubblingEvent<ButtonPressEventArgs> (); 

			PreviewMotionNotifyEvent = new TunnelingEvent<MotionNotifyEventArgs> ();
			MotionNotifyEvent = new BubblingEvent<MotionNotifyEventArgs> ();

			MouseEnterEvent = new DirectEvent<EventArgs> ();
			MouseLeaveEvent = new DirectEvent<EventArgs> ();
		}
		
		public Visual TargetElement { 
			get { return targetElement; }
			private set {
				if (targetElement == value)
					return;

				var oldTree = VisualTreeHelper.GetVisualBranch (targetElement);
				var newTree = VisualTreeHelper.GetVisualBranch (value);

				foreach (var element in oldTree.Except(newTree))
					RaiseMouseLeaveEvent (element);
				
				targetElement = value;

				foreach (var element in newTree.Except(oldTree))
					RaiseMouseEnterEvent (element);				
			}			
		}		
		
		public void RegistedMouseInputProvider (IMouseInputProvider provider)
		{
			provider.ButtonPressEvent += HandleProviderButtonPressEvent;
			provider.MotionNotifyEvent += HandleMotionNotifyEvent;
			providers.Add (provider);
		}		
		
		public void UnregisterMouseInputProvider (IMouseInputProvider provider)
		{
			provider.ButtonPressEvent -= HandleProviderButtonPressEvent;
			provider.MotionNotifyEvent -= HandleMotionNotifyEvent;
			providers.Remove (provider);			
		}

		private void HandleProviderButtonPressEvent (object o, ButtonPressEventArgs args)
		{
			RaisePreviewButtonPressEvent (args);
			RaiseButtonPressEvent (args);
		}
		
		private void HandleMotionNotifyEvent (object o, MotionNotifyEventArgs args)
		{
			var provider = providers.FirstOrDefault (p => p == o);
			
			if (provider == null)
				return;
			
			TargetElement = VisualTreeHelper.HitTest (new Point (args.Event.X, args.Event.Y), provider.RootElement);
			
			RaisePreviewMotionNotifyEvent (args);
			RaiseMotionNotifyEvent (args);
		}
		
		private void RaisePreviewButtonPressEvent (ButtonPressEventArgs args)
		{
			if (TargetElement == null)
				return;

			PreviewButtonPressEvent.RaiseEvent (TargetElement, args);
		}	
		
		private void RaiseButtonPressEvent (ButtonPressEventArgs args)
		{
			if (TargetElement == null)
				return;

			ButtonPressEvent.RaiseEvent (TargetElement, args);
		}
		
		private void RaisePreviewMotionNotifyEvent (MotionNotifyEventArgs args)
		{
			if (TargetElement == null)
				return;

			PreviewMotionNotifyEvent.RaiseEvent (TargetElement, args);
		}	
		
		private void RaiseMotionNotifyEvent (MotionNotifyEventArgs args)
		{
			if (TargetElement == null)
				return;

			MotionNotifyEvent.RaiseEvent (TargetElement, args);
		}
		
		private void RaiseMouseEnterEvent (Visual visual)
		{
			if (visual == null)
				return;

			MouseEnterEvent.RaiseEvent (visual, EventArgs.Empty);
		}
		
		private void RaiseMouseLeaveEvent (Visual visual)
		{
			if (visual == null)
				return;

			MouseLeaveEvent.RaiseEvent (visual, EventArgs.Empty);
		}
	}
}

