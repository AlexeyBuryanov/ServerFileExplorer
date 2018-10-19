using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Reflection;

namespace Client.Windows
{
    /// <summary>
    /// Логика взаимодействия для AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();

            productName.Text = AssemblyProduct;
            version.Text     = $"Версия {AssemblyVersion}";
            copyright.Text   = AssemblyCopyright;
            companyName.Text = AssemblyCompany;
            description.Text = AssemblyDescription;
        } // AboutWindow

        public AboutWindow(Window owner) : this()
        {
            InitializeComponent();

            productName.Text = AssemblyProduct;
            version.Text     = $"Версия {AssemblyVersion}";
            copyright.Text   = AssemblyCopyright;
            companyName.Text = AssemblyCompany;
            description.Text = AssemblyDescription;

            Owner = owner;
        } // AboutWindow


        #region Методы доступа к атрибутам сборки
        // Заголовок программы
        public string AssemblyTitle {
            get {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0) {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "") {
                        return titleAttribute.Title;
                    } // if
                } // if
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            } // get
        } // AssemblyTitle

        // Версия
        public string AssemblyVersion {
            get {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            } // get
        } // AssemblyVersion

        // Описание
        public string AssemblyDescription {
            get {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0) {
                    return "";
                } // if
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            } // get
        } // AssemblyDescription

        // Имя продукта (приложения)
        public string AssemblyProduct {
            get {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0) {
                    return "";
                } // if
                return ((AssemblyProductAttribute)attributes[0]).Product;
            } // get
        } // AssemblyProduct

        // Авторские права
        public string AssemblyCopyright {
            get {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0) {
                    return "";
                } // if
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            } // get
        } // AssemblyCopyright

        // Организация
        public string AssemblyCompany {
            get {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0) {
                    return "";
                } // if
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            } // get
        } // AssemblyCompany
        #endregion


        private void AboutWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        } // AboutWindow_MouseLeftButtonDown
    } // class AboutWindow : Window
} // namespace WPF_Template.Windows
