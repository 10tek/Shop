/*
 * 1. Регистрация и вход (смс-код / email код) - сделать до 11 октября
 * 2. История покупок 
 * 3. Категории и товары (картинка в файловой системе)
 * 4. Покупка (Корзина), оплата и доставка (PayPal/Qiwi/etc)
 * 5. Комментарии и рейтинги 
 * 6. Поиск (пагинация)
 * 
 * Кто сделает 3 версии (Подключённый, автономный и EF) получит автомат на экзамене.
 * unit of work
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.UI
{
    public static class StringExtensions
    {
        public static string ExtractOnlyText(this string text)
        {
            var result = string.Empty;
            foreach (var character in text)
            {
                if (char.IsDigit(character))
                {
                    continue;
                }
                result += character;
            }


            return result;
        }
    }
}
