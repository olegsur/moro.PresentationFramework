// 
// MathRegion.cs
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
using moro.Framework;
using fkalc.UI.ViewModels.MathRegion.Tokens;
using fkalc.UI.ViewModels.MathRegion;

namespace fkalc.UI
{
	public class MathRegion : UserControl
	{
		private Border border;
		private readonly DependencyProperty<Selection> selection;
		private readonly DependencyProperty<ICommand> insertCharacterCommand;
		private readonly DependencyProperty<ICommand> evaluateCommand;

		public Selection Selection {
			get { return selection.Value; }
		}

		public HBoxToken Root {
			get {
				return (border.Child as HBoxArea).DataContext as HBoxToken;
			}
		}

		public ICommand InsertCharacterCommand {
			get { return insertCharacterCommand.Value; }
		}

		public ICommand EvaluateCommand { 
			get { return evaluateCommand.Value; }
		}
		
		public MathRegion ()
		{
			Focusable = true;

			border = new Border ()
			{
				BorderColor = Colors.Bisque
			};	

			BindingOperations.SetBinding (this, "DataContext.Root", border.GetProperty ("Child"), new TokenAreaConverter ());
									
			Content = new AdornerDecorator () { Child = border };

			AdornerLayer.GetAdornerLayer (border).Add (new CursorLines (this));
			
			MouseEnterEvent += HandleMouseEnterEvent;
			MouseLeaveEvent += HandleMouseLeaveEvent;
			LostKeyboardFocusEvent += HandleLostKeyboardFocusEvent;
			
			new InsertCharacterProcessor (this);

			var plusKeyBinding = new KeyBinding ();
			BindingOperations.SetBinding (this, "DataContext.PlusCommand", plusKeyBinding.GetProperty ("Command"));
			plusKeyBinding.Gesture = new KeyGesture (Gdk.Key.plus);

			var minusKeyBinding = new KeyBinding ();
			BindingOperations.SetBinding (this, "DataContext.MinusCommand", minusKeyBinding.GetProperty ("Command"));
			minusKeyBinding.Gesture = new KeyGesture (Gdk.Key.minus);

			var multiplicationBinding = new KeyBinding ();
			BindingOperations.SetBinding (this, "DataContext.MultiplicationCommand", multiplicationBinding.GetProperty ("Command"));
			multiplicationBinding.Gesture = new KeyGesture (Gdk.Key.asterisk);

			var divideBinding = new KeyBinding ();
			BindingOperations.SetBinding (this, "DataContext.DivideCommand", divideBinding.GetProperty ("Command"));
			divideBinding.Gesture = new KeyGesture (Gdk.Key.slash);

			var assignmentBinding = new KeyBinding ();
			BindingOperations.SetBinding (this, "DataContext.AssignmentCommand", assignmentBinding.GetProperty ("Command"));
			assignmentBinding.Gesture = new KeyGesture (Gdk.Key.colon);

			var resultKeyBinding = new KeyBinding ();
			BindingOperations.SetBinding (this, "DataContext.ResultCommand", resultKeyBinding.GetProperty ("Command"));
			resultKeyBinding.Gesture = new KeyGesture (Gdk.Key.equal);

			var leftKeyBinding = new KeyBinding ();
			BindingOperations.SetBinding (this, "DataContext.LeftCommand", leftKeyBinding.GetProperty ("Command"));
			leftKeyBinding.Gesture = new KeyGesture (Gdk.Key.Left);

			var rightKeyBinding = new KeyBinding ();
			BindingOperations.SetBinding (this, "DataContext.RightCommand", rightKeyBinding.GetProperty ("Command"));
			rightKeyBinding.Gesture = new KeyGesture (Gdk.Key.Right);

			var openBracketBinding = new KeyBinding ();
			BindingOperations.SetBinding (this, "DataContext.OpenBracketCommand", openBracketBinding.GetProperty ("Command"));
			openBracketBinding.Gesture = new KeyGesture (Gdk.Key.parenleft);

			var closeBracketBinding = new KeyBinding ();
			BindingOperations.SetBinding (this, "DataContext.CloseBracketCommand", closeBracketBinding.GetProperty ("Command"));
			closeBracketBinding.Gesture = new KeyGesture (Gdk.Key.parenright);

			var commaBinding = new KeyBinding ();
			BindingOperations.SetBinding (this, "DataContext.CommaCommand", commaBinding.GetProperty ("Command"));
			commaBinding.Gesture = new KeyGesture (Gdk.Key.comma);

			var exponentiationBinding = new KeyBinding ();
			BindingOperations.SetBinding (this, "DataContext.ExponentiationCommand", exponentiationBinding.GetProperty ("Command"));
			exponentiationBinding.Gesture = new KeyGesture (Gdk.Key.asciicircum);

			var squareRootBinding = new KeyBinding ();
			BindingOperations.SetBinding (this, "DataContext.SquareRootCommand", squareRootBinding.GetProperty ("Command"));
			squareRootBinding.Gesture = new KeyGesture (Gdk.Key.backslash);

			var absoluteBinding = new KeyBinding ();
			BindingOperations.SetBinding (this, "DataContext.AbsoluteCommand", absoluteBinding.GetProperty ("Command"));
			absoluteBinding.Gesture = new KeyGesture (Gdk.Key.bar);

			InputBindings.Add (plusKeyBinding);
			InputBindings.Add (minusKeyBinding);
			InputBindings.Add (multiplicationBinding);
			InputBindings.Add (divideBinding);
			InputBindings.Add (assignmentBinding);
			InputBindings.Add (resultKeyBinding);
			InputBindings.Add (leftKeyBinding);
			InputBindings.Add (rightKeyBinding);
			InputBindings.Add (openBracketBinding);
			InputBindings.Add (closeBracketBinding);
			InputBindings.Add (commaBinding);
			InputBindings.Add (exponentiationBinding);
			InputBindings.Add (squareRootBinding);
			InputBindings.Add (absoluteBinding);

			selection = BuildProperty<Selection> ("Selection");
			insertCharacterCommand = BuildProperty<ICommand> ("InsertCharacterCommand");
			evaluateCommand = BuildProperty<ICommand> ("EvaluateCommand");

			BindingOperations.SetBinding (this, "DataContext.Selection", GetProperty ("Selection"));
			BindingOperations.SetBinding (this, "DataContext.InsertCharacterCommand", GetProperty ("InsertCharacterCommand"));
			BindingOperations.SetBinding (this, "DataContext.EvaluateCommand", GetProperty ("EvaluateCommand"));

			BindingOperations.SetBinding (this, "DataContext.HasError", GetProperty ("Background"), new HasErrorToBrushesConverter ());
		}

		private void HandleMouseEnterEvent (object sender, EventArgs e)
		{
			border.BorderColor = Colors.Red;

			Screen.QueueDraw ();
		}
		
		private void HandleMouseLeaveEvent (object sender, EventArgs e)
		{
			border.BorderColor = Colors.Bisque;
			
			Screen.QueueDraw ();
		}

		private void HandleLostKeyboardFocusEvent (object sender, EventArgs e)
		{
			if (EvaluateCommand != null) {
				EvaluateCommand.Execute (null);
			}
		}

		private class HasErrorToBrushesConverter : IValueConverter
		{
			public object Convert (object value)
			{
				return ((bool)value) ? Brushes.Pink : Brushes.Transparent;
			}

			public object ConvertBack (object value)
			{
				throw new System.NotImplementedException ();
			}
		}
	}
}