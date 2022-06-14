/*
}
---------------------------------------------------------------------
Установить акуматику
Опублтковать акуматику
Подключить акуматику
---------------------------------------------------------------------
Добавить фильтрацию в Api /api/v1/catalogs/{id}/products
    GET    (ApiResult<iFilter>)   - получить
---------------------------------------------------------------------
Политики авторированого доступа

---------------------------------------------------------------------
Добавить екраны 

/AspNetUsers/{Id}/Create
/AspNetUsers/{Id}/Edit

---------------------------------------------------------------------

API Account /api/v1/user/{Id}/Account

    POST    (iUser)   - новый
    PUT     (iUser)   - обновить
    GET     (iUser)   - получить
    DELETE  (iUser)   - удалить

Модель iUser{
  "email": "string",
  "password": "string",
  "oldpassword": "string",
  "name": "string",
}
---------------------------------------------------------------------
API Адреса доставки  /api/v1/user/{Id}/Address

    POST   (iAddress)   - новый
    PUT    (iAddress)   - обновить
    GET    (iAddress)   - получить
    DELETE (iAddress)   - удалить

---------------------------------------------------------------------
API Адреса доставки  /api/v1/user/{Id}/Contact

    POST   (iContact)    - новый
    PUT    (iContact)    - обновить
    GET    (iContact)    - получить
    DELETE (iContact)    - удалить

---------------------------------------------------------------------
API Кошилька  /api/v1/user/{Id}/Wallet

    POST   (iWallet)    - новый
    PUT    (iWallet)    - обновить
    GET    (iWallet)    - получить
    DELETE (iWallet)    - удалить

---------------------------------------------------------------------
API Коментария  /api/v1/user/{Id}/Comment

    POST   (iComment)    - новый
    PUT    (iComment)    - обновить
    GET    (iComment)    - получить
    DELETE (iComment)    - удалить

---------------------------------------------------------------------
API Строки заказа  /api/v1/user/{Id}/LineDetail

    POST   (iLineDetail) - новый
    PUT    (iLineDetail) - обновить
    GET    (iLineDetail) - получить
    DELETE (iLineDetail) - удалить

---------------------------------------------------------------------
API Заказа  /api/v1/user/{Id}/Order

Методы:
    POST   (iOrder) - новый
    PUT    (iOrder) - обновить
    GET    (iOrder) - получить
    DELETE (iOrder) - удалить

---------------------------------------------------------------------
Страничный просмотр и фильтр:
    Promoaction
    RootImage
    AspNetRoles
    AspNetUsers
    Addresses
    Orders

---------------------------------------------------------------------
Новые Таблици:
      Views
      TopSales

---------------------------------------------------------------------
Добавить ТабСеты:
      Products/Comments
      User/Cart
      User/Comments

---------------------------------------------------------------------
Новые страници:
      Comments/Index
      Comments/Create
      Comments/Edit
      Comments/Delete
      /User/{id}/Cart
      /User/{id}/Views
      /Portal/{id}/TopSales
      /Catalog/{id}/TopSales





 */
