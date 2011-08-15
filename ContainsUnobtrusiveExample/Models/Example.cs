/// <summary>
/// The example's model
/// </summary>
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Collections.Generic;
public class Example
{
    [Required(ErrorMessage="First is required")]
    [Contains("magic", ErrorMessage="First must contain the word 'magic'")]
    public string First { get; set; }

    [Contains("foo", ErrorMessage = "Second must contain the word 'foo'")]
    public string Second { get; set; }

    [Contains("any", ErrorMessage = "Third must contain the word 'any'")]
    public object Third { get; set; } // Third will fail validation if bound to integer, for instance.
}

/// <summary>
/// The attribute
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
public sealed class ContainsAttribute : ValidationAttribute
{
    readonly string _contain;
    public ContainsAttribute(string contain)
    {
        this._contain = contain;
    }

    public string Part
    {
        get { return _contain; }
    }

    public override bool IsValid(object value)
    {
        if (value == null) return true; // RequiredAttribute will check for null.
        return value is string && System.Convert.ToString(value).Contains(_contain);
    }
}

/// <summary>
/// The Adapter
/// </summary>
public class ContainsAttributeAdapter : DataAnnotationsModelValidator<ContainsAttribute>
{
    public ContainsAttributeAdapter(ModelMetadata metadata, ControllerContext context, ContainsAttribute attribute)
        : base(metadata, context, attribute) { }

    public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
    {
        // Here, I'm simplifying things a little. 
        // We just need to set a type name and error message, no need for a new type definition.

        // Example output: 
        // <input class="text-box single-line" data-val="true" 
        // data-val-contains="First must contain the word &amp;#39;magic&amp;#39;" 
        // data-val-contains-word="magic" data-val-required="First is required" 
        // id="First" name="First" type="text" value="">
        var rule = new ModelClientValidationRule()
            {
                ValidationType = "contains",
                ErrorMessage = Attribute.ErrorMessage
            };
        rule.ValidationParameters["word"] = Attribute.Part;
        return new[] { rule };
    }
}