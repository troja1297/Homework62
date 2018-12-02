using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication5.Models;

namespace WebApplication5
{
    public class DataLoader
    {
        private string Dir { get; set; }
        private string FileName { get; set; }

        public DataLoader(string dir, string filename)
        {
            this.Dir = dir;
            this.FileName = filename;
        }

        public void Save(List<Dish> dishes)
        {
            string json = JsonConvert.SerializeObject(dishes);
            SaveFile(json);
        }


        public List<Dish> Load()
        {
            try
            {
                string content = File.ReadAllText($"{Dir}/{FileName}");
                if (string.IsNullOrEmpty(content))
                {
                    throw new ApplicationException($"файл {FileName} пустой");
                }

                return JsonConvert.DeserializeObject<List<Dish>>(content);
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine($"Отсутствует директория {Dir}");
                return new List<Dish>();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Отсутствует файл {FileName}");
                return new List<Dish>();
            }
            catch (ApplicationException ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Dish>();
            }
        }

        private void SaveFile(string content)
        {

            try
            {
                File.WriteAllText($"{Dir}/{FileName}", content);
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(Dir);
                SaveFile(content);
            }
        }
    }
}
