﻿using System.Collections.Generic;
using System.Data.SQLite;
using InterpretBank.Model;
using InterpretBank.Service;
using InterpretBank.Service.Interface;
using InterpretBank.Service.Model;
using NSubstitute;
using Xunit;

namespace InterpretBankTests
{
	public class GlossaryServiceTests
	{
		private readonly GlossaryServiceBuilder _glossaryServiceBuilder;

		public GlossaryServiceTests()
		{
			_glossaryServiceBuilder = new GlossaryServiceBuilder();
		}

		[Fact]
		public void CreateGlossary_Test()
		{
			var connectionMock = Substitute.For<IDatabaseConnection>();
			var glossaryService = _glossaryServiceBuilder
				.WithDatabaseConnection(connectionMock)
				.Build();

			glossaryService.Create(new GlossaryMetadataEntry());

			connectionMock
				.Received()
				.ExecuteCommand(Arg.Is<SQLiteCommand>(s => s.CommandText == "SELECT * FROM DatabaseInfo"));
			connectionMock
				.Received()
				.ExecuteCommand(Arg.Is<SQLiteCommand>(s => s.CommandText ==
															@"INSERT INTO GlossaryMetadata (GlossaryCreator, GlossaryDataCreation, GlossaryDescription, GlossarySetting, Tag1, Tag2) VALUES (""@0"", ""@1"", ""@2"", ""@3"", ""@4"", ""@5"")"));
		}

		[Fact]
		public void CreateTerm_Test()
		{
			var connectionMock = Substitute.For<IDatabaseConnection>();
			var glossaryService = _glossaryServiceBuilder
				.WithDatabaseConnection(connectionMock)
				.Build();

			glossaryService.Create(new TermEntry());

			connectionMock
				.Received()
				.ExecuteCommand(Arg.Is<SQLiteCommand>(s => s.CommandText == "SELECT * FROM DatabaseInfo"));
			connectionMock
				.Received()
				.ExecuteCommand(Arg.Is<SQLiteCommand>(s => s.CommandText ==
					@"INSERT INTO GlossaryData (CommentAll, Tag1, Tag2, RecordCreation) VALUES (""@0"", ""@1"", ""@2"", ""@3"")"));
		}

		[Fact]
		public void DeleteTerm_Test()
		{
			var connectionMock = Substitute.For<IDatabaseConnection>();
			var glossaryService = _glossaryServiceBuilder
				.WithDatabaseConnection(connectionMock)
				.Build();

			glossaryService.DeleteTerm("5");

			connectionMock
				.Received()
				.ExecuteCommand(Arg.Is<SQLiteCommand>(s => s.CommandText == "SELECT * FROM DatabaseInfo"));

			connectionMock
				.Received()
				.ExecuteCommand(Arg.Is<SQLiteCommand>(s => s.CommandText == @"DELETE FROM GlossaryData WHERE (ID = @0)"));
		}
		
		[Fact]
		public void DeleteGlossary_Test()
		{
			var connectionMock = Substitute.For<IDatabaseConnection>();


			connectionMock
				.ExecuteCommand(default)
				.ReturnsForAnyArgs
				(
					null,
					new List<Dictionary<string, string>>
					{
						new() {["Tag1"] = "TestGlossary", ["Tag2"] = "TestSubGlossary"}
					},
					null,
					null
				);

			var glossaryService = _glossaryServiceBuilder
				.WithDatabaseConnection(connectionMock)
				.Build();

			glossaryService.DeleteGlossary("5");

			
			connectionMock
				.Received()
				.ExecuteCommand(Arg.Is<SQLiteCommand>(s => s.CommandText == "SELECT * FROM DatabaseInfo"));

			connectionMock
				.Received()
				.ExecuteCommand(Arg.Is<SQLiteCommand>(s =>
					s.CommandText == "SELECT Tag1, Tag2 FROM GlossaryMetadata WHERE ID = 5"));

			connectionMock
				.Received()
				.ExecuteCommand(
					Arg.Is<SQLiteCommand>(s =>
						s.CommandText ==
						@"DELETE FROM GlossaryData WHERE Tag1 = TestGlossary AND Tag2 = TestSubGlossary"));
		}

		[Fact]
		public void GetGlossaries_RealDB_Test()
		{
			var filepath = @"../../Resources/InterpretBankDatabaseV6.db";
			var glossaryService = _glossaryServiceBuilder
				.WithDatabaseConnection(new DatabaseConnection(filepath))
				.Build();

			var termList = glossaryService.GetGlossaries();

			Assert.Equal(6, termList.Count);
			Assert.Equal(typeof(GlossaryMetadataEntry), termList[0].GetType());
		}

