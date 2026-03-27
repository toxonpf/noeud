# AgentTemplate.md for Noeud

## Назначение

Этот файл описывает, как AI-агенту безопасно и последовательно работать в репозитории `Noeud`.

Цель документа:

- быстро объяснить устройство проекта;
- зафиксировать реальные границы между слоями;
- подсказать, куда класть новый код;
- перечислить рабочие команды для локальной разработки;
- явно отметить текущие ограничения и незавершённые зоны.

Если код и этот документ расходятся, источником правды считается код, а расхождение нужно упомянуть в итоговом отчёте.

---

## Краткая Сводка По Проекту

- Название проекта: Noeud
- Тип проекта: desktop application
- Однострочное описание: прототип desktop-приложения на Avalonia для работы с markdown-контентом, файловым explorer и будущим editor workflow.
- Основные пользователи: разработчик проекта и команда, которая развивает UI/редактор.
- Бизнес- или доменный контекст: локальный markdown/note editor с кастомной доменной моделью markdown AST.
- Стадия жизненного цикла: prototype
- Владельцы / ответственная команда: не зафиксировано в репозитории
- Основная ветка: `main`
- Важные заметки о состоянии репозитория: editor-часть пока почти пустая; explorer работает на sample-данных из `Assets/Samples`; тестовый контур отсутствует; в markdown-домене есть legacy namespace `CleaNoteMd.*`.

---

## Принципы Работы Агента

Если пользователь явно не попросил иначе, агент должен:

- предпочитать минимальное изменение, которое решает задачу;
- сохранять текущую четырёхслойную структуру `Domain -> Application -> Infrastructure -> Presentation`;
- не протаскивать UI- или файловую логику в `Domain`;
- не добавлять новые NuGet-зависимости без явной необходимости и отдельного объяснения;
- обновлять sample-assets, XAML и документацию, если поведение UI или путей меняется;
- проверять хотя бы сборку или минимальную локальную валидацию перед завершением;
- явно писать, если проверка не удалась из-за окружения, а не из-за кода.

### На Что Агент Должен Оптимизировать Работу

1. Correctness
2. Maintainability
3. Speed

### Чего Агент Не Должен Делать По Умолчанию

- Не переписывать архитектуру под полноценный DI/container, пока это отдельно не запрошено.
- Не переносить код между слоями без ясной причины.
- Не копировать legacy naming из markdown-моделей в новый код, если это не часть осознанной миграции.
- Не менять sample explorer path и ресурсные пути без проверки `csproj` и XAML.
- Не вводить тестовый фреймворк или codegen по умолчанию: это уже изменение tooling.

---

## Источники Правды

Перед нетривиальными изменениями сверяйтесь со следующим:

| Источник | Путь | Когда использовать |
| --- | --- | --- |
| Структура solution и project references | `Noeud.sln`, `Noeud.*/*.csproj` | Для проверки границ проектов, зависимостей и версий |
| Presentation shell | `Noeud.Presentation/Shell/` | Для изменений основного окна, layout и композиции фич |
| Explorer feature | `Noeud.Presentation/Features/Explorer/` | Для поведения дерева файлов и selection flow |
| Editor feature | `Noeud.Presentation/Features/Editor/` | Для развития редактора |
| Application contracts | `Noeud.Application/` | Для use cases и интерфейсов между слоями |
| Domain model | `Noeud.Domain/` | Для value objects, selection state и markdown AST |
| Markdown parser adapter | `Noeud.Infrastructure/Markdown-Parser/` | Для интеграции Markdig и конвертации в доменную модель |
| Шаблон инструкции | `AGENTS.ru.md template.md` | Для структуры и полноты агентного документа |

Если код и документация противоречат друг другу, приоритет у кода.

---

## Технологический Стек

### Основной Стек

