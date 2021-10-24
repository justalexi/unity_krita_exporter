using System.IO;
using UnityEngine;
using UnityEditor;

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
    public class KritaExporterAutomatic : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string str in importedAssets)
            {
                if (str.EndsWith(".kra"))
                {
                    string kraFilePath = (str.StartsWith("Assets/")) ? str.Substring(7) : str;

                    string kraFileFullPath = Path.Combine(Application.dataPath, kraFilePath);

                    FileInfo kraFile = new FileInfo(kraFileFullPath);

                    if (!kraFile.Exists)
                    {
                        Debug.LogError("KritaExporterAutomatic.OnPostprocessAllAssets: ERROR! Non-existent file: " + kraFileFullPath);
                        return;
                    }

                    var success = KritaExporterCommon.ExportKritaFile(kraFile);
                    if (success)
                    {
                        // 'ImportAsset' expects path starting from "Assets/"
                        var relativeKraFilePath = str.Substring(0, str.Length - 4) + ".png";
                        AssetDatabase.ImportAsset(relativeKraFilePath);
                    }
                }
            }
        }
    }
}