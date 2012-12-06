//
// Binding.cs
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

namespace moro.Framework.Data
{
	public class Binding
	{
		private bool updating = false;

		private PathExpression Source { get; set; }
		private PathExpression Target { get; set; }
		private IValueConverter Converter { get; set; }

		private IDependencyProperty SourceProperty { get; set; }
		private IDependencyProperty TargetProperty { get; set; }

		public Binding (PathExpression source, PathExpression target, IValueConverter converter)
		{
			Source = source;
			Target = target;
			Converter = converter;

			SourceProperty = Source.Property;
			TargetProperty = Target.Property;

			Source.GetProperty ("Property").DependencyPropertyValueChanged += HandleSourcePropertyChanged;
			Target.GetProperty ("Property").DependencyPropertyValueChanged += HandleTargetPropertyChanged;

			UpdateTargetValue ();

			if (SourceProperty != null)
				SourceProperty.DependencyPropertyValueChanged += HandleSourceValueChanged;

			if (TargetProperty != null)
				TargetProperty.DependencyPropertyValueChanged += HandleTargetValueChanged;

		}

		private void HandleSourcePropertyChanged (object sender, DPropertyValueChangedEventArgs e)
		{
			if (SourceProperty != null)
				SourceProperty.DependencyPropertyValueChanged -= HandleSourceValueChanged;

			SourceProperty = e.NewValue as IDependencyProperty;

			if (SourceProperty != null)
				SourceProperty.DependencyPropertyValueChanged += HandleSourceValueChanged;

			UpdateTargetValue ();
		}

		private void HandleTargetPropertyChanged (object sender, DPropertyValueChangedEventArgs e)
		{			
			if (TargetProperty != null)
				TargetProperty.DependencyPropertyValueChanged -= HandleTargetValueChanged;

			TargetProperty = e.NewValue as IDependencyProperty;

			if (TargetProperty != null)
				TargetProperty.DependencyPropertyValueChanged += HandleTargetValueChanged;

			UpdateTargetValue ();
		}

		private void HandleSourceValueChanged (object sender, DPropertyValueChangedEventArgs e)
		{
			UpdateTargetValue ();
		}


		private void HandleTargetValueChanged (object sender, DPropertyValueChangedEventArgs e)
		{
			UpdateSourceValue ();
		}

		private void UpdateTargetValue ()
		{
			if (SourceProperty == null || TargetProperty == null)
				return;

			if (updating)
				return;

			updating = true;

			TargetProperty.Value = Converter.Convert (SourceProperty.Value);

			updating = false;
		}

		private void UpdateSourceValue ()
		{
			if (SourceProperty == null || TargetProperty == null)
				return;

			if (updating)
				return;

			updating = true;

			SourceProperty.Value = Converter.ConvertBack (TargetProperty.Value);

			updating = false;
		}
	}
}

