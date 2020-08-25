﻿using System.Collections.Generic;
using System.Windows.Input;
using Sdl.Community.MtEnhancedProvider.Model;

namespace Sdl.Community.MtEnhancedProvider.ViewModel.Interface
{
	public interface IProviderControlViewModel
	{
		ModelBase ViewModel { get; set; }
		ICommand ShowSettingsCommand { get; set; }
		List<TranslationOption> TranslationOptions { get; set; }
		List<GoogleApiVersion> GoogleApiVersions { get; set; }
		//TODO: save this value in settings
		GoogleApiVersion SelectedGoogleApiVersion { get; set; }
		TranslationOption SelectedTranslationOption { get; set; }
		bool IsMicrosoftSelected { get; set; }
		string ApiKey { get; set; }
	}
}