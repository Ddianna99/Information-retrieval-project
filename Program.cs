using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab1_IR
{
    static class Program
    {
        //int[] frecventa;
        
        public static string StripHTML(string input)
        {
            //return Regex.Replace(input, "<.*?>", String.Empty);
            // Will this simple expression replace all tags???
            var tagsExpression = new Regex(@"</?.+?>");
            return tagsExpression.Replace(input, " ");
        }
        public static bool stopwords(string value)  
        {
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Diana\Desktop\stop_words_english.txt");
            foreach (string line in lines)
            {
                if (value == line)
                    return true;
            }
            return false;
        }
        public static void Append<K, V>(this Dictionary<K, V> first, Dictionary<K, V> second)
        {
            foreach (KeyValuePair<K, V> item in second)
            {
                first[item.Key] = item.Value;
            }
        }

        public static void Main(string[] args)
        {
            List<int> frecventa = new List<int>();
            List<string> elemente = new List<string>();
            List<string> list = new List<string>() { "countries", "industries", "topics" };
            String[] separator = { " ", ":", ",",">","<",".", "  "};
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            Dictionary<string, int> globalDictionary = new Dictionary<string, int>();
            string path = "C:\\Users\\Diana\\Desktop\\data.txt";
            List<string> listString = new List<string>();
            List<string> listAllString = new List<string>();
            List<string> listDir = new List<string>();
            List<string> dataForWrite = new List<string>();

            try
            {
                string[] dirs = Directory.GetFiles(@"C:\Facultate\IR\Reuters_34", "*" +
                    ".xml");
                Console.WriteLine("Numarul de fisiere: " + dirs.Length);

                foreach (string dir in dirs)
                {
                    listDir.Add(dir);
                    Console.WriteLine(dir);
                    dataForWrite.Add(dir);
                    string text = File.ReadAllText(dir);
                    string textUpdate = StripHTML(text);
                    string[] separators = new string[] { ",", ".", "!", "\'", " ", "\'s", "  ", ":"};

                    foreach (string word in textUpdate.Split(separators, StringSplitOptions.RemoveEmptyEntries)) {
                        if (stopwords(word) == false)
                        {
                            string wordUpdate = word.ToLower();
                            var txt = new PorterStemmer();
                            string stemWord = txt.stemTerm(wordUpdate);
                            if (dictionary.ContainsKey(stemWord))
                            {
                                dictionary[stemWord]++;
                            }
                            else
                            {
                                if (stopwords(stemWord) == false)
                                {
                                    dictionary.Add(stemWord, 1);
                                }
                                
                            }
                        }
                    }
                    if (dictionary != null)
                    {
                        foreach (var item in dictionary)
                        {
                            if (item.Key == " ")
                            {
                                dictionary.Remove(item.Key);
                            }
                            else
                            {

                                Console.WriteLine("Key " + item.Key + " Value " + item.Value);
                                string data = item.Key + " " + item.Value;
                                dataForWrite.Add(data);
                            }
                        }
                        
                    }

                    string[] words = text.Split(':');
                    foreach (var word in words)
                    {
                        if (list.Contains(word))
                        {
                            Console.WriteLine("#" + word);
                            dataForWrite.Add("#" + word);
                        }
                        

                    }
                    //dataForWrite.Add(System.Environment.NewLine);

                    try
                    {
                        //Pass the filepath and filename to the StreamWriter Constructor
                        StreamWriter sw = new StreamWriter(path);
                        foreach (string dataW in dataForWrite)
                        {
                            sw.WriteLine(dataW);
                        }
                        //Close the file.
                        sw.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception: " + e.Message);
                    }
                    finally
                    {
                        Console.WriteLine("Executing finally block.");
                    }


                    // Console.ReadLine();
                    foreach (string key in dictionary.Keys)
                    {
                        if (!listString.Contains(key))
                        {
                            listString.Add(key);
                        }
                    }
                    listAllString.AddRange(listString);

                }
                

            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }
    }
}