- Язык(и): C# для `net10.0`, `Nullable` включён
- Runtime(s): .NET 10
- Framework(s): Avalonia `11.3.12`
- Package manager(s): NuGet через `dotnet`
- Build tool(s): `dotnet build`, `dotnet run`, `dotnet format`
- База(ы) данных: none
- Messaging / queueing: none
- Кэш / хранилище: локальная файловая система, bundled assets
- Хостинг / инфраструктура: desktop/local

### Ключевые Библиотеки И Сервисы

| Область | Библиотека / сервис | Версия | Назначение | Примечания / ограничения |
| --- | --- | --- | --- | --- |
| UI | Avalonia | 11.3.12 | desktop UI framework | Используются XAML, compiled bindings и Fluent theme |
| UI | Avalonia.Desktop | 11.3.12 | desktop host | Точка входа приложения |
| UI | Avalonia.Svg.Skia | 11.1.0 | отображение SVG-иконок | Иконки лежат в `Assets/Icons` |
| UI | Avalonia.Fonts.Inter | 11.3.12 | встроенный UI font | Подключается через `.WithInterFont()` |
| MVVM | CommunityToolkit.Mvvm | 8.2.1 | `ObservableObject`, `SetProperty` | Базовый `ViewModelBase` наследуется от него |
| Rendering | SkiaSharp.NativeAssets.Linux | 2.88.9 | Linux runtime assets | Важен для запуска на Linux |
| Parsing | Markdig | 1.1.1 | markdown parser | Адаптирован в `Noeud.Infrastructure` |

### Политика Версий

- Обязательные версии: все ключевые версии зафиксированы в `*.csproj`
- Источник правды для версий: `Noeud.Presentation/Noeud.Presentation.csproj`, `Noeud.Infrastructure/Noeud.Infrastructure.csproj`
- Политика обновления зависимостей: manual
- Требования по совместимости: изменения не должны ломать `net10.0` target framework и текущую структуру проектных ссылок

---

## Архитектура

- Архитектурный стиль: layered desktop application with clean-ish project boundaries
- Высокоуровневое описание: `Presentation` отвечает за XAML, UI state и view composition; `Application` задаёт use cases и контракты; `Domain` хранит простые value objects и markdown AST; `Infrastructure` реализует внешние адаптеры, например парсер Markdig.
- Основные модули / bounded contexts: shell, explorer, editor, markdown parsing, selection state
- Основной поток данных: `UI event -> ViewModel -> Application use case -> Domain state`
- Дополнительный поток данных: `raw markdown -> IMarkdownParser -> MarkdigParser -> Domain markdown blocks/inlines`
- Подход к управлению состоянием: локальное состояние во `ViewModel` и небольшой in-memory state в `ExplorerSelectionState`
- Границы интеграций: локальная файловая система и библиотека Markdig
- Зоны миграции: editor workflow ещё не подключён к parser/use case; markdown domain использует legacy namespace `CleaNoteMd.*`; composition root пока ручной, без DI container
- Жёсткие ограничения: `Domain` не должен зависеть от Avalonia или Markdig; `Application` не должен знать о XAML/controls; `Infrastructure` не должен тащить UI-логику; `Presentation` не должен напрямую моделировать markdown AST вместо `Domain`

### Архитектурные Правила

- Value objects и доменные состояния размещать в `Noeud.Domain`, а не во view model или code-behind.
- Use case-логику размещать в `Noeud.Application`, а не в XAML code-behind.
- Интеграции с Markdig и другими внешними библиотеками размещать в `Noeud.Infrastructure`.
- Новые UI-фичи размещать по паттерну `Noeud.Presentation/Features/Explorer/Views` и `Noeud.Presentation/Features/Explorer/ViewModels`.
- Не обходить `IExplorerSelectionUseCase`, если задача касается выбора файла.
- Перед созданием новых shared-абстракций проверять, можно ли расширить существующие `ViewModelBase`, `ViewLocator`, `IExplorerSelectionUseCase`, `IMarkdownParser`.

