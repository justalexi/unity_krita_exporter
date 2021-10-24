using System;
using System.Diagnostics;
using System.IO;
using Debug = UnityEngine.Debug;


namespace Justalexi.KritaExporter.Editor
{
    /// <license>
    /// MIT License

    /// Copyright (c) 2021 justalexi

    /// Permission is hereby granted, free of charge, to any person obtaining a copy
    /// of this software and associated documentation files (the "Software"), to deal
    /// in the Software without restriction, including without limitation the rights
    /// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    /// copies of the Software, and to permit persons to whom the Software is
    /// furnished to do so, subject to the following conditions:

    /// The above copyright notice and this permission notice shall be included in all
    /// copies or substantial portions of the Software.
    /// 
    /// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    /// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    /// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    /// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    /// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    /// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    /// SOFTWARE.
    /// </license>
    public static class KritaExporterCommon
    {
        // A .kra file contains a file named "mergedimage.png" which contains the rendered image as you see it on your canvas (see https://docs.krita.org/en/general_concepts/file_formats/file_kra.html). 
        private const string PngFileNameInsideKraArchive = "mergedimage.png";

        // In Windows and MacOS this can be a different program
        private const string ProcessName = "bash";

        // x Extract with full paths
        // -aoa Overwrite All existing files without prompt
        // -o -o{Directory} Set Output directory // NO SPACE AFTER -o
        private const string ExtractCommand = "7z x -aoa \"[FullName]\" -o\"[Directory]\" [PngFileNameInsideKraArchive]";


        public static bool ExportKritaFile(FileInfo kraFile)
        {
            string command = ExtractCommand
                .Replace("[FullName]", kraFile.FullName)
                .Replace("[Directory]", kraFile.Directory?.ToString())
                .Replace("[PngFileNameInsideKraArchive]", PngFileNameInsideKraArchive);

            Process process = Process.Start(ProcessName, command);
            if (process != null)
            {
                process.WaitForExit();
                process.Close();
            }
            else
            {
                Debug.LogError("KritaExporterCommon.ExportKritaFile: ERROR! 'process' is null");
                return false;
            }

            var pngFileFromKra = new FileInfo(kraFile.Directory + "/" + PngFileNameInsideKraArchive);

            var renamedPngFileName = kraFile.Name.Replace(".kra", ".png");
            if (pngFileFromKra.Directory != null)
            {
                var finalPngFullPath = Path.Combine(pngFileFromKra.Directory.FullName, renamedPngFileName);
                if (File.Exists(finalPngFullPath))
                {
                    File.Delete(finalPngFullPath);
                }

                try
                {
                    pngFileFromKra.MoveTo(finalPngFullPath);
                }
                catch (Exception)
                {
                    Debug.LogError("KritaExporterCommon.ExportKritaFile: ERROR! Most likely there is a problem with getting 'mergedimage.png' file out of .kra archive\nFile at: " + finalPngFullPath);
                    return false;
                }
            }

            return true;
        }
    }
}