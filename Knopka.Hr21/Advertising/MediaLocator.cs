using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Knopka.Hr21.Advertising
{
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
        public IEnumerable<string> GetMediasForLocation(string location)
        {
            List<string> separLocals = location.Split("/")[1..].ToList();
            var result = tree.FindMedia(separLocals);
            return result;

        }
    }   
}