---

## Структура Репозитория

```text
Noeud/
├─ Noeud.sln                            # solution с 4 проектами
├─ Noeud.Domain/                        # доменные типы, selection state, markdown AST
├─ Noeud.Application/                   # use cases и контракты между слоями
├─ Noeud.Infrastructure/                # адаптеры к внешним библиотекам, сейчас главным образом Markdig
├─ Noeud.Presentation/                  # Avalonia UI, XAML, shell, feature view models, assets
├─ AGENTS.ru.md template.md             # исходный русский шаблон для agent instructions
└─ AgentTemplate.md                     # этот проектно-специфичный файл
```

### Ответственность Директорий

| Путь | Ответственность | Типичное содержимое | Чего быть не должно |
| --- | --- | --- | --- |
| `Noeud.Domain/` | доменная модель | `SelectedFile`, `FilePath`, `ExplorerSelectionState`, `Models/Blocks`, `Models/Inlines` | Avalonia controls, Markdig types, файловый I/O |
| `Noeud.Application/` | use cases и интерфейсы | `IExplorerSelectionUseCase`, `ExplorerSelectionUseCase`, `IMarkdownParser` | XAML, code-behind, прямые ссылки на Avalonia |
| `Noeud.Infrastructure/` | реализации внешних адаптеров | `MarkdigParser`, block/inline converters | UI layout, view models |
| `Noeud.Presentation/Features/` | фичи UI | `Views/*.axaml`, `ViewModels/*.cs` | доменные сущности, которые должны жить в `Domain` |
| `Noeud.Presentation/Shared/` | общие UI-типы и styles | `ViewModelBase`, `Styles/*.axaml` | feature-specific бизнес-логика |
| `Noeud.Presentation/Assets/` | иконки, шрифты, sample-файлы | `Icons/`, `Fonts/`, `Samples/` | секреты, runtime-конфиги |

### Правила Размещения Файлов

- Новые фичи UI размещать в каталогах вида `Noeud.Presentation/Features/Explorer/`.
- Общий UI-код размещать в `Noeud.Presentation/Shared/`.
- Доменные типы и AST-узлы размещать в `Noeud.Domain/`.
- Адаптеры парсинга и конвертеры размещать в `Noeud.Infrastructure/Markdown-Parser/`.
- Generated artifacts в проекте пока не используются.
- Env/config-файлы в репозитории не заведены; не вводите их без необходимости.
- Миграции, схемы и внешние контракты в проекте пока отсутствуют.

---

## Подготовка Окружения

### Обязательные Инструменты

- Обязательные инструменты: .NET SDK `10.0.104` или совместимый SDK для `net10.0`
- Установка зависимостей: `dotnet restore Noeud.sln`
- Запуск локального окружения: `dotnet run --project Noeud.Presentation/Noeud.Presentation.csproj`
- Запуск только зависимых сервисов: не требуется
- Seed / bootstrap данных: не требуется
- Откуда загружать env-переменные: env-конфигурация не используется
- Необходимые локальные сервисы: none

### Замечания По Setup

- На Linux для Avalonia/Skia нужны системные desktop-зависимости, установленные вне репозитория.
- Sample explorer content копируется в output через `Noeud.Presentation.csproj`; отдельный seed не нужен.
- В текущем окружении `dotnet restore` и `dotnet build` завершались кодом `1` без диагностического вывода, поэтому агент должен явно сообщать, если локальная верификация блокируется окружением.

---

## Команды Для Разработки

| Задача | Команда | Scope | Примечания |
| --- | --- | --- | --- |
| Установить зависимости | `dotnet restore Noeud.sln` | repo | Требует доступа к NuGet и рабочей .NET-среды |
| Запустить приложение | `dotnet run --project Noeud.Presentation/Noeud.Presentation.csproj` | presentation | Основной способ локальной проверки UI |
| Сборка solution | `dotnet build Noeud.sln` | repo | Базовая проверка на компиляцию |
| Сборка presentation | `dotnet build Noeud.Presentation/Noeud.Presentation.csproj` | project | Удобно при правках только UI |
| Форматирование | `dotnet format Noeud.sln` | repo | Использовать перед крупными правками или при style drift |

