using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

//Validation on Server side by kudvenkant https://www.youtube.com/watch?v=qopOqppDwc4
namespace ServiceManager.Models
{
    public class RemoteClientServerAttribute : RemoteAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var fields = validationContext.ObjectType.GetProperty(AdditionalFields);
            var fieldsValue = fields.GetValue(validationContext.ObjectInstance, null);


            Type controller = Assembly.GetExecutingAssembly().GetTypes()
                .FirstOrDefault(type => type.Name.ToLower() == string.Format("{0}Controller", this.RouteData["controller"].ToString()).ToLower());

            if (controller != null)
            {
                MethodInfo action = controller.GetMethods().FirstOrDefault(method => method.Name.ToLower() == this.RouteData["action"].ToString().ToLower());
                if (action != null)
                {
                    object instance = Activator.CreateInstance(controller);
                    object response = action.Invoke(instance, new object[] { value, fieldsValue });

                    if (response != null)
                    {
                        object jsonData = ((JsonResult)response).Data;
                        if (jsonData is bool)
                        {
                            return (bool)jsonData ? ValidationResult.Success : new ValidationResult(this.ErrorMessage);
                        }
                    }
                }
            }
            return new ValidationResult(this.ErrorMessage);
        }

        public RemoteClientServerAttribute(string routeName) : base(routeName)
        {

        }

        public RemoteClientServerAttribute(string action, string controller) : base(action, controller)
        {

        }

        public RemoteClientServerAttribute(string action, string controller, string areaName) : base(action, controller, areaName)
        {

        }
    }
}