using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web.Services;
using ClassLib;

namespace WebSite
{
    /// <summary>
    ///     Веб-служба WebService1 by Alexey Bur'yanov
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class WebService1 : WebService
    {
        /// <summary>Текущее серверное время</summary>
        /// <returns>Объект типа DateTime</returns>
        [WebMethod]
        public DateTime GetServerTime()
        {
            return DateTime.Now;
        } // GetServerTime


        /// <summary>Возвращает список файлов на сервере</summary>
        /// <returns>
        ///     Список объектов типа FileProps со всеми свойствами файла.
        ///     Но на самом деле массив, а не список.
        /// </returns>
        [WebMethod]
        public List<FileProps> GetFileList(string path)
        {
            var dir = new DirectoryInfo(path);
            var fi = dir.GetFiles();

            return fi.Select(f => new FileProps(f.Name, f.FullName, f.CreationTime, f.Extension, f.Length)).ToList();
        } // GetFileList


        /// <summary>Возвращает список подкаталогов текущего каталога</summary>
        /// <returns>
        ///     Список объектов типа FileProps со всеми свойствами файла.
        ///     Но на самом деле массив, а не список.
        /// </returns>
        [WebMethod]
        public List<FileProps> GetDirectoryList(string path)
        {
            var dir = new DirectoryInfo(path);
            var di = dir.GetDirectories();

            return di.Select(d => new FileProps(d.Name, d.FullName, d.CreationTime, "Папка с файлами", 0)).ToList();
        } // GetFileList
    } // WebService1
} // WebSite