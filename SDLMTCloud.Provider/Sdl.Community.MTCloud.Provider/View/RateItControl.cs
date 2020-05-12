﻿using System.Windows.Forms;
using Sdl.Community.MTCloud.Provider.Interfaces;
using Sdl.Community.MTCloud.Provider.ViewModel;

namespace Sdl.Community.MTCloud.Provider.View
{
	public partial class RateItControl : UserControl
	{
		public RateItControl(ITranslationService translationService)
		{
			InitializeComponent();
			
			var rateItViewModel = new RateItViewModel(translationService);
			var rateItWindow = new RateItWindow
			{
				DataContext = rateItViewModel
			};
			rateItWindow.InitializeComponent();
			rateItElementHost.Child = rateItWindow;
		}
	}
}