using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Models
{
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //we want order id of an item
        public int Id { get; set; }

        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public decimal UnitPrice { get; set; }

        //this is basically quantity
        public int Units { get; set; }
        public int ProductId { get; private set; }

        protected OrderItem() { }
        //implementing order here
        public Order Order { get; set; }
        public int OrderId { get; set; }

        //constructor
        public OrderItem(int productId, string productName, decimal unitPrice, string pictureUrl, int units = 1)
        {
            if (units <= 0)
            {
                //exception
                throw new OrderingDomainException("Invalid number of units");
            }

            ProductId = productId;

            ProductName = productName;
            UnitPrice = unitPrice;

            Units = units;
            PictureUrl = pictureUrl;
        }
        //checking the picture
        public void SetPictureUri(string pictureUri)
        {
            //checkng the null value or white space is showing in the ui
            if (!String.IsNullOrWhiteSpace(pictureUri))
            {
                PictureUrl = pictureUri;
            }
        }





        public void AddUnits(int units)
        {
            if (units < 0)
            {
                throw new OrderingDomainException("Invalid units");
            }

            Units += units;
        }
    }
}