### Стратегия Проверки

1. Проверить затронутый проект или сценарий локально.
2. Запустить `dotnet build` для ближайшего проекта.
3. При межслойных изменениях запускать `dotnet build Noeud.sln`.
4. Для UI-изменений по возможности дополнительно запускать приложение и проверять руками.

---

## Руководство По Тестированию

- Test framework(s): не настроены
- Где лежат unit tests: отсутствуют
- Где лежат integration tests: отсутствуют
- Где лежат e2e tests: отсутствуют
- Где лежат contract tests: none
- Паттерны именования: не применимо
- Где лежит CI workflow: не найден в репозитории

### Правила Тестирования

- Сейчас минимальный уровень проверки для большинства задач: сборка плюс ручная проверка сценария.
- Если изменение затрагивает `Domain` или `Application` и без тестов риск высокий, агент должен предложить добавить тестовый проект, но помнить, что это потребует новых NuGet-зависимостей.
- Исправления багов желательно сопровождать хотя бы воспроизводимым ручным сценарием, если автоматический тест пока некуда положить.

### Матрица Тестов

| Тип тестов | Путь / Scope | Команда | Когда запускать |
| --- | --- | --- | --- |
| Smoke build | repo | `dotnet build Noeud.sln` | Любые изменения в коде, если сборка доступна |
| UI smoke run | presentation | `dotnet run --project Noeud.Presentation/Noeud.Presentation.csproj` | Изменения XAML, code-behind, styles, view models |
| Manual scenario | feature | ручная проверка в приложении | Изменения поведения explorer/editor |

---

## Стиль Кода И Naming

- Formatter: `dotnet format`
- Linter: отдельный linter не настроен; опираться на компилятор и встроенные анализаторы .NET
- Политика типизации: статическая типизация C# с `Nullable` включённым
- Политика комментариев: комментарии только там, где поведение неочевидно; не пояснять очевидные конструкции
- Политика импортов: обычные `using` сверху файла, с логической группировкой через пустые строки
- Подход к обработке ошибок: guard clauses и исключения стандартной библиотеки для неверного ввода
- Подход к логированию: отдельная подсистема логирования отсутствует; Avalonia использует `LogToTrace()`
- Подход к конфигурации: code-first через `.csproj`, `App.axaml`, `Program.cs`

### Naming Conventions, Которые Мы Предпочитаем

| Сущность | Предпочтительно | Избегать | Пример |
| --- | --- | --- | --- |
| Файлы C# и XAML | PascalCase | snake_case, kebab-case | `ExplorerPanelViewModel.cs`, `MainWindow.axaml` |
| Директории feature/UI | PascalCase | смешение стилей в одной зоне | `Features/Explorer/ViewModels` |
| Классы / компоненты | PascalCase | сокращения без необходимости | `ExplorerItemViewModel` |
| Функции / методы | PascalCase с глаголом | `DoStuff`, `Handle` без контекста | `SelectFile`, `ClearSelection` |
| Переменные | `camelCase`, приватные поля `_camelCase` | венгерская нотация | `_selectedExplorerItem` |
| Константы | PascalCase для `const`, `_camelCase` для приватных readonly-полей | ALL_CAPS в C#-коде | `GetSampleExplorerRootPath` |
| Types / interfaces / schemas | PascalCase, интерфейсы с префиксом `I` | безымянные абстракции | `IExplorerSelectionUseCase` |
| Названия тестов | `Method_Scenario_ExpectedResult` | `WorksCorrectly` | `SelectFile_NullPath_ClearsSelection` |
| Названия веток | короткое описание задачи | случайная смесь стилей | `feature/editor-selection`, `fix/markdig-parser` |

