using System;
using StackExchange.Redis;
using System.Configuration;
using Newtonsoft.Json;

namespace WorkingWithRedis
{
    class Program
    {
        static void Main(string[] args)
        {

            IDatabase cache = lazyConnection.Value.GetDatabase();

            // writing data to redis cache
            Lesson newToAzure = new Lesson("001", "New To Azure");
            Console.WriteLine("Uploading first lesson : " +
                cache.StringSet("001", JsonConvert.SerializeObject(newToAzure)));

            // getting data from redis cache

            Lesson myLessons = JsonConvert.DeserializeObject<Lesson>(cache.StringGet("001"));
            Console.WriteLine("Your Lesson Details :\n");
            Console.WriteLine("\tLesson Name : " + myLessons.LessonName);
            Console.WriteLine("\tLesson Id   : " + myLessons.LessonId);

        }

        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            string cacheConnection = ConfigurationManager.AppSettings["CacheConnection"].ToString();
            return ConnectionMultiplexer.Connect(cacheConnection);
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

    }

    class Lesson
    {
        public string LessonId { get; set; }
        public string LessonName { get; set; }

        public Lesson(string LessonId, string LessonName)
        {
            this.LessonId = LessonId;
            this.LessonName = LessonName;
        }
    }
}
