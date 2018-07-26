using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using Moq;
using System;
using Xunit;

namespace Cake.PathTildeHelper.Tests
{
    public class PathReplaceTilde_DirectoryPathTests
    {
        Mock<ICakeContext> cakeContextMock;
        Mock<ICakeArguments> cakeArgumentsMock;
        Mock<ICakeEnvironment> cakeEnvironmentMock;

        DirectoryPath localProfileDirectoryPath;

        public PathReplaceTilde_DirectoryPathTests()
        {
            cakeContextMock = new Mock<ICakeContext>();
            cakeArgumentsMock = new Mock<ICakeArguments>();
            cakeEnvironmentMock = new Mock<ICakeEnvironment>();
            cakeContextMock.Setup(cakeContext => cakeContext.Arguments).Returns(cakeArgumentsMock.Object);
            cakeContextMock.Setup(cakeContext => cakeContext.Environment).Returns(cakeEnvironmentMock.Object);

            localProfileDirectoryPath = new DirectoryPath(System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile));
        }

        [Fact]
        public void TildeOnly_ReturnsProfileAbsolute()
        {
            var testPath = "~";

            var expected = localProfileDirectoryPath.FullPath;
            var actual = cakeContextMock.Object.PathReplaceTilde(new DirectoryPath(testPath)).FullPath;

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void TildeWithForwardSlashOnly_ReturnsProfileAbsolute()
        {
            var testPath = "~/";

            var expected = localProfileDirectoryPath.FullPath;
            var actual = cakeContextMock.Object.PathReplaceTilde(new DirectoryPath(testPath)).FullPath;

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void TildeWithBackwardSlashOnly_ReturnsProfileAbsolute()
        {
            var testPath = @"~\";

            var expected = localProfileDirectoryPath.FullPath;
            var actual = cakeContextMock.Object.PathReplaceTilde(new DirectoryPath(testPath)).FullPath;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TildeRelativeFileWithForwardSlashSeparatorAndNoTerminalSeparator_ReturnsProfileAbsoluteVersion()
        {
            var testPath = "~/some/path";

            var expected = localProfileDirectoryPath.FullPath + "/some/path";
            var actual = cakeContextMock.Object.PathReplaceTilde(new DirectoryPath(testPath)).FullPath;

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void TildeRelativeFileWithForwardSlashSeparatorAndTerminalSeparator_ReturnsProfileAbsoluteVersion()
        {
            var testPath = "~/some/path/";

            var expected = localProfileDirectoryPath.FullPath + "/some/path";
            var actual = cakeContextMock.Object.PathReplaceTilde(new DirectoryPath(testPath)).FullPath;

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void TildeRelativeFileWithBackwardSlashSeparatorAndNoTerminalSeparator_ReturnsProfileAbsoluteVersion()
        {
            var testPath = @"~\some\path";

            var expected = localProfileDirectoryPath.FullPath + "/some/path";
            var actual = cakeContextMock.Object.PathReplaceTilde(new DirectoryPath(testPath)).FullPath;

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void TildeRelativeFileWithBackwardSlashSeparatorAndTerminalSeparator_ReturnsProfileAbsoluteVersion()
        {
            var testPath = @"~\some\path\";

            var expected = localProfileDirectoryPath.FullPath + "/some/path";
            var actual = cakeContextMock.Object.PathReplaceTilde(new DirectoryPath(testPath)).FullPath;

            Assert.Equal(expected, actual);
        }
    }
}
