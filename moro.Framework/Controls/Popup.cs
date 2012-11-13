//
// Popup.cs
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
	public class Popup : FrameworkElement
	{
		public event EventHandler Closed;
		public event EventHandler Opened;

		private DependencyProperty<UIElement> placementTarget;
		private DependencyProperty<UIElement> child;
		private DependencyProperty<bool> isOpen;

		private DependencyProperty<double> verticalOffset;
		private DependencyProperty<double> horizontalOffset;

		public UIElement PlacementTarget { 
			get { return placementTarget.Value; }
			set { placementTarget.Value = value; }
		}

		public UIElement Child { 
			get { return child.Value; }
			set { child.Value = value; }
		}
		
		public bool IsOpen { 
			get { return isOpen.Value; }
			set { isOpen.Value = value; }
		}

		public double VerticalOffset { 
			get { return verticalOffset.Value; }
			set { verticalOffset.Value = value; }
		}

		public double HorizontalOffset { 
			get { return horizontalOffset.Value; }
			set { horizontalOffset.Value = value; }
		}

		public Popup ()
		{
			if (!Application.IsInitialized)
				throw new ApplicationException ("Application must be initialized");

			placementTarget = BuildProperty<UIElement> ("PlacementTarget");

			child = BuildProperty<UIElement> ("Child");
			child.DependencyPropertyValueChanged += HandleChildChanged;

			isOpen = BuildProperty<bool> ("IsOpen");
			isOpen.DependencyPropertyValueChanged += HandleIsOpenChanged;

			verticalOffset = BuildProperty<double> ("VerticalOffset");
			horizontalOffset = BuildProperty<double> ("HorizontalOffset");

			Application.Current.RegisterPopup (this);
		}

		private void HandleIsOpenChanged (object sender, DPropertyValueChangedEventArgs<bool> e)
		{
			if (e.NewValue)
				RaiseOpened ();
			else
				RaiseClosed ();
		}

		public void Open ()
		{
			IsOpen = true;
		}
		
		public void Close ()
		{
			IsOpen = false;
		}

		private void HandleChildChanged (object sender, DPropertyValueChangedEventArgs<UIElement> e)
		{
			if (e.OldValue != null)
				RemoveVisualChild (e.OldValue);
			
			if (e.NewValue != null)
				AddVisualChild (e.NewValue);
		}

		protected override Size MeasureOverride (Size availableSize)
		{
			if (Child == null || Child.Visibility == Visibility.Collapsed)
				return new Size (0, 0);
			
			Child.Measure (availableSize);			
			
			return Child.DesiredSize;
		}
		
		protected override void ArrangeOverride (Size finalSize)
		{
			if (Child == null)
				return;
			
			Child.Arrange (new Rect (finalSize));
		}
		
		public override int VisualChildrenCount {
			get {
				return Child == null ? 0 : 1;
			}
		}
		
		public override Visual GetVisualChild (int index)
		{
			return Child;
		}
		
		private void RaiseClosed ()
		{
			if (Closed != null)
				Closed (this, EventArgs.Empty);					
		}
		
		private void RaiseOpened ()
		{
			if (Opened != null)
				Opened (this, EventArgs.Empty);
		}
	}
}

