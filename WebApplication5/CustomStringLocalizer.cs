using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication5
{
    public class CustomStringLocalizer : IStringLocalizer
    {
        Dictionary<string, Dictionary<string, string>> resources;
        // ключи ресурсов
        const string HELLO = "Hello";
        const string DISHES = "Dishes";
        const string LOGIN = "Login";
        const string LOGOUT = "Logout";
        const string CREATENEW = "Create new";
        const string EDIT = "Edit";
        const string DETAILS = "Details";
        const string DELETE = "Delete";
        const string INSTITUTION = "Cafes";
        const string TITLE = "Title";
        const string DESCRIPTION = "Description";
        const string NAME = "Name";
        const string PRICE = "Price";
        const string TRASH = "Trash";
        const string TOTAL = "TOTAL";
        const string ADDTOBASKET = "Add to basket";

        public CustomStringLocalizer()
        {
            // словарь для английского языка
            Dictionary<string, string> enDict = new Dictionary<string, string>
            {
                { HELLO, "Hello"},
                { DISHES, "Dishes"},
                { LOGIN, "Login"},
                { LOGOUT, "Logout"},
                { CREATENEW, "Create new"},
                { EDIT, "Edit"},
                { DETAILS, "Details"},
                { DELETE, "Delete"},
                { INSTITUTION, "Cafes"},
                { TITLE, "Title"},
                { DESCRIPTION, "Description"},
                { NAME, "Name"},
                { PRICE, "Price"},
                { TRASH, "Basket"},
                { TOTAL, "TOTAL"},
                { ADDTOBASKET, "Add to basket"}
            };
            // словарь для русского языка
            Dictionary<string, string> ruDict = new Dictionary<string, string>
            {
                { HELLO, "Привет"},
                { DISHES, "Блюда"},
                { LOGIN, "Логин"},
                { LOGOUT, "Выйти"},
                { CREATENEW, "Добавить заведение"},
                { EDIT, "Изменить"},
                { DETAILS, "Подробнее"},
                { DELETE, "Удалить"},
                { INSTITUTION, "Заведения"},
                { TITLE, "Название"},
                { DESCRIPTION, "Описание"},
                { NAME, "Название"},
                { PRICE, "Цена"},
                { TRASH, "Корзина"},
                { TOTAL, "Итого:"},
                { ADDTOBASKET, "Добавить в корзину"}
            };
            // словарь для немецкого языка
            Dictionary<string, string> deDict = new Dictionary<string, string>
            {
                { HELLO, "Hallo"},
                { DISHES, "Gerichte"},
                { LOGIN, "Login"},
                { LOGOUT, "Logout"},
                { CREATENEW, "Einrichtung hinzufügen"},
                { EDIT, "Bearbeiten"},
                { DETAILS, "Mehr Info"},
                { DELETE, "Delete"},
                { INSTITUTION, "Restaurants"},
                { TITLE, "Titel"},
                { DESCRIPTION, "Description"},
                { NAME, "Titel"},
                { PRICE, "Preis"},
                { TRASH, "Legeny"},
                { TOTAL, "Total:"},
                { ADDTOBASKET, "In den Warenkorb legenу"}
            };
            // создаем словарь ресурсов
            resources = new Dictionary<string, Dictionary<string, string>>
            {
                {"en-US", enDict },
                {"ru-RU", ruDict },
                {"de-DE", deDict }
            };
        }
        // по ключу выбираем для текущей культуры нужный ресурс
        public LocalizedString this[string name]
        {
            get
            {
                var currentCulture = CultureInfo.CurrentUICulture;
                string val = "";
                if (resources.ContainsKey(currentCulture.Name))
                {
                    if (resources[currentCulture.Name].ContainsKey(name))
                    {
                        val = resources[currentCulture.Name][name];
                    }
                }
                return new LocalizedString(name, val);
            }
        }

        public LocalizedString this[string name, params object[] arguments] => throw new NotImplementedException();

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            return this;
        }
    }
}
