using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using Moq;
using System;
using Xunit;

namespace Cake.PathTildeHelper.Tests
{
    public class PathReplaceTilde_FilePathTests
    {
        Mock<ICakeContext> cakeContextMock;
        Mock<ICakeArguments> cakeArgumentsMock;
        Mock<ICakeEnvironment> cakeEnvironmentMock;

        DirectoryPath localProfileDirectoryPath;

        public PathReplaceTilde_FilePathTests()
        {
            cakeContextMock = new Mock<ICakeContext>();
            cakeArgumentsMock = new Mock<ICakeArguments>();
            cakeEnvironmentMock = new Mock<ICakeEnvironment>();
            cakeContextMock.Setup(cakeContext => cakeContext.Arguments).Returns(cakeArgumentsMock.Object);
            cakeContextMock.Setup(cakeContext => cakeContext.Environment).Returns(cakeEnvironmentMock.Object);

            localProfileDirectoryPath = new DirectoryPath(System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile));
        }

        [Fact]
        public void TildeRelativeFileWithForwardSlashSeparator_ReturnsProfileAbsoluteVersion()
        {
            var testPath = "~/a/path/to/somefile.txt";

            var expected = localProfileDirectoryPath.FullPath + "/a/path/to/somefile.txt";
            var actual = cakeContextMock.Object.PathReplaceTilde(new FilePath(testPath)).FullPath;

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void TildeRelativeFileWithBackwardSlashSeparator_ReturnsProfileAbsoluteVersion()
        {
            var testPath = @"~\a\path\to\somefile.txt";

            var expected = localProfileDirectoryPath.FullPath + "/a/path/to/somefile.txt";
            var actual = cakeContextMock.Object.PathReplaceTilde(new FilePath(testPath)).FullPath;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NonTildeRootRelativeFileWithForwardSlashSeparator_ReturnsRelativeCopyOfOriginal()
        {
            var testPath = "/a/path/to/somefile.txt";

            var expected = "/a/path/to/somefile.txt";
            var actual = cakeContextMock.Object.PathReplaceTilde(new FilePath(testPath)).FullPath;

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void NonTildeCurrentPathRelativeFileWithForwardSlashSeparator_ReturnsRelativeCopyOfOriginal()
        {
            var testPath = "a/path/to/somefile.txt";

            var expected = "a/path/to/somefile.txt";
            var processedFilePath = cakeContextMock.Object.PathReplaceTilde(new FilePath(testPath));

            Assert.True(processedFilePath.IsRelative);

            var actual = processedFilePath.FullPath;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TildeOnly_ReturnsProfileAbsolute()
        {
            // Technically not a "FilePath", but see if we handle it anyway.
            var testPath = "~";

            var expected = localProfileDirectoryPath.FullPath;
            var actual = cakeContextMock.Object.PathReplaceTilde(new FilePath(testPath)).FullPath;

            Assert.Equal(expected, actual);
        }
    }
}
