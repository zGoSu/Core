# Core
## Core.Logger
Проект отвечает за логирование. В проекте уже есть пример реализации логирование в консоли 
`ConsoleLogger.cs`  и в файл `FileLogger.cs`. На примере их так же можно будет создать логирование в базу данных и т.п.

Пример реализации:
```c#
public static ILogger Logger { get; private set; }

public static void Initialization()
{
    Logger = GetLoggers();
}

private static Logger GetLoggers()
{
    var configuration = new Setting<Configuration>() { FilePath = $"{Directory.GetCurrentDirectory()}/Setting/Logger.json" }.GetSetting();
    var loggers = new List<ILogger>()
    {
        new ConsoleLogger() { Configuration = configuration },
        new FileLogger() { Configuration = configuration }
    };
    return new Logger(loggers);
}
```
***FilePath*** - как будет называться наш файл конфигурации и в каком каталоге. По умолчанию он берет имя класса, на основе которого создается конфигурация. В примере 
```c# 
new Setting<Configuration>()
``` 
файл будет называться Configuration.

Пример файла конфигурации `Logger.json`:
```json
{
  "Debug": [
    "Console",
    "File"
  ],
  "Info": [
    "Console"
  ],
  "Error": [
    "Console"
  ],
  "Warning": [
    "Console"
  ]
}
```
## Core.MySQL
Проект отвечает за взаимодействие с СУБД MySQL.
## Core.Setting
Проект отвечает за конфигурацию. При выполнении программы создается файл конфигурации (если его нет) в формате JSON на основе класса. 
Если файл конфигурации уже создан, то программа подтянет оттуда данные и сохранит его в объекте.
## Core.Sql
Проект отвечает за формирование запросов в базу данных. 
На данный момент реализован только массовое добавление записей в базу данных MySQL с помощью запроса INSERT INTO.
