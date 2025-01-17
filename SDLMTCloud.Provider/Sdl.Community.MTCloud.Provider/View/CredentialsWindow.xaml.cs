﻿using System.Windows;
using System.Windows.Controls;
using Sdl.Community.MTCloud.Provider.ViewModel;

namespace Sdl.Community.MTCloud.Provider.View
{
	/// <summary>
	/// Interaction logic for Credentials.xaml
	/// </summary>
	public partial class CredentialsWindow : Window
	{
		public CredentialsWindow()
		{
			InitializeComponent();
		}

		private void UserPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
		{
			if (DataContext != null)
			{ ((CredentialsViewModel)DataContext).UserPassword = ((PasswordBox)sender).Password; }
		}

		private void ClientIdBox_PasswordChanged(object sender, RoutedEventArgs e)
		{
			if (DataContext != null)
			{ ((CredentialsViewModel)DataContext).ClientId = ((PasswordBox)sender).Password; }
		}

		private void ClientSecretBox_PasswordChanged(object sender, RoutedEventArgs e)
		{
			if (DataContext != null)
			{ ((CredentialsViewModel)DataContext).ClientSecret = ((PasswordBox)sender).Password; }
		}
	}
}
