using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Tools.Util
{
    public static class DirFileHelper
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static bool IsExistDirectory(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static string[] GetFileNames(string directoryPath)
        {
            bool flag = !DirFileHelper.IsExistDirectory(directoryPath);
            if (flag)
            {
                throw new FileNotFoundException();
            }
            return Directory.GetFiles(directoryPath);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static string[] GetDirectories(string directoryPath)
        {
            string[] directories;
            try
            {
                directories = Directory.GetDirectories(directoryPath);
            }
            catch (IOException ex)
            {
                throw ex;
            }
            return directories;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="searchPattern"></param>
        /// <param name="isSearchChild"></param>
        /// <returns></returns>
        public static string[] GetFileNames(string directoryPath, string searchPattern, bool isSearchChild)
        {
            bool flag = !DirFileHelper.IsExistDirectory(directoryPath);
            if (flag)
            {
                throw new FileNotFoundException();
            }
            string[] files;
            try
            {
                if (isSearchChild)
                {
                    files = Directory.GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    files = Directory.GetFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
            return files;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static bool IsEmptyDirectory(string directoryPath)
        {
            bool result;
            try
            {
                string[] fileNames = DirFileHelper.GetFileNames(directoryPath);
                bool flag = fileNames.Length != 0;
                if (flag)
                {
                    result = false;
                }
                else
                {
                    string[] directories = DirFileHelper.GetDirectories(directoryPath);
                    bool flag2 = directories.Length != 0;
                    if (flag2)
                    {
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                }
            }
            catch
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static bool Contains(string directoryPath, string searchPattern)
        {
            bool result;
            try
            {
                string[] fileNames = DirFileHelper.GetFileNames(directoryPath, searchPattern, false);
                bool flag = fileNames.Length == 0;
                if (flag)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="searchPattern"></param>
        /// <param name="isSearchChild"></param>
        /// <returns></returns>
        public static bool Contains(string directoryPath, string searchPattern, bool isSearchChild)
        {
            bool result;
            try
            {
                string[] fileNames = DirFileHelper.GetFileNames(directoryPath, searchPattern, true);
                bool flag = fileNames.Length == 0;
                if (flag)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        public static void CreateFileContent(string path, string content)
        {
            FileInfo fileInfo = new FileInfo(path);
            DirectoryInfo directory = fileInfo.Directory;
            bool flag = !directory.Exists;
            if (flag)
            {
                directory.Create();
            }
            StreamWriter streamWriter = new StreamWriter(path, false, Encoding.GetEncoding("GB2312"));
            streamWriter.Write(content);
            streamWriter.Close();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <param name="encoding"></param>
        public static void CreateFileContent(string path, string content, string encoding)
        {
            FileInfo fileInfo = new FileInfo(path);
            DirectoryInfo directory = fileInfo.Directory;
            bool flag = !directory.Exists;
            if (flag)
            {
                directory.Create();
            }
            StreamWriter streamWriter = new StreamWriter(path, false, Encoding.GetEncoding(encoding));
            streamWriter.Write(content);
            streamWriter.Close();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static string GetDateDir()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static string GetDateFile()
        {
            return DateTime.Now.ToString("HHmmssff");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <param name="Extension"></param>
        /// <returns></returns>
        public static DataRow[] GetFilesByTime(string path, string Extension)
        {
            bool flag = Directory.Exists(path);
            DataRow[] result;
            if (flag)
            {
                string searchPattern = string.Format("*{0}", Extension);
                string[] files = Directory.GetFiles(path, searchPattern);
                bool flag2 = files.Length != 0;
                if (flag2)
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add(new DataColumn("filename", Type.GetType("System.String")));
                    dataTable.Columns.Add(new DataColumn("createtime", Type.GetType("System.DateTime")));
                    for (int i = 0; i < files.Length; i++)
                    {
                        DataRow dataRow = dataTable.NewRow();
                        DateTime creationTime = File.GetCreationTime(files[i]);
                        string fileName = Path.GetFileName(files[i]);
                        dataRow["filename"] = fileName;
                        dataRow["createtime"] = creationTime;
                        dataTable.Rows.Add(dataRow);
                    }
                    result = dataTable.Select(string.Empty, "createtime desc");
                    return result;
                }
            }
            result = new DataRow[0];
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="varFromDirectory"></param>
        /// <param name="varToDirectory"></param>
        public static void CopyFolder(string varFromDirectory, string varToDirectory)
        {
            Directory.CreateDirectory(varToDirectory);
            bool flag = !Directory.Exists(varFromDirectory);
            if (!flag)
            {
                string[] directories = Directory.GetDirectories(varFromDirectory);
                bool flag2 = directories.Length != 0;
                if (flag2)
                {
                    string[] array = directories;
                    for (int i = 0; i < array.Length; i++)
                    {
                        string text = array[i];
                        DirFileHelper.CopyFolder(text, varToDirectory + text.Substring(text.LastIndexOf("\\")));
                    }
                }
                string[] files = Directory.GetFiles(varFromDirectory);
                bool flag3 = files.Length != 0;
                if (flag3)
                {
                    string[] array2 = files;
                    for (int j = 0; j < array2.Length; j++)
                    {
                        string text2 = array2[j];
                        File.Copy(text2, varToDirectory + text2.Substring(text2.LastIndexOf("\\")), true);
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="FilePath"></param>
        public static void ExistsFile(string FilePath)
        {
            bool flag = !File.Exists(FilePath);
            if (flag)
            {
                FileStream fileStream = File.Create(FilePath);
                fileStream.Close();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="varFromDirectory"></param>
        /// <param name="varToDirectory"></param>
        public static void DeleteFolderFiles(string varFromDirectory, string varToDirectory)
        {
            Directory.CreateDirectory(varToDirectory);
            bool flag = !Directory.Exists(varFromDirectory);
            if (!flag)
            {
                string[] directories = Directory.GetDirectories(varFromDirectory);
                bool flag2 = directories.Length != 0;
                if (flag2)
                {
                    string[] array = directories;
                    for (int i = 0; i < array.Length; i++)
                    {
                        string text = array[i];
                        DirFileHelper.DeleteFolderFiles(text, varToDirectory + text.Substring(text.LastIndexOf("\\")));
                    }
                }
                string[] files = Directory.GetFiles(varFromDirectory);
                bool flag3 = files.Length != 0;
                if (flag3)
                {
                    string[] array2 = files;
                    for (int j = 0; j < array2.Length; j++)
                    {
                        string text2 = array2[j];
                        File.Delete(varToDirectory + text2.Substring(text2.LastIndexOf("\\")));
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileName(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            return fileInfo.Name;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="cDir"></param>
        /// <param name="TempId"></param>
        public static void CopyFiles(string cDir, string TempId)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="directoryPath"></param>
        public static void CreateDirectory(string directoryPath)
        {
            bool flag = !DirFileHelper.IsExistDirectory(directoryPath);
            if (flag)
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filePath"></param>
        public static void CreateFile(string filePath)
        {
            try
            {
                bool flag = !DirFileHelper.IsExistFile(filePath);
                if (flag)
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    FileStream fileStream = fileInfo.Create();
                    fileStream.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="buffer"></param>
        public static void CreateFile(string filePath, byte[] buffer)
        {
            try
            {
                bool flag = !DirFileHelper.IsExistFile(filePath);
                if (flag)
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    FileStream fileStream = fileInfo.Create();
                    fileStream.Write(buffer, 0, buffer.Length);
                    fileStream.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static int GetLineCount(string filePath)
        {
            string[] array = File.ReadAllLines(filePath);
            return array.Length;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static int GetFileSize(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            return (int)fileInfo.Length;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="searchPattern"></param>
        /// <param name="isSearchChild"></param>
        /// <returns></returns>
        public static string[] GetDirectories(string directoryPath, string searchPattern, bool isSearchChild)
        {
            string[] directories;
            try
            {
                if (isSearchChild)
                {
                    directories = Directory.GetDirectories(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    directories = Directory.GetDirectories(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
            return directories;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static string GetFileText(string Path)
        {
            bool flag = !File.Exists(Path);
            string result;
            if (flag)
            {
                result = "";
            }
            else
            {
                StreamReader streamReader = new StreamReader(Path, Encoding.GetEncoding("gb2312"));
                result = streamReader.ReadToEnd();
                streamReader.Close();
                streamReader.Dispose();
            }
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="text"></param>
        /// <param name="encoding"></param>
        public static void WriteText(string filePath, string text, Encoding encoding)
        {
            File.WriteAllText(filePath, text, encoding);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        public static void AppendText(string filePath, string content)
        {
            File.AppendAllText(filePath, content);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="destFilePath"></param>
        public static void Copy(string sourceFilePath, string destFilePath)
        {
            File.Copy(sourceFilePath, destFilePath, true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileNameNoExtension(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            return fileInfo.Name.Split(new char[]
            {
                '.'
            })[0];
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetExtension(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            return fileInfo.Extension;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filePath"></param>
        public static void ClearFile(string filePath)
        {
            File.Delete(filePath);
            DirFileHelper.CreateFile(filePath);
        }

        /// <summary>
        /// 重命名文件
        /// </summary>
        /// <param name="ReNameFilePath"></param>
        public static string FileReName(string ReNameFilePath)
        {
            string NewFilePath = ReNameFilePath;
            int i = 1;
            while (File.Exists(NewFilePath))
            {
                NewFilePath = Path.GetDirectoryName(ReNameFilePath);//文件路径
                NewFilePath += @"\" + Path.GetFileNameWithoutExtension(ReNameFilePath);//文件名（不包含扩展名）
                NewFilePath += "(" + i + ")";
                NewFilePath += Path.GetExtension(ReNameFilePath);//文件扩展名
                i++;
            }
            return NewFilePath;
        }

        /// <summary>
        /// 重命名文件
        /// </summary>
        /// <param name="ReNameFilePath"></param>
        public static string FileReNameBySize(string ReNameFilePath)
        {
            string NewFilePath = ReNameFilePath;
            int i = 1;
            while (File.Exists(NewFilePath) && FileSizeByKB(NewFilePath) > 10240)
            {
                NewFilePath = Path.GetDirectoryName(ReNameFilePath);//文件路径
                NewFilePath += @"\" + Path.GetFileNameWithoutExtension(ReNameFilePath);//文件名（不包含扩展名）
                NewFilePath += "(" + i + ")";
                NewFilePath += Path.GetExtension(ReNameFilePath);//文件扩展名
                i++;
            }
            return NewFilePath;
        }

        #region --计算文件大小--

        /// <summary>
        ///
        /// </summary>
        /// <param name="_file"></param>
        /// <returns></returns>
        public static long FileSizeByM(string _file)
        {
            return FileSizeBybytes(_file) / (1024 * 1024);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="_file"></param>
        /// <returns></returns>
        public static long FileSizeByKB(string _file)
        {
            return FileSizeBybytes(_file) / 1024;
        }

        /// <summary>
        /// 计算文本大小(字节)
        /// </summary>
        /// <param name="_file">文本路径</param>
        /// <returns></returns>
        public static long FileSizeBybytes(string _file)
        {
            long _r = 0;
            if (File.Exists(_file))
            {
                FileInfo fileInfo = new FileInfo(_file);
                _r = fileInfo.Length;
            }
            return _r;
        }

        #endregion

        /// <summary>
        /// 把一个文件夹下所有文件复制到另一个文件夹下
        /// </summary>
        /// <param name="srcPath">被复制文件夹路径</param>
        /// <param name="destPath">复制到文件夹路径</param>
        /// <param name="fileName">文件名集合</param>
        public static void CopyDirectory(string srcPath, string destPath, string[] fileName)
        {
            try
            {
                if (!Directory.Exists(destPath))
                {
                    //目标目录下不存在此文件夹即创建子文件夹
                    Directory.CreateDirectory(destPath);
                }
                DirectoryInfo dir = new DirectoryInfo(srcPath);
                //获取目录下（不包含子目录）的文件和子目录
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();
                foreach (FileSystemInfo i in fileinfo)
                {
                    //判断是否文件夹
                    if (i is DirectoryInfo)
                    {
                        if (!Directory.Exists(destPath + "\\" + i.Name))
                        {
                            //目标目录下不存在此文件夹即创建子文件夹
                            Directory.CreateDirectory(destPath + "\\" + i.Name);
                        }
                        //递归调用复制子文件夹
                        CopyDirectory(i.FullName, destPath + "\\" + i.Name, fileName);
                    }
                    else
                    {
                        if (fileName != null && fileName.Contains(i.Name) && File.Exists(i.FullName))
                        {
                            //不是文件夹即复制文件，true表示可以覆盖同名文件
                            File.Copy(i.FullName, destPath + "\\" + i.Name, true);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //TODO
                //MessageBox.Show(e.ToString());
                throw;
            }
        }

        /// <summary>
        /// 把一个文件夹下所有文件复制到另一个文件夹下
        /// </summary>
        /// <param name="srcPath">被复制文件夹路径</param>
        /// <param name="destPath">复制到文件夹路径</param>
        /// <param name="dirChildrens">子级文件夹</param>
        /// <param name="fileNames">文件名集合</param>
        public static void CopyDirectory(string srcPath, string destPath, List<string> dirChildrens, List<string> fileNames)
        {
            try
            {
                if (!Directory.Exists(destPath))
                {
                    //目标目录下不存在此文件夹即创建子文件夹
                    Directory.CreateDirectory(destPath);
                }
                DirectoryInfo dir = new DirectoryInfo(srcPath);
                //获取目录下（不包含子目录）的文件和子目录
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();
                foreach (FileSystemInfo i in fileinfo)
                {
                    //判断是否文件夹
                    if (i is DirectoryInfo)
                    {
                        if (dirChildrens.Contains(i.Name))
                        {
                            if (!Directory.Exists(destPath + "\\" + i.Name))
                            {
                                //目标目录下不存在此文件夹即创建子文件夹
                                Directory.CreateDirectory(destPath + "\\" + i.Name);
                            }
                            //递归调用复制子文件夹
                            CopyDirectory(i.FullName, destPath + "\\" + i.Name, dirChildrens, fileNames);
                        }
                    }
                    else
                    {
                        if (fileNames != null && fileNames.Contains(i.Name) && File.Exists(i.FullName))
                        {
                            //不是文件夹即复制文件，true表示可以覆盖同名文件
                            File.Copy(i.FullName, destPath + "\\" + i.Name, true);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //TODO
                //MessageBox.Show(e.ToString());
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="searchPatterns"></param>
        /// <returns></returns>
        public static FileInfo[] GetFiles(string dirPath, params string[] searchPatterns)
        {
            return GetFilesChild(dirPath, false, searchPatterns);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="isSearchChild"></param>
        /// <param name="searchPatterns"></param>
        /// <returns></returns>
        public static FileInfo[] GetFilesChild(string dirPath, bool isSearchChild, params string[] searchPatterns)
        {
            if (searchPatterns.Length <= 0)
            {
                return null;
            }
            else
            {
                DirectoryInfo di = new DirectoryInfo(dirPath);
                List<FileInfo[]> list = new List<FileInfo[]>();
                int fLength = 0;
                foreach (string sPatterns in searchPatterns)
                {
                    FileInfo[] fileInfos = di.GetFiles(sPatterns, isSearchChild ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                    if (fileInfos.Length > 0)
                    {
                        list.Add(fileInfos);
                        fLength += fileInfos.Length;
                    }
                }
                FileInfo[] files = new FileInfo[fLength];
                int i = 0;
                foreach (FileInfo[] file in list)
                {
                    file.CopyTo(files, i);
                    i += file.Length;
                }
                return files;
            }
        }

        /// <summary>
        /// 删除n天内文件
        /// </summary>
        /// <param name="number"></param>
        public static void DeleteFileByDay(int number)
        {
            if (DateTime.Now.Day == 1 && Directory.Exists(BaseUtil.baseDirectory + "\\log\\"))
            {
                //删除30天之前的文件
                var list = from c in Directory.GetFiles(BaseUtil.baseDirectory + "\\log\\", "*.log", SearchOption.AllDirectories)
                           where (new DirectoryInfo(c).CreationTime) <= DateTime.Now.AddDays(-number)
                           select c;
                foreach (var item in list)
                {
                    File.Delete(item);
                }
                //删除空文件
                var subdirs = new DirectoryInfo(BaseUtil.baseDirectory + "\\log\\").GetDirectories("*.*", SearchOption.AllDirectories);
                foreach (var subdir in subdirs)
                {
                    FileSystemInfo[] subFiles = subdir.GetFileSystemInfos();
                    if (subFiles.Count() == 0)
                    {
                        try
                        {
                            subdir.Delete();
                        }
                        catch { }
                    }
                }
            }
        }
    }
}
