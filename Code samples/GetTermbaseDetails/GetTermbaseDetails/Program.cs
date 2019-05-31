﻿using System;
using Sdl.Community.GroupShareKit;
using Sdl.Community.GroupShareKit.Clients;
using Sdl.Community.GroupShareKit.Models.Response;

namespace GetTermbaseDetails
{
	class Program
	{
		static void Main(string[] args)
		{
			var token =  GroupShareClient.GetRequestToken(
				"SDLCommunity",
				"Commun1tyRocks",
				new Uri("http://gs2017dev.sdl.com"),
				GroupShareClient.AllScopes).Result;

			var gsClient = GroupShareClient.AuthenticateClient(
				token,
				"SDLCommunity",
				"Commun1tyRocks",
				new Uri("http://gs2017dev.sdl.com"),
				GroupShareClient.AllScopes).Result;
			var projectRequest = new ProjectsRequest("/Test API",true, 1) { Filter = { ProjectName = "a" } };
			var result =  gsClient.Project.GetProject(projectRequest).Result;


		}
	}
}
