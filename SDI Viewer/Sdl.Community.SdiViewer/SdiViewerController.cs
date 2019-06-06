﻿using System.Windows.Forms;
using Sdl.Desktop.IntegrationApi;
using Sdl.Desktop.IntegrationApi.Extensions;
using Sdl.TranslationStudioAutomation.IntegrationApi;

namespace Sdl.Community.SdiViewer
{
	[ViewPart(
		Id = "DSI Viewer",
		Name = "DSI Viewer",
		Description = "DSI Viewer",
		Icon = "")]
	[ViewPartLayout(typeof(EditorController), Dock = DockType.Bottom)]
	public class SdiViewerController : AbstractViewPartController
	{
		private readonly SdiControl _control = new SdiControl(); 
		protected override Control GetContentControl()
		{
			return _control;
		}

		protected override void Initialize()
		{
		}
	}
}
