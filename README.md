# MyLab.EmailManager

Сервис, предоставляющий возможность учёта, подтверждения и отправки сообщений адресам электронной почты. 

Ознакомьтесь с последними изменениями в [журнале изменений](./CHANGELOG.md).

Docker образ: [![Docker image](https://img.shields.io/static/v1?label=docker&style=flat&logo=docker&message=image&color=blue)](https://github.com/mylab-tools/email-manager) 

Спецификация API:  [![API specification](https://img.shields.io/badge/OAS3-v1%20(actual)-green)](https://app.swaggerhub.com/apis/OZZYEXT/MyLab.EmailManager/1.0.0)

NuGet клиента: [![NuGet](https://img.shields.io/nuget/v/MyLab.EmailManager.Client.svg)](https://www.nuget.org/packages/MyLab.EmailManager.Client/)

Лицензия: [![License](https://img.shields.io/github/license/mylab-search-fx/delegate)](./LICENSE)

## Обзор

Сервис позволяет создавать/изменять/удалять записи об адресах электронной почты (далее **email**). При создании или изменнии можно указать список меток, которые в дальнейшем будут использоваться для выборки адресов при отправке сообщений.

Создание записи об **email** инициирует процесс подтверждения: отправка сообщения с ссылкой для подтверждения.

Все сообщения, в т.ч. и подтверждение формируются на основе предсутановленных разработчиком шаблонов. Шаблоны могут параметризироватсья метками **email** и переданными при отправке аргументами. Кроме того, при отправке сообщения можно указать непосредственно готовый текст сообщения.  

При отправке сообщения указываются метки записей **email**-ов. Сообщения отправлятся тем адресам, у которых все метки совпали. 

Взаимодействие с сервисом происходит только через **REST API**. 

Для отправки сообщений используется сторонний почтовый сервер. 

Поддерживаемая база данных - **MySQL**.

## Конфигурация

Корневой элемент конфигурации `EmailManager`:

* `PendingMsgScanPeriodSec` - период проверки наличия ожидающих отправки сообщений в секундах. 5 сек - по умолчанию;
* `Tempaltes` - настройки шабонов сообщений:
  * `BasePath` - путь к директории, где хранятся файлы шаблонов. По умолчанию - `/etc/emailmgr/templates`;
* `Smtp` - настройки отправки почты через SMTP сервер:
  * `Host` - имя хоста или IP SMTP сервера;
  * `Port` - порт, по умолчанию - 587;
  * `Login` - логин пользователя;
  * `Password` - пароль пользователя;
  * `SenderName` - (опционально) имя отправителя;
  * `SenderEmail` - адрес отправителя;
* `Confirmation` -папраметры подтверждения:
  * `Title` - заголовок сообщения. По умолчанию - "Email confirmation". 

## Клиент

[Ключ конфигурации](https://github.com/mylab-tools/apiclient?tab=readme-ov-file#%D1%81%D0%BE%D0%BF%D0%BE%D1%81%D1%82%D0%B0%D0%B2%D0%BB%D0%B5%D0%BD%D0%B8%D0%B5-%D0%BA%D0%BE%D0%BD%D1%82%D1%80%D0%B0%D0%BA%D1%82%D0%BE%D0%B2) API клиента - `email-manager` или имена контрактов:

* `IEmailManagerEmailsV1` - разбота с записями об **email**;
* `IEmailManagerConfirmationsV1` - работа с подтверждениями **email**;
* `IEmailManagerSendingsV1` - отправка сообщений.
