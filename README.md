# kpi-practice-onion
**1. Функциональные требования**


As a/an | I want to | So that | To do 
------- | --------- | ------- | ------
Пользователь | Записывать свои расходы по существующим категориям. Я хочу чтобы приложение учитывало дату записи. | Я смогу открыв приложение,выбрать нужные мне категории,записать сколько в каждой было потрачено и сохранить данные на сервере. | Создать форму для ввода сумы для каждой категории.Добавить кнопки для добавление новой категории из списка.Создать систему преобразования данных в JSON файл и отправки их на сервер | Получать доступ к своим прошлым записям. Я хочу иметь возможность получить запись сделанную в конкретный день или summary(все траты по всем категориям вида “категория - сумма”) за год/месяц/неделю. 
| | Получать доступ к своим прошлым записям. Я хочу иметь возможность получить запись сделанную в конкретный день или summary(все траты по всем категориям вида “категория - сумма”) за год/месяц/неделю. | Я смогу просматривать затраты за каждый день и за промежуток времени(неделя/месяц/год) | Создать систему поиска нужных данных на сервере,их компоновку и отправку в приложение.
| | Иметь возможность построить график своих трат подневно.На графике одновременно должны быть все категории,каждая выделена своим цветом. | Я сравниваю свои траты за разные промежутки времени. | Создать программу на стороне пользователя которая бы на основе полученных с сервера данных строила графики


**USE CASES**

**Цель** пользователя в том, чтобы хранить данные о своих тратах,то есть хранить сколько он потратил на каждую категорию товаров денег в конкретный день. Строить линейные графики на основе своих трат.

**Задачи** пользователя:
* Загружать сколько и в какой категории было потрачено денег 
* Получать информацию обратно в виде:"категория1:сумма,категори2:сумма..."
* Строить линейный график на основе потраченных в каждой категории денег

**Загрузка информации**:
*	Пользователь запускает приложение и вводит свои данные для входа или регистрируется. Регистрация происходит прямо в приложении,а логины и пароли пользователей хранятся в БД.
*	На первой же странице пользователь кликает на кнопку ‘+’ в пустой форме.
*	В выпадающем списке он выбирает нужную ему категорию.
*	В форме появляется строка с названием категории и пустым полем
*	Пользователь кликает на пустое поле и вводит сумму в гривнах. Ввести меньше 1 гривны нельзя,можно вводить нецелые значения до 2 знаков после запятой. Максимальное число которое может ввести пользователь равно 2147483647(INT_MAX). 
*	Пользователь нажимает кнопку “Сохранить”
*	Если данные введены корректно пользователь выбирает в календарике день и месяц
*	Пользователь нажимает “ОК” и видит сообщение о результате выполнения запроса

**Получать информацию обратно в отформатированном виде**:
*	Пользователь нажимает на кнопку с изображением листа бумаги
*	На открывшейся странице он выбирает начальную и дату и конечную,за время между которыми хотел бы получить данные
*	Пользователь нажимает на кнопку “получить”
*	Пользователь видит на екране “отчет”

**Строить график на основе данных**:
*	Пользователь нажимает на кнопку с рисунком графика
*	На открывшейся странице он выбирает начальную и дату и конечную,за время между которыми хотел бы получить данные
*	Пользователь нажимает кнопку “построить”
*	Пользователь видит на экране линейный график с функциями разного цвета(по цвету на каждую категорию)

# **Endpoints** #

## **User** ##


**GET**
* Endpoint: /User
* Input parameters: userId (INT)
* Output: 
```
{
  "id": 0,
  "login": "string",
  "password": "string"
}
```
* Success code: 200
* Failure code: 404
* Exceptions: 
  - NotFoundException if user with given id doesnt exists or if deleted

**GET**
* Endpoint: /User
* Input body: 
```
{
"login": "string",   [MIN 3 symbols, All symbols allowed]
"password": "string" [MIN 3 symbols, All symbols allowed]
}
```
* Output: 
```
{
  "id": 0,
  "login": "string",
  "password": "string"
}
```
* Success code: 200
* Failure code: 400
* Exceptions: 
  - WrongLoginDataException if user with given login and password doesnt exists or if deleted
  
