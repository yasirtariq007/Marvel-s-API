using MarvelCharacters;
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
        private const string PrivateKey = "13afd975425abdaf29a013f26ef12789ed536c06";
        private const string PublicKey = "4b76a06259953851972ad8977efec731";
        private const string NoImagePath = "http://i.annihil.us/u/prod/marvel/i/mg/b/40/image_not_available";
        //private const int MaxCharacters = 1500;

        public static async Task FindMarvelCharactersAsync(ObservableCollection<Character> marvelheroes)//method to get characters from marvels
        {
            //await can only be used in async methods
            var getCharacter = await CharacterDataWrapperAsync();// wait to get the character

            var heroes = getCharacter.data.results;// give the results of the characters

            foreach (var person in heroes)
            {

                if (person.thumbnail != null && person.thumbnail.path != "" && person.thumbnail.path != NoImagePath)//filtering those characters 
                    //whose images are not found
                {
                    person.thumbnail.small = String.Format("{0}/standard_small.{1}", person.thumbnail.path, person.thumbnail.extension);

                    person.thumbnail.large = String.Format("{0}/portrait_xlarge.{1}", person.thumbnail.path, person.thumbnail.extension);

                    marvelheroes.Add(person);// adding all characters in marvelheroes
                }
            }

        }

        private static async Task<CharacterDataWrapper> CharacterDataWrapperAsync()// getting all the characters from json and
                                                                                      // deserialize it (making objects) into our classes 
                                                                                      //its also an awaitable task thats why 
                                                                                      // it uses async
        {
            Random random = new Random();// create the object for the offset
            var offset = random.Next(1500);//1500 is the maximum number of characters in the database, and random.Next will return a non neg random number
            //within the range of 1500

            // Getting the MD5 Hash
            var ts = DateTime.Now.Ticks.ToString();
            var hash = Hash(ts);

            string url = String.Format("http://gateway.marvel.com:80/v1/public/characters?limit=10&offset={0}&apikey={1}&ts={2}&hash={3}"
                                        , offset, PublicKey, ts, hash);// Marvels API url

            // Call out to Marvel
            HttpClient http = new HttpClient();
            var resp = await http.GetAsync(url);// passing in the url request so we can get the response back
            var msg = await resp.Content.ReadAsStringAsync();

            // Response -> string / json -> deserialize
            var ser = new DataContractJsonSerializer(typeof(CharacterDataWrapper));// deserialization of the json format
            var memory = new MemoryStream(Encoding.UTF8.GetBytes(msg));// it will pile in all the information about json 
                                                                      //and give it to serializer to deserialize it

            var result = (CharacterDataWrapper)ser.ReadObject(memory);//reading the serializable objects
            return result;
        }

        private static string Hash(string ts)// creating hash as it is required to use Marvels API
        {

            var compute_hash = ts + PrivateKey + PublicKey;
            var final_hash = CreateMD5(compute_hash);
            return final_hash;
        }

        private static string CreateMD5(string s)// creating MD5 for hashing. Thanks to stackoverflow to help me out with that
                                                   // http://stackoverflow.com/questions/8299142/how-to-generate-md5-hash-code-for-my-winrt-app-using-c
        {
            var alg = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            IBuffer buff = CryptographicBuffer.ConvertStringToBinary(s, BinaryStringEncoding.Utf8);
            var hashed = alg.HashData(buff);
            var final_result = CryptographicBuffer.EncodeToHexString(hashed);
            return final_result;
        }

    }
}