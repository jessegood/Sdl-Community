﻿using System.Linq;
using System.Net.Http.Headers;
using System.Windows;
using Sdl.Community.MTCloud.Provider.Helpers;
using Sdl.Community.MTCloud.Provider.Interfaces;
using Sdl.Community.MTCloud.Provider.Service;
using Sdl.Community.MTCloud.Provider.Service.Interface;
using Sdl.Desktop.IntegrationApi;
using Sdl.Desktop.IntegrationApi.Extensions;
using Sdl.ProjectAutomation.FileBased;
using Sdl.TranslationStudioAutomation.IntegrationApi;

namespace Sdl.Community.MTCloud.Provider
{
	[ApplicationInitializer]
	internal class MtCloudApplicationInitializer : IApplicationInitializer
	{
		private const string BatchProcessing = "batch processing";
		private const string CreateNewProject = "create a new project";
		private static EditorController _editorController;
		public static IHttpClient Client { get; } = new HttpClient();

		public static CurrentViewDetector CurrentViewDetector { get; set; } = new CurrentViewDetector();

		public static EditorController EditorController
			=> _editorController = _editorController ?? SdlTradosStudio.Application.GetController<EditorController>();

		public static MetadataSupervisor MetadataSupervisor
			=> new MetadataSupervisor(new SegmentMetadataCreator(), EditorController);

		public static ProjectsController ProjectsController { get; private set; }
		public static TranslationService TranslationService { get; private set; }

		public static void CloseOpenedDocuments()
		{
			var activeDocs = _editorController.GetDocuments().ToList();

			foreach (var activeDoc in activeDocs)
			{
				_editorController.Close(activeDoc);
			}
		}

		public static Window GetCurrentWindow() => Application.Current.Windows.Cast<Window>().FirstOrDefault(
			window => window.Title.ToLower() == BatchProcessing || window.Title.ToLower().Contains(CreateNewProject));

		public static FileBasedProject GetProjectInProcessing()
		{
			if (GetCurrentWindow()?.Title.ToLower().Contains(CreateNewProject) ?? false) return null;

			FileBasedProject projectInProcessing;
			switch (CurrentViewDetector.View)
			{
				case CurrentViewDetector.CurrentView.ProjectsView:
					projectInProcessing = ProjectsController.SelectedProjects.FirstOrDefault() ?? ProjectsController.CurrentProject;
					break;
				case CurrentViewDetector.CurrentView.FilesView:
				case CurrentViewDetector.CurrentView.EditorView:
					projectInProcessing = ProjectsController.CurrentProject;
					break;
				default:
					projectInProcessing = null;
					break;
			}
			return projectInProcessing;
		}

		public static void SetTranslationService(IConnectionService connectionService)
		{
			TranslationService = new TranslationService(connectionService, Client, new MessageBoxService());

			//TODO: start supervising when a QE enabled model has been chosen
			MetadataSupervisor.StartSupervising(TranslationService);
		}

		public void Execute()
		{
			ProjectsController = SdlTradosStudio.Application.GetController<ProjectsController>();
			Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}
	}
}