using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;


namespace Snake.Models
{
    public static class AnyFuncs
    {
        /// <summary>
        /// Deep, ooh, very deeeep....
        /// </summary>
        /// <param name="o"></param>
        public static T DeepCopy<T>(this T obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// Парсит путь к файлу и возвращает директорию
        /// </summary>
        /// <param name="fileWay"></param>
        /// <returns></returns>
        public static string ParseFileWayToDirectory(this string fileWay)
        {
            string[] ss = fileWay.Split('\\');
            ss[ss.Length - 1] = null;
            return string.Join("\\", ss);
        }


        /// <summary>
        /// Возвращает короткое имя без расширения, получая путь.
        /// </summary>
        /// <param name="way"></param>
        /// <returns></returns>
        public static string ParseWay(string way)
        {
            int end = way.LastIndexOf(".");
            if (end == -1)
                end = way.Length;
            int beg = way.LastIndexOf("\\");

            way = way.Remove(end, way.Length - end);
            way = way.Remove(0, beg + 1);

            return way;
        }


        /// <summary>
        /// Находит и убивает процесс по имени(title) и типу процесса(например "Notepad").
        /// Тип процесса можно не указывать, тогда поиск будет только по имени.
        /// </summary>
        /// <param name="sName"></param>
        public static bool KillProcessByName(string sName)
        {
            Process[] proc = Process.GetProcesses();

            foreach (Process process in proc)
            {
                string name = ParseWay(process.MainWindowTitle);
                if (name == sName)
                {
                    process.Kill();
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Находит и убивает процесс по имени(title) и типу процесса(например "Notepad").
        /// Тип процесса можно не указывать, тогда поиск будет только по имени.
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="procType"></param>
        public static bool KillProcessByName(string sName, string procType)
        {
            Process[] proc = Process.GetProcessesByName(procType);

            foreach (Process process in proc)
            {
                string name = ParseWay(process.MainWindowTitle);
                if (name == sName)
                {
                    process.Kill();
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Удаление файла по полному имени. Можно задать имя файла для возвратной строки описания.
        /// </summary>
        /// <param name="fileway"></param>
        public static bool DeleteFile(string fileway)
        {
            try
            {
                new FileInfo(fileway).Delete();
                return true;
            }
            catch { return false; }
        }
        /// <summary>
        /// Удаление файла по полному имени. Можно задать имя файла для возвратной строки описания.
        /// </summary>
        /// <param name="fileway"></param>
        /// <param name="nameForDescription"></param>
        public static string DeleteFile(string fileway, string nameForDescription)
        {
            string messsage = null;
            try
            {
                new FileInfo(fileway).Delete();
                messsage = "Файл " + nameForDescription + "\r <" + fileway + ">\r удален";
            }
            catch (Exception ex)
            {
                messsage = ("Отказано в удалении файла " + nameForDescription +
                      "\r<" + fileway + ">\r" + ex.Message);
            }
            return messsage;
        }

    }
}