		[Fact]
		public void GetTerms_RealDB_Test()
		{
			var filepath = @"../../Resources/InterpretBankDatabaseV6.db";
			var glossaryService = _glossaryServiceBuilder
				.WithDatabaseConnection(new DatabaseConnection(filepath))
				.Build();

			var languages = new List<int> { 1, 2 };
			var termList = glossaryService.GetTerms(null, languages, null,
				new List<string> { "AnotherTagOne" });

			Assert.Single(termList);
			Assert.Equal(2, ((TermEntry)termList[0]).LanguageEquivalents.Count);
			Assert.Equal(typeof(TermEntry), termList[0].GetType());
		}

		[Fact]
		public void GetTerms_Test()
		{
			//Arrange
			var connectionMock = Substitute.For<IDatabaseConnection>();
			var glossaryService = _glossaryServiceBuilder
				.WithDatabaseConnection(connectionMock)
				.Build();
			var languages = new List<int> { 1, 2 };
			var expectedSqlStatement =
				"SELECT ID, Tag1, Tag2, CommentAll, Term1, Comment1a, Comment1b, Term2, Comment2a, Comment2b FROM GlossaryData WHERE Tag1 IN (SELECT Tag1 FROM GlossaryMetadata INNER JOIN TagLink ON GlossaryID=ID WHERE TagName IN (@0))";

			var sqlData = new List<Dictionary<string, string>>
			{
				new()
				{
					["ID"] = "2",
					["Tag1"] =  null,
					["Tag2"] = null,
					["Term1"] = null,
					["Term2"] = null,
					["CommentAll"] = null,
					["Comment1a"] = null,
					["Comment2a"] = null,
					["Comment1b"] = null,
					["Comment2b"] = null
				}
			};

			connectionMock
				.ExecuteCommand(Arg.Is<SQLiteCommand>(s => s.CommandText == expectedSqlStatement))
				.Returns(sqlData);

			//Act
			var termList = glossaryService.GetTerms(null, languages, null,
				new List<string> { "AnotherTagOne" });

			//Assert
			connectionMock
				.Received()
				.ExecuteCommand(Arg.Is<SQLiteCommand>(s => s.CommandText == "SELECT * FROM DatabaseInfo"));

			connectionMock
				.Received()
				.ExecuteCommand(Arg.Is<SQLiteCommand>(s => s.CommandText == expectedSqlStatement));

			Assert.Single(termList);
			Assert.Equal(typeof(TermEntry), termList[0].GetType());
		}

		[Fact]
		public void MergeGlossaries_Test()
		{
			var connectionMock = Substitute.For<IDatabaseConnection>();
			var glossaryService = _glossaryServiceBuilder
				.WithDatabaseConnection(connectionMock)
				.Build();

			glossaryService.MergeGlossaries("Glossary", "ToBeMergedGlossary");

			connectionMock
				.Received()
				.ExecuteCommand(Arg.Is<SQLiteCommand>(s => s.CommandText == "SELECT * FROM DatabaseInfo"));
			connectionMock
				.Received()
				.ExecuteCommand(Arg.Is<SQLiteCommand>(s =>
					s.CommandText ==
					@"UPDATE GlossaryData SET Tag1 = ""@0"", Tag2 = ""@1"" WHERE Tag1 = ""ToBeMergedGlossary"""));
		}

		[Fact]
		public void UpdateGlossaryMetadata_Test()
		{
			var connectionMock = Substitute.For<IDatabaseConnection>();
			var glossaryService = _glossaryServiceBuilder
				.WithDatabaseConnection(connectionMock)
				.Build();

			glossaryService.UpdateContent(new GlossaryMetadataEntry
			{
				ID = 2,
				Tag1 = "Glossary"
			});

			connectionMock
				.Received()
				.ExecuteCommand(Arg.Is<SQLiteCommand>(s => s.CommandText == "SELECT * FROM DatabaseInfo"));

			connectionMock
				.Received()
				.ExecuteCommand(Arg.Is<SQLiteCommand>(s =>
					s.CommandText ==
					@"UPDATE GlossaryMetadata SET GlossaryCreator = ""@0"", GlossaryDataCreation = ""@1"", GlossaryDescription = ""@2"", GlossarySetting = ""@3"", Tag1 = ""@4"", Tag2 = ""@5"" WHERE (ID = @6)"));
		}

		[Fact]
		public void UpdateTermContent_Test()
		{
			var connectionMock = Substitute.For<IDatabaseConnection>();
			var glossaryService = _glossaryServiceBuilder
				.WithDatabaseConnection(connectionMock)
				.Build();

			glossaryService.UpdateContent(new TermEntry
			{
				ID = 2,
				Tag1 = "Glossary"
			});

			connectionMock
				.Received()
				.ExecuteCommand(Arg.Is<SQLiteCommand>(s => s.CommandText == "SELECT * FROM DatabaseInfo"));

			connectionMock
				.Received()
				.ExecuteCommand(Arg.Is<SQLiteCommand>(s =>
					s.CommandText ==
					@"UPDATE GlossaryData SET CommentAll = ""@0"", Tag1 = ""@1"", Tag2 = ""@2"", RecordCreation = ""@3"" WHERE (ID = @4)"));
		}
	}
}