using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.OData.Edm;
using System.Collections;
using System.Diagnostics.Contracts;
using System.Net;
using System.Reflection;
using TSE.Nexus.SDK.API.Models;
using UMBIT.Precatorios.SDK.API.Models;

namespace UMBIT.Precatorios.Core.API.Basicos.Atributos
{
    public class ODataQueryAttribute : EnableQueryAttribute
    {

        public override bool Match(object? obj)
        {
            return base.Match(obj);
        }
        public override bool IsDefaultAttribute()
        {
            return base.IsDefaultAttribute();
        }
        public override void OnResultExecuted(ResultExecutedContext context)
        {

            base.OnResultExecuted(context);
        }
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);
        }
        public override object ApplyQuery(object entity, ODataQueryOptions queryOptions)
        {
            return base.ApplyQuery(entity, queryOptions);
        }
        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext == null)
            {
                throw new ArgumentNullException(nameof(actionExecutedContext));
            }

            HttpRequest request = actionExecutedContext.HttpContext.Request;
            if (request == null)
            {
                throw new Exception("actionExecutedContext");
            }

            ActionDescriptor actionDescriptor = actionExecutedContext.ActionDescriptor;
            if (actionDescriptor == null)
            {
                throw new Exception("actionExecutedContext");
            }

            HttpResponse response = actionExecutedContext.HttpContext.Response;

            // Check is the response is set and successful.
            if (response != null && IsSuccessStatusCode(response.StatusCode) && actionExecutedContext.Result != null)
            {
                // actionExecutedContext.Result might also indicate a status code that has not yet
                // been applied to the result; make sure it's also successful.
                IStatusCodeActionResult statusCodeResult = actionExecutedContext.Result as IStatusCodeActionResult;
                if (statusCodeResult?.StatusCode == null || IsSuccessStatusCode(statusCodeResult.StatusCode.Value))
                {
                    ObjectResult responseContent = actionExecutedContext.Result as ObjectResult;

                    ControllerActionDescriptor controllerActionDescriptor = actionDescriptor as ControllerActionDescriptor;
                    Type returnType = controllerActionDescriptor.MethodInfo.ReturnType;

                    if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(ActionResult<>))
                    {
                        returnType = returnType.GetGenericArguments().First();

                        if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(IAsyncEnumerable<>))
                        {
                            responseContent.DeclaredType = returnType;
                        }
                    }

                    if (responseContent != null)
                    {
                        // Get collection from SingleResult.
                        IQueryable singleResultCollection = null;
                        SingleResult singleResult = responseContent.Value as SingleResult;
                        if (singleResult != null)
                        {
                            // This could be a SingleResult, which has the property Queryable.
                            // But it could be a SingleResult() or SingleResult<T>. Sort by number of parameters
                            // on the property and get the one with the most parameters.
                            PropertyInfo propInfo = responseContent.Value.GetType()
                                                                         .GetProperties()
                                                                         .OrderBy(p => p.GetIndexParameters().Length)
                                                                         .Where(p => p.Name.Equals("Queryable", StringComparison.Ordinal))
                                                                         .LastOrDefault();

                            singleResultCollection = propInfo.GetValue(singleResult) as IQueryable;
                        }

                        // Execution the action.
                        object queryResult = OnActionExecuted(
                            actionExecutedContext,
                            responseContent.Value,
                            singleResultCollection,
                            actionDescriptor as ControllerActionDescriptor,
                            request);

                        if (queryResult != null)
                        {
                            responseContent.Value = queryResult;
                        }
                    }
                }
            }
        }
        public override void OnActionExecuting(ActionExecutingContext actionExecutingContext)
        {
            base.OnActionExecuting(actionExecutingContext);
        }
        public override void ValidateQuery(HttpRequest request, ODataQueryOptions queryOptions)
        {
            base.ValidateQuery(request, queryOptions);
        }
        public override IQueryable ApplyQuery(IQueryable queryable, ODataQueryOptions queryOptions)
        {
            return base.ApplyQuery(queryable, queryOptions);
        }
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            return base.OnActionExecutionAsync(context, next);
        }
        public override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            return base.OnResultExecutionAsync(context, next);
        }
        public override IEdmModel GetModel(Type elementClrType, HttpRequest request, ActionDescriptor actionDescriptor)
        {
            return base.GetModel(elementClrType, request, actionDescriptor);
        }
        protected override ODataQueryOptions CreateQueryOptionsOnExecuting(ActionExecutingContext actionExecutingContext)
        {
            return base.CreateQueryOptionsOnExecuting(actionExecutingContext);
        }
        protected override ODataQueryOptions CreateAndValidateQueryOptions(HttpRequest request, ODataQueryContext queryContext)
        {
            return base.CreateAndValidateQueryOptions(request, queryContext);
        }

        private object OnActionExecuted(
            ActionExecutedContext actionExecutedContext,
            object responseValue,
            IQueryable singleResultCollection,
            ControllerActionDescriptor actionDescriptor,
            HttpRequest request)
        {

            // Apply the query if there are any query options, if there is a page size set, in the case of
            // SingleResult or in the case of $count request.
            bool shouldApplyQuery = responseValue != null &&
               request.GetEncodedUrl() != null &&
               (!string.IsNullOrWhiteSpace(request.QueryString.Value) ||
               singleResultCollection != null ||
               request.IsCountRequest());

            object returnValue = null;
            if (shouldApplyQuery)
            {
                try
                {
                    object queryResult = ExecuteQuery(responseValue, singleResultCollection, actionDescriptor, request);
                    if (queryResult == null && (request.ODataFeature().Path == null || singleResultCollection != null))
                    {
                        // This is the case in which a regular OData service uses the EnableQuery attribute.
                        // For OData services ODataNullValueMessageHandler should be plugged in for the service
                        // if this behavior is desired.
                        // For non OData services this behavior is equivalent as the one in the v3 version in order
                        // to reduce the friction when they decide to move to use the v4 EnableQueryAttribute.
                        actionExecutedContext.Result = new StatusCodeResult((int)HttpStatusCode.NotFound);
                    }

                    returnValue = queryResult;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return returnValue;
        }
        private object ExecuteQuery(
            object responseValue,
            IQueryable singleResultCollection,
            ControllerActionDescriptor actionDescriptor,
            HttpRequest request)
        {
            ODataQueryContext queryContext = GetODataQueryContext(responseValue, singleResultCollection, actionDescriptor, request);

            // Create and validate the query options.
            ODataQueryOptions queryOptions = CreateAndValidateQueryOptions(request, queryContext);
            ODataQueryOptions _queryOptions = new ODataQueryOptionsCustom(queryContext, request);


            if (responseValue is Resposta resposta)
            {
                // apply the query
                IEnumerable enumerableDados = resposta.Dados as IEnumerable;
                if (enumerableDados == null || responseValue is string || responseValue is byte[])
                {
                    // response is not a collection; we only support $select and $expand on single entities.
                    ValidateSelectExpandOnly(_queryOptions);

                    if (singleResultCollection == null)
                    {
                        // response is a single entity.
                        resposta.Dados = ApplyQuery(entity: responseValue, queryOptions: _queryOptions);
                    }
                    else
                    {
                        IQueryable queryable = singleResultCollection as IQueryable;
                        queryable = ApplyQuery(queryable, _queryOptions);
                        resposta.Dados = SingleOrDefault(queryable, actionDescriptor);
                    }
                }
                else
                {
                    // response is a collection.
                    IQueryable queryable = enumerableDados as IQueryable ?? enumerableDados.AsQueryable();
                    queryable = ApplyQuery(queryable, _queryOptions);

                    if (request.IsCountRequest())
                    {
                        long? count = request.ODataFeature().TotalCount;

                        if (count.HasValue)
                        {
                            // Return the count value if it is a $count request.
                            resposta.Dados = count.Value;
                        }
                    }

                    resposta.Dados = queryable;
                }

                return resposta;
            }

            // apply the query
            IEnumerable enumerable = responseValue as IEnumerable;
            if (enumerable == null || responseValue is string || responseValue is byte[])
            {
                // response is not a collection; we only support $select and $expand on single entities.
                ValidateSelectExpandOnly(_queryOptions);

                if (singleResultCollection == null)
                {
                    // response is a single entity.
                    return ApplyQuery(entity: responseValue, queryOptions: _queryOptions);
                }
                else
                {
                    IQueryable queryable = singleResultCollection as IQueryable;
                    queryable = ApplyQuery(queryable, _queryOptions);
                    return SingleOrDefault(queryable, actionDescriptor);
                }
            }
            else
            {
                // response is a collection.
                IQueryable queryable = enumerable as IQueryable ?? enumerable.AsQueryable();
                queryable = ApplyQuery(queryable, _queryOptions);

                if (request.IsCountRequest())
                {
                    long? count = request.ODataFeature().TotalCount;

                    if (count.HasValue)
                    {
                        // Return the count value if it is a $count request.
                        return count.Value;
                    }
                }

                return queryable;
            }

        }

        private static object SingleOrDefault(
            IQueryable queryable,
            ControllerActionDescriptor actionDescriptor)
        {
            var enumerator = queryable.GetEnumerator();
            try
            {
                var result = enumerator.MoveNext() ? enumerator.Current : null;

                if (enumerator.MoveNext())
                {

                    throw new Exception();
                }

                return result;
            }
            finally
            {
                // Ensure any active/open database objects that were created
                // iterating over the IQueryable object are properly closed.
                var disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }
        private ODataQueryContext GetODataQueryContext(
            object responseValue,
            IQueryable singleResultCollection,
            ControllerActionDescriptor actionDescriptor,
            HttpRequest request)
        {
            if (responseValue is Resposta resposta)
            {
                Type elementClrTypeRespota = GetElementType(resposta.Dados, singleResultCollection, actionDescriptor);

                IEdmModel modelDados = GetModel(elementClrTypeRespota, request, actionDescriptor);
                if (modelDados == null)
                {
                    throw new Exception();
                }

                return new ODataQueryContext(modelDados, elementClrTypeRespota, request.ODataFeature().Path);

            }

            Type elementClrType = GetElementType(responseValue, singleResultCollection, actionDescriptor);

            IEdmModel model = GetModel(elementClrType, request, actionDescriptor);
            if (model == null)
            {
                throw new Exception();
            }

            return new ODataQueryContext(model, elementClrType, request.ODataFeature().Path);
        }
        private static Type GetElementType(
            object responseValue,
            IQueryable singleResultCollection,
            ControllerActionDescriptor actionDescriptor)
        {
            Contract.Assert(responseValue != null);

            IEnumerable enumerable = responseValue as IEnumerable;
            if (enumerable == null)
            {
                if (singleResultCollection == null)
                {
                    return responseValue.GetType();
                }

                enumerable = singleResultCollection;
            }

            Type elementClrType = GetImplementedIEnumerableType(enumerable.GetType());
            if (elementClrType == null)
            {
                // The element type cannot be determined because the type of the content
                // is not IEnumerable<T> or IQueryable<T>.

                throw new Exception();
            }

            return elementClrType;
        }

        private static bool IsSuccessStatusCode(int statusCode)
        {
            return statusCode >= 200 && statusCode < 300;
        }
        private static Type GetInnerGenericType(Type interfaceType)
        {
            // Getting the type T definition if the returning type implements IEnumerable<T>
            Type[] parameterTypes = interfaceType.GetGenericArguments();

            if (parameterTypes.Length == 1)
            {
                return parameterTypes[0];
            }

            return null;
        }
        private static Type GetImplementedIEnumerableType(Type type)
        {
            // get inner type from Task<T>
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>))
            {
                type = type.GetGenericArguments().First();
            }

            if (type.IsGenericType && type.IsInterface &&
                (type.GetGenericTypeDefinition() == typeof(IEnumerable<>) ||
                 type.GetGenericTypeDefinition() == typeof(IQueryable<>)))
            {
                // special case the IEnumerable<T>
                return GetInnerGenericType(type);
            }
            else
            {
                // for the rest of interfaces and strongly Type collections
                Type[] interfaces = type.GetInterfaces();
                foreach (Type interfaceType in interfaces)
                {
                    if (interfaceType.IsGenericType &&
                        (interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>) ||
                         interfaceType.GetGenericTypeDefinition() == typeof(IQueryable<>)))
                    {
                        // special case the IEnumerable<T>
                        return GetInnerGenericType(interfaceType);
                    }
                }
            }

            return null;
        }
        private static void ValidateSelectExpandOnly(ODataQueryOptions queryOptions)
        {
            if (queryOptions.Filter != null || queryOptions.Count != null || queryOptions.OrderBy != null
                || queryOptions.Skip != null || queryOptions.Top != null)
            {
                throw new Exception();
            }
        }
    }
}
