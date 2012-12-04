// 
// FrameworkElement.cs
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
	public class FrameworkElement : UIElement
	{
		public double Height { get; private set; }
		public double Width { get; private set; }	
		
		public double? HeightRequest { get; set; }
		public double? WidthRequest { get; set; }

		public Transform LayoutTransform { get; set; }

		private readonly DependencyProperty<object> dataContext;
		private readonly DependencyProperty<Thickness> margin;
		private readonly DependencyProperty<HorizontalAlignment> horizontalAlignment;
		private readonly DependencyProperty<VerticalAlignment> verticalAlignment;
		
		private readonly DependencyProperty<Style> style;

		public object DataContext { 
			get { return dataContext.Value;} 
			set { dataContext.Value = value; }
		}

		public Thickness Margin { 
			get { return margin.Value;} 
			set { margin.Value = value; }
		}

		public HorizontalAlignment HorizontalAlignment { 
			get { return horizontalAlignment.Value;} 
			set { horizontalAlignment.Value = value; }
		}

		public VerticalAlignment VerticalAlignment { 
			get { return verticalAlignment.Value;} 
			set { verticalAlignment.Value = value; }
		}
		
		public Style Style { 
			get { return style.Value;} 
			set { style.Value = value; }
		}

		private readonly List<SetterOperation> setterOperations = new List<SetterOperation> ();
		
		public FrameworkElement ()
		{	
			dataContext = BuildProperty<object> ("DataContext");
			margin = BuildProperty<Thickness> ("Margin");
			horizontalAlignment = BuildProperty<HorizontalAlignment> ("HorizontalAlignment");
			verticalAlignment = BuildProperty<VerticalAlignment> ("VerticalAlignment");
			style = BuildProperty<Style> ("Style");			

			Margin = new Thickness (0);
			HorizontalAlignment = HorizontalAlignment.Left;
			VerticalAlignment = VerticalAlignment.Top;
			
			style.DependencyPropertyValueChanged += HandleStyleChanged;
		}			
		
		private Size savedSize;
		
		protected sealed override Size MeasureCore (Size availableSize)
		{
			var size = MeasureOverride (availableSize);

			var height = HeightRequest ?? size.Height;
			var width = WidthRequest ?? size.Width;

			if (LayoutTransform != null) {
				savedSize = size;
				
				size = LayoutTransform.TransformBounds (new Rect (0, 0, width, height)).Size;
				height = size.Height;
				width = size.Width;
			}				
			
			return new Size (width, height);
		}
		
		protected sealed override void ArrangeCore (Rect finalRect)
		{	
			base.ArrangeCore (finalRect);
			
			Height = VerticalAlignment == VerticalAlignment.Stretch ? finalRect.Height : DesiredSize.Height;
			Width = HorizontalAlignment == HorizontalAlignment.Stretch ? finalRect.Width : DesiredSize.Width;
			
			if (LayoutTransform != null) {
				//TODO: adjust height and width depends on vertical/horizontal aligments
				Height = savedSize.Height;
				Width = savedSize.Width;
			}
			
			ArrangeOverride (new Size (Width, Height));
		}
		
		protected virtual Size MeasureOverride (Size availableSize)
		{		
			return new Size (0, 0);
		}
		
		protected virtual void ArrangeOverride (Size finalSize)
		{	
		}

		public override Visual HitTest (Point point)
		{
			return IsVisible 
				&& point.X >= 0 && point.X <= Width 
				&& point.Y >= 0 && point.Y <= Height ? this : null;
		}
		
		private void HandleStyleChanged (object sender, DPropertyValueChangedEventArgs<Style> e)
		{
			if (e.OldValue != null)
				RemoveStyle (e.OldValue);

			if (e.NewValue != null)
				ApplyStyle (e.NewValue);
		}

		private void RemoveStyle (Style style)
		{
			foreach (var operation in setterOperations) {
				operation.Remove ();
			}

			setterOperations.Clear ();
		}

		private void ApplyStyle (Style style)
		{
			foreach (var setter in style.Setters.OfType<Setter>()) {
				var operation = new SetterOperation (this, setter);
				operation.Apply ();

				setterOperations.Add (operation);
			}
		}
	}
}