### Style Do / Don't

Делать:

- следовать уже существующим file-scoped namespace и структуре проектов;
- держать view model маленькими и конкретными;
- использовать value objects вроде `FilePath` вместо голых строк там, где это уже заведено;
- сохранять пары `View`/`ViewModel` в соседних feature-папках.

Не делать:

- складывать новую бизнес-логику в `MainWindow.axaml.cs`;
- смешивать `Noeud.*` и `CleaNoteMd.*` namespace в новой функциональности без осознанного решения;
- создавать общий `Utils` для всего подряд;
- обходить слой `Application` прямым доступом к состоянию, если уже есть use case.

---

## Предпочтительные Паттерны И Эталонные Реализации

### Хорошие Примеры, Которые Можно Копировать

- `Noeud.Presentation/Features/Explorer/ViewModels/ExplorerPanelViewModel.cs`: хороший пример feature-local view model, которая держит UI state и делегирует бизнес-смысл use case-слою.
- `Noeud.Application/ExplorerSelectionUseCase.cs`: хороший пример тонкой application-логики поверх доменного состояния.
- `Noeud.Presentation/ViewLocator.cs`: хороший пример принятой конвенции соответствия `ViewModels` -> `Views`.
- `Noeud.Presentation/Shell/MainWindow.axaml`: хороший пример композиции нескольких feature views в shell.

### Паттерны, Которые Не Нужно Копировать

- `Noeud.Infrastructure/Markdown-Parser/MarkdigParser.cs`: рабочий адаптер, но не копируйте legacy namespace `CleaNoteMd.*` в новый код.
- `Noeud.Domain/Models/Blocks/*.cs` и `Noeud.Domain/Models/Inlines/*.cs`: эти типы полезны как доменная модель, но namespace в них пока не приведён к имени проекта.
- `Noeud.Presentation/Features/Editor/ViewModels/EditorPanelViewModel.cs`: это пока заглушка, не эталон полноценной реализации.
- Параметрless-конструкторы с `new ExplorerSelectionUseCase()`: допустимы для текущего дизайнерского/простого compose flow, но не стоит без нужды размножать ручное создание зависимостей по всему проекту.

---

## Данные, Контракты, Codegen И Миграции

- Где лежат схемы: отсутствуют
- Где лежат миграции: отсутствуют
- Где лежат API-контракты: отсутствуют
- Где лежат event-контракты: отсутствуют
- Где лежит generated code: отсутствует
- Команда для регенерации: не требуется

### Правила

- Markdown AST в `Noeud.Domain/Models` считается ручным кодом, а не generated.
- При изменении структуры AST синхронно обновляйте parser-конвертеры в `Noeud.Infrastructure/Markdown-Parser/Converters`.
- Если добавляется новый внешний контракт или codegen, это нужно отдельно описать в этом файле.

---

## Границы Безопасности И Safety

### Жёсткие Правила

- Не коммитить локальные ключи, токены и machine-specific secrets.
- Не превращать sample-данные из `Assets/Samples` в хранилище чувствительной информации.
- Любой новый файловый ввод должен валидироваться на корректность пути и null/empty значения.
- При работе с локальной файловой системой избегать разрушительных операций без явного запроса пользователя.

### Перед Этими Действиями Нужна Подтверждённая Проверка Человеком

- удаление sample-файлов или папок из `Assets/Samples`;
- замена структуры проектов и project references;
- добавление новых внешних зависимостей;
- изменение runtime target framework;
- любая операция, которая пишет вне workspace или меняет системные настройки.

### Чувствительные Зоны

- Authentication / authorization: отсутствуют
- Payments / billing: отсутствуют
- Personal or regulated data: отсутствуют
- Production configuration / infrastructure: отсутствуют

---

## Git, PR И Definition Of Done

