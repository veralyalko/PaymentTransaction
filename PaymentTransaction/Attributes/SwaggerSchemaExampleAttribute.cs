namespace PaymentTransaction.Attributes
{
  
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class SwaggerSchemaExampleAttribute : Attribute
    {
        public object Example { get; }
        public SwaggerSchemaExampleAttribute(object example)
        {
            Example = example;
        }
    }
}