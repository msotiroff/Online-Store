using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MSToolKit.Core.IO.Abstraction;

namespace MSToolKit.Core.IO
{
    internal class FileProcessor : IFileProcessor
    {
        private const string pattern = @"\((?<Index>\d+)\)\.[A-Za-z]+$";

        /// <summary>
        /// Generate an unique file name.
        /// </summary>
        /// <param name="fileName">
        /// Current file name.
        /// </param>
        /// <returns>
        /// The unique file name.
        /// </returns>
        public string GetUniqueFileName(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            var guid = Guid.NewGuid().ToString();
            var fileNameSuffix = guid.Substring(guid.Length - 10);
            var newFileNameWithoutExtension = $"{fileNameWithoutExtension}-{fileNameSuffix}";
            var newFileName = $"{newFileNameWithoutExtension}{extension}";

            return newFileName;
        }

        /// <summary>
        /// Finds and opens a file in a specified directory.
        /// </summary>
        /// <param name="fileFolder">The directory, that contains the given file.</param>
        /// <param name="fileName">The file name (incl. extension), that should be opened.</param>
        /// <exception cref="System.ComponentModel.Win32Exception">
        /// A System.ComponentModel.Win32Exception will be thrown if the file does not exists.
        /// </exception>
        public void Open(string fileFolder, string fileName)
        {
            var fileFullPath = Path.Combine(fileFolder, fileName);

            this.Open(fileFullPath);
        }

        /// <summary>
        /// Finds and opens a file with a specified path.
        /// </summary>
        /// <param name="fileFullPath">The full path of the file, that should be opened. For example: C:\examples\myFile.txt</param>
        /// <exception cref="System.ComponentModel.Win32Exception">
        /// A System.ComponentModel.Win32Exception will be thrown if the file does not exists.
        /// </exception>
        public void Open(string fileFullPath)
        {
            using (var process = new Process())
            {
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.FileName = fileFullPath;
                process.Start();
            }            
        }

        /// <summary>
        /// Creates a file with specified name and extension, and writes it to a specified folder in the file system. 
        /// If a file with the same name already exists in this directory 
        /// appends an index to the end of file name. For example C:\examples\myFile(1).txt, C:\examples\myFile(2).txt, ect.
        /// </summary>
        /// <param name="fileBytes">The content (in bytes) of the file, that should be created.</param>
        /// <param name="fileFolder">The folder where the created file should be stored.</param>
        /// <param name="fileName">The name for the file, that should be creted.</param>
        /// <param name="fileExtension">The extension for the file, that should be creted.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation,
        /// containing the full path of the created file as string.
        /// </returns>
        public async Task<string> WriteToFileAsync(
            IEnumerable<byte> fileBytes, string fileFolder, string fileName, string fileExtension)
        {
            var fileFullPath = Path.Combine(fileFolder, $"{fileName}.{fileExtension}");
            return await this.WriteAllBytesAsync(fileBytes, fileFullPath);
        }

        /// <summary>
        /// Creates a file and writes it to the file system. 
        /// If a file with the same full name already exists
        /// appends an index to the end of file name. For example C:\examples\myFile(1).txt, C:\examples\myFile(2).txt, ect.
        /// </summary>
        /// <param name="fileBytes">The content (in bytes) of the file, that should be created.</param>
        /// <param name="fileFullPath">The full path (directory, file name, extension) of the file, that should be stored.</param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous operation,
        /// containing the full path of the created file as string.
        /// </returns>
        public async Task<string> WriteToFileAsync(IEnumerable<byte> fileBytes, string fileFullPath)
        {
            return await this.WriteAllBytesAsync(fileBytes, fileFullPath);
        }

        private async Task<string> WriteAllBytesAsync(IEnumerable<byte> fileBytes, string fileFullPath)
        {
            await Task.Run(() =>
            {
                var regex = new Regex(pattern);
                while (File.Exists(fileFullPath))
                {
                    var extension = Path.GetExtension(fileFullPath);
                    var match = regex.Match(fileFullPath);
                    if (match.Success)
                    {
                        var index = int.Parse(match.Groups["Index"].Value);
                        fileFullPath = $"{fileFullPath.Replace(match.Value, $"({++index})")}{extension}";
                    }
                    else
                    {
                        var filePathWithoutExtension = fileFullPath.Replace(extension, string.Empty);
                        fileFullPath = $"{filePathWithoutExtension}(1){extension}";
                    }
                }

                File.WriteAllBytes(fileFullPath, fileBytes.ToArray());
            });

            return fileFullPath;
        }
    }
}
