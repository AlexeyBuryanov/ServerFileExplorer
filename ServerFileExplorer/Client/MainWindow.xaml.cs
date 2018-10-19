using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Client.localhost;
using Client.Windows;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WebService1 _webservice;
        private DispatcherTimer _timer;

        private List<FileProps> _serverFS;
        private List<FileProps> _files;
        private List<FileProps> _dirs;

        private string _path;
        private string _pathBack;

        private string _data;

        public MainWindow()
        {
            InitializeComponent();

            _path = null;
            _pathBack = null;

            _serverFS = new List<FileProps>();
            _files = new List<FileProps>();
            _dirs = new List<FileProps>();

            _webservice = new WebService1();

            // Установка таймера
            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(Timer_Tick);
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();
        } // MainWindow


        /// <summary>По тику таймера</summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            textBlockTime.Text = _webservice.GetServerTime().ToLongTimeString();
        } // Timer_Tick


        /// <summary>По кнопке "Показать файлы"</summary>
        private void ButtonGetFileList_Click(object sender, RoutedEventArgs e)
        {
            try {
                if (!string.IsNullOrWhiteSpace(_pathBack)) {
                    ButtonBackPath.IsEnabled = true;
                } else {
                    _pathBack = TextBoxPath.Text;
                } // if-else

                _path = TextBoxPath.Text;
                
                ShowFilesAndDirs(_path);
            } catch (Exception ex) {
                MessageBox.Show($"Невозможно получить список файлов на сервере.\n{ex.Message}", 
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            } // try-catch
        } // ButtonGetFileList_Click


        /// <summary>Показать файлы и папки</summary>
        private void ShowFilesAndDirs(string path)
        {
            _serverFS.Clear();
            _files.Clear();
            _dirs.Clear();
            DataGridMain.ItemsSource = null;

            GetFileList(path);
            GetDirList(path);

            DataGridMain.ItemsSource = _serverFS;
        } // ShowFilesAndDirs


        /// <summary>Получить список папок. 
        /// Для взаимодействия с методом используется поле в главном окне.</summary>
        private void GetDirList(string path)
        {
            FileProps[] temp = _webservice.GetDirectoryList(path);

            foreach (FileProps item in temp) {
                _dirs.Add(item);
            } // foreach

            foreach (FileProps item in _dirs) {
                FileProps prop = new FileProps {
                    Name = item.Name,
                    FullName = item.FullName,
                    CreationTime = item.CreationTime,
                    Extension = item.Extension,
                    Length = item.Length
                };

                _serverFS.Add(prop);
            } // foreach
        } // GetDirList


        /// <summary>Получить список папок. 
        /// Для взаимодействия с методом используется поле в главном окне.</summary>
        private void GetFileList(string path)
        {
            FileProps[] temp = _webservice.GetFileList(path);

            foreach (var item in temp) {
                _files.Add(item);
            } // foreach

            foreach (FileProps item in _files) {
                FileProps prop = new FileProps {
                    Name = item.Name,
                    FullName = item.FullName,
                    CreationTime = item.CreationTime,
                    Extension = item.Extension,
                    Length = ((item.Length/1024)/1024)
                };

                _serverFS.Add(prop);
            } // foreach
        } // GetFileList


        /// <summary>По кнопке "Назад"</summary>
        private void ButtonBackPath_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_pathBack)) {
                return;
            } else {
                TextBoxPath.Text = _pathBack;
                ShowFilesAndDirs(_pathBack);
            } // if-else
        } // ButtonBackPath_Click


        /// <summary>Дабл-клик по ДатаГриду</summary>
        private void DataGridMain_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGridMain.SelectedItem == null) {
                return;
            } else if ((DataGridMain.SelectedItem as FileProps).Extension == "Папка с файлами") {
                _pathBack = TextBoxPath.Text;
                ButtonBackPath.IsEnabled = true;
                _path = TextBoxPath.Text = (DataGridMain.SelectedItem as FileProps).FullName;
                ShowFilesAndDirs(TextBoxPath.Text);
            } // if-else
        } // DataGridMain_MouseDoubleClick


        /// <summary>Когда отпускаешь левую кнопку мыши на ДатаГриде</summary>
        private void DataGridMain_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try {
                if (DataGridMain.SelectedItem == null) {
                    return;
                } else if ((DataGridMain.SelectedItem as FileProps).Extension != "Папка с файлами") {
                    FileProps temp = (DataGridMain.SelectedItem as FileProps);
                    if (temp.Length < 20) {
                        _data = File.ReadAllText(temp.FullName);
                        TextBoxMain.Text = _data;
                    } else {
                        TextBoxMain.Text = "Похоже, что файл слишком велик. \nБыстрый просмотр возможен только текстовых файлов.";
                    } // if-else
                } // if-else
            } catch (Exception) {
                TextBoxMain.Text = "Невозможно прочесть данный файл...";
                return;
            } // try-catch
        } // DataGridMain_MouseLeftButtonUp


        /// <summary>Выход</summary>
        private void MenuMainExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        } // MenuMainExit_Click


        /// <summary>О программе</summary>
        private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow win = new AboutWindow(this);
            win.ShowDialog();
        } // MenuItemAbout_Click
    } // MainWindow
} // Client