- Схема именования веток: в репозитории нет строгой конвенции; существующие ветки разностильные, но для новых изменений предпочитать короткие описательные имена вроде `feature/editor-selection` или следовать формату пользователя
- Формат commit message: короткий императивный summary
- Формат заголовка PR: краткое описание пользовательского изменения
- Политика changelog: не настроена
- Политика release notes: не настроена

### Definition Of Done

Изменение не считается завершённым, пока:

1. не проверена хотя бы релевантная команда сборки или честно не описано, почему она не выполнима;
2. не обновлены ресурсы, стили или sample-файлы, если они затронуты;
3. не соблюдены границы слоёв и правила размещения файлов;
4. не зафиксированы assumptions, ограничения и потенциальные follow-up work.

---

## Правила Для Монорепозитория

Репозиторий не является монорепозиторием в классическом смысле, но это multi-project .NET solution.

- Корневой `AgentTemplate.md` описывает глобальные правила для всех проектов solution.
- Если один из проектов станет заметно сложнее остальных, для него можно добавить локальный `AGENTS.md` рядом с `.csproj`.
- При конфликте локальный файл рядом с изменяемым кодом должен уточнять, а не противоречить корневому документу.

---

## Известные Подводные Камни

- В текущем окружении `dotnet restore` и `dotnet build` могут завершаться кодом `1` без диагностического вывода; если это повторяется, фиксируйте это как ограничение окружения, а не как подтверждённую ошибку кода.
- `ExplorerPanelViewModel` читает дерево из `Assets/Samples/explorer` через output directory; при смене путей обязательно проверяйте `CopyToOutputDirectory` в `Noeud.Presentation.csproj`.
- Editor UI пока не связан с markdown parser и selection state; изменение только editor-слоя само по себе не включит функциональность.
- В markdown-модели используется namespace `CleaNoteMd.Domain.*`; это legacy-состояние, и новые файлы лучше писать в актуальном namespace проекта, если задача не про миграцию.
- В проекте нет централизованного DI, тестового контура и CI, поэтому агенту нельзя предполагать наличие автоматических guardrails.

---

## Когда Агент Должен Остановиться И Спросить

Агент должен остановиться и спросить человека, если:

- нужно выбрать между несколькими вариантами поведения editor/preview, и из кода это не следует;
- изменение требует новой зависимости, тестового фреймворка или перестройки solution;
- требуется массовая миграция namespace `CleaNoteMd` -> `Noeud`;
- нужно менять структуру `Assets/Samples` или источник данных explorer;
- сборка падает, и непонятно, проблема в коде или в локальном окружении;
- безопасный путь зависит от UX-решения, которое ещё не выбрано.

---

## Дополнительная Синхронизация С Другими Инструментами

Сейчас в репозитории не обнаружены `README.md`, `.github/copilot-instructions.md`, `CLAUDE.md` или другие аналогичные файлы с инструкциями.

Если такие файлы появятся:

- держите `AgentTemplate.md` или будущий `AGENTS.md` главным источником правил;
- не дублируйте подробно одно и то же в нескольких местах;
- синхронизируйте команды, архитектурные границы и known pitfalls.

---

## Чеклист Поддержки Для Людей

- Обновляйте этот файл при изменении слоёв, команд или зависимостей.
- Если в проект добавятся тесты, CI или DI-container, сразу отразите это здесь.
- При начале namespace-миграции отдельно зафиксируйте её scope и правила.
- Держите список эталонных файлов актуальным, чтобы агент копировал хорошие паттерны, а не случайные заготовки.

---

## Чеклист Перед Использованием

Этот файл уже заполнен под текущий репозиторий, но перед долгосрочным использованием стоит проверить:

- действительно ли основной веткой остаётся `main`;
- появился ли официальный `README.md` или CI workflow;
- исправлено ли текущее поведение `dotnet restore/build` в локальном окружении;
- не завершена ли миграция editor-части и markdown namespace.
