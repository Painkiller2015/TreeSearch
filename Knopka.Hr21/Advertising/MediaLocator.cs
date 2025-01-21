using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Knopka.Hr21.Advertising
{
    // В конструктор передаются данные о рекламоносителях и локациях.
    // ===== пример данных =====
    // Яндекс.Директ:/ru
    // Бегущая строка в троллейбусах Екатеринбурга:/ru/svrd/ekb
    // Быстрый курьер:/ru/svrd/ekb
    // Ревдинский рабочий:/ru/svrd/revda,/ru/svrd/pervik
    // Газета уральских москвичей:/ru/msk,/ru/permobl/,/ru/chelobl
    // ===== конец примера данных =====
    // inputStream будет уничтожен после вызова конструктора.
    public class MediaLocator
    {
        Tree tree = new();
        public MediaLocator(Stream inputStream)
        {
            using (StreamReader sr = new(inputStream))
            {
                string line;
                string media;
                string[] locations;


                List<string> separLocations = new();

                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    int separatorPos = line.IndexOf(':');

                    media = line.Substring(0, separatorPos);
                    locations = line.Substring(separatorPos + 1).Split(",");

                    foreach (var location in locations)
                    {
                        separLocations = location.Split("/")[1..].ToList();
                        tree.InsertChield(separLocations, media);
                    }
                }
            }
        }

        // В метод передаётся локация.
        // Надо вернуть все рекламоносители, которые действуют в этой локации.
        // Например, GetMediasForLocation("/ru/svrd/pervik") должен вернуть две строки:
        // "Яндекс.Директ", "Ревдинский рабочий"
        // Порядок строк не имеет значения.
        public IEnumerable<string> GetMediasForLocation(string location)
        {
            List<string> separLocals = location.Split("/")[1..].ToList();
            var result = tree.FindMedia(separLocals);
            return result;

        }
    }
    class Tree()
    {
        public string nameLocation = "";
        List<Tree> trees = new();
        List<string> medias = new();

        public void InsertChield(List<string> locations, string media)
        {
            if (medias.Any(el => el == media))
            {
                return;
            }

            for (int i = 0; i < locations.Count(); i++)
            {
                var location = locations[i];

                if (nameLocation == location)
                {
                    if (locations.Count > 1)
                    {
                        locations.Remove(location);
                        i--;
                    }
                    else
                    {
                        if (!medias.Any(el => el == media))
                        {
                            medias.Add(media);
                        }
                    }
                    continue;
                }

                if (trees.Any(el => el.nameLocation == location))
                {
                    var chield = trees.First(el => el.nameLocation == location);
                    chield.InsertChield(locations, media);
                }
                else
                {
                    var newChield = new Tree()
                    {
                        nameLocation = location
                    };
                    trees.Add(newChield);
                    newChield.InsertChield(locations, media);
                }
            }
        }
        public List<string> FindMedia(List<string> separLocations)
        {
            List<string> resultMedais = new();
            string currLocation = separLocations[0];

            if (trees.Any(el => el.nameLocation == currLocation))
            {
                var chield = trees.First(el => el.nameLocation == currLocation);
                if (separLocations.Count() > 1)
                {
                    separLocations.RemoveAt(0);
                    resultMedais.AddRange(chield.medias);
                    resultMedais.AddRange(chield.FindMedia(separLocations));

                }
                else
                {
                    resultMedais.AddRange(chield.medias);
                }
            }
            return resultMedais;
        }
    }
}