using System;
using System.Linq;
using System.Text;
using System.IO;

namespace HTMLGen
{
    class Program
    {
      
        static void Main(string[] args)
        {            
            InputFile();           
        }



        /// <summary>
        /// Метод для импортирования файлов
        /// </summary>
        private static void InputFile ()
        {

            string infileTextName = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Text.txt";
            string infileDictName = AppDomain.CurrentDomain.BaseDirectory.ToString() + "Dict.txt";
            int N = 1000;
            

            Console.WriteLine("Введите N (Введите \"0\", если хотите оставить значение по умолчанию - 1000)");
            string line = Console.ReadLine();
            int value;
            if (int.TryParse(line, out value))
            {
                if (value != 0)
                    N = value;
            }
            else
            {
                Console.WriteLine("Вы ввели не число.Значение останется по умолчанию.");
            }


            Console.WriteLine("Введите название файла с текстом без указания формата файла "+
                              "(Введите \"0\", если хотите оставить значение по умолчанию Text)");
            line = Console.ReadLine();
            if (line != "0")
                infileTextName = AppDomain.CurrentDomain.BaseDirectory.ToString() + line + ".txt";

            Console.WriteLine("Введите название файла с словарем без указания формата файла "+
                              "(Введите \"0\", если хотите оставить значение по умолчанию Dict)");
            line = Console.ReadLine();
            if (line != "0")
                infileDictName = AppDomain.CurrentDomain.BaseDirectory.ToString() +line+ ".txt";


            if (!File.Exists(infileDictName))
            {
                Console.WriteLine("Файл словаря не найден");
                Console.ReadKey();
                return;
            }

            if (!File.Exists(infileTextName))
            {
                Console.WriteLine("Файл с текстом не найден");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Файлы загружены.Началась обработка.Пожалуйста подождите");
            CreateHtML(infileDictName, infileTextName, N);
            Console.WriteLine("Готово.Весь текст был обработан.");
            Console.ReadKey();

        }



        /// <summary>
        /// Метод гененрирования html файла из текстового файла
        /// </summary>
        /// <param name="fileDictName">Путь до файла словаря</param>
        /// <param name="fileTextName">Путь до текстового файла</param>
        /// <param name="N">Количество строк в файле</param>

        public static void CreateHtML(string fileDictName, string fileTextName,int N)
        {
            string fileHtmL = "";
            string head = @"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01//EN"" ""http://www.w3.org/TR/html4/strict.dtd""><html> <head><title>Носов А.А. Курсы C#</title></head> <body>";
            int colStrok = 0;
            int nameFile = 1;

            //воруем из файла тект
            string[] words = File.ReadAllLines(fileDictName, Encoding.GetEncoding("windows-1251"));
            StreamReader osnText = new StreamReader(fileTextName, Encoding.GetEncoding("windows-1251"));

            // читам данные пока могем и умеем
            while (!osnText.EndOfStream)
            {
                string[] linewords = osnText.ReadLine().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                for (int i = 0; i < linewords.Length; i++)
                {
                    if (words.Contains(linewords[i]))
                        fileHtmL += @"<b><i>" + linewords[i] + "</i></b>" + " ";
                    else
                        fileHtmL += linewords[i] + " ";
                }

                if (N - colStrok <= 0)  // если количество обработанных строк достигло количства в настройках.Отрицательное значение если в тексте еще не было точек продолжить поиск в следующем.
                {

                    int razdel = fileHtmL.LastIndexOf("."); //ищем последнию точку
                    if (razdel > 0)                         //если в тексте встречалась точка
                    {
                        string tempik = string.Copy(fileHtmL); //Создаем временную переменную в которую кидаем остатки после точки
                        tempik = tempik.Remove(0, razdel);
                        fileHtmL = head + fileHtmL.Remove(razdel + 1);
                        File.WriteAllText("File" + nameFile + ".html", fileHtmL, Encoding.Default);
                        Console.WriteLine("Файл " + nameFile + " создан.Началось создание следующего файла.");
                        nameFile++;
                        //обнуляем параметры
                        fileHtmL = "";
                        tempik = "";
                        colStrok = 0;
                    }
                }
                else
                {
                    colStrok++;
                }
            }
        }
    }
}
