# Unity Krita Exporter
Unity plugin to convert krita files to png.


A .kra file contains a file named "mergedimage.png" which contains the rendered image as you see it on your canvas (see https://docs.krita.org/en/general_concepts/file_formats/file_kra.html). Thanks to that, we can automatically take png file out and use it in Unity Editor, while having the original file close at hand for further modifications. Also many file managers can show png files, but not krita files. And many VCS can show changes for png files, which is very convenient.
This editor plugin listens for changes in krita files, gets "mergedimage.png" out of them and renames to the original name (changing the type to png).


Usage:
1. Copy the repository to your Unity project.
2. Add path to 7z to path variables, or update 'ExtractCommand' variable in 'KritaExporterCommon.cs' to reflect location of your file archiver.

If all goes well, then every time you change kra files, Unity will create png files next to them. 

!WARNING! This was tested in Linux Mint, so be ready to dive into code a bit during development on Windows or MacOS.
