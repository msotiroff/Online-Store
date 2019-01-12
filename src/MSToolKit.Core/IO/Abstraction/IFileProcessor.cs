using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSToolKit.Core.IO.Abstraction
{
    /// <summary>
    /// Provides an abstraction for processing files.
    /// </summary>
    public interface IFileProcessor
    {
        /// <summary>
        /// Generate an unique file name.
        /// </summary>
        /// <param name="fileName">
        /// Current file name.
        /// </param>
        /// <returns>
        /// The unique file name.
        /// </returns>
        string GetUniqueFileName(string fileName);

        /// <summary>
        /// Creates a file with specified name and extension, and writes it to a specified folder in the file system.
        /// </summary>
        /// <param name="fileBytes">The content (in bytes) of the file, that should be created.</param>
        /// <param name="fileFolder">The folder where the created file should be stored.</param>
        /// <param name="fileName">The name for the file, that should be creted.</param>
        /// <param name="fileExtension">The extension for the file, that should be creted.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation,
        /// containing the full path of the created file as string.
        /// </returns>
        Task<string> WriteToFileAsync(
            IEnumerable<byte> fileBytes, string fileFolder, string fileName, string fileExtension);

        /// <summary>
        /// Creates a file and writes it to the file system.
        /// </summary>
        /// <param name="fileBytes">The content (in bytes) of the file, that should be created.</param>
        /// <param name="fileFullPath">The full path (directory, file name, extension) of the file, that should be stored.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation,
        /// containing the full path of the created file as string.
        /// </returns>
        Task<string> WriteToFileAsync(IEnumerable<byte> fileBytes, string fileFullPath);

        /// <summary>
        /// Finds and opens a file in a specified directory.
        /// </summary>
        /// <param name="fileFolder">The directory, that contains the given file.</param>
        /// <param name="fileName">The file name (incl. extension), that should be opened.</param>
        /// <exception cref="System.ComponentModel.Win32Exception">
        /// A System.ComponentModel.Win32Exception will be thrown if the file does not exists.
        /// </exception>
        void Open(string fileFolder, string fileName);

        /// <summary>
        /// Finds and opens a file with a specified path.
        /// </summary>
        /// <param name="fileFullPath">The full path of the file, that should be opened. For example: C:\examples\myFile.txt</param>
        /// <exception cref="System.ComponentModel.Win32Exception">
        /// A System.ComponentModel.Win32Exception will be thrown if the file does not exists.
        /// </exception>
        void Open(string fileFullPath);
    }
}
