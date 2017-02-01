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

namespace Tree
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion
        #region on Loaded
        /// <summary>
        /// Когда приложение впервые запускается
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Получение локальных данных о дисках
            foreach (var drive in Directory.GetLogicalDrives())
            {
                //Создание нового экземпляра для содержимого
                var item = new TreeViewItem();

                //Запись заголовка и пути
                item.Header = drive;
                item.Tag = drive;
                item.Items.Add(null);

                //Проверка открытых элементов
                item.Expanded += Folder_Expanded;

                //Добавление в текущее древо
                FolderView.Items.Add(item);
            }
        }
        #region Folder Expanded
        
        /// <summary>
        /// Когда открывается папка, поиск вложенных папок и файлов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       
        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            #region Initial check
            var item = (TreeViewItem)sender;
            
            // Если объект содержит только пустое значение
            if (item.Items.Count != 1 || item.Items[0] != null)
                return;

            // Удаляем пустые значения
            item.Items.Clear();

            // Получаем полный путь
            var fullPath = (string)item.Tag;
            #endregion
            #region Get Folders

            // Создаём пустой лист для директорий
            var directories = new List<string>();

            // Получаем директории с папок
            try
            {
                var dirs = Directory.GetDirectories(fullPath);
                if (dirs.Length > 0)
                    directories.AddRange(dirs);
            }
            catch { }

            // Для каждой директории
            directories.ForEach(directoryPath =>
            {
                // Создаём новый элемент директории 
                var subItem = new TreeViewItem()
                {
                    // Устанавливаем значание заголовка как название директории
                    Header = GetFileFolderName(directoryPath),
                    
                    // И записываем полный путь 
                    Tag = directoryPath
                };
                
                // Добавляем пустой элемент
                subItem.Items.Add(null);

                // Раскрываем
                subItem.Expanded += Folder_Expanded;

                // Добавляем элемент родителю
                item.Items.Add(subItem);
            });
            #endregion
            #region Get Files

            // Создаём пустой лист для файлов
            var files = new List<string>();
            
            // Получаем директории с папок
            try
            {
                var fs = Directory.GetFiles(fullPath);
                if (fs.Length > 0)
                    files.AddRange(fs);
            }
            catch { }
            
            // Для каждого файла
            files.ForEach(filesPath =>
            {
                // Создаём новый элемент файла
                var subItem = new TreeViewItem()
                {
                    // Устанавливаем значание заголовка как название файла
                    Header = GetFileFolderName(filesPath),
                    
                    // И записываем полный путь 
                    Tag = filesPath
                };
                
                // Добавляем элемент родителю
                item.Items.Add(subItem);
            });
            #endregion
        }
        #endregion
        #region Get Folders Name
        
        /// <summary>
        /// Поиск имени файла или папки в полном пути файла
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        
        public static string GetFileFolderName(string path)
        {
            // Если пути нет ничего не возвращается
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            // Замена всех слешей задними 
            var normalizedPath = path.Replace('/', '\\');

            // Находим последний бэкслеш
            var lastIndex = normalizedPath.LastIndexOf('\\');

            //Если не находим, используем полный путь файла
            if (lastIndex <= 0)
                return path;
            
            //Возвращаем имя после последнего бэкслеша
            return path.Substring(lastIndex + 1);
        }
        #endregion
        #endregion
    }
}
