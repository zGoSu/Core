using System.IO;
using System.Linq;
using System.Text.Json;

namespace Core.Setting
{
    public class Setting<T> where T : new()
    {
        private T _instance;

        /// <summary>
        /// Инициализирует класс настройки
        /// </summary>
        private void InitInstance()
        {
            _instance ??= new T();
        }

        /// <summary>
        /// Возвращает имя настройки
        /// </summary>
        /// <returns></returns>
        protected virtual string GetFileName()
        {
            return $"{Directory.GetCurrentDirectory()}/setting/{new T().ToString().Split('.').Last()}.json";
        }

        /// <summary>
        /// Создает директорию, если ее нет
        /// </summary>
        private void CreateDirectory()
        {
            if (!Directory.Exists(Path.GetDirectoryName(GetFileName())))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(GetFileName()));
            }
        }

        /// <summary>
        /// Создает файл. Если файл уже есть, то пересоздает его
        /// </summary>
        protected virtual void CreateFile()
        {
            if (File.Exists(GetFileName()))
            {
                File.Delete(GetFileName());
            }
            CreateDirectory();

            InitInstance();

            var json = JsonSerializer.Serialize(_instance, new JsonSerializerOptions() { WriteIndented = true });
            var file = File.CreateText(GetFileName());

            file.WriteLine(json);
            file.Close();
        }

        /// <summary>
        /// Возращает настройки
        /// </summary>
        /// <returns></returns>
        public virtual T GetSetting()
        {
            if (!File.Exists(GetFileName()))
            {
                CreateFile();
            }

            string json = File.ReadAllText(GetFileName());
            _instance = JsonSerializer.Deserialize<T>(json);

            return _instance;
        }
    }
}