**POST**
* Endpoint: /User
* Input body: 
```
{
"login": "string",   [MIN 3 symbols, All symbols allowed]
"password": "string" [MIN 3 symbols, All symbols allowed]
}
```
* Output: 
```
{
  "id": 0,
  "login": "string",
  "password": "string"
}
```
* Success code: 200
* Failure code: 400
* Exceptions:
  - FailedInsertionException if user with same login already exists

**PATCH**
* Endpoint: /User
* Input parameters: userId (INT), newLogin (STRING)
* Output:
* Success code: 200
* Failure code: 404
* Exceptions:
  - NotFoundException if user with id userId does not exist in database
 
**DELETE**
* Endpoint: /User
* Input parameters: userId (INT), newLogin (STRING)
* Output:
* Success code: 200
* Failure code: 409
* Exceptions:
  - AlreadyDeletedException if user with id userId does not exist in database or if already deleted



//User should not be deleted from database, but marked as deleted.


## **Record** ##


**POST**
* Endpoint: /User/{userId}/Record
* Input body: 
```
{
  "categoryId": 0,
  "date": "2021-05-14",
  "amount": 2147483647   [MIN 1, MAX INT.MAX]
}
```
, userId (INT)
* Output: 
```
{
  "id": 0,
  "userId": 0,
  "categoryId": 0,
  "date": "2021-05-14T17:41:16.503Z",
  "amount": 0
}
```
* Success code: 200
* Exceptions:


**GET (List)**
* Endpoint: /User/{userId}/Record
* Input parameters: userId (INT), from (dd.mm.yyyy STRING), till (dd.mm.yyyy STRING)
* Output: 
```
{
  "id": 0,
  "userId": 0,
  "categoryId": 0,
  "date": "2021-05-14T17:41:16.503Z",
  "amount": 0
}
```
* Success code: 200
* Exceptions:


**GET**
* Endpoint: /User/Record
* Input parameters: recordId (INT)
* Output: 
```
{
  "id": 0,
  "userId": 0,
  "categoryId": 0,
  "date": "2021-05-14T17:41:16.503Z",
  "amount": 0
}
```
* Success code: 200
* Failure code: 404
* Exceptions:
  - NotFoundException if record with id recordId does not exist in database


**DELETE (List)**
* Endpoint: /User/{userId}/Record
* Input parameters: userId (INT), from (dd.mm.yyyy STRING), till (dd.mm.yyyy STRING)
* Output: 
```
[
  {
    "id": 0,
    "userId": 0,
    "categoryId": 0,
    "date": "2021-05-14T17:50:44.712Z",
    "amount": 0
  },
  .....
]
```
* Success code: 200
* Exceptions:

**DELETE**
* Endpoint: /User/Record
* Input parameters: recordId (INT)
* Output:
* Success code: 200
* Exceptions:
  - InvalidOperationException if record with id recordId does not exist in database


**PUT**
* Endpoint: /User/Record
* Input parameters: recordId (INT)
* Input body:
```
{
  "categoryId": 0,
  "date": "2021-05-14",
  "amount": 2147483647
}
```
* Output:
* Success code: 200
* Failure code: 404
* Exceptions:
  - DeletionFailedException if record with id recordId does not exist in database


**Patch**
* Endpoint: /User/Record
* Input parameters: recordId (INT),newAmount (INT)
* Output:
* Success code: 200
* Failure code: 404/400
* Exceptions:
  - NotFoundException if record with id recordId does not exist in database
  - OverflowException if old amound + newAmount > INT.MAX
 
 
 ## **Category** ##


**POST**
* Endpoint: /Category
* Input body:
```
{
  "name": "string"  [MIN 3, MAX 15]
}
```
* Output:
```
{
  "id": 0,
  "name": "string"
}
```
* Success code: 200
* Failure code: 400
* Exceptions:
  - FailedInsertionException if same category already exists


**GET**
* Endpoint: /Category
* Input parameters: categoryId (INT)
* Output:
```
{
  "id": 0,
  "name": "string"
}
```
* Success code: 200
* Failure code: 404
* Exceptions:
  - NotFoundException if category with id categoryId does not exist in database


**DELETE**
* Endpoint: /Category
* Input parameters: categoryId (INT)
* Output:
```
{
  "id": 0,
  "name": "string"
}
```
* Success code: 200
* Failure code: 404
* Exceptions:
  - NotFoundException if category with id categoryId does not exist in database

**2. Нефункциональные требования**

  2.1 Все запросы должны отрабатывать не более чем за секунду

