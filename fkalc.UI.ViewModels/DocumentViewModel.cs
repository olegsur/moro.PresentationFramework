//
// DocumentViewModel.cs
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
using fkalc.UI.ViewModels.MathRegion;
using fkalc.UI.Common;

namespace fkalc.UI.ViewModels
{
	public class DocumentViewModel : DependencyObject, IDocumentViewModel
	{
		private readonly DependencyProperty<ObservableCollection<MathRegionViewModel>> regions;
		private readonly DependencyProperty<DocumentCursorViewModel> documentCursor;
		private readonly DependencyProperty<ICommand> newRegionCommand;

		public ObservableCollection<MathRegionViewModel> Regions { get { return regions.Value; } }
		public DocumentCursorViewModel DocumentCursor { get { return documentCursor.Value; } }

		public DocumentViewModel ()
		{
			regions = BuildProperty<ObservableCollection<MathRegionViewModel>> ("Regions");
			regions.Value = new ObservableCollection<MathRegionViewModel> ();

			documentCursor = BuildProperty<DocumentCursorViewModel> ("DocumentCursor");
			documentCursor.Value = new DocumentCursorViewModel ();

			newRegionCommand = BuildProperty<ICommand> ("NewRegionCommand");
			newRegionCommand.Value = new DelegateCommand<object> (o => NewRegion ()); 
		}

		private void NewRegion ()
		{
			var region = new MathRegionViewModel (this);

			region.X = DocumentCursor.X;
			region.Y = DocumentCursor.Y;

			Regions.Add (region);
		}
	}
}

