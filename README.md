# Events API (ASP.NET Core Web API)

Простой REST API для управления мероприятиями.

---

## 🚀 Возможности

* Получение списка событий с фильтрацией и пагинацией
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
* xUnit (тестирование)

---

## 📦 Структура проекта

* Controllers — обработка HTTP-запросов
* Services — бизнес-логика
* Models — доменные модели
* DTOs — модели для API
* Contracts/Responses — ответы API
* Mapping — профили AutoMapper
* Middlewares — глобальная обработка ошибок
* Tests — юнит-тесты

---

## ▶️ Запуск проекта

### 1. Клонировать репозиторий

git clone https://github.com/maksonMacaron/events-api-practicum-yandex
cd events-api-practicum-yandex

### 2. Запустить проект

dotnet run

---

## 📘 Swagger

После запуска открой:

https://localhost:<port>/swagger  
или  
http://localhost:<port>/swagger

---

## 🔍 Фильтрация и пагинация

GET /events

### Query-параметры:

- title — поиск по названию (регистронезависимый)
- from — дата начала
- to — дата окончания
- page — номер страницы (по умолчанию 1)
- pageSize — размер страницы (по умолчанию 10)

Пример:

GET /events?title=концерт&from=2026-06-01&page=1&pageSize=5

---

## 📄 Ответ

{
  "data": {
    "page": 1,
    "pageSize": 5,
    "total": 12,
    "count": 5,
    "items": []
  },
  "success": true,
  "statusCode": 200,
  "message": "Список событий"
}

---

## ❌ Ошибки

{
  "status": 404,
  "detail": "Событие не найдено"
}

---

## 🧪 Тесты

Запуск:

dotnet test
