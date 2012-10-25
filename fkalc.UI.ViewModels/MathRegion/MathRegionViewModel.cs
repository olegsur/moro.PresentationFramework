//
// MathRegionViewModel.cs
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
using System.Linq;
using moro.Framework;
using fkalc.UI.Common.MathRegion;
using fkalc.UI.Common.MathRegion.Tokens;
using fkalc.UI.ViewModels.MathRegion.Actions;
using fkalc.UI.ViewModels.MathRegion.Tokens;

namespace fkalc.UI.ViewModels.MathRegion
{
	public class MathRegionViewModel : DependencyObject, IMathRegionViewModel
	{	
		public DocumentViewModel Document { get; private set; }

		private readonly DependencyProperty<double> x;
		private readonly DependencyProperty<double> y;
		private readonly DependencyProperty<HBoxToken> root;
		private readonly DependencyProperty<Selection> selection;

		private readonly DependencyProperty<ICommand> insertCharacterCommand;
		private readonly DependencyProperty<ICommand> resultCommand;
		private readonly DependencyProperty<ICommand> plusCommand;
		private readonly DependencyProperty<ICommand> minusCommand;
		private readonly DependencyProperty<ICommand> multiplicationCommand;
		private readonly DependencyProperty<ICommand> divideCommand;
		private readonly DependencyProperty<ICommand> assignmentCommand;
		private readonly DependencyProperty<ICommand> leftCommand;
		private readonly DependencyProperty<ICommand> rightCommand;
		private readonly DependencyProperty<ICommand> evaluateCommand;
		private readonly DependencyProperty<ICommand> openBracketCommand;
		private readonly DependencyProperty<ICommand> closeBracketCommand;
		private readonly DependencyProperty<ICommand> commaCommand;
		private readonly DependencyProperty<ICommand> exponentiationCommand;
		private readonly DependencyProperty<ICommand> squareRootCommand;
		private readonly DependencyProperty<ICommand> absoluteCommand;

		private readonly DependencyProperty<bool> needToEvaluate;
		private readonly DependencyProperty<bool> hasError;

		public double X { 
			get { return x.Value; }
			set { x.Value = value; }
		}

		public double Y { 
			get { return y.Value; }
			set { y.Value = value; }
		}

		public HBoxToken Root { 
			get { return root.Value; }
			private set { root.Value = value; } 
		}

		public Selection Selection { 
			get { return selection.Value; }
			private set { selection.Value = value; }
		}

		public bool NeedToEvaluate {
			get { return needToEvaluate.Value; }
			private set { needToEvaluate.Value = value; }
		}

		public bool HasError {
			get { return hasError.Value; }
			set { hasError.Value = value; }
		}

		public MathRegionViewModel (DocumentViewModel document)
		{
			Document = document;

			x = BuildProperty<double> ("X");
			y = BuildProperty<double> ("Y");
			root = BuildProperty<HBoxToken> ("Root");
			selection = BuildProperty<Selection> ("Selection");

			insertCharacterCommand = BuildProperty<ICommand> ("InsertCharacterCommand");
			insertCharacterCommand.Value = new DelegateCommand<uint> (o => new InsertCharacterAction (o, this).Do ());

			plusCommand = BuildProperty<ICommand> ("PlusCommand");
			plusCommand.Value = new DelegateCommand<object> (o => new PlusAction (this).Do ());

			minusCommand = BuildProperty<ICommand> ("MinusCommand");
			minusCommand.Value = new DelegateCommand<object> (o => new MinusAction (this).Do ());

			multiplicationCommand = BuildProperty<ICommand> ("MultiplicationCommand");
			multiplicationCommand.Value = new DelegateCommand<object> (o => new MultiplicationAction (this).Do ());
			
			divideCommand = BuildProperty<ICommand> ("DivideCommand");
			divideCommand.Value = new DelegateCommand<object> (o => new DivideAction (this).Do ());

			assignmentCommand = BuildProperty<ICommand> ("AssignmentCommand");
			assignmentCommand.Value = new DelegateCommand<object> (o => new AssignmentAction (this).Do ());

			resultCommand = BuildProperty<ICommand> ("ResultCommand");
			resultCommand.Value = new DelegateCommand<object> (o => new ResultAction (this).Do ());

			leftCommand = BuildProperty<ICommand> ("LeftCommand");
			leftCommand.Value = new DelegateCommand<object> (o => new LeftAction (this).Do ());

			rightCommand = BuildProperty<ICommand> ("RightCommand");
			rightCommand.Value = new DelegateCommand<object> (o => new RightAction (this).Do ());

			evaluateCommand = BuildProperty<ICommand> ("EvaluateCommand");
			evaluateCommand.Value = new DelegateCommand<object> (o => new EvaluateAction (this).Do ());

			openBracketCommand = BuildProperty<ICommand> ("OpenBracketCommand");
			openBracketCommand.Value = new DelegateCommand<object> (o => new OpenBracketAction (this).Do ());

			closeBracketCommand = BuildProperty<ICommand> ("CloseBracketCommand");
			closeBracketCommand.Value = new DelegateCommand<object> (o => new CloseBracketAction (this).Do ());

			commaCommand = BuildProperty<ICommand> ("CommaCommand");
			commaCommand.Value = new DelegateCommand<object> (o => new CommaAction (this).Do ());

			exponentiationCommand = BuildProperty<ICommand> ("ExponentiationCommand");
			exponentiationCommand.Value = new DelegateCommand<object> (o => new ExponentiationAction (this).Do ());

			squareRootCommand = BuildProperty<ICommand> ("SquareRootCommand");
			squareRootCommand.Value = new DelegateCommand<object> (o => new SquareRootAction (this).Do ());

			absoluteCommand = BuildProperty<ICommand> ("AbsoluteCommand");
			absoluteCommand.Value = new DelegateCommand<object> (o => new AbsoluteAction (this).Do ());

			needToEvaluate = BuildProperty<bool> ("NeedToEvaluate");
			needToEvaluate.DependencyPropertyValueChanged += HandleNeedToEvaluateChanged;

			Root = new HBoxToken ();
			Selection = new Selection ();

			var token = new TextToken ();
			Root.Add (token);

			Selection.SelectedToken = token; 

			hasError = BuildProperty<bool> ("HasError");
		}


		IToken IMathRegionViewModel.Root {
			get { return Root; }
		}		

		public void SetResult (string result)
		{
			var resultToken = Root.Tokens.OfType<ResultToken> ().FirstOrDefault ();

			if (resultToken == null)
				return;

			resultToken.Child = new TextToken () { Text = result };
		}

		public void SetNeedToEvaluate (bool value)
		{
			HasError = false;

			if (!Root.Tokens.OfType<ResultToken> ().Any ())
				return;

			NeedToEvaluate = value;
		}

		private void HandleNeedToEvaluateChanged (object sender, DPropertyValueChangedEventArgs<bool> e)
		{
			if (!e.NewValue)
				return;

			var resultToken = Root.Tokens.OfType<ResultToken> ().FirstOrDefault ();

			if (resultToken == null)
				return;

			resultToken.Child = new TextToken ();
		}	
	}
}