using AutoMapper;
using LogisticsTrack.Domain.HelperModels;
using LogisticsTrack.Service.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Reflection; 

namespace LogisticsTrack.Service.Extensions
{


    public static class ValidationExtensions
    {
        private static IServiceProvider _serviceProvider;

        public static void Configure(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static List<ValidationError> Validate(this object validationObject)
        {
            var validationResults = new List<ValidationResult>();
            bool result = Validator.TryValidateObject(validationObject, new ValidationContext(validationObject, null, null), validationResults, true);
            var mapper = _serviceProvider.GetRequiredService<IMapper>();
            return validationResults
                   .Where(r => !string.IsNullOrWhiteSpace(r.ErrorMessage))
                   .Select(err => mapper.Map<ValidationError>(err))
                   .ToList();
        }

        public static void RejectIfInvalid(this object objectToCheck)
        {
            if (objectToCheck == null)
            {
                throw new InvalidModelException();
            }

            var errors = objectToCheck.Validate();
            if (errors.Any())
            {
                throw new InvalidModelException(errors);
            }
        }

        public static void RejectIfNotFound(this object objectToCheck)
        {
            if (objectToCheck == null)
            {
                throw new NotFoundException();
            }
        }

        public static void RejectUnauthorized<T>(this T objectToCheck, Func<T, bool> reason)
        {
            if (reason(objectToCheck))
            {
                throw new UnauthorizedException();
            }
        }

        [Obsolete("Causes stack overflow on a bit bigger object")]
        private static bool TryValidateObjectRecursive<T>(T obj, List<ValidationResult> results)
        {
            bool result = Validator.TryValidateObject(obj, new ValidationContext(obj, null, null), results, true);

            var properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(prop => prop.CanRead).ToList();

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string) || property.PropertyType.GetTypeInfo().IsValueType)
                {
                    continue;
                }

                var value = obj.GetPropertyValue(property.Name);

                if (value == null)
                {
                    continue;
                }

                var asEnumerable = value as IEnumerable;
                if (asEnumerable != null)
                {
                    foreach (var enumObj in asEnumerable)
                    {
                        var nestedResults = new List<ValidationResult>();
                        if (!TryValidateObjectRecursive(enumObj, nestedResults))
                        {
                            result = false;
                            foreach (var validationResult in nestedResults)
                            {
                                PropertyInfo property1 = property;
                                results.Add(new ValidationResult(validationResult.ErrorMessage, validationResult.MemberNames.Select(x => property1.Name + '.' + x)));
                            }
                        }
                    }
                }
                else
                {
                    var nestedResults = new List<ValidationResult>();
                    if (!TryValidateObjectRecursive(value, nestedResults))
                    {
                        result = false;
                        foreach (var validationResult in nestedResults)
                        {
                            PropertyInfo property1 = property;
                            results.Add(new ValidationResult(validationResult.ErrorMessage, validationResult.MemberNames.Select(x => property1.Name + '.' + x)));
                        }
                    }
                }
            }

            return result;
        }

        private static object GetPropertyValue(this object o, string propertyName)
        {
            object objValue = string.Empty;

            var propertyInfo = o.GetType().GetProperty(propertyName);
            if (propertyInfo != null)
            {
                objValue = propertyInfo.GetValue(o, null);
            }

            return objValue;
        }
    }
}
