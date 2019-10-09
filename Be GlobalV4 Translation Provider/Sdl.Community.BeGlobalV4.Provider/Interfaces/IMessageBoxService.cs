﻿namespace Sdl.Community.BeGlobalV4.Provider.Interfaces
{
	public interface IMessageBoxService
	{
		void ShowMessage(string text, string header);
		void ShowWarningMessage(string text, string header);
		void ShowInformationMessage(string text, string header);
		bool AskForConfirmation(string message);
	}
}