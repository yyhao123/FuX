using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Unility
{
    public class FileHandler
    {
        public static string GetFolderPath(string Path, bool Create = true)
        {
            string directoryName = System.IO.Path.GetDirectoryName(Path);
            if (!string.IsNullOrWhiteSpace(directoryName) && Create && !Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            return directoryName;
        }

        public static string GetFileName(string Path)
        {
            return System.IO.Path.GetFileName(Path);
        }

        public static string FileToString(string filePath)
        {
            string result = "";
            byte[] array = null;
            byte[] array2 = new byte[1024];
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    int num = 0;
                    while ((num = fileStream.Read(array2, 0, array2.Length)) > 0)
                    {
                        memoryStream.Write(array2, 0, num);
                    }

                    array = memoryStream.ToArray();
                    fileStream.Close();
                    fileStream.Dispose();
                }

                memoryStream.Close();
                memoryStream.Dispose();
            }

            stopwatch.Stop();
            if (array != null)
            {
                result = Encoding.UTF8.GetString(array);
            }

            return result;
        }

        public static void StringToFile(string filePath, string Data)
        {
            string folderPath = GetFolderPath(filePath);
            if (!string.IsNullOrEmpty(folderPath) && !Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            File.WriteAllText(filePath, Data);
        }

        public static byte[] ConvertToBinary(string Path)
        {
            FileStream fileStream = new FileInfo(Path).OpenRead();
            byte[] array = new byte[fileStream.Length];
            fileStream.Read(array, 0, Convert.ToInt32(fileStream.Length));
            return array;
        }

        public static bool FileDelete(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            FileAttributes attributes = FileAttributes.None;
            bool flag = false;
            try
            {
                attributes = fileInfo.Attributes;
                fileInfo.Attributes &= ~FileAttributes.ReadOnly;
                flag = true;
                fileInfo.Delete();
            }
            catch (Exception)
            {
                if (flag)
                {
                    fileInfo.Attributes = attributes;
                }

                return false;
            }

            return true;
        }

        public static void FolderClear(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] files = directoryInfo.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                FileDelete(files[i].FullName);
            }

            DirectoryInfo[] directories = directoryInfo.GetDirectories();
            for (int i = 0; i < directories.Length; i++)
            {
                FolderClear(directories[i].FullName);
            }
        }

        public static void RunCmd(string cmd)
        {
            Process.Start("cmd.exe", "/c " + cmd);
        }

        public static void CleanHistory()
        {
            string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.History), "*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                FileDelete(files[i]);
            }

            RunCmd("RunDll32.exe InetCpl.cpl,ClearMyTracksByProcess 1");
        }

        public static void CleanTempFiles()
        {
            FolderClear(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache));
            RunCmd("RunDll32.exe InetCpl.cpl,ClearMyTracksByProcess 8");
        }

        public static void CleanCookie()
        {
            string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Cookies), "*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                FileDelete(files[i]);
            }

            RunCmd("RunDll32.exe InetCpl.cpl,ClearMyTracksByProcess 2");
        }

        public static void CleanAll()
        {
            CleanHistory();
            CleanCookie();
            CleanTempFiles();
        }
    }
}
