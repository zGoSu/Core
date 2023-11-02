using System.IO;
using System.Linq;
using System.Text.Json;

namespace Core.Setting
{
    public class Setting<T> where T : new()
    {
        public string FilePath { get; set; } = $"{Directory.GetCurrentDirectory()}/Setting/{new T().ToString().Split('.').Last()}.json";

        private T _instance;

        /// <summary>
        /// Инициализирует класс настройки
        /// </summary>
        private void InitInstance()
        {
            _instance ??= new T();
        }

        /// <summary>
        /// Создает директорию, если ее нет
        /// </summary>
        private void CreateDirectory()
        {
            if (!Directory.Exists(Path.GetDirectoryName(FilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
            }
        }

        /// <summary>
        /// Создает файл. Если файл уже есть, то пересоздает его
        /// </summary>
        private void CreateFile()
        {
            File.Delete(FilePath);
            CreateDirectory();

            InitInstance();

            var json = JsonSerializer.Serialize(_instance, new JsonSerializerOptions() { WriteIndented = true });
            using var file = File.CreateText(FilePath);

            file.WriteLine(json);
            file.Close();
        }

        /// <summary>
        /// Возращает настройки
        /// </summary>
        /// <returns></returns>
        public T GetSetting()
        {
            if (!File.Exists(FilePath))
            {
                CreateFile();
            }

            var json = File.ReadAllText(FilePath);
            _instance = JsonSerializer.Deserialize<T>(json);

            return _instance;
        }
    }
}
