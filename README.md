# Events API (ASP.NET Core Web API)

Простой REST API для управления мероприятиями.

## 🚀 Возможности

* Получение списка событий
* Получение события по Id
* Создание события
* Обновление события
* Удаление события

---

## 🧱 Технологии

* C#
* ASP.NET Core Web API
* AutoMapper
* Swagger (OpenAPI)

---

## 📦 Структура проекта

* `Controllers` — обработка HTTP-запросов
* `Services` — бизнес-логика
* `Models` — доменные модели
* `DTOs` — модели для API
* `Contracts/Responses` — ответы API
* `Mapping` — профили AutoMapper

---

## ▶️ Запуск проекта

### 1. Клонировать репозиторий

```bash
git clone https://github.com/maksonMacaron/events-api-practicum-yandex
cd <название-папки>
```

### 2. Запустить проект

```bash
dotnet run
```

---

## 📘 Swagger

После запуска открой в браузере:

```
https://localhost:<port>/swagger
```

или

```
http://localhost:<port>/swagger
```

## 💾 Хранение данных

Данные хранятся в памяти приложения (`List<Event>`).
База данных не используется.

---