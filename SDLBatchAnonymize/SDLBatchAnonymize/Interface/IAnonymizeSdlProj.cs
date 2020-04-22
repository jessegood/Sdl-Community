﻿using Sdl.Core.Globalization;

namespace Sdl.Community.SDLBatchAnonymize.Interface
{
	public interface IAnonymizeSdlProj
	{
		void RemoveFileVersionComment(string projectPath);
		void RemoveTemplateId(string projectPath);
	}
}
