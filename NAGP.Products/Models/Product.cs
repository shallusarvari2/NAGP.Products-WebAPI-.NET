using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Runtime.Serialization;

namespace NAGP.Products
{
    [Serializable]
    public class Product
    {
        [DataMember]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id;

        [DataMember]
        public string name;

        [DataMember]
        public string type;

        [DataMember]
        public double cost;

        [DataMember]
        public int discount;

        [DataMember]
        public bool isIncludedInOfferList;

        [DataMember]
        public bool isOutOfStock;
    }
}