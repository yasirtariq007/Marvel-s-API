using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarvelCharacters
{
    public class Thumbnail
    {
        public string path { get; set; }
        public string extension { get; set; }
    }

    public class ItemsInComic
    {
        public string resourceURI { get; set; }
        public string name { get; set; }
    }

    public class Comics
    {
        public int available { get; set; }
        public string collectionURI { get; set; }
        public List<ItemsInComic> items { get; set; }
        public int returned { get; set; }
    }

    public class ItemsInSeries
    {
        public string resourceURI { get; set; }
        public string name { get; set; }
    }

    public class Series
    {
        public int available { get; set; }
        public string collectionURI { get; set; }
        public List<ItemsInSeries> items { get; set; }
        public int returned { get; set; }
    }

    public class ItemsInStories
    {
        public string resourceURI { get; set; }
        public string name { get; set; }
        public string type { get; set; }
    }

    public class Stories
    {
        public int available { get; set; }
        public string collectionURI { get; set; }
        public List<ItemsInStories> items { get; set; }
        public int returned { get; set; }
    }

    public class ItemsInEvents
    {
        public string resourceURI { get; set; }
        public string name { get; set; }
    }

    public class Events
    {
        public int available { get; set; }
        public string collectionURI { get; set; }
        public List<ItemsInEvents> items { get; set; }
        public int returned { get; set; }
    }

    public class Url
    {
        public string type { get; set; }
        public string url { get; set; }
    }

    public class Character // using the same objects as Marvel's API using
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime modified { get; set; }
        public Thumbnail thumbnail { get; set; }
        public string resourceURI { get; set; }
        public Comics comics { get; set; }
        public Series series { get; set; }
        public Stories stories { get; set; }
        public Events events { get; set; }
        public List<Url> urls { get; set; }
    }

    public class CharacterDataContainer // using the same objects as Marvel's API using
    {
        public int offset { get; set; }
        public int limit { get; set; }
        public int total { get; set; }
        public int count { get; set; }
        public List<Character> results { get; set; }
    }

    public class CharacterDataWrapper // using the same objects as Marvel's API using
    {
        public int code { get; set; }
        public string status { get; set; }
        public string copyright { get; set; }
        public string attributionText { get; set; }
        public string attributionHTML { get; set; }
        public string etag { get; set; }
        public CharacterDataContainer data { get; set; }
    }
}
