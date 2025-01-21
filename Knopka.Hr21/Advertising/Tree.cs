using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knopka.Hr21.Advertising
{
    public class Tree
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
