using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using System.Linq;

namespace Cake.PathTildeHelper
{
    /// <summary>
    /// Path tilde (~) helpers.
    /// An alias for <see href="http://cakebuild.net">Cake Build</see> to help translate leading tilde (~) characters into paths with the current user.
    /// </summary>
    [CakeAliasCategory("Path")]
    public static class PathTildeHelper
    {
        /// <summary>
        /// Creates a path string with any leading tilde (~) character, used to denote the current user path, replaced with the absolute path to the current user's directory.
        /// </summary>
        /// <returns>If the <paramref name="filePath"/> contains a leading tilde character (~), returns an new string with the absolute path to the same location (user path replacing the tilde). If the <paramref name="filePath"/> doesn't contain a tilde, a copy of the original string is returned.</returns>
        /// <param name="context">The context.</param>
        /// <param name="filePath">Path potentially containing a leading tilde character (~) to denote a path relative to the current user's directory.</param>
        [CakeMethodAlias]
        public static FilePath PathReplaceTilde(this ICakeContext context, FilePath filePath)
        {
            var processedFilePath = new FilePath(filePath.FullPath);
            if (processedFilePath.Segments.FirstOrDefault() == "~")
            {
                var profileDirectoryPath = new DirectoryPath(System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile));
                // Get the path without tilde and next separator (assumed to be a single character).
                if (filePath.FullPath.Length > 2)
                {
                    var pathWithoutTildeSegment = new FilePath(filePath.FullPath.Substring(2));
                    processedFilePath = pathWithoutTildeSegment.MakeAbsolute(profileDirectoryPath).FullPath;
                }
                else
                {
                    // Special case when only `~` or `~/` or `~\` because the relative file path would be empty and throw an exception.
                    // This shouldn't be reached since such paths are actually directories, but we can be flexible.
                    processedFilePath = profileDirectoryPath.FullPath;
                }
            }
            return processedFilePath;
        }

        /// <summary>
        /// Creates a path string with any leading tilde (~) character, used to denote the current user path, replaced with the absolute path to the current user's directory.
        /// </summary>
        /// <returns>If the <paramref name="directoryPath"/> contains a leading tilde character (~), returns an new string with the absolute path to the same location (user path replacing the tilde). If the <paramref name="directoryPath"/> doesn't contain a tilde, a copy of the original string is returned.</returns>
        /// <param name="context">The context.</param>
        /// <param name="directoryPath">Path potentially containing a leading tilde character (~) to denote a path relative to the current user's directory.</param>
        [CakeMethodAlias]
        public static DirectoryPath PathReplaceTilde(this ICakeContext context, DirectoryPath directoryPath)
        {
            var processedFilePath = new DirectoryPath(directoryPath.FullPath);
            if (processedFilePath.Segments.FirstOrDefault() == "~")
            {
                var profileDirectoryPath = new DirectoryPath(System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile));
                // Get the path without tilde and next separator (assumed to be a single character).
                if (directoryPath.FullPath.Length > 2)
                {
                    var pathWithoutTildeSegment = new FilePath(directoryPath.FullPath.Substring(2));
                    processedFilePath = pathWithoutTildeSegment.MakeAbsolute(profileDirectoryPath).FullPath;
                }
                else
                {
                    // Special case when only `~` or `~/` or `~\` because the relative file path would be empty and throw an exception.
                    processedFilePath = profileDirectoryPath.FullPath;
                }
            }
            return processedFilePath;
        }
    }
}
