using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Models
{
    public class OrderingDomainException : Exception
    {
        //there are two ways to create an exception
        //this means we are not going to get any message
        public OrderingDomainException()
        {

        }

        //with message what the exception is and then passed on to the base class
        public OrderingDomainException(string message) : base(message)
        {

        }
        //which is the message and anything else going on inside inner exception interns it calls the base class
        public OrderingDomainException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
