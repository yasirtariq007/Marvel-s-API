using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace MarvelCharacters
{
    public class DesignPattern
    {
        private const string PrivateKey = "111e7db13ff145edd81a48c3352809d92fe760e5";
        private const string PublicKey = "6db41c01b00655c94177492af2cf5c28";
        private const string NoImagePath = "http://i.annihil.us/u/prod/marvel/i/mg/b/40/image_not_available";

        public static async Task FindMarvelCharactersAsync(ObservableCollection<Character> marvelheores)
        {
            var characterDataWrapper = await GetCharacterDataWrapperAsync();// wait to get the character
            var heroes = characterDataWrapper.data.results;// give the results of the characters

            foreach (var person in heroes)
            {

                if (person.thumbnail != null && person.thumbnail.path != "" && person.thumbnail.path != NoImagePath)//filtering those characters whose images are not found
                {
                    person.thumbnail.small = String.Format("{0}/standard_small.{1}", person.thumbnail.path, person.thumbnail.extension);
                    person.thumbnail.large = String.Format("{0}/portrait_xlarge.{1}", person.thumbnail.path, person.thumbnail.extension);
                    marvelheores.Add(person);// adding all characters in marvelheroes
                }
            }
        }
        private static async Task<CharacterDataWrapper> GetCharacterDataWrapperAsync() // getting all the characters from json and
                                          // deserialize it (making objects) into our classes its also an awaitable task thats why 
                                          // i use async
        {
            Random random = new Random();// create the object for the offset
            var offset = random.Next(1500);

            var ts = DateTime.Now.Ticks.ToString();
            var hash = Hash(ts);

            string url = String.Format("https://gateway.marvel.com:443/v1/public/characters?orderBy=name&limit=10&offset={0}&apikey={1}&ts{2}&hash{3}", offset, PublicKey, ts, hash); // Marvels API url

            HttpClient http = new HttpClient();
            var resp = await http.GetAsync(url); // passing in the url request so we can get the response back

            var msg = await resp.Content.ReadAsStringAsync();// read as a string

            var ser = new DataContractJsonSerializer(typeof(CharacterDataWrapper));// deserialization of the json format
            var memory = new MemoryStream(Encoding.UTF8.GetBytes(msg));// it will pile in all the information about json and give it to serializer to deserialize it

            var final_result = (CharacterDataWrapper)ser.ReadObject(memory);// reading the serializable objects
            return final_result;

        }

        private static string Hash(string ts) // creating hash as it is required to use Marvels API
        {
            var compute_hash = ts + PrivateKey + PublicKey;
            var final_hash = CreateMD5(compute_hash);
            return final_hash;
        }

        private static string CreateMD5(string s) // creating MD5 for hashing. Thanks to stackoverflow to help me out with that
        {
            var alg = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            IBuffer buff = CryptographicBuffer.ConvertStringToBinary(s, BinaryStringEncoding.Utf8);
            var hashed = alg.HashData(buff);
            var result = CryptographicBuffer.EncodeToHexString(hashed);
            return result;
        }


    }
}
