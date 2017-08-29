using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace NAGP.Products.Controllers
{
    public class ProductController : ApiController
    {
        /// <summary>
        /// The mongo database
        /// </summary>
        private IMongoDatabase database;

        public ProductController()
        {
            var mongoClient = new MongoClient(ConfigurationManager.AppSettings["MongoDBConectionString"]);
            this.database = mongoClient.GetDatabase("test");          
        }

        // GET: api/Product
        public IEnumerable<Product> Get()
        {
            var collection = this.database.GetCollection<BsonDocument>("products");

            var documents = collection.Find(Builders<BsonDocument>.Filter.Eq("isIncludedInOfferList", true)).ToList();

            IList<BsonDocument> docList = new List<BsonDocument>();

            if (documents != null)
            {
                docList = documents.ToList();
            }

            IList<Product> products = new List<Product>();
            foreach (var doc in docList)
            {
                products.Add(BsonSerializer.Deserialize<Product>(doc));
            }

            return products;
        }

        public IEnumerable<Product> GetSearchedProducts(string searchText)
        {
            var collection = this.database.GetCollection<BsonDocument>("products");
            searchText = searchText == null ? "" : searchText;
            var documents = collection.Find(Builders<BsonDocument>.Filter.Regex("name", searchText));
            IList<BsonDocument> docList = new List<BsonDocument>();

            if(documents != null)
            {
                docList = documents.ToList();
            }

            IList<Product> products = new List<Product>();
            foreach (var doc in docList)
            {
                products.Add(BsonSerializer.Deserialize<Product>(doc));
            }
            

            return products;
        }


        // GET: api/Product/5
        public Product Get(string productId)
        {
            var collection = this.database.GetCollection<BsonDocument>("products");

            var document = collection.Find(Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(productId))).ToList().First();

            var product = BsonSerializer.Deserialize<Product>(document);

            return product;

        }

        // POST: api/Product
        [HttpPost]
        public void Post(Product product)
        {
            var collection = this.database.GetCollection<BsonDocument>("products");

            var doc = new BsonDocument
            {
                {"name" ,product.name },
                { "type" ,  product.type },
                {"cost", product.cost },
                {"discount", product.discount },
                {"isIncludedInOfferList", product.isIncludedInOfferList },
                { "isOutOfStock", product.isOutOfStock}
            };

            collection.InsertOneAsync(doc).Wait();
        }

        // PUT: api/Product/5
        public void Put(string id, [FromBody]Product product)
        {
            var updatedDocument = new BsonDocument
            {
                {"name" ,product.name },
                { "type" ,  product.type },
                {"cost", product.cost },
                {"discount", product.discount },
                {"isIncludedInOfferList", product.isIncludedInOfferList },
                { "isOutOfStock", product.isOutOfStock}
            };

            var collection = this.database.GetCollection<BsonDocument>("products");

            var document = collection.Find(Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id))).ToList().First();

            collection.ReplaceOne(document, updatedDocument);
        }

        // DELETE: api/Product/5
        public void Delete(string id)
        {
            var collection = this.database.GetCollection<BsonDocument>("products");

            collection.DeleteOne(Builders<BsonDocument>.Filter.Eq("_id", id));
        }
    }
}
