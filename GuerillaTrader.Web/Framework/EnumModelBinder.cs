using System;
using System.Web.Mvc;

public class EnumModelBinder<T> : IModelBinder
where T : struct
{
    private readonly T defaultValue;
    private readonly bool hasDefaultValue;

    public EnumModelBinder(T defaultValue)
    {
        this.defaultValue = defaultValue;
        this.hasDefaultValue = true;
    }

    public EnumModelBinder()
    {
        this.hasDefaultValue = false;
    }

    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
        var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        var modelState = new ModelState() { Value = valueResult };

        object actualValue = null;

        if (valueResult == null && this.hasDefaultValue)
        {
            actualValue = this.defaultValue;
        }
        else if (valueResult == null)
        {
            modelState.Errors.Add("No default representation of enum " + typeof(T).Name + " could be found.");
        }
        else
        {
            string value = valueResult.AttemptedValue;
            T enumValue;

            if (Enum.TryParse<T>(value, out enumValue) && Enum.IsDefined(typeof(T), enumValue))
            {
                actualValue = enumValue;
            }
            else if (this.hasDefaultValue)
            {
                actualValue = this.defaultValue;
            }
            else
            {
                modelState.Errors.Add("Could not parse " + value +
                                      " as a valid numerical value of enum " + typeof(T).Name + ".");
            }
        }

        bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
        return actualValue;
    }
}
