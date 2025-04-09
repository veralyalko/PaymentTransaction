using System;
namespace PaymentTransaction.Attributes
{
  
  [AttributeUsage(AttributeTargets.Method)]
  public class RequiresIdempotencyKeyHeaderAttribute : Attribute
  {
  }
}