using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogApi.Domain
{
    public class CatalogItem
    {
        //Creating a property  for the Id of an item
        public int Id { get; set; }

        //Creating a property for the name of an item
        public string Name { get; set; }

        //Creating a property for the price of an item
        public decimal Price { get; set; }

        //Creating a property for the description of an item
        public string Description { get; set; }

        //Creating a propety for the picture of an item
        public string PictureUrl { get; set; }

        //Creating foreign key for the id because each table is having the id and type in the schema of the data base 
        public int CatalogTypeId { get; set; }

        //Creating foreign key for the id because each table is having the id and type in the schema of the data base 
        public int CatalogBrandId { get; set; }

        // this is like a composition(has a relation) catalog type we will be using here means to navigate from here to there we use 
        //virtual catalalog type
        public virtual CatalogType  CatalogType { get; set; }

        // this is like a composition(has a relation) catalog brand we will be using here means to navigate from here to there we use 
        //virtual catalalog brand
        public virtual CatalogBrand CatalogBrand { get; set; }



    }
}
