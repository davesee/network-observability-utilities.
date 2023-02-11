using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using Xunit;

namespace EFR.NetworkObservability.Common.Test;

[ExcludeFromCodeCoverage]
public class UtilsTest
{
	public class GetEnvVarTest
	{
		[Fact]
		public void ShouldReturnStringEnvVar()
		{
			var envName = "TEST_STRING_VAR";
			var envVal = "This is a test";
			Environment.SetEnvironmentVariable(envName, envVal);

			var result = Utils.GetEnvVar(envName);
			Assert.Equal(envVal, result);
		}

		[Fact]
		public void ShouldReturnIntEnvVar()
		{
			var envName = "TEST_INT_VAR";
			var envVal = 10;
			Environment.SetEnvironmentVariable(envName, envVal.ToString());

			var result = Utils.GetEnvVar<int>(envName);
			Assert.Equal(envVal, result);
		}

		[Fact]
		public void ShouldReturnDefaultIntEnvVar()
		{
			var envName = "TEST_DEFAULT_INT_VAR";
			var envVal = 10;

			var result = Utils.GetEnvVarOrDefault(envName, envVal);
			Assert.Equal(envVal, result);
		}

		[Fact]
		public void ShouldReturnEnvAndNotDefault()
		{
			var envName = "TEST_DEFAULT_STRING_VAR";
			var envVal = "THIS_IS_THE_VALUE";
			Environment.SetEnvironmentVariable(envName, envVal);
			var result = Utils.GetEnvVarOrDefault(envName, "DEFAULT");
			Assert.Equal(envVal, result);
		}
	}

	public class FileSystemTest
	{
		private readonly string directoryName = Path.Join(Path.GetTempPath(), "TestDir");

		~FileSystemTest()
		{
			if (Directory.Exists(directoryName))
			{
				Directory.Delete(directoryName, true);
			}
		}

		[Fact]
		public void ShouldCreateDirectory()
		{
			Utils.CreateDirectory(directoryName);
			Assert.True(Directory.Exists(directoryName));
		}

		[Fact]
		public void ShouldOverwriteDirectory()
		{
			Directory.CreateDirectory(directoryName);

			Assert.True(Directory.Exists(directoryName), $"Directory '{directoryName}' wasn't created");

			DateTime initalTime = Directory.GetCreationTime(directoryName);

			Thread.Sleep(1000);

			Utils.CreateDirectory(directoryName, true);

			DateTime newTime = Directory.GetCreationTime(directoryName);

			Assert.False(initalTime.Equals(newTime), $"Initial time {initalTime:MM/dd/yyyy HH:mm:ss.fffffff} = new time {newTime:MM/dd/yyyy HH:mm:ss.fffffff}");
		}

		[Fact]
		public void ShouldDeleteFile()
		{
			var fileName = Path.Join(directoryName, "testingFile.txt");
			Directory.CreateDirectory(directoryName);
			File.WriteAllText(fileName, "test contents");
			Assert.True(File.Exists(fileName) is true);
			Utils.DeleteFile(fileName);
			Assert.True(File.Exists(fileName) is false);
		}
	}

	[Fact]
	public void ShouldGetLoggingPrefix()
	{
		string jobID = "12345";
		var prefix = Utils.GetLoggingPrefix(jobID);
		Assert.True(prefix.Contains(jobID) is true);
	}
}
