using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;


namespace LineGram
{
    class Execute : Form1
    {
        private static string[] flwrs = new string[1024];
        private static string[] flwng = new string[1024];
        private static string[] result = new string[1024];

        public static int followersValue = 0;
        public static int followValue = 0;

        internal protected static bool IsLogged(WebBrowser webBrowser1)
        {
            try
            {
                HtmlElementCollection buttons = webBrowser1.Document.GetElementsByTagName("a");
                bool result = false;
                foreach (HtmlElement button in buttons)
                {
                    if (button.InnerText != null)
                    {
                        if (button.InnerText.Contains("Profile") || button.InnerText.Contains("Профиль"))
                        {
                            result = true;
                            break;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        } // Проверка на авторизацию
        internal protected static bool IsOnProfilePage(WebBrowser webBrowser1)
        {
            try
            {
                HtmlElementCollection headers = webBrowser1.Document.GetElementsByTagName("h1");
                bool result = false;
                foreach (HtmlElement header in headers)
                {
                    if (header.InnerText != null)
                    {
                        if (header.OuterHtml.Contains("AC5d8 notranslate"))
                        {
                            result = true;
                            break;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        } // Проверка на открытую страницу
        private static string GetPageName(WebBrowser webBrowser1)
        {
            string name = "";
            try
            {
                HtmlElementCollection headers = webBrowser1.Document.GetElementsByTagName("h1");
                foreach (HtmlElement header in headers)
                {
                    if (header.InnerText != null)
                    {
                        if (header.OuterHtml.Contains("AC5d8 notranslate"))
                        {
                            name = header.InnerText;
                            break;
                        }
                    }
                }
                Console.WriteLine(name);
                return name;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "";
            }
        } // Возвращает имя профиля для генерации ссылки
        private static void AnalysePage(WebBrowser webBrowser1) // Парсит числовые значение подписчиков и подписок (с конвертацией в Int32)
        {
            HtmlElementCollection elements = webBrowser1.Document.GetElementsByTagName("a");
            foreach (HtmlElement element in elements)
            {
                if(element.InnerText != null)
                {
                    if (element.InnerText.Contains(" подписчиков"))
                    {
                        followersValue = Convert.ToInt32(Regex.Replace(element.InnerText, @"[^\d]+", ""));
                    }
                    if (element.InnerText.Contains("Подписки:"))
                    {
                        followValue = Convert.ToInt32(Regex.Replace(element.InnerText, @"[^\d]+", ""));
                    }
                }
            }
        }
        private static void LoadHref(WebBrowser webBrowser1, string link)
        {
            // followers link = "/followers/"
            // followings link = "/following/"
            string name = GetPageName(webBrowser1);
            var aLinks = webBrowser1.Document.GetElementsByTagName("a");
            string userFollowers = "/" + name + link;
            foreach (HtmlElement alink in aLinks)
            {
                if (alink.GetAttribute("href") == userFollowers)
                {
                    alink.InvokeMember("click");
                }
            }
            
        } // Находит на странице href-ссылки на модальные окна с подписчиками/подписками
        private static HtmlElement FocusOnModal(WebBrowser webBrowser1)
        {
            HtmlElementCollection modals = webBrowser1.Document.GetElementsByTagName("ul");
            HtmlElement result = null;
            foreach (HtmlElement modal in modals)
            {
                if (modal.OuterHtml.ToString().Contains("jSC57  _6xe7A"))
                {
                    modal.Focus();
                    result = modal;
                }
            }
            return result;
        } // Метод фокусировки на диалоговом окне для дальнейшей прокрутки
        private static void CloseModal(WebBrowser webBrowser1)
        {
            HtmlElementCollection exitElemets = webBrowser1.Document.GetElementsByTagName("span");
            foreach (HtmlElement exit in exitElemets)
            {
                if(exit.OuterHtml.ToString().Contains("glyphsSpriteX__outline__24__grey_9 u-__7"))
                {
                    exit.InvokeMember("click");
                }
            }
        } // Метод закрытия диалогового окна
        private static int GetUsersValue(WebBrowser webBrowser1, string container, string[] usersArray)
        {
            // followers container = FPmhX
            // followings container = "FPmhX notranslate _0imsa "
            HtmlElementCollection users = webBrowser1.Document.GetElementsByTagName("a");
            int counter = 0;

            foreach (HtmlElement user in users)
            {
                if (user.OuterHtml.ToString().Contains(container))
                {
                    usersArray[counter] = user.InnerText;
                    counter += 1;
                }
            }
            return counter;
        } // Возвращает число подписчиков/подписок в зависимости от заданого div'a
        private static void SetCurrentUserList(WebBrowser webBrowser1, string container, string[] usersArray) // Заполняет массивы именами пользователей для дальнейшей обработки

        {
            // followers container = FPmhX
            // followings container = "FPmhX notranslate _0imsa "
            HtmlElementCollection users = webBrowser1.Document.GetElementsByTagName("a");
            int counter = 0;

            foreach (HtmlElement user in users)
            {
                if (user.OuterHtml.ToString().Contains(container))
                {
                    usersArray[counter] = user.InnerText;
                    counter += 1;
                }
            }
        }
        private static void Compare(WebBrowser webBrowser1) 
        {
            ResultForm resultForm = new ResultForm();
            int counter = flwrs.Length;
            int p_counter = 0;
            for (int i = 0; i < counter; i++)
            {
                if (flwrs.Contains(flwng[i]) == false)
                {
                    result[p_counter] = flwng[i];
                    resultForm.listBox1.Items.Add(result[p_counter]);
                    p_counter += 1;
                }
            }
            resultForm.Show();
        } // Сравнивает списки и выводит новое окно с результатом
        public async static void ProcessPage(WebBrowser webBrowser1)
        {
            AnalysePage(webBrowser1);
            await Task.Delay(3000);
            LoadHref(webBrowser1, "/followers/");
            await Task.Delay(3000);
            HtmlElement modal = FocusOnModal(webBrowser1);
            int currentFollowers = 0;
            while (currentFollowers < followersValue)
            {
                try
                {
                    await Task.Delay(1000);
                    currentFollowers = GetUsersValue(webBrowser1, "FPmhX", flwrs);
                    modal.ScrollIntoView(false);
                }
                catch
                {
                    break;
                }
            }
            CloseModal(webBrowser1);

            await Task.Delay(3000);
            LoadHref(webBrowser1, "/following/");
            await Task.Delay(3000);
            modal = FocusOnModal(webBrowser1);
            int currentFollowing = 0;
            while (currentFollowing < followValue)
            {
                try
                {
                    await Task.Delay(500);
                    currentFollowing = GetUsersValue(webBrowser1, "FPmhX notranslate _0imsa ", flwng);
                    modal.ScrollIntoView(false);
                }
                catch
                {
                    break;
                }
            }
            CloseModal(webBrowser1);
            Compare(webBrowser1);
        } // Основной метод
    }
}
