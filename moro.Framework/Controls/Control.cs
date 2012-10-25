using System;
namespace moro.Framework
{
	public class Control : FrameworkElement
	{
		private DependencyProperty<Brush> background;
		private DependencyProperty<Color> borderColor;
		private DependencyProperty<double> borderThickness;	
		private DependencyProperty<Thickness> padding;
		private DependencyProperty<ControlTemplate> template;

		public Brush Background { 
			get { return background.Value;}
			set { background.Value = value;}
		}

		public Color BorderColor { 
			get { return borderColor.Value;}
			set { borderColor.Value = value;}
		}
		
		public double BorderThickness { 
			get { return borderThickness.Value;}
			set { borderThickness.Value = value;}
		}
		
		public Thickness Padding { 
			get { return padding.Value;}
			set { padding.Value = value;}
		}
		
		public ControlTemplate Template { 
			get { return template.Value;}
			set { template.Value = value;}
		}
		
		private UIElement templateControl;
		
		public Control ()
		{
			background = BuildProperty<Brush> ("Background");
			borderColor = BuildProperty<Color> ("BorderColor");
			borderThickness = BuildProperty<double> ("BorderThickness");
			padding = BuildProperty<Thickness> ("Padding");
			template = BuildProperty<ControlTemplate> ("Template");

			Background = Brushes.Transparent;
			BorderThickness = 1;
			
			template.DependencyPropertyValueChanged += HandleTemplateChanged;
		}		
	
		public sealed override int VisualChildrenCount {
			get {
				return templateControl != null ? 1 : GetVisualChildrenCountCore ();
			}
		}
		
		protected virtual int GetVisualChildrenCountCore ()
		{
			return 0;
		}
		
		public sealed override Visual GetVisualChild (int index)
		{
			return templateControl != null ? templateControl : GetVisualChildCore (index);
		}
		
		protected virtual Visual GetVisualChildCore (int index)
		{
			return null;
		}		
		
		private void HandleTemplateChanged (object sender, DPropertyValueChangedEventArgs<ControlTemplate> e)
		{
			if (templateControl != null)				
				RemoveVisualChild (templateControl);
			
			templateControl = e.NewValue != null ? e.NewValue.LoadContent (this) : null;
			
			if (templateControl != null)
				AddVisualChild (templateControl);
		}
		
		protected override Size MeasureOverride (Size availableSize)
		{
			var child = GetVisualChild (0) as UIElement;
			
			if (child == null || child.Visibility == Visibility.Collapsed)
				return new Size (0, 0);
			
			child.Measure (availableSize);
			
			return child.DesiredSize;			
		}
		
		protected override void ArrangeOverride (Size finalSize)
		{
			var child = GetVisualChild (0) as UIElement;
			
			if (child == null || child.Visibility == Visibility.Collapsed)
				return;
			
			child.Arrange (new Rect (finalSize));
		}
	}
}

